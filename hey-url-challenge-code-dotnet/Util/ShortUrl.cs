using System;
using System.Text;

namespace hey_url_challenge_code_dotnet.Util
{
    public static class ShortUrl
    {
        private const string CharacterPool = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
        private static readonly Random Random = new();
        private static readonly object ThreadLock = new();

        public static string Generated(int length)
        {
            StringBuilder poolBuilder = new(CharacterPool);
            string pool = poolBuilder.ToString();
            char[] shortUlr = new char[length];
            for (int i = 0; i < length; i++)
            {
                lock (ThreadLock)
                {
                    int charIndex = Random.Next(0, pool.Length);
                    shortUlr[i] = pool[charIndex];
                }
            }

            return new string(shortUlr);
        }
    }
}
