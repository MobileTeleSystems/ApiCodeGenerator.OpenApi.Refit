using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ApiCodeGenerator.Tests.Helpers;

namespace ApiCodeGenerator.OpenApi.Refit.Tests
{
    public class OpenApi3_Tests
    {
        [Theory]
        [TestCase(OpenApiParameterStyle.Undefined, true, "Multi")]
        [TestCase(OpenApiParameterStyle.Form, true, "Multi")]
        [TestCase(null, null, "Multi")]
        [TestCase(OpenApiParameterStyle.SpaceDelimeted, true, "Multi")]
        [TestCase(OpenApiParameterStyle.PipeDelimited, true, "Multi")]
        [TestCase(OpenApiParameterStyle.Form, false, "Csv")]
        [TestCase(OpenApiParameterStyle.SpaceDelimeted, false, "Ssv")]
        [TestCase(OpenApiParameterStyle.PipeDelimited, false, "Pipes")]
        [TestCase(OpenApiParameterStyle.Simple, false, "\"Collection style 'Simple'\" not supported")]
        [TestCase(OpenApiParameterStyle.DeepObject, false, "\"Collection style 'DeepObject'\" not supported")]
        public void CollectionFormat_From_Style(OpenApiParameterStyle? style, bool? explode, string expectedColFormat)
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
            };
            var doc = CreateOpenApiDoc(style, explode);
            settings.CSharpGeneratorSettings.Namespace = "TestNS";
            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <returns>valid input</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Get(\"/test\")]\n" +
                $"        System.Threading.Tasks.Task GetTest([Query(CollectionFormat.{expectedColFormat})]System.Collections.Generic.IEnumerable<string> tParam, [Query]int tParam2);\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected, doc, "    \n");
        }

        private static OpenApiDocument CreateOpenApiDoc(OpenApiParameterStyle? style, bool? explode)
        {
            var json = "{" + OpenApiDocumentDeclaration + @"
  ""paths"": {
    ""/test"": {
      ""get"": {
        ""operationId"": ""GetTest"",
        ""parameters"": [
          {
            ""in"": ""query"",
            ""name"": ""tParam"",
            ""schema"": {
              ""type"": ""array"",
              ""items"": {
                ""type"": ""string""
              }
            }
          },
          {
            ""in"": ""query"",
            ""name"": ""tParam2"",
            ""required"": true,
            ""schema"": {
              ""type"": ""integer""
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""valid input""
          }
        }
      }
    }
  }}";
            var doc = OpenApiDocument.FromJsonAsync(json).GetAwaiter().GetResult();
            var parameter = doc.Operations.First().Operation.Parameters.First();

            if (style.HasValue)
                parameter.Style = style.Value;

            if (explode.HasValue)
                parameter.Explode = explode.Value;

            return doc;
        }
    }
}
