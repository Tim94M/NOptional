using System;

namespace NOptional
{
    public static class Optional
    {
        public static IOptional<T> Empty<T>() => new Optional<T>();
        public static IOptional<T> Of<T>(T value) => new Optional<T>(value);
        public static IOptional<T> OfNullable<T>(T value) => value == null ? new Optional<T>() : new Optional<T>(value);
    }

    public class Optional<T> : IOptional<T>
    {
        private readonly bool hasValue;
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
            hasValue = false;
        }

        internal Optional(T value)
        {
            if(value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.value = value;
            hasValue = true;
        }

        public bool HasValue() => hasValue;
        public bool IsEmpty() => !HasValue();

        public IOptional<T> Filter(Predicate<T> filter) => HasValue() && filter(Value) ? Optional.Of(Value) : Optional.Empty<T>();

        public void IfPresent(Action<T> action)
        {
            if (HasValue())
            {
                action(Value);
            }
        }
        public void IfPresentOrElse(Action<T> presentAction, Action elseAction)
        {
            if (HasValue())
            {
                presentAction(Value);
            }
            else
            {
                elseAction();
            }
        }

        public T OrElse(T elseValue) => HasValue() ? Value : elseValue;
        public T OrElseGet(Func<T> elseGenerator) => HasValue() ? Value : elseGenerator();
        public T OrElseThrow(Func<Exception> exceptionGenerator) => HasValue() ? Value : throw exceptionGenerator();

        public IOptional<U> Map<U>(Func<T, U> mapper) => HasValue() ? Optional.OfNullable(mapper(Value)) : Optional.Empty<U>();
        public IOptional<U> FlatMap<U>(Func<T, IOptional<U>> mapper) => HasValue() ? mapper(Value) : Optional.Empty<U>();


    }
}
