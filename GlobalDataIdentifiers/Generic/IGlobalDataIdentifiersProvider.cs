using System.Collections.Generic;

namespace XyloCode.Tools.GlobalDataIdentifiers.Generic
{
    public interface IGlobalDataIdentifiersProvider
    {
        Dictionary<string, Identifier> Identifiers { get; }
        List<KeyValuePair<Identifier, string>> Decode(string input);
        string Encode(ICollection<KeyValuePair<string, string>> collection);
    }
}
