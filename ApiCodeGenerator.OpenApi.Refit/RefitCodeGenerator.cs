﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApiCodeGenerator.OpenApi.Refit.Model;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace ApiCodeGenerator.OpenApi.Refit
{
    /// <summary>
    /// Реализует логику генерации кода клиента Service Registry по документу OpenApi.
    /// </summary>
    public class RefitCodeGenerator : CSharpGeneratorBase
    {
        private readonly RefitCodeGeneratorSettings _settings;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="RefitCodeGenerator"/>.
        /// </summary>
        /// <param name="openApiDocument">Описание Api.</param>
        /// <param name="settings">Настройки генерации кода.</param>
        /// <param name="resolver">Резолвер.</param>
        public RefitCodeGenerator(OpenApiDocument openApiDocument, RefitCodeGeneratorSettings settings, CSharpTypeResolver resolver)
            : base(Cleanup(openApiDocument, settings), settings, resolver)
        {
            OpenApiDocument = openApiDocument;
            BaseSettings = settings;
            _settings = settings;
            _settings.CSharpGeneratorSettings.ExcludedTypeNames =
                [
                    "FileParameter",
                    "FileResponse",
                    .. _settings.CSharpGeneratorSettings.ExcludedTypeNames
                ];
            SetUsages();
        }

        internal RefitCodeGenerator(OpenApiDocument openApiDocument, RefitCodeGeneratorSettings settings)
            : this(openApiDocument, settings, CreateResolverWithExceptionSchema(settings.CSharpGeneratorSettings, openApiDocument))
        {
        }

        /// <inheritdoc/>
        public override ClientGeneratorBaseSettings BaseSettings { get; }

        /// <summary>
        /// OpenApi документ.
        /// </summary>
        protected OpenApiDocument OpenApiDocument { get; }

        public override string GetBinaryResponseTypeName() => _settings.BinaryResponseType;

        /// <inheritdoc />
        protected override IEnumerable<CodeArtifact> GenerateClientTypes(string controllerName, string controllerClassName, IEnumerable<CSharpOperationModel> operations)
        {
            RefitClientTemplateModel model = CreateTemplateModel(controllerName, 'I' + controllerClassName, operations);
            if (model.HasOperations && model.GenerateClientInterfaces)
            {
                model.InitWrappedQueryParameters();
                var template = _settings.CSharpGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Client.Interface", model);
                yield return new CodeArtifact(model.InterfaceName, CodeArtifactType.Interface, CodeArtifactLanguage.CSharp, CodeArtifactCategory.Contract, template);
            }
        }

        /// <summary>
        /// Создание модели кода интерфейса.
        /// </summary>
        /// <param name="controllerName">Имя контроллера.</param>
        /// <param name="controllerClassName">Имя класса класса контроллера.</param>
        /// <param name="operations">Перечень операций.</param>
        /// <returns>Модель кода интерфейса.</returns>
        protected virtual RefitClientTemplateModel CreateTemplateModel(string controllerName, string controllerClassName, IEnumerable<CSharpOperationModel> operations)
        {
            return new RefitClientTemplateModel(controllerName, controllerClassName, _settings, operations);
        }

        /// <inheritdoc />
        protected override CSharpOperationModel CreateOperationModel(OpenApiOperation operation, ClientGeneratorBaseSettings settings)
        {
            return new RefitOperationModel(operation, _settings, this, (CSharpTypeResolver)Resolver);
        }

        /// <summary>
        /// Создание модели файла.
        /// </summary>
        /// <param name="clientTypes">Код типов клиента.</param>
        /// <param name="dtoTypes">Код типов DTO.</param>
        /// <param name="outputType">Типов вывода.</param>
        /// <returns>Возвращает экземпляр модели.</returns>
        protected virtual CSharpFileTemplateModel CreateFileTemplateModel(IEnumerable<CodeArtifact> clientTypes, IEnumerable<CodeArtifact> dtoTypes, ClientGeneratorOutputType outputType)
            => new CSharpFileTemplateModel(clientTypes, dtoTypes, outputType, OpenApiDocument, _settings, this, (CSharpTypeResolver)Resolver);

        /// <summary> Перечень пространств имен, которые требуется включить в вывод. </summary>
        /// <returns> Массив пространств имен. </returns>
        protected virtual IEnumerable<string> GetUsages()
            => ["Refit"];

        protected void SetUsages()
        {
            var usages = GetUsages();
            _settings.AdditionalContractNamespaceUsages = [.. _settings.AdditionalContractNamespaceUsages, .. usages];
            _settings.AdditionalNamespaceUsages = [.. _settings.AdditionalNamespaceUsages, .. usages];
        }

        private static OpenApiDocument Cleanup(OpenApiDocument document, RefitCodeGeneratorSettings settings)
        {
            RemoveStringLengtAttribute(document, settings);
            return document;
        }

        /// <summary>Если возвращаемый тип не является строкой то меняем настройки чтоб не генерился атрибут StringLength.</summary>
        private static void RemoveStringLengtAttribute(OpenApiDocument document, RefitCodeGeneratorSettings settings)
        {
            var resolver = new CSharpTypeResolver(settings.CSharpGeneratorSettings);
            foreach (var item in document.Components.Schemas.Values)
            {
                CleanupProperties(item.ActualProperties);
            }

            void CleanupProperties(IEnumerable<KeyValuePair<string, JsonSchemaProperty>> actualProperties)
            {
                if (actualProperties != null)
                {
                    foreach (var pair in actualProperties)
                    {
                        var prop = pair.Value;
                        var targetType = resolver.Resolve(prop, prop.IsNullable(settings.CSharpGeneratorSettings.SchemaType), string.Empty);
                        if (prop.Type == JsonObjectType.String && targetType?.Equals("string", StringComparison.OrdinalIgnoreCase) != true)
                        {
                            prop.ActualSchema.MaxLength = null;
                            prop.ActualSchema.MinLength = null;
                        }

                        CleanupProperties(prop.Properties);
                    }
                }
            }
        }
    }
}
