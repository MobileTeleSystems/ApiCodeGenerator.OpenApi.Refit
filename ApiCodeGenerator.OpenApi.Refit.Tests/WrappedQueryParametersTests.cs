using System;
using System.Collections.Generic;
using System.Text;
using static ApiCodeGenerator.Tests.Helpers;

namespace ApiCodeGenerator.OpenApi.Refit.Tests
{
    public class WrappedQueryParametersTests
    {
        [Test]
        public void WrappAllOperations()
        {
            // Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                CSharpGeneratorSettings = { Namespace = "TestNS" },
                WrapQueryParmetersMethods = new[] { "*" },
            };
            var expected =
               "    public partial interface IClient\n" +
               "    {\n" +
               "        /// <summary>\n" +
               "        /// Test operation\n" +
               "        /// </summary>\n" +
               "        /// <returns>Запрос успешно принят</returns>\n" +
               "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
               "        [Get(\"/testService/testOper\")]\n" +
               "        System.Threading.Tasks.Task GetTestOper([Query]GetTestOperQueryParameters wrappedQueryParameters);\n" +
               "\n" +
               "        /// <returns>Запрос успешно принят</returns>\n" +
               "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
               "        [Get(\"/testService/testOper2\")]\n" +
               "        System.Threading.Tasks.Task GetTestOper2([Query]GetTestOper2QueryParameters wrappedQueryParameters);\n" +
               "\n" +
               "    }\n" +
               "\n" +
               "    public class GetTestOperQueryParameters\n" +
               "    {\n" +
               "        /// <summary>Параметр со значением по умолчанию</summary>\n" +
               "        [AliasAs(\"paramWithDef\")]\n" +
               "        public string ParamWithDef { get; set; }\n" +
               "\n" +
               "        /// <summary>Параметр передаваемый в строке запроса</summary>\n" +
               "        [AliasAs(\"queryParametr\")]\n" +
               "        public string QueryParametr { get; set; }\n" +
               "\n" +
               "    }\n" +
               "\n" +
               "    public class GetTestOper2QueryParameters\n" +
               "    {\n" +
               "        [AliasAs(\"param\")]\n" +
               "        public string Param { get; set; }\n" +
               "\n" +
               "    }\n" +
               "\n    ";

            //Act & Assert
            RunTest(settings, expected, "wrappedParams.json", string.Empty);
        }

        [Test]
        public void WrappOneOperation()
        {
            // Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                CSharpGeneratorSettings = { Namespace = "TestNS" },
                WrapQueryParmetersMethods = new[] { "Client.GetTestOper" },
            };

            var expected =
               "    public partial interface IClient\n" +
               "    {\n" +
               "        /// <summary>\n" +
               "        /// Test operation\n" +
               "        /// </summary>\n" +
               "        /// <returns>Запрос успешно принят</returns>\n" +
               "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
               "        [Get(\"/testService/testOper\")]\n" +
               "        System.Threading.Tasks.Task GetTestOper([Query]GetTestOperQueryParameters wrappedQueryParameters);\n" +
               "\n" +
               "        /// <returns>Запрос успешно принят</returns>\n" +
               "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
               "        [Get(\"/testService/testOper2\")]\n" +
               "        System.Threading.Tasks.Task GetTestOper2([Query]string param);\n" +
               "\n" +
               "    }\n" +
               "\n" +
               "    public class GetTestOperQueryParameters\n" +
               "    {\n" +
               "        /// <summary>Параметр со значением по умолчанию</summary>\n" +
               "        [AliasAs(\"paramWithDef\")]\n" +
               "        public string ParamWithDef { get; set; }\n" +
               "\n" +
               "        /// <summary>Параметр передаваемый в строке запроса</summary>\n" +
               "        [AliasAs(\"queryParametr\")]\n" +
               "        public string QueryParametr { get; set; }\n" +
               "\n" +
               "    }\n" +
               "\n    ";

            //Act & Assert
            RunTest(settings, expected, "wrappedParams.json", string.Empty);
        }
    }
}
