using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NSwag.CodeGeneration.CSharp;

namespace ApiCodeGenerator.OpenApi.Refit
{
    /// <summary>
    /// Настройки генератора кода.
    /// </summary>
    public class RefitCodeGeneratorSettings : CSharpClientGeneratorSettings
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="RefitCodeGeneratorSettings"/>.
        /// </summary>
        public RefitCodeGeneratorSettings()
        {
            CSharpGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(
                CodeGeneratorSettings,
                new Assembly[]
                {
                    GetType().Assembly,
                    typeof(CSharpClientGeneratorSettings).Assembly,
                    typeof(NJsonSchema.CodeGeneration.CSharp.CSharpGenerator).Assembly,
                });

            GenerateExceptionClasses = false;
            GenerateResponseClasses = false;
            ResponseClass = "IApiResponse";
            ExceptionClass = "Refit.ApiException";
        }

        /// <summary>
        /// Предоставляет или задает модификатор доступа интерфейса.
        /// </summary>
        public string InterfaceAccessModifier { get; set; }

        /// <summary>
        /// Предоставляет или задает значение указывающее нужно ли добавлять к имени операции суффикс Async.
        /// </summary>
        public bool OperationAsyncSuffix { get; set; }

        /// <summary>
        /// Предоставляет или задает нужно ли в параметры включать токен отмены.
        /// </summary>
        public bool OperationCancelationToken { get; set; }

        /// <summary>
        /// Регулярное выражение для получения пути операции.
        /// </summary>
        public string PathExtractExpression { get; set; }

        /// <summary>
        /// Список методов для которых нужно включить оборачивание параметров передаваемых в запросе.
        /// </summary>
        public string[] WrapQueryParmetersMethods { get; set; }

        /// <summary>
        /// Включает генерацию параметра для передачи заголовка авторизации.
        /// </summary>
        public bool AuthorizationHeaderParameter { get; internal set; }
    }
}
