using System;

namespace NOptional
{
    public interface IOptional<T> where T : class
    {
        bool HasValue();
        T Value{ get; }
        IOptional<T> Filter(Predicate<T> filter);
        void IfPresent(Action<T> action);
        T OrElse(T elseValue);
        T OrElseGet(Func<T> elseGenerator);
        T OrElseThrow(Func<Exception> p);
        IOptional<U> Map<U>(Func<T, U> mapper) where U : class;
        IOptional<U> FlatMap<U>(Func<T, IOptional<U>> mapper) where U : class;
    }
}
