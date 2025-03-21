using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STRING_OBSFUCATE
{
    class Service
    {
        private string ReplaceAt(string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }
        public string Obsfucate(string value, int start, int end)
        {
            // example: abcdefghijklmnopqrstuvwxyz
            // start = 2, end = 5
            // result: **fghijklmnopqrstu*****
            string newValue = value;
            for (int i = 0; i < Math.Min(value.Length, start); i++)
            {
                newValue = ReplaceAt(newValue, i, '*');
            }
            for (int i = Math.Min(value.Length, value.Length - end); i < value.Length; i++)
            {
                newValue = ReplaceAt(newValue, i, '*');
            }
            return newValue;
        }
    }
}
