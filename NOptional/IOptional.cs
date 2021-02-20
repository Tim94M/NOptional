using System;
using System.Collections.Generic;

namespace NOptional
{
    public interface IOptional<T>
    {
        T Value { get; }
        bool HasValue();
        bool IsEmpty();
        
        IOptional<T> IfApplies(Predicate<T> filter);

        void DoIfPresent(Action<T> action);
        void DoIfPresentOrElse(Action<T> presentAction, Action elseAction);
        
        IOptional<T> GetValueOrElse(Func<IOptional<T>> elseGenerator);
        T GetValueOrElse(T elseValue);
        T GetValueOrElse(Func<T> elseGenerator);
        T GetValueOrElseThrow();
        T GetValueOrElseThrow(Func<Exception> p);
        
        IOptional<U> MapValue<U>(Func<T, U> mapper);
        IOptional<U> FlatMapValue<U>(Func<T, IOptional<U>> mapper);

        IEnumerable<T> AsEnumerable();
    }
}
