using Regex_urn_demo.UrnValidation.Abstractions;
using Regex_urn_demo.UrnValidation.Models;
using System.Text.RegularExpressions;

namespace Regex_urn_demo.UrnValidation
{
    public class RegexUrnParser : IUrnParser
    {
        #region Regex Pattern
        // Matches only ascii characters from A-Z uppercase and lowercase
        private const string Letter = @"A-Za-z";
        // Matches only digits from 0-9
        private const string Digit = @"0-9";
        // Matches allowed percent encoded characters
        private const string PercentEncodedChar = @"%(?!00)[0-9A-Fa-f]{2}";
        // Matches the allowed special characters
        private const string Delimiters = @"@\-:;!?$&=.,_~";

        /*
         * Here we can set up the pattern to be used in the matches
         * We can use constants to define specific character ranges to make the final pattern easuer to read by a human
         */
        private const string Pattern =
            // We put a negative lookbehind before the urn to ensure we only capture urns that are not a part of another string for example a web address
            @"(?<![^\s])urn:"
            // The group id can contain Ascii characters, Digits, and some select special characters "- . _ ~"
            // It is also a maximum of 31 chracters long
            + $"(?<groupId>[{Letter}{Digit}\\-_.~]{{0,31}}):"
            // The sub id can contain Ascii characters, Digis, Hexidecimal digits, and many special characters
            // It can also be indeterminately long, but should always contain at least one character
            + $"(?<subId>(?:[{Letter}{Digit}]+|{PercentEncodedChar})+[{Delimiters}]?)+";
        #endregion

        #region Regex Object
        // Regex Options
        private const RegexOptions Options = RegexOptions.Multiline | RegexOptions.Compiled;

        /*
         * Creating a Regex Object and storing it in a static field can improve performance
         * This is especially true when the regex pattern is going to be used in non static method calls,
         * as the engine does not have to recompile the pattern on each run
         */
        private static readonly Regex RegexObject;

        /*
         * You can create an object and pass the pattern into its constructor
         * This gives you some nice syntax highlighting that you might find easier to read
        */
        private static readonly Regex RegexWithHighlight =
            new(@"(?<![^\s])urn:(?<groupId>[A-Za-z0-9\-_.~]{0,31}):(?<subId>(?:[A-Za-z0-9]+|%(?!00)[0-9A-Fa-f]{2})+[@\-:;!?$&=.,_~]?)+");
        #endregion

        static RegexUrnParser()
        {
            RegexObject = new Regex(Pattern, Options, TimeSpan.FromMilliseconds(20));
        }

        public UrnDataObject[] GetUrnData(string input)
        {
            var matches = RunRegex(input);

            List<UrnDataObject> data = new List<UrnDataObject>();

            foreach (Match match in matches)
            {
                var urnObject = new UrnDataObject(input, true);

                if (match.Groups.TryGetValue("groupId", out var groupId))
                {
                    urnObject.ContentGroupId = groupId.Value;
                }

                if (match.Groups.TryGetValue("subId", out var subId))
                {
                    urnObject.SubIds = subId.Captures.Select(c => c.Value).ToArray();
                }

                data.Add(urnObject);
            }

            return data.ToArray();
        }

        private static MatchCollection RunRegex(string input)
        {
            return RegexObject.Matches(input);
        }
    }
}
