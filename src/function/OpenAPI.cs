using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Aliencube.AzureFunctions.Extensions.OpenApi;
using System.Reflection;
using Microsoft.OpenApi;
using System.Net;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;

namespace OpenApi
{
    public static class OpenAPI
    {
        [FunctionName(nameof(RenderSwaggerDocument))]
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger.json")]
            HttpRequest req,
            ILogger log)
        {
            var settings = new AppSettings();
            var helper = new DocumentHelper();
            var document = new Document(helper);
            var result = await document.InitialiseDocument()
                .AddMetadata(settings.OpenApiInfo)
                .AddServer(req, settings.HttpSettings.RoutePrefix)
                .Build(Assembly.GetExecutingAssembly())
                .RenderAsync(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json)
                .ConfigureAwait(false);
            var response = new ContentResult()
            {
                Content = result,
                ContentType = "application/json",
                StatusCode = (int) HttpStatusCode.OK
            };

            return response;
        }
    }
}