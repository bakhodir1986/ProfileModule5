using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Security.Cryptography;
using System.Text;

namespace HashCodeBanchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<HashCodeGeneration>();

            Console.ReadKey();
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt, HashAlgorithmName hashAlgorithm, bool isBufferCopy)
        {

            var iterate = 10000;

            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate, hashAlgorithm);

            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];

            if (isBufferCopy)
            {
                Array.Copy(salt, 0, hashBytes, 0, 16);

                Array.Copy(hash, 0, hashBytes, 16, 20);
            }
            else
            {
                Buffer.BlockCopy(salt, 0, hashBytes, 0, 16 * sizeof(byte));
                Buffer.BlockCopy(hash, 0, hashBytes, 16, 20 * sizeof(byte));
            }

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;

        }

    }

    public class HashCodeGeneration
    {
        private readonly string password;
        private readonly byte[] salt;

        public HashCodeGeneration()
        {
            password = "pass@word123";
            salt = Encoding.UTF8.GetBytes("!@#sdf#$%ertertert");
        }

        [Benchmark]
        public string MD5() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.MD5, false);

        [Benchmark]
        public string SHA1() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.SHA1, false);

        [Benchmark]
        public string SHA256() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.SHA256, false);

        [Benchmark]
        public string SHA384() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.SHA384, false);

        [Benchmark]
        public string SHA512() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.SHA512, false);


        //=================

        [Benchmark]
        public string MD5BlockCopy() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.MD5, true);

        [Benchmark]
        public string SHA1BlockCopy() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.SHA1, true);

        [Benchmark]
        public string SHA256BlockCopy() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.SHA256, true);

        [Benchmark]
        public string SHA384BlockCopy() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.SHA384, true);

        [Benchmark]
        public string SHA512BlockCopy() => Program.GeneratePasswordHashUsingSalt(password, salt, HashAlgorithmName.SHA512, true);
    }
}
