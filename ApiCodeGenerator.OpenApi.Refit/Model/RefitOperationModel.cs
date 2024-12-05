using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace ApiCodeGenerator.OpenApi.Refit.Model
{
    /// <summary>
    /// Педставляет описание операции API передаваемое используемое в шаблоне.
    /// </summary>
    public class RefitOperationModel : CSharpOperationModel
    {
        private readonly OpenApiOperation _operation;
        private readonly CSharpGeneratorBase _generator;
        private readonly CSharpTypeResolver _resolver;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="RefitOperationModel"/>.
        /// </summary>
        /// <param name="operation">Описание операции.</param>
        /// <param name="settings">Настройки генератора кода.</param>
        /// <param name="generator">Экземпляр генератора.</param>
        /// <param name="resolver">Резолвер типов.</param>
        public RefitOperationModel(OpenApiOperation operation, RefitCodeGeneratorSettings settings, CSharpGeneratorBase generator, CSharpTypeResolver resolver)
            : base(operation, settings, generator, resolver)
        {
            _operation = operation;
            Settings = settings;
            _generator = generator;
            _resolver = resolver;

            IEnumerable<OpenApiParameter> parameters = GetActualParameters();

            if (Settings.AuthorizationHeaderParameter && RequiresAuthentication)
            {
                var openApiParameter = new OpenApiParameter
                {
                    Name = "Authorization",
                    Type = JsonObjectType.String,
                    Kind = OpenApiParameterKind.Header,
                };
                parameters = parameters.Append(openApiParameter);
            }

            if (Settings.GenerateOptionalParameters)
            {
                parameters = parameters
                    .OrderBy(p => !p.IsRequired)
                    .ThenBy(p => p.Position ?? 0);
            }

            Parameters = parameters
                           .Select(parameter =>
                                new RefitParameterModel(
                                   parameter.Name,
                                   GetParameterVariableName(parameter, operation.Parameters),
                                   GetParameterVariableIdentifier(parameter, operation.Parameters),
                                   ResolveParameterType(parameter),
                                   parameter,
                                   operation.Parameters,
                                   Settings.CodeGeneratorSettings,
                                   generator,
                                   resolver))
                           .ToList<CSharpParameterModel>();
        }

        /// <inheritdoc/>
        public override string ActualOperationName
            => base.ActualOperationName.Replace($"Using{HttpMethod.ToUpper()}", string.Empty);

        public new virtual IEnumerable<CSharpExceptionDescriptionModel> ExceptionDescriptions
        {
            get => [];
        }

        /// <inheritdoc/>
        public override string ResultType
        {
            get
            {
                if (UnwrappedResultType == "void")
                {
                    return typeof(Task).FullName;
                }

                return $"{typeof(Task).FullName}<{UnwrappedResultType}>";
            }
        }

        /// <summary>
        /// Предоставляет путь к операции.
        /// </summary>
        public new string Path
        {
            get
            {
                if (!string.IsNullOrEmpty(Settings.PathExtractExpression))
                {
                    var match = Regex.Match(base.Path, Settings.PathExtractExpression);
                    var extractedPathGroup = match?.Groups[1];

                    if (extractedPathGroup.Success)
                    {
                        return extractedPathGroup.Value;
                    }
                }

                return base.Path;
            }
        }

        /// <summary>
        /// Список параметров которые требуется обернуть в класс.
        /// </summary>
        public IEnumerable<RefitParameterModel> WrappedParameters { get; private set; }

        /// <summary>
        /// Имя класса-обертки для параметров.
        /// </summary>
        public string WrappedParametersClassName { get; private set; }

        public virtual string WrappedResultType
        {
            get
            {
                if (UnwrappedResultType == "void")
                {
                    return Settings.ResponseClass.EndsWith("IApiResponse")
                        ? $"{typeof(Task).FullName}<{Settings.ResponseClass}>"
                        : $"{typeof(Task).FullName}<{Settings.ResponseClass}<object>>";
                }

                return $"{typeof(Task).FullName}<{Settings.ResponseClass}<{UnwrappedResultType}>>";
            }
        }

        /// <summary>
        /// Возвращает настройки генератора кода.
        /// </summary>
        protected RefitCodeGeneratorSettings Settings { get; }

        /// <summary>
        /// Инициализация оберток параметров запроса.
        /// </summary>
        internal void InitWrappedQueryParameters()
        {
            var fullMethodName = GetOperationPath();
            var isWrapQueryParamters = Settings.WrapQueryParmetersMethods?.Any(i => i == "*" || i == fullMethodName) == true;
            if (isWrapQueryParamters)
            {
                WrappedParametersClassName = $"{ActualOperationName}QueryParameters";
                var wrappedQueryParmeter = new CSharpParameterModel(
                    null,
                    "wrappedQueryParameters",
                    string.Empty,
                    WrappedParametersClassName,
                    new OpenApiParameter { Kind = OpenApiParameterKind.Query },
                    _operation.Parameters,
                    Settings.CodeGeneratorSettings,
                    _generator,
                    _resolver);

                WrappedParameters = Parameters.OfType<RefitParameterModel>()
                                              .Where(p => p.Kind == OpenApiParameterKind.Query)
                                              .ToArray();
                Parameters = Parameters.Select(p => p.Kind == OpenApiParameterKind.Query ? wrappedQueryParmeter : p)
                    .Distinct()
                    .ToList();
            }
        }

        protected override string ResolveParameterType(OpenApiParameter parameter)
        {
            if (Settings is not null)
            {
                if (ConsumesFormUrlEncoded && parameter.Kind == OpenApiParameterKind.Body)
                {
                    var isNullable = parameter.IsRequired == false || parameter.IsNullable(Settings.CodeGeneratorSettings.SchemaType);
                    var typeNameHint = $"{Id}FormData";
                    return _resolver?.Resolve(parameter.ActualSchema, isNullable, typeNameHint);
                }

                if (_resolver is not null && HasFormParameters)
                {
                    var typeName = Settings.BinaryPartType;
                    if (parameter.IsBinaryBodyParameter || parameter.ActualSchema.IsBinary)
                    {
                        return typeName;
                    }

                    if (parameter.ActualSchema.IsArray && parameter.ActualSchema.Item.IsBinary)
                    {
                        return $"{Settings.CSharpGeneratorSettings.ArrayType}<{typeName}>";
                    }
                }
            }

            return base.ResolveParameterType(parameter);
        }

        private string GetOperationPath()
        {
            return $"{Settings.GenerateControllerName(ControllerName)}.{ActualOperationName}";
        }
    }
}
