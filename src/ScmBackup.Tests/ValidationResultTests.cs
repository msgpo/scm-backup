﻿using System.Linq;
using Xunit;

namespace ScmBackup.Tests
{
    public class ValidationResultTests
    {
        private ValidationResult sut;

        public ValidationResultTests()
        {
            sut = new ValidationResult();
        }

        [Fact]
        public void AddMessageAddsSingleMessage()
        {
            sut.AddMessage(ErrorLevel.Info, "i");

            Assert.Equal(1, sut.Messages.Count);
        }

        [Fact]
        public void AddMessageAddsConfigSourceTitle()
        {
            var source = new ConfigSource();
            source.Title = "foo";

            sut = new ValidationResult(source);
            sut.AddMessage(ErrorLevel.Info, "message");

            var createdText = sut.Messages.First().Message;
            Assert.True(createdText.StartsWith("foo"));
            Assert.True(createdText.EndsWith("message"));
        }

        [Fact]
        public void IsValidIsTrueWithNoMessages()
        {
            Assert.False(sut.Messages.Any());
            Assert.True(sut.IsValid);
        }

        [Fact]
        public void IsValidIsTrueWithInfoAndWarnMessage()
        {
            sut.AddMessage(ErrorLevel.Info, "i");
            sut.AddMessage(ErrorLevel.Warn, "w");

            Assert.Equal(2, sut.Messages.Count);
            Assert.True(sut.IsValid);
        }

        [Fact]
        public void IsValidIsFalseWithErrorMessage()
        {
            sut.AddMessage(ErrorLevel.Error, "e");

            Assert.Equal(1, sut.Messages.Count);
            Assert.False(sut.IsValid);
        }

        [Fact]
        public void IsValidIsFalseWithAllMessages()
        {
            sut.AddMessage(ErrorLevel.Info, "i");
            sut.AddMessage(ErrorLevel.Warn, "w");
            sut.AddMessage(ErrorLevel.Error, "e");

            Assert.Equal(3, sut.Messages.Count);
            Assert.False(sut.IsValid);
        }
    }
}
