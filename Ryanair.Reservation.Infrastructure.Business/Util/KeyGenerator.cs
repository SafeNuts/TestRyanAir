using System;
using System.Text;

namespace Ryanair.Reservation.Infrastructure.Business.Util
{
    public static class KeyGenerator
    {
        public static string GenerateKey()
        {
            var stringPart = GenerateRandomString();
            var numberPart = GenerateRandomNumber();

            return string.Concat(stringPart, numberPart);
        }

        private static string GenerateRandomString()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char character;

            for (int i = 0; i < 3; i++)
            {
                character = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(character);
            }

            return builder.ToString();
        }

        private static int GenerateRandomNumber()
        {
            var random = new Random();
            return random.Next(100, 999);
        }
    }
}
