using System;
using System.Collections.Generic;

namespace Object.Extensions.ScopeFunction
{
    public static class ScopeFunctionExtensions
    {
        /// <summary>
        /// Applies function to a given object and returns original object
        /// </summary>
        public static T Apply<T>(
            this T target, 
            Action<T> fn)
        {
            fn(target);
            return target;
        }

        /// <summary>
        /// Applies function to each item in ICollection and returns original ICollection
        /// If you are working with IEnumerable - use ApplyForEach
        /// </summary>
        public static TCollection ApplyForEachEager<TCollection, T>(
            this TCollection target,
            Action<T> fn) where TCollection: ICollection<T>
        {
            foreach (var item in target)
            {
                fn(item);
            }
            return target;
        }

        /// <summary>
        /// Applies function to each item in IEnumerable and yields the items
        /// </summary>
        public static IEnumerable<T> ApplyForEach<T>(
            this IEnumerable<T> target,
            Action<T> fn)
        {
            foreach (var item in target)
            {
                fn(item);
                yield return item;
            }
        }

        /// <summary>
        /// Applies function to a given object and returns that functions returns that function return.
        /// Analog of |> (pipe-forward) operator in F#.
        /// </summary>
        public static TResp Map<TTarget, TResp>(
            this TTarget target, 
            Func<TTarget, TResp> fn)
        {
            return fn(target);
        }
    }
}
