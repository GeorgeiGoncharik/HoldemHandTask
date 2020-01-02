using System.Collections.Generic;
using System.Linq;

namespace HoldemHand.Helpers
{
    internal static class Permutations
    {
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            var enumerable = items.ToList();
            foreach (var item in enumerable)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(enumerable.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }
    }
}