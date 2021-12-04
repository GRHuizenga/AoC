using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class IP
{
    public IEnumerable<string> HypernetSequences { get; set; }
    public IEnumerable<string> SupernetSequences { get; set; }
}

namespace _2016.Days
{
    public class Day7 : Day
    {
        private readonly Regex HypernetSequences = new Regex(@"(\[([a-z]+)\])");
        private IEnumerable<IP> Ips;

        private static Func<string, bool> IsAbba = (string input) => input.Substring(0, 2) == string.Join(string.Empty, input.Substring(2, 2).Reverse());

        private static Func<string, bool> IsAba = (string input) => input[0] == input[2] && input[0] != input[1];

        public Day7() : base(@"Day7.txt")
        {
            Ips = Lines.Select(ip =>
            {
                var hs = HypernetSequences.Matches(ip).Select(match => match.Groups.Values.Skip(1).First().Value);
                foreach (var h in hs)
                {
                    ip = ip.Replace(h, " ");
                }
                var rest = ip.Split(" ");
                return new IP
                {
                    HypernetSequences = hs,
                    SupernetSequences = rest,
                };
            });
        }

        public int PartOne()
        {
            return Ips.Where(ip => !ip.HypernetSequences.Any(x => x.FindPattern(IsAbba, 4).FirstOrDefault() != null) && ip.SupernetSequences.Any(x => x.FindPattern(IsAbba, 4).FirstOrDefault() != null)).Count();
        }

        public int PartTwo()
        {
            return Ips
                .Select(ip => new
                {
                    ss = string.Join(' ', ip.SupernetSequences),
                    hs = string.Join(' ', ip.HypernetSequences),
                })
                .Where(ip =>
                {
                    return ip.ss.FindPattern(IsAba, 3).Where(aba => aba.ElementAt(1) != ' ').Any(aba =>
                    {
                        return ip.hs.FindPattern(inp => inp[0] == aba[1] && inp[1] == aba[0] && inp[2] == aba[1], 3).FirstOrDefault() != null;
                    });
                })
                .Count();
        }
    }
}
