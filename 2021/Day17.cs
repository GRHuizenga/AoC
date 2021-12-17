using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2021
{
    public class Day17 : Day<int>
    {
        private int MinTargetX;
        private int MaxTargetX;
        private int MinTargetY;
        private int MaxTargetY;

        public Day17() : base(2021, "day17") {
            var targets = Regex.Matches(Input.First(), @"(-?\d+)");
            MinTargetX = int.Parse(targets.ElementAt(0).Value);
            MaxTargetX = int.Parse(targets.ElementAt(1).Value);
            MinTargetY = int.Parse(targets.ElementAt(2).Value);
            MaxTargetY = int.Parse(targets.ElementAt(3).Value);
        }

        public override int PartOne()
        {
            // max. velocity in y when projectile falls straight through y = 0 and ends in min. y of the box
            // so max. y velocity (for positive displacement) = abs(targetY) - 1
            // to reach this velocity when velocity decreases by 1 every step of the way means just sum 1 to max. y velocity: V*(V+1) / 2
            var maxYDisplacement = Math.Abs(MinTargetY) - 1;
            var maxY = (maxYDisplacement * (maxYDisplacement + 1)) / 2;
            return maxY;
        }

        public override int PartTwo()
        {
            // max. y velocity (see part one)
            var maxYVelocity = MinTargetY * -1;
            // min. x velocity using euler => vX(vX + 1) / 2 = MinTargetX => vX^2 + vX - 188 => ABC-formula
            var minXVelocity = (int)Math.Floor((-1 + Math.Sqrt(1 - (4 * -2 * MinTargetX))) / 2);
            // max. x velocity is furthest x coordinate of the target box
            var maxXVelocity = MaxTargetX;

            //target area: x=20..30, y=-10..-5
            var withinTargetArea = 0;
            for (int vX = minXVelocity; vX <= maxXVelocity; vX++)
            {
                for (int vY = maxYVelocity; vY >= -maxYVelocity; vY--)
                {
                    var xVelocity = vX;
                    var yVelocity = vY;
                    var x = 0;
                    var y = 0;
                    while (x <= MaxTargetX && y >= MinTargetY)
                    {
                        x += xVelocity;
                        y += yVelocity;

                        if (x >= MinTargetX && x <= MaxTargetX && y >= MinTargetY && y <= MaxTargetY)
                        {
                            withinTargetArea++;
                            break;
                        }
                        
                        xVelocity -= xVelocity == 0 ? 0 : 1;
                        yVelocity--;
                    }
                }
            }

            return withinTargetArea;
        }
    }
}
