using System;
using System.Linq;
using System.Text;

namespace STRING_LOREM
{
    public class StringLoremService
    {
        private static readonly string[] LoremWords =
        {
            "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
            "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore",
            "magna", "aliqua"
        };

        public string GenerateLoremIpsum(int length = 0)
        {
            if (length <= 0) return string.Empty;

            var random = new Random();
            var sb = new StringBuilder();

            while (sb.Length < length)
            {
                string word = LoremWords[random.Next(LoremWords.Length)];
                if (sb.Length + word.Length + 1 > length) break;

                if (sb.Length > 0)
                    sb.Append(" ");

                sb.Append(word);
            }

            return sb.ToString();
        }


    }
}
