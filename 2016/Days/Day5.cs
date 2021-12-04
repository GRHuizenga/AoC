using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace _2016.Days
{
    public class Day5
    {
        private readonly string DoorId = "reyedfim";

        public string PartOne()
        {
            return string.Join(string.Empty, FindHashes().Take(8).Select(hash => hash[5]));
        }

        public string PartTwo()
        {
            char[] password = new char[8] { '-', '-', '-', '-', '-', '-', '-', '-' };
            var enumerator = FindHashes().GetEnumerator();
            while (string.Join(string.Empty, password).Contains("-"))
            {
                enumerator.MoveNext();
                var hash = enumerator.Current;
                if (int.TryParse(hash[5].ToString(), out int index) && index < 8 && password[index] == '-')
                    password[index] = hash[6];
            }
            return string.Join(string.Empty, password);
        }

        private IEnumerable<string> FindHashes()
        {
            var hashes = new List<string>();
            var counter = 0;
            using (var md5 = MD5.Create())
            {
                while (true)
                {
                    var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes($"{DoorId}{counter}"));
                    var hash = string.Join(string.Empty, hashBytes.Select(b => ((int)b).ToString("x2")));
                    if (hash.StartsWith("00000"))
                        yield return hash;
                    counter++;
                }
            }
        }
    }
}
