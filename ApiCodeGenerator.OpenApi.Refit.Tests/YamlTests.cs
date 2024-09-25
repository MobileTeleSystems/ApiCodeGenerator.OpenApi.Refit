using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ApiCodeGenerator.Tests.Helpers;

namespace ApiCodeGenerator.OpenApi.Refit.Tests
{
    public class YamlTests
    {
        [Test]
        public void Generate()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
            };
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <returns>test</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/testService/testOper\")]\n" +
                "        System.Threading.Tasks.Task<TestOperResponse> TestOper();\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected, schemaFile: "testSchema.yml");
        }
    }
}
