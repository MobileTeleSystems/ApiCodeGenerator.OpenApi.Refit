using System;
using System.Collections.Generic;
using System.Text;
using ApiCodeGenerator.Abstraction;

namespace ApiCodeGenerator.OpenApi.Refit
{
    /// <summary>
    /// Точка подключения к генератору.
    /// </summary>
    public static class AcgExtension
    {
        /// <summary>
        /// Регистрация дополнительного генератора.
        /// </summary>
        public static Dictionary<string, ContentGeneratorFactory> CodeGenerators { get; } = new()
        {
            ["OpenApiToRefitClient"] = RefitContentGenerator.CreateAsync,
        };
    }
}
