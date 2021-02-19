using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace NOptional
{
    [TestClass]
    public class OptionalTests
    {
        private const string TestString = "myString";

        [TestMethod]
        public void CreateNewEmptyOptional()
        {
            var emptyOptional = Optional.Empty<string>();
            
            Assert.IsFalse(emptyOptional.HasValue());
        }

        [TestMethod]
        public void CreateNewFilledOptional()
        {
            var filledOptional = Optional.Of(TestString);
            
            Assert.IsTrue(filledOptional.HasValue());
            Assert.IsFalse(filledOptional.IsEmpty());
        }

        [TestMethod]
        public void GivenNullValueThenEmptyOptionalIsCreated()
        {
            var emptyOptional = Optional.OfNullable<string>(null);

            Assert.IsFalse(emptyOptional.HasValue());
            Assert.IsTrue(emptyOptional.IsEmpty());
        }

        [TestMethod]
        public void GivenValidValueThenFilledOptionalIsCreated()
        {
            var filledOptional = Optional.OfNullable(TestString);

            Assert.IsTrue(filledOptional.HasValue());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenGetValueGetValueAndOriginalValueAreEqual()
        {
            var optional = Optional.Of(TestString);
            var gotValue = optional.Value;
            
            Assert.AreEqual(TestString, gotValue);
        }

        [TestMethod]
        public void GivenFilledOptionalWhenFilteringWithTrueFilterFilledObjectIsReturend()
        {
            var optional = Optional.Of(TestString);
            var filtered = optional.Filter(s => !string.IsNullOrWhiteSpace(s));

            Assert.IsTrue(filtered.HasValue());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenFilteringWithFalseFilterEmptyObjectIsReturend()
        {
            var optional = Optional.Of(TestString);
            var filtered = optional.Filter(s => string.IsNullOrWhiteSpace(s));

            Assert.IsFalse(filtered.HasValue());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullPredicateOnFilterThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.Filter(null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullPredicateOnFilterThenEmptyOptionalIsReturned()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.Filter(null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionOnIsPresentThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.IfPresent(null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionOnIsPresentThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.IfPresent(null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionToEitherParameterOnIsPresentOrElseThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.IfPresentOrElse(null, null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionToEitherParameterOnIsPresentOrElseThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.IfPresentOrElse(null, null));
        }

        [TestMethod]
        public void GivenEmptyOptionalThenThrowsInvalidOperationExceptionOnGet()
        {
            var optional = Optional.Empty<string>();
            
            Assert.ThrowsException<InvalidOperationException>(() => optional.Value);
        }

        [TestMethod]
        public void GivenEmptyOptionalThenDoesNothingOnPreset()
        {
            var optional = Optional.Empty<string>();
            optional.IfPresent(s => Assert.Fail());

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GivenFilledOptionalThenExecutesAction()
        {
            var optional = Optional.Of(TestString);
            var called = false;
            optional.IfPresent(s => called = true);

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void GivenEmptyOptionalThenSuppliedValueIsUsed()
        {
            var optional = Optional.Empty<string>();
            var value = optional.OrElse(TestString);

            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void GivenFilledOptionalThenSuppliedValueIsUnused()
        {
            var optional = Optional.Of(TestString);
            var value = optional.OrElse("other");

            Assert.AreNotEqual("other", value);
        }

        [TestMethod]
        public void GivenEmptyOptionalThenFunctionCreatesValue()
        {
            var optional = Optional.Empty<string>();
            var value = optional.OrElseGet(() => TestString);

            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void GivenFilledOptionalThenFunctionIsNotCalled()
        {
            var optional = Optional.Of(TestString);
            var value = optional.OrElseGet(() =>
            {
                Assert.Fail();
                return string.Empty;
            });

            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void GivenEmptyOptionalThenThrowsExceptionWhenCallingOrElseThrow()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<Exception>(() => optional.OrElseThrow(() => new Exception()));
        }

        [TestMethod]
        public void GivenFilledOptionalThenReturnsValue()
        {
            var optional = Optional.Of(TestString);
            var value = optional.OrElseThrow(() =>
            {
                Assert.Fail();
                return new Exception();
            });

            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenMappingThenEmptyOptionalIsReturned()
        {
            var optional = Optional.Empty<string>();
            var enumeratorOptional = optional.Map(s =>
            {
                Assert.Fail();
                return s.GetEnumerator();
            });

            Assert.IsFalse(enumeratorOptional.HasValue());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenMappingThenMappedOptionalIsReturned()
        {
            var optional = Optional.Of(TestString);
            var enumeratorOptional = optional.Map(s => s.GetEnumerator());

            Assert.IsTrue(enumeratorOptional.HasValue());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenFlatMappingThenEmptyOptionalIsReturned()
        {
            var optional = Optional.Empty<string>();
            var enumeratorOptional = optional.FlatMap(s =>
            {
                Assert.Fail();
                return Optional.Of(s.GetEnumerator());
            });

            Assert.IsFalse(enumeratorOptional.HasValue());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenFlatMappingThenMappedOptionalIsReturned()
        {
            var optional = Optional.Of(TestString);
            var enumeratorOptional = optional.FlatMap(s => Optional.Of(s.GetEnumerator()));

            Assert.IsTrue(enumeratorOptional.HasValue());
        }

        [TestMethod]
        public void GivenCreatedWithIntThenHasValue()
        {
            var optional = Optional.Of(5);
            
            Assert.IsTrue(optional.HasValue());
        }

        [TestMethod]
        public void GivenCreatedWithNullInterfaceImplementationThenNotHasValue()
        {
            Assert.ThrowsException<ArgumentNullException>( () => Optional.Of<ITestInterface>(null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenCallingIfPresentOrElseElseActionIsExecuted()
        {
            var optional = Optional.Empty<string>();
            
            optional.IfPresentOrElse(s => Assert.Fail(), () => Assert.IsTrue(true));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCallingIfPresentOrElseThenPresentActionIsExecuted()
        {
            var optional = Optional.Of(TestString);

            optional.IfPresentOrElse(s => Assert.IsTrue(true), () => Assert.Fail());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenCallingOrThenOrIsExecuted()
        {
            var optional = Optional.Empty<string>();
            var value = optional.Or(() => Optional.Of(TestString));

            Assert.IsTrue(value.Filter(s => s.Equals(TestString)).HasValue());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCallingOrThenOrIsExecuted()
        {
            var optional = Optional.Of(TestString);
            var value = optional.Or(() =>
            {
                Assert.Fail();
                return Optional.Empty<string>();
            });

            Assert.IsTrue(value.Filter(s => s.Equals(TestString)).HasValue());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenCallingOrElseThrowThenInvalidOperationExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();
            
            Assert.ThrowsException<InvalidOperationException>(() => optional.OrElseThrow());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCallingOrElseThrowThenValuesAreEqual()
        {
            var optional = Optional.Of(TestString);
            var value = optional.OrElseThrow();

            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenCallingGetEnumerableThenContainsNoElements()
        {
            Assert.IsFalse(Optional.Empty<string>().Any());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCallingGetEnumerableThenContainsOneElement()
        {
            Assert.IsTrue(Optional.Of(TestString).Any());
        }

        [TestMethod]
        public void GivenTwoFilledOptionalsWhenBothUseSameValueThenTheyAreEqual()
        {
            var optional = Optional.Of(TestString);
            var optional2 = Optional.Of(TestString);
            
            Assert.IsTrue(optional.Equals(optional2));
            Assert.IsTrue(optional2.Equals(optional));
        }

        [TestMethod]
        public void GivenOneFilledAndOneEmptyOptionalWhenComparingThenTheyAreNotEqual()
        {
            var optional = Optional.Of(TestString);
            var optional2 = Optional.Empty<string>();
            
            Assert.IsFalse(optional.Equals(optional2));
            Assert.IsFalse(optional2.Equals(optional));
        }

        [TestMethod]
        public void GivenTwoEmptyOptionalsWhenComparingThenTheyAreNotEqual()
        {
            var optional = Optional.Empty<string>();
            var optional2 = Optional.Empty<string>();

            Assert.IsFalse(optional.Equals(optional2));
            Assert.IsFalse(optional2.Equals(optional));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenComparingWithSelfThenTheyAreEqual()
        {
            var optional = Optional.Empty<string>();

            Assert.IsTrue(optional.Equals(optional));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCalculatingHashThenHashIsEqualToOriginalvalue()
        {
            var optional = Optional.Of(TestString);
            
            Assert.AreEqual(TestString.GetHashCode(), optional.GetHashCode());
        }

        [TestMethod]
        public void GivenTwoEmptyOptionalWhenCalculatingHashThenHashIsDifferent()
        {
            var optional = Optional.Empty<string>();
            var optional2 = Optional.Empty<string>();

            Assert.IsTrue(optional.GetHashCode() != optional2.GetHashCode());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenComparedWithNullThenTheyAreNotEqual()
        {
            var optional = Optional.Empty<string>();

            Assert.IsFalse(optional.Equals(null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenComparedWithNullThenTheyAreNotEqual()
        {
            var optional = Optional.Of(TestString);

            Assert.IsFalse(optional.Equals(null));
        }
    }

    interface ITestInterface
    {

    }
}