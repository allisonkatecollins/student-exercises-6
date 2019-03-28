using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;
using StudentExercisesAPI;
using System.Threading.Tasks;

namespace StudentExercisesAPITests
{
    public class StudentControllerTests
    {
        [Fact]
        public async Task GetSingleStudent()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/Student");

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
