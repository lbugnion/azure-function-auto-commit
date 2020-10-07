# Autocommit Azure Function

Welcome to the Autocommit serverless function for Microsoft Azure.

## If you know already

You can deploy the Function to Azure using this ARM template. Simply [click on the `Deploy to Azure` button below](#deploy) and follow the instructions.

## And if you don't know

To be continued

<a id="deploy"></a>

### Deploying to Azure

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Flbugnion%2Fazure-function-auto-commit%2Fmaster%2FDeploy%2Fautocommit-template.json)

### Using the Function

In order to commit files to a repo using the Function, you need to send a POST request to the HTTP endpoint that hosts the Function.

You can do this from any HTTP client, for example in .NET, Azure Logic App, GitHub actions and many many more.

Here is a sample request:

```http
POST https://[FUNCTION NAME].azurewebsites.net/api/autocommit
Content-Type: application/json
x-functions-key: [FUNCTION KEY]

{
    "account": "[GITHUB ACCOUNT NAME]",
    "repo": "[GITHUB REPO]",
    "branch": "[BRANCH TO COMMIT]",
    "message": "[COMMIT MESSAGE]",
    "files": [
        {
            "path": "[PATH OF FILE 1]",
            "content": "[CONTENT OF FILE 1]"
        },
        {
            "path": "[PATH OF FILE 2]",
            "content": "[CONTENT OF FILE 2]"
        }
    ]
}

```

#### Explanation

> **Consider the following:**

> **Branches**: If a branch does not exist in the repo, an error will be returned to the user and nothing will be committed. Always ensure that the branch already exists before you call the Function.

> **Folders**: If a folder (in the path) is non existing, it will be automatically created.

> **Files**: If a file doesn't exist yet, it will be created. If the file already exist, then a new version of the file will be created and committed to the repo.

|Placeholder|Description|Example/Note|
|-----------|-----------|-------|
|`[FUNCTION NAME]`|The name of the Function application which you created earlier.||
|`[FUNCTION KEY]`|The function key authorizing access to your Azure Function|[See below](#function-key)|
|`[GITHUB ACCOUNT NAME]`|The name of the GitHub account to which you want to commit|`lbugnion`|
|`[GITHUB REPO]`|The repository to which you want to commit the files|`my-test-repo`|
|`[BRANCH TO COMMIT]`|The branch to which you want to commit|[IMPORTANT NOTE ABOUT BRANCHES](#branches)|
|`[COMMIT MESSAGE]`|The message that will be appended to this commit|`This is a commit message`|
|`files`|You can commit multiple files at once with the same message. Pass the files as a JSON array of objects||
|`[PATH OF FILE]`|The path of the file, relative to the repo's root.**If a folder doesn't exist yet, it will be automatically created**|`myfolder/myfile.md`
|`[CONTENT OF FILE]`|The content of the file that you want to create or modify. At the moment we only support text files|
