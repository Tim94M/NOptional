using System;

namespace NOptional
{
    public static class Optional
    {
        public static IOptional<T> Empty<T>() where T : class => new Optional<T>();
        public static IOptional<T> Of<T>(T value) where T : class => new Optional<T>(value);
        public static IOptional<T> OfNullable<T>(T value) where T : class => value == null ? new Optional<T>() : new Optional<T>(value);
    }

    public class Optional<T> : IOptional<T> where T : class
    {
        private readonly T value;
        public T Value
        {
            get
            {
                if (!HasValue())
                {
                    throw new InvalidOperationException("Cannot retrieve value if it is not set");
                }

                return value;
            }
        }

        internal Optional()
        {
            value = null;
        }

        internal Optional(T value)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            this.value = value;
        }

        public bool HasValue() => value != null;

        public IOptional<T> Filter(Predicate<T> filter) => HasValue() && filter(Value) ? Optional.Of(Value) : Optional.Empty<T>();

        public void IfPresent(Action<T> action)
        {
            if (HasValue())
            {
                action(Value);
            }
        }

        public T OrElse(T elseValue) => HasValue() ? Value : elseValue;
        public T OrElseGet(Func<T> elseGenerator) => HasValue() ? Value : elseGenerator();
        public T OrElseThrow(Func<Exception> exceptionGenerator) => HasValue() ? Value : throw exceptionGenerator();

        public IOptional<U> Map<U>(Func<T, U> mapper) where U : class => HasValue() ? Optional.OfNullable(mapper(Value)) : Optional.Empty<U>();

        public IOptional<U> FlatMap<U>(Func<T, IOptional<U>> mapper) where U : class => HasValue() ? mapper(Value) : Optional.Empty<U>();
    }
}
