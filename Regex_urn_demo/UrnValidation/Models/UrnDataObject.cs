using Regex_urn_demo.Benchmark;
using Regex_urn_demo.Benchmark.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Regex_urn_demo.UrnValidation.Models;

public class UrnDataObject : IEquatable<UrnDataObject>
{
    public string? ContentGroupId;
    public string[]? SubIds;
    public bool isValid;
    public string inputData;
    public RunResult runResult;

    public UrnDataObject(string _inputData, bool _isValid)
    {
        inputData = _inputData;
        isValid = _isValid;
    }

    public override bool Equals(object? obj) => Equals(obj as UrnDataObject);
    public bool Equals(UrnDataObject? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ContentGroupId != obj.ContentGroupId)
        {
            return false;
        }

        if (SubIds != null && obj.SubIds != null)
        {
            if (SubIds?.Length == obj.SubIds?.Length)
            {
                for (int i = 0; i < SubIds!.Length; i++)
                {
                    if (SubIds[i] != obj.SubIds![i])
                    {
                        return false;
                    }
                }
            }
        }

        if (isValid != obj.isValid)
        {
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (inputData.Length > 30)
        {
            sb.Append("Input data is to long to display. This is likely since the input was a large body of text\n");
        }
        else
        {
            sb.Append($"Input Data: {inputData}\n");
        }

        sb.Append($"Urn Is Valid: {isValid}\n");

        sb.Append($"Content Group Id: {ContentGroupId}");

        if (SubIds != null)
        {
            for (int i = 0; i < SubIds.Length; i++)
            {
                sb.Append($"\n\tSub-ID {i + 1}: {SubIds[i]}");
            }
        }

        sb.Append($"\nMatch Time: {runResult!.TotalMatchTime:0.0000}ms");
        sb.Append($"\nParse Time: {runResult.TotalParseTime:0.0000}ms");
        sb.Append($"\nTotal Execution Time: {runResult.TotalRunTime:0.0000}ms");

        return sb.ToString();
    }
}