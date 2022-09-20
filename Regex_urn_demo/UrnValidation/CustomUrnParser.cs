using BenchmarkDotNet.Columns;
using Regex_urn_demo.UrnValidation.Abstractions;
using Regex_urn_demo.UrnValidation.Models;
using System;

namespace Regex_urn_demo.UrnValidation
{
    public class CustomUrnParser : IUrnParser
    {
        private readonly static char[] contentGroupDelims = { '\\', '-', '_', '.', '~' };
        private readonly static char[] delimiters = { '@', '\\', '-', ':', ';', '!', '?', '$', '&', '=', '.', ',', '_', '~' };

        public UrnDataObject[] GetUrnData(string input)
        {
            // Seperate any passed in text into lines ass a urn will always be on a single line
            var splits = input.Split("\n");
            List<string> urnLinesList = new List<string>();
            foreach (var line in splits)
            {
                // Only add lines that contain a urn to the list
                if (line.Contains("urn:"))
                {
                    urnLinesList.Add(line);
                }
            }

            // Extract urns from the line, A valid urn should have whitespace or nothing either side of it
            List<string> urnList = new List<string>();
            foreach (var line in urnLinesList)
            {
                var lineSplits = line.Split(' ');
                foreach (var split in lineSplits)
                {
                    if (split.Contains("urn:"))
                    {
                        var trim = split.Trim();

                        if (trim.StartsWith("urn:"))
                        {
                            urnList.Add(split.Trim());
                        }
                    }
                }
            }

            List<UrnDataObject> dataObjects = new List<UrnDataObject>();
            foreach (var urn in urnList)
            {
                // A urn should always comprise of 3 parts seperated by a colon, anything else and it's not valid
                var urnParts = urn.Split(':', 3);

                var obj = new UrnDataObject(input, true);

                // Validate the content group id and the sub id's, We return an object with isValid set to false if these are not true
                if (IsValidUrn(urnParts))
                {
                    obj.ContentGroupId = urnParts[1];

                    // Split the sub ids into parts and add them to the object
                    var subIdParts = urnParts[2].Split(delimiters);
                    List<string> subIds = new List<string>();
                    for (int i = 2; i < subIdParts.Length; i++)
                    {
                        subIds.Add(subIdParts[i]);
                    }
                    obj.SubIds = subIds.ToArray();
                }
                else
                {
                    obj.IsValid = false;
                }
                dataObjects.Add(obj);
            }

            return dataObjects.ToArray();
        }

        private static bool IsValidUrn(string[] input)
        {
            if (input is null)
            {
                return false;
            }

            // This is very simple validation. It does not currenly cover every edge case the Regex Implementation does
            if (input.Length > 0 && input.Length <= 3)
            {
                if (input[0] == "urn")
                {
                    if (IsValidId(input[1], contentGroupDelims, true))
                    {
                        if (IsValidId(input[2], delimiters, false))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool IsValidId(string input, char[] validDelimiters, bool isGroupId)
        {
            // Urn Group Id Cannot be over 32 Characters
            if (isGroupId && input.Length > 32)
            {
                return false;
            }

            bool lastCharWasDelim = false;
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (char.IsWhiteSpace(c))
                {
                    return false;
                }

                if (!char.IsLetterOrDigit(c))
                {
                    if (!validDelimiters.Any(d => d == c) || lastCharWasDelim)
                    {
                        return false;
                    }
                    lastCharWasDelim = true;
                }
                else
                {
                    lastCharWasDelim = false;
                }
            }

            return true;
        }
    }
}
