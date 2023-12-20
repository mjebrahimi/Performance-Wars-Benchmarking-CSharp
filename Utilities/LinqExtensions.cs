namespace Utilities;

public static class LinqExtensions
{
    /// <summary>
    /// Splits/Chunks by condition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="removeEmptyLists">if set to <c>true</c> [remove empty lists].</param>
    /// <returns></returns>
    public static IEnumerable<List<T>> SplitBy<T>(this IEnumerable<T> source, Func<T, bool> condition, bool removeEmptyLists = true)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(condition);

        if (removeEmptyLists)
        {
            //for example calling new[] { 0, 0, 1, 2, 0, 0, 3, 4, 0, 0 }.SplitBy(p => p == 0) must returns [ [1,2], [3,4] ]
            var section = 0;
            return source.GroupBy(p =>
            {
                if (condition(p))
                    section++;
                return section;
            })
            .Select(p => p.Where(p => !condition(p)).ToList())
            .Where(p => p.Count > 0);
        }
        else
        {
            //for example calling new[] { 0, 0, 1, 2, 0, 0, 3, 4, 0, 0 }.SplitBy(p => p == 0) must returns [ [0], [0], [1,2], [0], [0], [3,4], [0], [0] ]
            var section = 0;
            var conditionMet = false;
            return source.GroupBy(p =>
            {
                if (condition(p))
                {
                    conditionMet = true;
                    section++;
                }
                else if (conditionMet)
                {
                    section++;
                    conditionMet = false;
                }
                return section;
            })
            .Select(p => p.ToList());
        }
    }
}