using FluentAssertions;
using Regex_urn_demo.UrnValidation;
using System.Text.Json;

namespace Regex_urn_demo_Tests
{
    [UsesVerify]
    public class RegexUrnParserTests
    {
        [Theory]
        [InlineData("valid_1","urn:abcd:1234")]
        [InlineData("valid_2","urn:1234:abcd")]
        [InlineData("valid_3","urn:abc:abc-123")]
        [InlineData("valid_4","urn:abc-a:abc:123")]
        public async void ValidUrn_ShouldReturnObject(string scenario, string input)
        {
            RegexUrnParser regexUrnParser = new();

            var result = regexUrnParser.GetUrnData(input);

            result.Should().NotBeNullOrEmpty();

            var json = JsonSerializer.Serialize(result);
            await VerifyJson(json).UseParameters(scenario);
        }

        [Fact]
        public async void Result_ShouldMatchExpectedObject()
        {
            RegexUrnParser regexUrnParser = new();

            var input = "urn:abc-a:123:abc-a1b2";
            var result = regexUrnParser.GetUrnData(input);

            result.Should().NotBeNullOrEmpty();

            var json = JsonSerializer.Serialize(result);
            await VerifyJson(json);
        }
    }
}