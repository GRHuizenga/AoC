using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day02 : Day<int>
    {
        private IEnumerable<Command> Commands;

        public Day02() : base(2021, "day02")
        {
            Commands = Input.Select(line =>
            {
                var x = line.Split(' ');
                return new Command(x[0], int.Parse(x[1]));
            });
        }

        public override int PartOne()
        {
            var x = Commands
                .Aggregate(new Position(), (acc, curr) =>
                {
                    switch (curr.Cmd)
                    {
                        case "forward":
                            acc.Horizontal += curr.Units;
                            break;
                        case "up":
                            acc.Depth -= curr.Units;
                            break;
                        case "down":
                            acc.Depth += curr.Units;
                            break;
                        default:
                            break;
                    }
                    return acc;
                });
            return x.Depth * x.Horizontal;
        }

        public override int PartTwo()
        {
            var x = Commands
                .Aggregate(new Position(), (acc, curr) =>
                {
                    switch (curr.Cmd)
                    {
                        case "forward":
                            acc.Horizontal += curr.Units;
                            acc.Depth += curr.Units * acc.Aim;
                            break;
                        case "up":
                            acc.Aim -= curr.Units;
                            break;
                        case "down":
                            acc.Aim += curr.Units;
                            break;
                        default:
                            break;
                    }
                    return acc;
                });
            return x.Depth * x.Horizontal;
        }
    }

    public class Position
    {
        public int Horizontal { get; set; }
        public int Depth { get; set; }
        public int Aim { get; set; }

        public Position()
        {
            Horizontal = 0;
            Depth = 0;
            Aim = 0;
        }
    }
    
    public class Command
    {
        public string Cmd { get; private set; }
        public int Units { get; private set; }

        public Command(string command, int units)
        {
            Cmd = command;
            Units = units;
        }
    }
}
