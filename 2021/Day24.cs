using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    /// <summary>
    //inp w			w = input digit
    //mul x 0			x = 0
    //add x z x = z
    //mod x 26		x = z % 26
    //div z 1			z = z
    //add x 12		x = (z % 26) + 12
    //eql x w			if x == w then x = 1 else x = 0
    //eql x 0			if x != w then x = 1 else x = 0						=> always: x = 1
    //mul y 0			y = 0
    //add y 25		y = 25
    //mul y x y = x * 25(either 25 or 0)
    //add y 1			y = y + 1  (either 26 or 1)
    //mul z y z *= y(either z*26 or z*1) => always: z = z* 26
    //mul y 0			y = 0
    //add y w y = input
    //add y 1			y = input + 1
    //mul y x y = (input + 1) * 1
    //add z y z = z + input + 1                                   => end result: z = (z* 26) + (input + 1)

    //inp w
    //mul x 0			x = 0
    //add x z x = z
    //mod x 26		x = z % 26
    //div z 1			z = z
    //add x 12		x = (z % 26) + 12
    //eql x w
    //eql x 0			x = 1
    //mul y 0			y = 0
    //add y 25		y = 25
    //mul y x y = 25
    //add y 1			y = 26
    //mul z y z = z * 26
    //mul y 0			y = 0
    //add y w y = input
    //add y 1			y = input + 1
    //mul y x y = input + 1
    //add z y z = (z * 26) + (input + 1)                          => end result: z = (z* 26) + (input + 1)

    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 1
    //add x 15
    //eql x w
    //eql x 0			x = 1
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y z = z * 26
    //mul y 0
    //add y w
    //add y 16		y = input + 16
    //mul y x
    //add z y         z = (z* 26) + (input + 16)							=> end result: z = (z* 26) + (input + 16)

    //inp w
    //mul x 0			x = 0
    //add x z x = z
    //mod x 26		x = z % 26
    //div z 26		z = z / 26
    //add x -8		x = (z % 26) + -8
    //eql x w
    //eql x 0			if x != w then x = 1 else x = 0
    //mul y 0			y = 0
    //add y 25		y = 25
    //mul y x y = x * 25(either 25 or 0)
    //add y 1			y = (x* 25) + 1 (either 26 or 1)
    //mul z y z = z * y(either z * 26 or z)
    //mul y 0			y = 0
    //add y w y = input
    //add y 5			y = input + 5
    //mul y x y = x * (input + 5)(either(input + 5) or 0)
    //add z y z = z + y

    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 26
    //add x -4
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 9
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 1
    //add x 15
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 3
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 1
    //add x 14
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 2
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 1
    //add x 14
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 15
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 26
    //add x -13
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 5
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 26
    //add x -3
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 11
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 26
    //add x -7
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 7
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 1
    //add x 10
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 1
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 26
    //add x -6
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 10
    //mul y x
    //add z y
    //inp w
    //mul x 0
    //add x z
    //mod x 26
    //div z 26
    //add x -8
    //eql x w
    //eql x 0
    //mul y 0
    //add y 25
    //mul y x
    //add y 1
    //mul z y
    //mul y 0
    //add y w
    //add y 3
    //mul y x
    //add z y
    /// </summary>
    public class Day24 : Day<double>
    {
        public Day24() : base(2021, "day24") { }

        /// <summary>
        /// 2 kinds of blocks of instructions (see partly worked out example in the comments above):
        ///     - end result always: z = (z* 26) + (intput + constant) where the constant comes from instruction 16
        ///     - the last (intput + constant) is being extracted into x with mod (instruction 4)
        ///       instruction z becomes z / 26 (instruction 5)
        ///       Combined with the end result of the first type of block just means that z is being used a stack (example first 4 blocks):
        ///       
        ///       1: z = (0 * 26) + (input1 + 1) = input1 + 1;
        ///       2: z = ((input + 1) * 26) + (input2 + 1) = (input1 * 26) + (input2 + 1)
        ///       3: z = (((input1 * 26) + (input2 + 1)) * 26) + (input3 + 16)
        ///       4: x = z % 26  => x = (input3 + 16)
        ///          z = z / 26  => z = (input1 * 26) + (input2 + 1)
        ///    
        ///       Next, it checks if the popped value + constant == current input(instructions 6-8)
        ///       if true, do nothing
        ///       if not true, push another value into z
        ///       
        /// This means that if we want z to have value 0 (MONAD is valid), we need the comparison of type 2 chunk to fail: we push and pop
        /// exactly 7 times to z resulting in 0. So for part 1 we need all 7 comparisons to fail, i.e. the difference between the previous
        /// value on the stack + constants == the current digit: previous + A + B = current => D(ifference) = A + B.
        /// So for the maximum value, one of the 2 being compared is 9 means the other one is 9 - D (or 9 + D in the case of negative value).
        /// We can queue all these comparisons (the values A and B are in the instructions) and evaluate them all starting with a 9.
        /// </summary>
        /// <returns></returns>
        public override double PartOne()
        {
            var monad = Enumerable.Repeat(0, 14).ToArray();
            foreach (var comparison in GetComparisons(InputLines.ToList()))
            {
                if (comparison.difference > 0)
                {
                    monad[comparison.monadDigit1] = 9;
                    monad[comparison.monadDigit2] = 9 - comparison.difference;
                }
                else
                {
                    monad[comparison.monadDigit1] = 9 + comparison.difference;
                    monad[comparison.monadDigit2] = 9;
                }
            }
            return monad.Aggregate(0.0, (acc, monadDigit) => (acc * 10) + monadDigit);
        }

        /// <summary>
        /// Analogous to part 1, just use 1 instead of 9 (minimum instead of maximum)
        /// </summary>
        /// <returns></returns>
        public override double PartTwo()
        {
            var monad = Enumerable.Repeat(0, 14).ToArray();
            foreach (var comparison in GetComparisons(InputLines.ToList()))
            {
                if (comparison.difference > 0)
                {
                    monad[comparison.monadDigit1] = 1 + comparison.difference;
                    monad[comparison.monadDigit2] = 1;
                }
                else
                {
                    monad[comparison.monadDigit1] = 1;
                    monad[comparison.monadDigit2] = 1 - comparison.difference;
                }
            }
            return monad.Aggregate(0.0, (acc, monadDigit) => (acc * 10) + monadDigit);
        }

        private IEnumerable<(int monadDigit1, int monadDigit2, int difference)> GetComparisons(IEnumerable<string> input)
        {
            var zStack = new Stack<(int monadDigit, int z)>();

            foreach (var monadDigit in Enumerable.Range(0, 14))
            {
                input = input.Skip(4);
                var instruction = input.First();

                // type 1
                if (instruction == "div z 1")
                {
                    instruction = input.Skip(11).First();
                    zStack.Push((monadDigit, int.Parse(instruction.Split().Last())));
                    input = input.Skip(14);
                }
                // type 2
                else
                {
                    instruction = input.Skip(1).First();
                    var b = int.Parse(instruction.Split().Last());
                    (var zMonadDigit, var a) = zStack.Pop();
                    yield return (monadDigit, zMonadDigit, a + b);
                    input = input.Skip(14);
                }
            }
        }
    }
}
