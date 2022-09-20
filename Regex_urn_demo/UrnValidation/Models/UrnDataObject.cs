using System.Text;

namespace Regex_urn_demo.UrnValidation.Models;

public class UrnDataObject : IEquatable<UrnDataObject>
{
    public string? ContentGroupId { get; set; }
    public string[]? SubIds { get; set; }
    public bool IsValid { get; set; }
    public string InputData { get; set; }

    public UrnDataObject(string _inputData, bool _isValid)
    {
        InputData = _inputData;
        IsValid = _isValid;
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

        if (IsValid != obj.IsValid)
        {
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (InputData.Length > 30)
        {
            sb.Append("Input data is to long to display. This is likely since the input was a large body of text\n");
        }
        else
        {
            sb.Append($"Input Data: {InputData}\n");
        }

        sb.Append($"Urn Is Valid: {IsValid}\n");

        sb.Append($"Content Group Id: {ContentGroupId}");

        if (SubIds != null)
        {
            for (int i = 0; i < SubIds.Length; i++)
            {
                sb.Append($"\n\tSub-ID {i + 1}: {SubIds[i]}");
            }
        }

        return sb.ToString();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(InputData, IsValid);
    }
}