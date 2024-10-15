using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiCodeGenerator.OpenApi.Refit;
using static ApiCodeGenerator.Tests.Helpers;

namespace ApiCodeGenerator.OpenApi.Refit.Tests
{
    public class FormDataTests
    {
        [Test]
        public async Task FormData()
        {
            var json = $$"""
            {
                {{OpenApiDocumentDeclaration}}
                "paths": {
                    "/test": {
                        "post": {
                            "operationId": "GetTest",
                            "requestBody": {
                                "content":{
                                    "application/x-www-form-urlencoded":{
                                        "schema":{
                                            "type": "object",
                                            "properties": {
                                                "testProp":{"type":"string"}
                                            },
                                            "additionalProperties": false
                                        }
                                    }
                                }
                            },
                            "responses": {
                                "200": {"description": "valid input"}
                            }
                        }
                    }
                }
            }
            """;
            var document = await OpenApiDocument.FromJsonAsync(json);
            RefitCodeGeneratorSettings settings = new()
            {
                GenerateClientInterfaces = true,
                CSharpGeneratorSettings =
                {
                    Namespace = "TestNS",
                },
            };
            var expectedInterfaceCode =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <returns>valid input</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Post(\"/test\")]\n" +
                $"        System.Threading.Tasks.Task GetTest([Body(BodySerializationMethod.UrlEncoded)]GetTestFormData body);\n" +
                "\n" +
                "    }\n";
            var expectedClassCode =
               $"    {GENERATED_CODE}\n" +
                "    public partial class GetTestFormData\n" +
                "    {\n" +
                "        [Newtonsoft.Json.JsonProperty(\"testProp\", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]\n" +
                "        public string TestProp { get; set; }\n" +
                "\n" +
                "    }\n";
            RunTest(settings, expectedInterfaceCode, document, expectedClassCode);
        }

        [Test]
        public void FileUpload()
        {
            //Arrange
            var settings = new RefitCodeGeneratorSettings
            {
                GenerateClientInterfaces = true,
                CSharpGeneratorSettings = { Namespace = "TestNS" },
            };

            var expected =
                "    public partial interface IClient\n" +
                "    {\n" +
                "        /// <returns>OK</returns>\n" +
                "        /// <exception cref=\"Refit.ApiException\">A server side error occurred.</exception>\n" +
                "        [Multipart]\n" +
                "        [Post(\"/file\")]\n" +
                "        System.Threading.Tasks.Task Upload(int? id, StreamPart file, System.Collections.Generic.ICollection<StreamPart> fileArr);\n" +
                "\n" +
                "    }\n";

            //Act & Assert
            RunTest(settings, expected, "multipart.yml", "    \n");
        }
    }
}
