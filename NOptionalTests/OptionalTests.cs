using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
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
            var filtered = optional.IfApplies(s => !string.IsNullOrWhiteSpace(s));

            Assert.IsTrue(filtered.HasValue());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenFilteringWithFalseFilterEmptyObjectIsReturend()
        {
            var optional = Optional.Of(TestString);
            var filtered = optional.IfApplies(s => string.IsNullOrWhiteSpace(s));

            Assert.IsFalse(filtered.HasValue());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullPredicateOnFilterThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.IfApplies(null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullPredicateOnFilterThenEmptyOptionalIsReturned()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.IfApplies(null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionOnIsPresentThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.DoIfPresent(null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionOnIsPresentThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.DoIfPresent(null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionToEitherParameterOnIsPresentOrElseThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.DoIfPresentOrElse(null, null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionToEitherParameterOnIsPresentOrElseThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.DoIfPresentOrElse(null, null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionOnOrThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.GetValueOrElse((Func<IOptional<string>>)null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionOnOrThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.GetValueOrElse((Func<IOptional<string>>)null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionOnOrElseGetThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.GetValueOrElse((Func<string>)null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionOnOrElseGetThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.GetValueOrElse((Func<string>)null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionOnOrElseThrowThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.GetValueOrElseThrow(null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionOnOrElseThrowThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.GetValueOrElseThrow(null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionOnMapThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.MapValue<string>(null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionOnMapThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.MapValue<string>(null));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenProvidingNullActionOnFlatMapThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Of(TestString);

            Assert.ThrowsException<ArgumentNullException>(() => optional.FlatMapValue<string>(null));
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenProvidingNullActionOnFlatMapThenArgumentNullExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();

            Assert.ThrowsException<ArgumentNullException>(() => optional.FlatMapValue<string>(null));
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
            optional.DoIfPresent(s => Assert.Fail());

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GivenFilledOptionalThenExecutesAction()
        {
            var optional = Optional.Of(TestString);
            var called = false;
            optional.DoIfPresent(s => called = true);

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void GivenEmptyOptionalThenSuppliedValueIsUsed()
        {
            var optional = Optional.Empty<string>();
            var value = optional.GetValueOrElse(TestString);

            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void GivenFilledOptionalThenSuppliedValueIsUnused()
        {
            var optional = Optional.Of(TestString);
            var value = optional.GetValueOrElse("other");

            Assert.AreNotEqual("other", value);
        }

        [TestMethod]
        public void GivenEmptyOptionalThenFunctionCreatesValue()
        {
            var optional = Optional.Empty<string>();
            var value = optional.GetValueOrElse(() => TestString);

            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void GivenFilledOptionalThenFunctionIsNotCalled()
        {
            var optional = Optional.Of(TestString);
            var value = optional.GetValueOrElse(() =>
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

            Assert.ThrowsException<Exception>(() => optional.GetValueOrElseThrow(() => new Exception()));
        }

        [TestMethod]
        public void GivenFilledOptionalThenReturnsValue()
        {
            var optional = Optional.Of(TestString);
            var value = optional.GetValueOrElseThrow(() =>
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
            var enumeratorOptional = optional.MapValue(s =>
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
            var enumeratorOptional = optional.MapValue(s => s.GetEnumerator());

            Assert.IsTrue(enumeratorOptional.HasValue());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenFlatMappingThenEmptyOptionalIsReturned()
        {
            var optional = Optional.Empty<string>();
            var enumeratorOptional = optional.FlatMapValue(s =>
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
            var enumeratorOptional = optional.FlatMapValue(s => Optional.Of(s.GetEnumerator()));

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
            
            optional.DoIfPresentOrElse(s => Assert.Fail(), () => Assert.IsTrue(true));
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCallingIfPresentOrElseThenPresentActionIsExecuted()
        {
            var optional = Optional.Of(TestString);

            optional.DoIfPresentOrElse(s => Assert.IsTrue(true), () => Assert.Fail());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenCallingOrThenOrIsExecuted()
        {
            var optional = Optional.Empty<string>();
            var value = optional.GetValueOrElse(() => Optional.Of(TestString));

            Assert.IsTrue(value.IfApplies(s => s.Equals(TestString)).HasValue());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCallingOrThenOrIsExecuted()
        {
            var optional = Optional.Of(TestString);
            var value = optional.GetValueOrElse(() =>
            {
                Assert.Fail();
                return Optional.Empty<string>();
            });

            Assert.IsTrue(value.IfApplies(s => s.Equals(TestString)).HasValue());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenCallingOrElseThrowThenInvalidOperationExceptionIsThrown()
        {
            var optional = Optional.Empty<string>();
            
            Assert.ThrowsException<InvalidOperationException>(() => optional.GetValueOrElseThrow());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCallingOrElseThrowThenValuesAreEqual()
        {
            var optional = Optional.Of(TestString);
            var value = optional.GetValueOrElseThrow();

            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenCallingGetEnumerableThenContainsNoElements()
        {
            Assert.IsFalse(Optional.Empty<string>().AsEnumerable().ToList().Any());
        }

        [TestMethod]
        public void GivenFilledOptionalWhenCallingGetEnumerableThenContainsOneElement()
        {
            Assert.IsTrue(Optional.Of(TestString).AsEnumerable().ToList().Any());
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
        public void GivenTwoEmptyOptionalsWhenComparingThenTheyAreEqual()
        {
            var optional = Optional.Empty<string>();
            var optional2 = Optional.Empty<string>();

            Assert.IsTrue(optional.Equals(optional2));
            Assert.IsTrue(optional2.Equals(optional));
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
        public void GivenTwoEmptyOptionalWhenCalculatingHashThenHashIsEqual()
        {
            var optional = Optional.Empty<string>();
            var optional2 = Optional.Empty<string>();

            Assert.IsTrue(optional.GetHashCode() == optional2.GetHashCode());
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

        [TestMethod]
        public void GivenNullableIntWithNullValueWhenUsingOptionalOfNullableThenEmptyOoptionalIsReturned()
        {
            int? nullableInt = null;
            var optionalOfNullableInt = Optional.OfNullable(nullableInt);

            Assert.IsTrue(optionalOfNullableInt.IsEmpty());
        }
    }

    interface ITestInterface
    {

    }
}