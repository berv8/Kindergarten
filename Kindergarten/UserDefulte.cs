using Kindergarten.Data;
using System.Security.Cryptography;
using System.Text;

namespace Kindergarten
{
    public static class UserDefulte
    {
        public static void SeedData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MyDbContext>();
                context.Database.EnsureCreated();

                if (!context.Logins.Any())
                {
                    var hash = HashPasword("123456", out var salt);
                    var defaultUser = new Login
                    {
                        UserName = "admin",
                        HashedPassword = hash,
                        SaltPassword = salt
                    };
                    context.Add(defaultUser);
                    context.SaveChanges();
                }


                const int keySize = 64;
                const int iterations = 350000;

                static string HashPasword(string password, out byte[] salt)
                {
                    HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

                    salt = RandomNumberGenerator.GetBytes(keySize);
                    var hash = Rfc2898DeriveBytes.Pbkdf2(
                        Encoding.UTF8.GetBytes(password),
                    salt,
                    iterations,
                        hashAlgorithm,
                        keySize);
                    return Convert.ToHexString(hash);
                }

            }


        }

    }
}
