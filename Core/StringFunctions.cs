using System;

namespace Core
{
    public static class StringFunctions
    {
        static readonly Random random = new Random();
        public static string RandomLetters(int length) {
            var result = string.Empty;

            for (int i = 1; i <= length; i++) {
                int num = random.Next(0, 26);
                result += (char)('a' + num);
            }

            return result;
        }
    }
}