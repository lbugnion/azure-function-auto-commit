using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AutoCommit.Model.GitHub;
using System.Collections.Generic;

namespace AutoCommit
{
    public static class AutoCommit
    {
        private const string GitHubTokenVariableName = "GitHubToken";

        [FunctionName("AutoCommit")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function, 
                "post", 
                Route = "autocommit")] 
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var account = (data.account ??= string.Empty).ToString();
            var repo = (data.repo ??= string.Empty).ToString();
            var path = (data.path ??= string.Empty).ToString();
            var content = (data.content ??= string.Empty).ToString();
            var message = (data.message ??= string.Empty).ToString();

            if (string.IsNullOrEmpty(message))
            {
                message = "Committed by AutoCommit";
            }

            if (string.IsNullOrEmpty(account))
            {
                return new BadRequestObjectResult("account may not be empty");
            }

            if (string.IsNullOrEmpty(repo))
            {
                return new BadRequestObjectResult("repo may not be empty");
            }

            if (string.IsNullOrEmpty(path))
            {
                return new BadRequestObjectResult("path may not be empty");
            }

            if (string.IsNullOrEmpty(content))
            {
                return new BadRequestObjectResult("content may not be empty");
            }

            var token = Environment.GetEnvironmentVariable(GitHubTokenVariableName);

            if (string.IsNullOrEmpty(token))
            {
                return new BadRequestObjectResult("GitHub token must be configured in the App Settings");
            }

            var helper = new GitHubHelper();

            var error = await helper.CommitFiles(
                account,
                repo,
                token,
                message,
                new List<(string, string)>
                {
                    (path, content)
                });

            // TODO Allow function to receive a list of path / content objects

            if (!string.IsNullOrEmpty(error))
            {
                return new BadRequestObjectResult(error);
            }    

            return new OkObjectResult("Done");
        }
    }
}
