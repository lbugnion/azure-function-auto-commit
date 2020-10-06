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
using AutoCommit.Model;
using System.Linq;

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

            var data = JsonConvert.DeserializeObject<CommitData>(requestBody);

            if (data == null)
            {
                return new BadRequestObjectResult("No data");
            }

            if (string.IsNullOrEmpty(data.Message))
            {
                data.Message = "Committed by AutoCommit";
            }

            if (string.IsNullOrEmpty(data.Account))
            {
                return new BadRequestObjectResult("Account may not be empty");
            }

            if (string.IsNullOrEmpty(data.Repo))
            {
                return new BadRequestObjectResult("Repo may not be empty");
            }

            if (data.Files == null
                || data.Files.Count == 0)
            {
                return new BadRequestObjectResult("No files to commit");
            }

            var token = Environment.GetEnvironmentVariable(GitHubTokenVariableName);

            if (string.IsNullOrEmpty(token))
            {
                return new BadRequestObjectResult("GitHub token must be configured in the App Settings");
            }

            var commitFilesData = data.Files
                .Select(f => (f.Path, f.Content))
                .ToList();

            var helper = new GitHubHelper();

            var error = await helper.CommitFiles(
                data.Account,
                data.Repo,
                token,
                data.Message,
                commitFilesData);

            // TODO Allow function to receive a list of path / content objects

            if (!string.IsNullOrEmpty(error))
            {
                return new BadRequestObjectResult(error);
            }    

            return new OkObjectResult("Done");
        }
    }
}
