using Regex_urn_demo.UrnValidation.Models;

namespace Regex_urn_demo.UrnValidation.Abstractions
{
    public interface IUrnParser
    {
        public UrnDataObject[] GetUrnData(string input);
    }
}
