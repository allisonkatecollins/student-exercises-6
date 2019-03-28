# Student Exercises - Using Query String Parameters

## Practice

1. Student JSON response should have all exercises that are assigned to them if the `include=exercise` query string parameter is there.
1. Exercise JSON response should have all currently assigned students if the `include=students` query string parameter is there.
1. Provide support for each resource (Instructor, Student, Cohort, Exercise) and the `q` query string parameter. If it is provided, your SQL should search relevant property for a match, search all properties of the resource for a match.
    1. `FirstName`, `LastName`, and `SlackHandle` for instructors and students.
    1. `Name` and `Language` for exercises.
    1. `Name` for cohorts.


> **Hint:** Use [LIKE](https://www.techonthenet.com/sql_server/like.php) in the SQL query for pattern matching.

# Validating Student Exercise Data

## Practice

1. `Name` and `Language` properties on an exercises should be required.
1. Instructor `FirstName`, `LastName`, and `SlackHandle` should be required.
1. Cohort `Name` should be required.
1. Cohort `Name` should be a minimum of of 5 characters and and no more than 11.
1. Student, and Instructor `SlackHandle` value should be a minimum of 3 characters and no more than 12.

# Testing Student Exercise API
## Steps for setting up project

1. Open your Web API solution in Visual Studio
2. In the **Solution Explorer**, right-click the Solution to open a menu.
3. Use the menu select **Add** / **New Project** to open the **Add New Project** dialog.
4. On the left panel, click the **Test** node under **Visual C#**.
5. In the center panel, click **xUnit Test Project (.NET Core)**.
6. Enter a name for your project in the **Name** textbox at the bottom of the form. (something like "<YOUR_WEB_API_PROJECT_NAME>.Tests",  ex. `StudentExercisesAPI.Tests`)
  > Now we need to setup the new Test Project with a reference to your Web API project AND with the correct Nuget packages needed to perform Integration Tests. Visual Studio offers several different ways of adding these references, however, the simplest way is to edit the test project's **.csproj** file.
7. In the **Solution Explorer**, right-click the Test Project and select **Edit <Your_Test_Project>.csproj** in the middle of the context menu.
8. Add the appropriate **PackageReference**s to ensure you have the following references in your project.
```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore" Version="2.2.0" />
    <PackageReference Include="Microsoft.AspNetCore.HttpsPolicy" Version="2.2.0" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc" Version="2.2.0" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="2.2.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="15.9.0" />
    <PackageReference Include="xunit" Version="2.4.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.0" />
  </ItemGroup>
```
9. Below the **PackageReferences**s add the following **ProjectReference** section to give your Test Project access to your Web API Project. 
**NOTE:** Make sure you fill in the `__YOUR SOLUTION NAME__` and `__YOUR WEB API PROJECT NAME__` placeholders with the names of your solution and project.
```xml
  <ItemGroup>
    <ProjectReference Include="..\__YOUR SOLUTION NAME__\__YOUR WEB API PROJECT NAME__.csproj" />
  </ItemGroup>
```
10. Create a new **class** in your Test Project. Call it `APIClientProvider`. Replace the code in the new file with the following code.
**Note:** Make sure to fill in the `__YOUR WEB API PROJECT NAMESPACE__` and `__YOUR TEST PROJECT NAMESPACE__` placeholders.
```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;
using __YOUR WEB API PROJECT NAMESPACE__;

namespace __YOUR TEST PROJECT NAMESPACE__
{
    class APIClientProvider : IClassFixture<WebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; private set; }
        private readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();

        public APIClientProvider()
        {
            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _factory?.Dispose();
            Client?.Dispose();
        }
    }
}
```
11. In the **Test** menu click the **Window**/**Test Explorer** item to open the **Test Explorer**.
  > Now it's time to write soem tests
12. Make a new **class** called something like `<SOME_CONTROLLER_NAME>Tests` (ex. `StudentsControllerTests`.
13. Add a test **method** to the new class.
> Example
```csharp
    public class StudentsControllerTests
    {
        [Fact]
        public async Task Get_All_Students_Returns_Some_Students()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/students");

                response.EnsureSuccessStatusCode();
            }
        }
    }
```

## Practice

You need to create integration tests for the following features of your Student Exercises API.

1. Basic CRUD for Students
1. Basic CRUD for Cohorts
1. Basic CRUD for Instructors
1. Basic CRUD for Exercises

