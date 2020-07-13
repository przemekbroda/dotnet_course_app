using course_app.Models;
using course_app.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_app.Data
{
    public class Seed : ISeed
    {
        private readonly DataContext _context;
        private readonly IPasswordHashService _hashService;

        public Seed(DataContext context, IPasswordHashService hashService)
        {
            _context = context;
            _hashService = hashService;
        }

        public void SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    _hashService.CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    _context.Users.Add(user);
                }

                _context.SaveChanges();
            }
        }

    }
}
