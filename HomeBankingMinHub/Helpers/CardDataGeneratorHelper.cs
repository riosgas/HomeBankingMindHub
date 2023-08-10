using System;

namespace HomeBankingMindHub.Helpers
{
    public static class CardDataGeneratorHelper
    {
        private static Random _random = new Random();

        public static (string number, int cvv) Generate()
        {
            string number = GenerateCardNumber();
            int cvv = GenerateCvv();

            return (number, cvv);
        }

        private static string GenerateCardNumber()
        {
            char[] cardNumber = new char[19];

            for (int i = 0; i < 19; i++)
            {
                if (i is 4 or 9 or 14)
                {
                    cardNumber[i] = '-';
                    continue;
                }

                if (i == 0)
                {
                    int firstDigit = _random.Next(1, 10);
                    cardNumber[i] = char.Parse(firstDigit.ToString());
                }
                else
                {
                    int digit = _random.Next(10);
                    cardNumber[i] = char.Parse(digit.ToString());
                }

            }

            return new string(cardNumber);
        }

        private static int GenerateCvv()
        {
            //int cvv = _random.Next(0, 1000);
            //return cvv.ToString("D3");
            return _random.Next(100, 1000);
        }
    }
}
