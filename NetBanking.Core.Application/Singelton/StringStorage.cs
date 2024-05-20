using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Singelton
{
    public class StringStorage
    {
        private static StringStorage instance;

        public static StringStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StringStorage();
                }
                return instance;
            }
        }


        private string storedString;

        public string GetStoredString()
        {
            return storedString;
        }

        public void SetStoredString(string newString)
        {
            storedString = newString;
        }

        private StringStorage()
        {
            storedString = "";
        }
    }

}
