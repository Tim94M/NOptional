using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        public IOptional<T> Filter(Predicate<T> filter)
        {
            CheckNullOrThrowException(filter);

            return HasValue() && filter(Value) ? Optional.Of(Value) : Optional.Empty<T>();
        }

        public void IfPresent(Action<T> action)
        {
            CheckNullOrThrowException(action);

            if (HasValue())
            {
                action(Value);
            }
        }

        public void IfPresentOrElse(Action<T> presentAction, Action elseAction)
        {
            CheckNullOrThrowException(presentAction);
            CheckNullOrThrowException(elseAction);

            if (HasValue())
            {
                presentAction(Value);
            }
            else
            {
                elseAction();
            }
        }

        public IOptional<T> Or(Func<IOptional<T>> elseGenerator)
        {
            CheckNullOrThrowException(elseGenerator);

            return HasValue() ? Optional.Of(Value) : elseGenerator();
        }

        public T OrElse(T elseValue) => HasValue() ? Value : elseValue;

        public T OrElseGet(Func<T> elseGenerator)
        {
            CheckNullOrThrowException(elseGenerator);

            return HasValue() ? Value : elseGenerator();
        }

        public T OrElseThrow() => HasValue() ? Value : throw new InvalidOperationException("Could not retrieve value because value was not set");
        public T OrElseThrow(Func<Exception> exceptionGenerator)
        {
            CheckNullOrThrowException(exceptionGenerator);

            return HasValue() ? Value : throw exceptionGenerator();
        }

        public IOptional<U> Map<U>(Func<T, U> mapper) => HasValue() ? Optional.OfNullable(mapper(Value)) : Optional.Empty<U>();
        public IOptional<U> FlatMap<U>(Func<T, IOptional<U>> mapper) => HasValue() ? mapper(Value) : Optional.Empty<U>();

        public IEnumerator<T> GetEnumerator()
        {
            if (HasValue())
            {
                yield return Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void CheckNullOrThrowException(object toCheck, [CallerMemberName] string parameterName = "") 
            => _ = toCheck ?? throw new ArgumentNullException(parameterName);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((Optional<T>)obj);
        }

        public bool Equals(Optional<T> other)
        {
            if(HasValue() && other.HasValue())
            {
                return Value.Equals(other.Value);
            }

            if(!HasValue() && !other.HasValue())
            {
                return ReferenceEquals(this, other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (HasValue())
            {
                return Value.GetHashCode();
            }
            
            return base.GetHashCode();
        }
    }
}
