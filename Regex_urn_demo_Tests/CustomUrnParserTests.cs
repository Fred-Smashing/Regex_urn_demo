using FluentAssertions;
using Regex_urn_demo.UrnValidation;
using Regex_urn_demo.UrnValidation.Models;

namespace Regex_urn_demo_Tests
{
    public class CustomUrnParserTests
    {
        [Theory]
        [InlineData("urn:abcd:1234")]
        [InlineData("urn:1234:abcd")]
        [InlineData("urn:abc:abc-123")]
        [InlineData("urn:abc-a:abc:123")]
        public void ValidUrn_ShouldReturnObject(string input)
        {
            CustomUrnParser customUrnParser = new();

            var result = customUrnParser.GetUrnData(input);

            result.Should().Satisfy(
                i => i.inputData == input && i.isValid == true
                );
        }

        [Fact]
        public void Result_ShouldMatchExpectedObject()
        {
            CustomUrnParser cutomUrnParser = new();

            var input = "urn:abc-a:123:abc-a1b2";
            var expectedObject = new UrnDataObject(input, true)
            {
                ContentGroupId = "abc-a",
                SubIds = new[]
                {
                "123",
                "abc",
                "a1b2"
                }
            };

            var result = cutomUrnParser.GetUrnData(input);

            var equal = result[0].Equals(expectedObject);

            equal.Should().BeTrue();
        }
    }
}
