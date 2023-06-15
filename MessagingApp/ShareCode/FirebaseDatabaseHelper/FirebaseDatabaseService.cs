using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MessagingApp
{
    public class FirebaseDatabaseService
    {
        private readonly FirebaseClient _client;

        public FirebaseDatabaseService()
        {
            _client = new FirebaseClient("https://chatapp-cc22e-default-rtdb.firebaseio.com/");
        }
        public async Task<List<UserModel>> GetAllUsers()
        {
            return (await _client
                .Child("UserModel")
                .OnceAsync<UserModel>()).Select(item  => new UserModel
                { 
                    Id= item.Key,
                    Name = item.Object.Name,
                    MobileNumber = item.Object.MobileNumber,
                    DateOfBirth= item.Object.DateOfBirth,
                    ProfileImage = item.Object.ProfileImage,
                    IsProfileImageRemoved = item.Object.IsProfileImageRemoved,
                    Bio= item.Object.Bio,
                }).ToList();
        }
        
        public async Task AddUser<T>(T t)
        {
            var data = JsonConvert.SerializeObject(t);
            var newItem = await _client
                .Child("UserModel")
                .PostAsync(data);
        }

        public async Task Update<T>(string key,T t)
        {
            var data = JsonConvert.SerializeObject(t);
            await _client
                 .Child("UserModel")
                 .Child(key)
                 .PutAsync(data);
        }
        public async Task<UserModel> GetUser(string mobileNumber)
        {
            var allPerson = await GetAllUsers();
            var r = await _client
                .Child("UserModel")
                .OnceAsync<UserModel>();
            return allPerson.Where(u => u.MobileNumber == mobileNumber).FirstOrDefault();


        }
    }
}
