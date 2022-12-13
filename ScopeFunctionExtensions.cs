using System;

namespace Object.Extensions.ScopeFunction
{
    public static class ScopeFunctionExtensions
    {
        public static TResp Map<TTarget, TResp>(
            this TTarget target, 
            Func<TTarget, TResp> block)
        {
            return block(target);
        }

        public static T Apply<T>(
            this T target, 
            Action<T> block)
        {
            block(target);
            return target;
        }
    }
}
