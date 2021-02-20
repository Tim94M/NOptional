using System;
using System.Collections.Generic;

namespace NOptional
{
    /// <summary>
    /// A type to represent a return type, which may or may not contain a result. It forces the caller to properly handle the missing value and provides methods to react to missing or present values.
    /// </summary>
    /// <typeparam name="T">The type of the underlying value.</typeparam>
    public interface IOptional<T>
    {
        /// <summary>
        /// Returns the underlying value of this <see cref="IOptional{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the optional is Empty</exception>
        T Value { get; }

        /// <summary>
        /// Returns, whether a value was set or not.
        /// </summary>
        /// <returns><see langword="true"/>, if value was set., otherwise <see langword="false"/></returns>
        bool HasValue();

        /// <summary>
        /// Inverse of <see cref="HasValue"/>. Returns, whether value was not set.
        /// </summary>
        /// <returns><see langword="true"/>, if value was not set, otherwise <see langword="false"/></returns>
        bool IsEmpty();

        /// <summary>
        /// Applies the provided <paramref name="filter"/> to the value, if there value was set.
        /// </summary>
        /// <param name="filter">The predicate to filter the value by.</param>
        /// <returns>A new filled <see cref="Optional{T}"/>, if value was set and the predicate applies to the value. Otherwise returns an empty <see cref="IOptional{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="filter"/> is null.</exception>
        IOptional<T> IfApplies(Predicate<T> filter);

        /// <summary>
        /// Applies the provided <paramref name="action"/>, if this <see cref="Optional{T}"/> has a value.
        /// </summary>
        /// <param name="action">The action the value is applied to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="action"/> is null.</exception>
        void DoIfPresent(Action<T> action);

        /// <summary>
        /// Applies the value of this if this <see cref="Optional{T}"/> to the provided <paramref name="presentAction"/>, if this <see cref="Optional{T}"/> has a value. If this <see cref="Optional{T}"/> does not have a value, the provided <paramref name="elseAction"/> is executed.
        /// </summary>
        /// <param name="presentAction">The <see cref="Action{T}"/> to execute, if a value is present.</param>
        /// <param name="elseAction">The <see cref="Action{T}"/> to execute, if a value is not present.</param>
        /// <exception cref="ArgumentNullException">If either of <paramref name="presentAction"/> or <paramref name="elseAction"/> parameters is null.</exception>
        void DoIfPresentOrElse(Action<T> presentAction, Action elseAction);

        /// <summary>
        /// Returns a new filled <see cref="IOptional{T}"/>, if there is a value. If not, the supplied <paramref name="elseGenerator"/> is used to generate a new <see cref="IOptional{T}"/>.
        /// </summary>
        /// <param name="elseGenerator">The function to generate a new <see cref="IOptional{T}"/>.</param>
        /// <returns>A new filled <see cref="IOptional{T}"/>, if there is a value. Otherwise the <see cref="IOptional{T}"/> returned by <paramref name="elseGenerator"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="elseGenerator"/> is null.</exception>
        IOptional<T> OrElse(Func<IOptional<T>> elseGenerator);

        /// <summary>
        /// Returns the underlying value, if the value is present. Otherwise returns the provided <paramref name="elseValue"/>.
        /// </summary>
        /// <param name="elseValue">The value to return, if no value is present.</param>
        /// <returns>The underlying value, if the value is present. Otherwise returns the provided <paramref name="elseValue"/>.</returns>
        /// <remarks>If null is provided for <paramref name="elseValue"/>, this method may return null!</remarks>
        T GetValueOrElse(T elseValue);

        /// <summary>
        /// Returns the underlying value, if the value is present. Otherwise returns the value provided by <paramref name="elseGenerator"/>.
        /// </summary>
        /// <param name="elseGenerator">The provider for the return value, if no value is present.</param>
        /// <returns>The underlying value, if the value is present. Otherwise returns the value provided by <paramref name="elseGenerator"/>.</returns>
        /// <remarks>If null is provided for <paramref name="elseGenerator"/>, this method may return null!</remarks>
        T GetValueOrElse(Func<T> elseGenerator);

        /// <summary>
        /// Returns the underlying value, if the value is present. Otherwise throws a <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <returns>The underlying value, if the value is present.</returns>
        /// <exception cref="InvalidOperationException">If this instance does not have a value associated with it.</exception>
        T GetValueOrElseThrow();

        /// <summary>
        /// Returns the underlying value, if the value is present. Otherwise throws a <see cref="InvalidOperationException"/> as provided by <paramref name="exceptionGenerator"/>./>.
        /// </summary>
        /// <returns>The underlying value, if the value is present.</returns>
        /// <exception cref="InvalidOperationException">If this instance does not have a value associated with it.</exception>
        T GetValueOrElseThrow(Func<Exception> exceptionGenerator);
        
        /// <summary>
        /// Maps the underlying value to a new instance of <see cref="IOptional{U}"/> according to the transformation supplied by <paramref name="mapper"/>. Otherwise returns an empty <see cref="IOptional{U}"/>. 
        /// </summary>
        /// <typeparam name="U">Result type of the transformation.</typeparam>
        /// <param name="mapper">Function to transform value to a new <see cref="IOptional{U}"/>.</param>
        /// <returns>If there is an value, an <see cref="IOptional{U}"/> according to the result of <paramref name="mapper"/>. Otherwise an empty <see cref="IOptional{U}"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="mapper"/> is null.</exception>
        IOptional<U> MapValue<U>(Func<T, U> mapper);

        /// <summary>
        /// Maps the underlying value to a new instance of <see cref="IOptional{U}"/> according to the transformation supplied by <paramref name="mapper"/>. Otherwise returns an empty <see cref="IOptional{U}"/>. 
        /// </summary>
        /// <typeparam name="U">Result type of the transformation.</typeparam>
        /// <param name="mapper">Function to transform value to a new <see cref="IOptional{U}"/>.</param>
        /// <returns>If there is an value, an <see cref="IOptional{U}"/> according to the result of <paramref name="mapper"/>. Otherwise an empty <see cref="IOptional{U}"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="mapper"/> is null.</exception>
        IOptional<U> FlatMapValue<U>(Func<T, IOptional<U>> mapper);

        /// <summary>
        /// Transforms this <see cref="IOptional{T}"/> into an <see cref="IEnumerable{T}"/> of type <typeparamref name="T"/>. This <see cref="IEnumerable{T}"/> is empty, if this <see cref="IOptional{T}"/> is empty.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of the underlying value.</returns>
        IEnumerable<T> AsEnumerable();
    }
}
