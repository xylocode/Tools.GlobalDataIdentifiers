using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using XyloCode.Tools.GlobalDataIdentifiers.Generic;

namespace XyloCode.Tools.GlobalDataIdentifiers.MH10
{
    public partial class MH10Provider : IGlobalDataIdentifiersProvider
    {
        public const char FNC1 = '\xE8';
        public const char GroupSeparator = '\x1D';
        public const char RecordSeparator = '\x1E';
        public const char EndOfTransmission = '\x04';


        private const string RegexStringPattern = @"\A[1-9]{0,1}(\d){0,2}[A-Z]";

#if NET6_0
        private readonly Regex RegexPattern = new Regex(RegexStringPattern, RegexOptions.Compiled);
#endif

#if NET7_0_OR_GREATER
        [GeneratedRegex(RegexStringPattern, RegexOptions.Compiled)]
        private static partial Regex MH10Regex();
        private readonly Regex RegexPattern = MH10Regex();
#endif

        private readonly Dictionary<string, Identifier> identifiers;
        public Dictionary<string, Identifier> Identifiers => identifiers;

        public MH10Provider()
        {
            identifiers = new Dictionary<string, Identifier>();
        }

        public virtual List<KeyValuePair<Identifier, string>> Decode(string input)
        {
            var list = new List<KeyValuePair<Identifier, string>>();
            KeyValuePair<Identifier, string>? value;

            input = input.TrimStart(FNC1);
            input = input.TrimEnd(EndOfTransmission);

            var strings = input.Split(new char[] { GroupSeparator, RecordSeparator }, StringSplitOptions.RemoveEmptyEntries);
            foreach ( var s in strings )
            {
                list.Add(StringDecode(s));
            }

            return list;
        }

        protected virtual KeyValuePair<Identifier, string> StringDecode(string input)
        {
            var math = RegexPattern.Match(input);
            if (!math.Success)
                throw new Exception("String decoding error: " + input);
            
            var di = new Identifier { Code = math.Value };
            var value = input[math.Length..];
            return new KeyValuePair<Identifier, string>(di, value);
        }

        public virtual string Encode(ICollection<KeyValuePair<string, string>> collection)
        {
            var list = new List<string>();

            foreach ( var kvp in collection)
            {
                if (!RegexPattern.IsMatch(kvp.Key))
                    throw new Exception($"The Data Identifier ({kvp.Key}) has an incorrect format!");
                list.Add(kvp.Key + kvp.Value);
            }

            return FNC1 + string.Join(GroupSeparator, list);
        }
    }
}
