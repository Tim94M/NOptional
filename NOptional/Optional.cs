﻿using System;
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

    internal struct Optional<T> : IOptional<T>
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

        public IOptional<T> IfApplies(Predicate<T> filter)
        {
            CheckNullOrThrowException(filter);

            return HasValue() && filter(Value) ? Optional.Of(Value) : Optional.Empty<T>();
        }

        public void DoIfPresent(Action<T> action)
        {
            CheckNullOrThrowException(action);

            if (HasValue())
            {
                action(Value);
            }
        }

        public void DoIfPresentOrElse(Action<T> presentAction, Action elseAction)
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

        public IOptional<T> GetValueOrElse(Func<IOptional<T>> elseGenerator)
        {
            CheckNullOrThrowException(elseGenerator);

            return HasValue() ? Optional.Of(Value) : elseGenerator();
        }

        public T GetValueOrElse(T elseValue) => HasValue() ? Value : elseValue;

        public T GetValueOrElse(Func<T> elseGenerator)
        {
            CheckNullOrThrowException(elseGenerator);

            return HasValue() ? Value : elseGenerator();
        }

        public T GetValueOrElseThrow() => HasValue() ? Value : throw new InvalidOperationException("Could not retrieve value because value was not set");
        public T GetValueOrElseThrow(Func<Exception> exceptionGenerator)
        {
            CheckNullOrThrowException(exceptionGenerator);

            return HasValue() ? Value : throw exceptionGenerator();
        }

        public IOptional<U> MapValue<U>(Func<T, U> mapper)
        {
            CheckNullOrThrowException(mapper);

            return HasValue() ? Optional.OfNullable(mapper(Value)) : Optional.Empty<U>();
        }

        public IOptional<U> FlatMapValue<U>(Func<T, IOptional<U>> mapper)
        {
            CheckNullOrThrowException(mapper);

            return HasValue() ? mapper(Value) : Optional.Empty<U>();
        }

        public IEnumerable<T> AsEnumerable()
        {
            if (HasValue())
            {
                yield return Value;
            }
        }

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

            // since Optional is a struct, ReferenceEquals would always result in inequality, 
            // even if we would compare with self, because the struct is boxed into an new object 
            // before performing the actual reference check.
            // see: https://docs.microsoft.com/en-us/dotnet/api/system.object.referenceequals?view=net-5.0#remarks
            return !HasValue() && !other.HasValue();
        }

        public override int GetHashCode()
        {
            if (HasValue())
            {
                return Value.GetHashCode();
            }

            // this effectively means, that in a hash-based collection, there can always only 
            // be one empty Optional of the specified type
            return hasValue.GetHashCode();
        }
    }
}
