using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021.PacketDecoder
{
    public class Decoder
    {
        public List<int> PacketVersions { get; private set; }
        public double Result { get; private set; }

        public Decoder() { }

        public void Decode(IEnumerable<char> hexString)
        {
            PacketVersions = new List<int>();
            IEnumerable<char> binaryString = hexString.SelectMany(hex => hex.ToBinary());
            (binaryString, Result) = ParsePacket(binaryString);
        }

        private (IEnumerable<char> remainder, double value) ParsePacket(IEnumerable<char> binaryString)
        {
            var packetVersion = Convert.ToInt32(string.Join(string.Empty, binaryString.Take(3)), 2);
            PacketVersions.Add(packetVersion);
            var packetId = Convert.ToInt32(string.Join(string.Empty, binaryString.Skip(3).Take(3)), 2);

            double value = 0;
            IEnumerable<double> values = new List<double>();
            
            if ((PacketType)packetId != PacketType.Literal)
                (binaryString, values) = ParseOperator(binaryString.Skip(6));
            
            switch ((PacketType)packetId)
            {
                case PacketType.Sum:
                    value = values.Sum();
                    break;
                case PacketType.Product:
                    value = values.Aggregate(1.0, (acc, curr) => acc * curr);
                    break;
                case PacketType.Minimum:
                    value = values.Min();
                    break;
                case PacketType.Maximum:
                    value = values.Max();
                    break;
                case PacketType.Literal:
                    (binaryString, value) = ParseLiteral(binaryString.Skip(6));
                    break;
                case PacketType.GreaterThan:
                    value = values.First() > values.Last() ? 1 : 0;
                    break;
                case PacketType.LessThan:
                    value = values.First() < values.Last() ? 1 : 0;
                    break;
                case PacketType.EqualTo:
                    value = values.First() == values.Last() ? 1 : 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(packetId));
            }

            return (binaryString, value);
        }

        private (IEnumerable<char> remainder, double literal) ParseLiteral(IEnumerable<char> binaryString)
        {
            string literal = string.Empty;
            IEnumerable<char> group;

            do
            {
                group = binaryString.Take(5);
                binaryString = binaryString.Skip(5);
                literal += string.Join(string.Empty, group.Skip(1));
            } while (group.First() != '0');

            return (binaryString, Convert.ToUInt64(literal, 2));
        }

        private (IEnumerable<char> remainder, IEnumerable<double> values) ParseOperator(IEnumerable<char> binaryString)
        {
            var lengthTypeId = int.Parse(binaryString.First().ToString());
            var values = new List<double>();

            if (lengthTypeId == 0)
            {
                // 15 bits indicating total length in bits of sub packets
                var bitsOfSubPackets = Convert.ToInt32(string.Join(string.Empty, binaryString.Skip(1).Take(15)), 2);
                binaryString = binaryString.Skip(16);
                var totalLength = binaryString.Count();
                while (binaryString.Count() > totalLength - bitsOfSubPackets)
                {
                    (binaryString, double value) = ParsePacket(binaryString);
                    values.Add(value);
                }
            }
            else
            {
                // 11 bits indicating number of sub-packets
                var numberOfSubPackets = Convert.ToInt32(string.Join(string.Empty, binaryString.Skip(1).Take(11)), 2);
                binaryString = binaryString.Skip(12);
                for (int subPacket = 0; subPacket < numberOfSubPackets; subPacket++)
                {
                    (binaryString, double value) = ParsePacket(binaryString);
                    values.Add(value);
                }
            }

            return (binaryString, values);
        }
    }

    public enum PacketType
    {
        Sum = 0,
        Product = 1,
        Minimum = 2,
        Maximum = 3,
        Literal = 4,
        GreaterThan = 5,
        LessThan = 6,
        EqualTo = 7,
    }
}
