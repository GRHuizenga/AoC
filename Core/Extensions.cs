using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class Extensions
    {
        /// <summary>
        /// Transposes a matrix (rows become columns)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            var enumerators = @this.Select(t => t.GetEnumerator()).Where(e => e.MoveNext());
            while (enumerators.Any())
            {
                yield return enumerators.Select(e => e.Current);
                enumerators = enumerators.Where(e => e.MoveNext());
            }
        }

        /// <summary>
        /// Groups items in an Enumerable in groups of size groupSize turning the Enumerable in an
        /// Enumerable of Enumerables.
        /// 
        /// Example: int[8] with groupSize 2 becomes int[4][2]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="groupSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Group<T>(this IEnumerable<T> @this, int groupSize)
        {
            var copy = @this.Select(t => t);
            while (copy.Any())
            {
                yield return copy.Take(groupSize);
                copy = copy.Skip(groupSize);
            }
        }

        /// <summary>
        /// Finds all substrings of length 'length' that comply with pattern.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<string> FindPattern(this string input, Func<string, bool> pattern, int length)
        {
            foreach (var candidate in Enumerable.Range(0, input.Length + 1 - length).Select(index => input.Substring(index, length)))
            {
                if (pattern(candidate)) yield return candidate;
            }
        }

        /// <summary>
        /// Uses a sliding window of size windowSize to return a stream of groups.
        /// 
        /// Example: [1, 2, 3, 4, 5, 6] with windowSize = 3 => [[1, 2, 3], [2, 3, 4], [3, 4, 5], [4, 5, 6]]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> @this, int windowSize)
        {
            foreach (var index in Enumerable.Range(0, @this.Count() - (windowSize - 1)))
            {
                yield return @this.Skip(index).Take(windowSize);
            };
        }

        /// <summary>
        /// Returns all unique pairs in an enumerable.
        /// 
        /// Example:
        /// [1, 2, 3, 4] => [(1, 2), (1, 3), (1, 4), (2, 3), (2, 4), (3, 4)]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<(T, T)> Permutations<T>(this IEnumerable<T> @this)
        {
            var arr = @this.ToArray();
            return Enumerable.Range(0, @this.Count()).SelectMany(index => arr.Skip(index + 1).Select(entry => (arr[index], entry)));
        }
    }
}
