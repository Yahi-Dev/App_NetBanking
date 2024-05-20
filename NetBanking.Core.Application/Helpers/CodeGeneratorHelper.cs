using NetBanking.Core.Domain.Entities;
using System.Text;

namespace NetBanking.Core.Application.Helpers
{
    public static class CodeGeneratorHelper
    {
        public static string GenerateCode(Type productType)
        {
            var prefix = GeneretePrefix(productType);
            Random rand = new Random();
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i <= 5; i++)
            {
                string randomNumber = rand.Next(1, 10).ToString();
                builder.Append(randomNumber);
            }
            string FullCode = prefix + builder.ToString();
            return FullCode;
        }

        public static string GeneretePrefix(Type ProductType)
        {
            Random rand = new Random();
            if (ProductType == typeof(CreditCard))
            {
                string prefix = rand.Next(100, 300).ToString();
                return prefix;
            }
            else if (ProductType == typeof(SavingsAccount))
            {
                string prefix = rand.Next(300, 600).ToString();
                return prefix;
            }
            else
            {
                string prefix = rand.Next(600, 1000).ToString();
                return prefix;
            }
        }
    }
}
