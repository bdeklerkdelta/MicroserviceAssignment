using MicroserviceB.Service.Commands;
using Xunit;
using FluentValidation.TestHelper;

namespace MicroserviceA.Service.Tests
{
    public class DisplayNameCommandValidatorTests
    {
        private readonly DisplayNameCommandValidator _validator;

        public DisplayNameCommandValidatorTests()
        {
            _validator = new DisplayNameCommandValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Name_WhenNullOrEmpty_ShouldHaveValidationError(string name)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Name, name).WithErrorMessage("Name cannot be empty.");
        }

        [Theory]
        [InlineData("Testname")]
        public void Name_WhenValid_ShouldNotHaveValidationError(string name)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, name);
        }

        [Theory]
        [InlineData("!@#")]
        public void Name_WhenContainsSymbol_ShouldHaveValidationError(string name)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Name, name).WithErrorMessage("Name contains invalid characters.");
        }


        [Theory]
        [InlineData("Test123")]
        public void Name_WhenContainsNumber_ShouldHaveValidationError(string name)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Name, name).WithErrorMessage("Name may only contain letters.");
        }
    }
}
