﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        }

        [TestMethod]
        public void GivenNullValueThenEmptyOptionalIsCreated()
        {
            var emptyOptional = Optional.OfNullable<string>(null);

            Assert.IsFalse(emptyOptional.HasValue());
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
            IOptional<string> filtered = optional.Filter(s => !string.IsNullOrWhiteSpace(s));

            Assert.IsTrue(filtered.HasValue());
        }

        [TestMethod]
        public void GivenEmptyOptionalWhenFilteringWithFalseFilterEmptyObjectIsReturend()
        {
            var optional = Optional.Of(TestString);
            IOptional<string> filtered = optional.Filter(s => string.IsNullOrWhiteSpace(s));

            Assert.IsFalse(filtered.HasValue());
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
    }
}