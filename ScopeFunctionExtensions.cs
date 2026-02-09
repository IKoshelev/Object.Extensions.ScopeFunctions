using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// Applies function to a given object and returns original object
        /// </summary>
        public static async Task<T> ApplyAsync<T>(
            this T target,
            Func<T, Task> fn)
        {
            await fn(target);
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
        /// Applies function to each item in ICollection and returns original ICollection
        /// If you are working with IEnumerable - use ApplyForEach
        /// </summary>
        public static async Task<TCollection> ApplyForEachEagerAsync<TCollection, T>(
            this TCollection target,
            Func<T, Task> fn) where TCollection : ICollection<T>
        {
            foreach (var item in target)
            {
                await fn(item);
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

#if NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Applies function to each item in IEnumerable and yields the items
        /// </summary>
        public static async IAsyncEnumerable<T> ApplyForEachAsync<T>(
            this IAsyncEnumerable<T> target,
            Func<T, Task> fn)
        {
            await foreach (var item in target)
            {
                await fn(item);
                yield return item;
            }
        }

        /// <summary>
        /// Applies function to each item in IEnumerable and yields the items
        /// </summary>
        public static async IAsyncEnumerable<T> ApplyForEachAsync<T>(
            this IEnumerable<T> target,
            Func<T, Task> fn)
        {
            foreach (var item in target)
            {
                await fn(item);
                yield return item;
            }
        }
#endif

        //public static async void test()
        //{
        //    #if NETSTANDARD2_1_OR_GREATER
        //    IAsyncEnumerable<int> a = null;
        //    await a.ApplyForEachAsync(async x => {});
        //    #endif
        //}

        /// <summary>
        /// Applies function to a given object and returns that functions return.
        /// Analog of |> (pipe-forward) operator in F#.
        /// <example>
        /// <code>
        /// <para/> int? userId = 5;
        /// <para/> var user = userId?.Map(i => cache[i]);
        /// </code>
        /// </example>
        /// </summary>
        public static TResp Map<TTarget, TResp>(
            this TTarget target, 
            Func<TTarget, TResp> fn)
        {
            return fn(target);
        }

        /// <summary>
        /// Applies function to a given object and returns that functions return.
        /// Analog of |> (pipe-forward) operator in F#.
        /// <example>
        /// <code>
        /// <para/> int? userId = 5;
        /// <para/> var user = await userId?.Map(i => userRepo.GetUser(i));
        /// </code>
        /// </example>
        /// </summary>
        public static async Task<TResp> MapAsync<TTarget, TResp>(
            this TTarget target,
            Func<TTarget, Task<TResp>> fn)
        {
            return await fn(target);
        }

        /// <summary>
        /// Applies function to a given object and returns that functions return.
        /// If an exception is thrown - calls onException where you can either recover
        /// by returtning a default TTarget or provide better exception by throwing a different exception.
        /// <example>
        /// <code>
        /// <para/> ruleCache.WrapException(c => c.getActiveRuleset(userId),
        /// <para/>                        (c,ex) => config.defaultRuleset ?? throw new RulesetNotAvailableException(userId, ex))
        /// <para/>          .Where(rule => .....)
        /// </code>
        /// </example>
        /// </summary>
        public static TResp WrapException<TTarget, TResp>(
            this TTarget target,
            Func<TTarget, TResp> fn,
            Func<TTarget, Exception, TResp> onException)
        {
            try
            {
                return fn(target);
            }
            catch (Exception ex)
            {
                return onException(target, ex);
            }
        }

        /// <summary>
        /// Applies function to a given object and returns that functions return.
        /// If an exception is thrown - calls onException where you can either recover
        /// by returtning a default TTarget or provide better exception by throwing a different exception.
        /// <example>
        /// <code>
        /// <para/> await ruleRepository.WrapException(r => r.getActiveRuleset(userId),
        /// <para/>                                   async (r,ex) => await r.GetDefaultRuleSet() ?? throw new RulesetNotAvailableException(userId, ex))          
        /// </code>
        /// </example>
        /// </summary>
        public static async Task<TResp> WrapExceptionAsync<TTarget, TResp>(
            this TTarget target,
            Func<TTarget, Task<TResp>> fn,
            Func<TTarget, Exception, Task<TResp>> onException)
        {
            try
            {
                return await fn(target);
            }
            catch (Exception ex)
            {
                return await onException(target, ex);
            }
        }
    }
}
