using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;

public static class CollectionExtensions
{
    public static IEnumerable<T> GetAll<T>(this DataServiceQuery<T> query)
    {
        int count = 0;
        int formerCount;
        do
        {
            formerCount = count;
            var currentExecution = (query.Skip(count) as DataServiceQuery<T>).Execute();
            foreach(var entry in currentExecution)
            {
                count++;
                yield return entry;
            }
        }
        while (count > formerCount);
    }


    public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childSelector)
    {
        // Do standard error checking here.

        // Create a stack for recursion.  Push all of the items
        // onto the stack.
        var stack = new Stack<T>(source);

        // While there are items on the stack.
        while (stack.Count > 0)
        {
            // Pop the item.
            T item = stack.Pop();

            // Yield the item.
            yield return item;

            // Push all of the children on the stack.
            foreach (T child in childSelector(item)) stack.Push(child);
        }
    }


    public static bool In<T>(this T source, params T[] list)
    {
        if (null == source) throw new ArgumentNullException("source");
        return list.Contains(source);
    }

}