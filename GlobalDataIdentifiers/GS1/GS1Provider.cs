using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using XyloCode.Tools.GlobalDataIdentifiers.Generic;

namespace XyloCode.Tools.GlobalDataIdentifiers.GS1
{
    public class GS1Provider : IGlobalDataIdentifiersProvider
    {
        public const char FNC1 = '\xE8';
        public const char GroupSeparator = '\x1D';

        private readonly Dictionary<string, Identifier> identifiers;
        
        public Dictionary<string, Identifier> Identifiers => identifiers;

        public GS1Provider()
        {
            identifiers = GetIdentifiers()
                .ToDictionary(
                    keySelector => keySelector.Code,
                    elementSelector => elementSelector
                    );
        }


        protected virtual IEnumerable<Identifier> GetIdentifiers()
        {
            var srcFile = Helper.GetFileData(Properties.Resources.GS1_Application_Identifiers);
            var gs1 = JsonSerializer.Deserialize<GS1Root>(srcFile);
            return gs1.applicationIdentifiers
                .Where(x => !string.IsNullOrWhiteSpace(x.applicationIdentifier))
                .Select(x => new Identifier
                {
                    Code = x.applicationIdentifier,
                    Title = x.title,
                    Description = x.description,
                    Note = x.note,
                    Regex = x.regex,
                    MaxLength = x.components[0].length,
                    SeparatorRequired = x.separatorRequired,
                });
        }


        public virtual List<KeyValuePair<Identifier, string>> Decode(string input)
        {
            var list = new List<KeyValuePair<Identifier, string>>();
            KeyValuePair<Identifier, string>? value;

            input = input.TrimStart(FNC1);

            var groups = input.Split(GroupSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in groups)
            {
                int pointer = 0;
                while (Read(s, ref pointer, out value))
                {
                    list.Add(value.Value);
                }
            }
            return list;
        }

        protected virtual bool Read(string input, ref int pointer, out KeyValuePair<Identifier, string>? value)
        {
            value = null;
            string code;
            Identifier ai;

            if (input.Length == pointer)
                return false;

            for (int i = 2; i < 5; i++)
            {
                if (input.Length < pointer + i)
                    throw new Exception("Insufficient amount of data to decode!");

                code = input.Substring(pointer, i);
                
                if (Identifiers.TryGetValue(code, out ai))
                {
                    pointer += i;
                    var stringValue = GetAIValue(input, ref pointer, ai);
                    value = new(ai, stringValue);
                    return true;
                }

                if (i == 4)
                    throw new Exception($"Application Identifiers (AI) {code} not found in dictionaries.");
            }

            return false;
        }

        protected virtual string GetAIValue(string input, ref int pointer, Identifier identifier)
        {
            int startPosition = pointer;
            if (identifier.SeparatorRequired)
            {
                pointer = input.Length;
                return input[startPosition..];
            }

            if (input.Length < pointer + identifier.MaxLength)
                throw new Exception("Insufficient amount of data to decode!");

            pointer += identifier.MaxLength;
            return input[startPosition..pointer];
        }

        public virtual string Encode(ICollection<KeyValuePair<string, string>> collection)
        {
            var sb = new StringBuilder();
            sb.Append(FNC1);

            int count = collection.Count;
            int i = 0;

            foreach (var item in collection)
            {
                i++;
                if (!Identifiers.TryGetValue(item.Key, out Identifier ai))
                    throw new Exception($"Application Identifiers (AI) {item.Key} not found in dictionaries.");

                var regex = new Regex('^' + ai.Regex + '$');
                if (!regex.IsMatch(item.Value))
                    throw new Exception("The encoded data does not match the regular expression pattern!");

                sb.Append(item.Key);
                sb.Append(item.Value);
                if (ai.SeparatorRequired && i < count)
                    sb.Append(GroupSeparator);
            }
            return sb.ToString();
        }
    }
}
