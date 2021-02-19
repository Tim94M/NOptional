﻿using System;

namespace NOptional
{
    public interface IOptional<T>
    {
        T Value { get; }
        bool HasValue();
        bool IsEmpty();
        
        IOptional<T> Filter(Predicate<T> filter);

        void IfPresent(Action<T> action);
        void IfPresentOrElse(Action<T> presentAction, Action elseAction);
        
        IOptional<T> Or(Func<IOptional<T>> elseGenerator);
        T OrElse(T elseValue);
        T OrElseGet(Func<T> elseGenerator);
        T OrElseThrow();
        T OrElseThrow(Func<Exception> p);
        
        IOptional<U> Map<U>(Func<T, U> mapper);
        IOptional<U> FlatMap<U>(Func<T, IOptional<U>> mapper);
    }
}
