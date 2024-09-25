using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace ApiCodeGenerator.OpenApi.Refit.Model
{
    /// <summary>
    /// Педставляет модель клиента используемое в шаблоне.
    /// </summary>
    public class RefitClientTemplateModel : CSharpTemplateModelBase
    {
        private readonly RefitCodeGeneratorSettings _settings;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="RefitClientTemplateModel"/>.
        /// </summary>
        /// <param name="controllerName">Имя контролера.</param>
        /// <param name="controllerClassName">Имя класса.</param>
        /// <param name="settings">Настройки генерации кода.</param>
        /// <param name="operations">Список операций контролера.</param>
        public RefitClientTemplateModel(string controllerName, string controllerClassName, RefitCodeGeneratorSettings settings, IEnumerable<CSharpOperationModel> operations)
            : base(controllerName, settings)
        {
            _settings = settings;
            InterfaceOperations = operations;
            InterfaceName = controllerClassName;
            ExceptionClass = settings.ExceptionClass.Replace("{controller}", controllerName);
        }

        /// <summary>
        /// Получает список операций.
        /// </summary>
        public IEnumerable<CSharpOperationModel> InterfaceOperations { get; }

        /// <summary>
        /// Получает модификатор доступа для интерфейса.
        /// </summary>
        public string InterfaceAccessModifier => _settings.InterfaceAccessModifier ?? "public";

        /// <summary>
        /// Получает значение указывающее нужно ли в имя операции добавлять суффикс Async.
        /// </summary>
        public bool OperationAsyncSuffix => _settings.OperationAsyncSuffix;

        /// <summary>
        /// Получает значение указывающее нужно ли в парметры операции добавлять токен отмены действия.
        /// </summary>
        public bool OperationCancelationToken => _settings.OperationCancelationToken;

        /// <summary>
        /// Получает значение указывающее нужно ли для опциональных парметров добавлять значения по умолчанию.
        /// </summary>
        public bool GenerateOptionalParameters => _settings.GenerateOptionalParameters;

        /// <summary>
        /// Получает значение указывающее наличие операций.
        /// </summary>
        public bool HasOperations => InterfaceOperations.Any();

        /// <summary>
        /// Получает значение указывающее нужно ли генерировать интерфейс.
        /// </summary>
        public bool GenerateClientInterfaces => _settings.GenerateClientInterfaces;

        /// <summary>
        /// Получает значение указывающее базовые иинтерфейсы, разднлннные запятой.
        /// </summary>
        public string ClientBaseInterface => _settings.ClientBaseInterface;

        /// <summary>
        /// Получает значение указывающее наличие базовых интерфейсов.
        /// </summary>
        public bool HasClientBaseInterface => !string.IsNullOrWhiteSpace(ClientBaseInterface);

        /// <summary>
        /// Получает имя интерфейса.
        /// </summary>
        public string InterfaceName { get; }

        /// <summary>
        /// <summary>
        /// Список операций для которых включено оборачивание параметров запроса.
        /// </summary>
        public IEnumerable<RefitOperationModel> OperationsWithWrappedParameters
            => InterfaceOperations.OfType<RefitOperationModel>().Where(o => !string.IsNullOrEmpty(o.WrappedParametersClassName));

        /// <summary>
        /// Имя класса исключения.
        /// </summary>
        public string ExceptionClass { get; }

        /// <summary>
        /// Инициализация оберток параметров запроса.
        /// </summary>
        internal void InitWrappedQueryParameters()
        {
            if (_settings.WrapQueryParmetersMethods?.Any() == true)
            {
                foreach (var operation in InterfaceOperations.OfType<RefitOperationModel>())
                {
                    operation.InitWrappedQueryParameters();
                }
            }
        }
    }
}
