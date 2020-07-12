using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_app.Service
{
    public class PasswordHashService : IPasswordHashService
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                hmac.Key = userPasswordSalt;
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for(var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userPasswordHash[i]) return false;
                }
            }

            return true;
        }
    }
}
