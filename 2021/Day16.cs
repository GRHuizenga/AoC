using Core;
using System.Linq;

namespace _2021
{
    public class Day16 : Day<double>
    {
        private PacketDecoder.Decoder PacketDecoder = new PacketDecoder.Decoder();

        public Day16() : base(2021, "day16") {
            PacketDecoder.Decode(Input.First().ToCharArray());
        }

        public override double PartOne()
        {
            return PacketDecoder.PacketVersions.Sum();
        }

        public override double PartTwo()
        {
            return PacketDecoder.Result;
        }
    }
}
