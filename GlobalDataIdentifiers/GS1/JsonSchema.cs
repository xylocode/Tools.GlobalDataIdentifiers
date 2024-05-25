#pragma warning disable CS1591, IDE1006
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XyloCode.Tools.GlobalDataIdentifiers.GS1
{
    internal class GS1Root
    {
        public Applicationidentifier[] applicationIdentifiers { get; set; }
    }

    internal class Applicationidentifier
    {
        public string applicationIdentifier { get; set; }
        public string formatString { get; set; }
        public string description { get; set; }
        public bool fnc1required { get; set; }
        public string regex { get; set; }
        public string note { get; set; }
        public string title { get; set; }
        public bool separatorRequired { get; set; }
        public Component[] components { get; set; }
        public bool gs1DigitalLinkPrimaryKey { get; set; }
        public string[][] gs1DigitalLinkQualifiers { get; set; }

        public Range[] excludes { get; set; }
        public Range[] requires { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }

    internal class Component
    {
        public bool optional { get; set; }
        public string type { get; set; }
        public bool fixedLength { get; set; }
        public int length { get; set; }
        public bool checkDigit { get; set; }
        public bool key { get; set; }
        public string format { get; set; }
        public bool checkCharacters { get; set; }
    }

    [JsonConverter(typeof(RangeConverter))]
    internal class Range
    {
        public string start { get; set; }
        public string end { get; set; }
        public List<string> List { get; set; }
    }

    internal class Range2
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    internal class RangeConverter : JsonConverter<Range>
    {
        public override Range Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                return new Range { start = s, end = s };
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var jsonDoc = JsonDocument.ParseValue(ref reader);
                if (jsonDoc == null) { return null; }
                var x = (Range2)jsonDoc.RootElement.Deserialize(typeof(Range2));
                return new Range { start = x.start, end = x.end };
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var jsonDoc = JsonDocument.ParseValue(ref reader);
                if (jsonDoc == null) { return null; }
                var x = (List<string>)jsonDoc.RootElement.Deserialize(typeof(List<string>));
                return new Range { List = x };
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Range value, JsonSerializerOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}
