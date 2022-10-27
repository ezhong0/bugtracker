using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BugTracker.Views.Models
{
    public class UserViewModel
    {

        HttpClient client;

        public UserViewModel(int id)
        {
            User = GetUserAsync(id).GetAwaiter().GetResult();
        }

        public UserModel User { get; set; }

        async Task<UserModel> GetUserAsync(int id)
        {
            var path = $"Users/{id}";
            client = new HttpClient();
            client.BaseAddress = new Uri("/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var jsonResult = JsonDocument.Parse(result);
                JsonElement jsonElement = jsonResult.RootElement.Clone();


                UserModel emp = new UserModel
                {
                    UserId = jsonElement.GetProperty("userId").GetInt32(),
                    Name = jsonElement.GetProperty("name").GetString(),
                    RoleId = jsonElement.GetProperty("roleId").GetInt32()

                };

                return emp;
            }

            return null;
        }
    }
}