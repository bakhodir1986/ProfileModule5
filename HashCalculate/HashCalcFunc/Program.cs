using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ProfilingTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            var password = "pass@word123";
            var salt = Encoding.UTF8.GetBytes("!@#sdf#$%ertertert");
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            GeneratePasswordHashUsingSaltOptimizied(password, salt);
            stopwatch.Stop();

            Console.WriteLine($"Elapsed Milliseconds Optimized: {stopwatch.ElapsedMilliseconds}ms");

            stopwatch.Start();
            GeneratePasswordHashUsingSalt(password, salt);
            stopwatch.Stop();

            Console.WriteLine($"Elapsed Milliseconds Normal: {stopwatch.ElapsedMilliseconds}ms");

            Console.ReadKey();
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {

            var iterate = 10000;

            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);

            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);

            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;

        }


        public static string GeneratePasswordHashUsingSaltOptimizied(string passwordText, byte[] salt)
        {
            var iterate = 10000;

            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);

            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];

            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16 * sizeof(byte));
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20 * sizeof(byte));

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
}
