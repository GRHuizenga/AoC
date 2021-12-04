using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2016.Days
{
    public class Day4: Day
    {
        private IEnumerable<Room> Rooms;

        public Day4(): base(@"Day4.txt")
        {
            Rooms = Lines.Select(input => new Room(input));
        }

        public int PartOne()
        {
            return Rooms
                .Where(room => string.Join(string.Empty, room.OccurrencesPerCharacter.Take(5).Select(r => r.Key)) == room.Checksum)
                .Select(room => room.SectorId)
                .Sum();
        }

        public int PartTwo()
        {
            return Rooms.Where(room => room.DecryptName().Contains("northpole")).First().SectorId;
        }
    }

    public class Room
    {
        private readonly Regex regex = new Regex(@"([a-z-]+)(\d+)\[([a-z]+)\]");

        public int SectorId { get; private set; }
        public string EncryptedName { get; private set; }
        public string Checksum { get; private set; }

        public IOrderedEnumerable<KeyValuePair<char, int>> OccurrencesPerCharacter { get; private set; }

        public Room(string description)
        {
            var match = regex.Match(description);
            EncryptedName = match.Groups[1].Value;
            SectorId = int.Parse(match.Groups[2].Value);
            Checksum = match.Groups[3].Value;
            Analyze();
        }

        private void Analyze()
        {
            OccurrencesPerCharacter = EncryptedName
                .Replace("-", string.Empty)
                .ToHashSet()
                .AsEnumerable()
                .ToDictionary(c => c, c => EncryptedName.Count(ch => ch == c))
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key);
        }

        public string DecryptName()
        {
            var deltaPosition = SectorId % 26;
            return string.Join(string.Empty, EncryptedName.Select(c => c == '-' ? ' ' : (char)(((c - 97) + deltaPosition) % 26 + 97)));
        }
    }
}
