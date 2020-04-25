using webApiV1.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using webApiV1.Config;

namespace webApiV1.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> _users;


        public UserServices(IMongoDatabaseSetting settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>("User");
        }

        /* 
         * get all users
         */
        public List<User> Get() => _users.Find(user => true).ToList();

        /* 
        * get one users
        */
        public User Get(string Id) => _users.Find(user => user.Id == Id).FirstOrDefault();



        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string Id, User user)
        {
            _users.ReplaceOne(user => user.Id == Id, user);
        }

        public void Remove(string Id) => _users.DeleteOne(user => user.Id == Id);

    }
}


