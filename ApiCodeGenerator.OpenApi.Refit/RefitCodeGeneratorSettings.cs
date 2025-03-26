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
    public class RefitCodeGeneratorSettings : CSharpGeneratorBaseSettings
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="RefitCodeGeneratorSettings"/>.
        /// </summary>
        public RefitCodeGeneratorSettings()
        {
            CSharpGeneratorSettings.TemplateFactory = new TemplateFactory(
                CodeGeneratorSettings,
                [
                    GetType().Assembly,
                    typeof(CSharpClientGeneratorSettings).Assembly,
                    typeof(NJsonSchema.CodeGeneration.CSharp.CSharpGenerator).Assembly,
                ]);

            GenerateResponseClasses = false;
            ResponseClass = "IApiResponse";
            ClassName = "{controller}Client";
        }

        /// <summary>
        /// Предоставляет или задает базовый интерфейс.
        /// </summary>
        public string ClientBaseInterface { get; set; }

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
        public bool AuthorizationHeaderParameter { get; set; }

        /// <summary>
        /// Тип используемый для двоичного содержимого.
        /// </summary>
        public string BinaryPartType { get; set; } = "StreamPart";

        /// <summary>
        /// Тип используемый для двоичного ответа.
        /// </summary>
        public string BinaryResponseType { get; set; } = "System.IO.Stream";
    }
}
