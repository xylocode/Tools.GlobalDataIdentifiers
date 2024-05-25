using System.Collections.Generic;
using System.Linq;
using XyloCode.Tools.GlobalDataIdentifiers.Generic;

namespace XyloCode.Tools.GlobalDataIdentifiers.GS1
{
    public class EurasianEconomicUnionGS1Provider : GS1Provider
    {
        const string InternalRegex = "([!%-?A-Z_a-z\x22]{1,90})";
        protected override IEnumerable<Identifier> GetIdentifiers()
        {
            var gs1 = base.GetIdentifiers().Where(x => !x.Code.StartsWith('9'));
            var eeu = new List<Identifier>
            {
                new(){Code = "90", Title = "INTERNAL", Description = "Information mutually agreed between trading partners", Regex = InternalRegex, MaxLength = 90, SeparatorRequired = true },
                new(){Code = "91", Title = "ИД ключа проверки", Description = "Честный знак", Regex = InternalRegex, MaxLength = 4, SeparatorRequired = true },
                new(){Code = "92", Title = "Код проверки", Description = "Честный знак", Regex = InternalRegex, MaxLength = 44, SeparatorRequired = true },
                new(){Code = "93", Title = "Код проверки оператора", Description = "Честный знак", Regex = InternalRegex, MaxLength = 4, SeparatorRequired = true },
                new(){Code = "94", Title = "INTERNAL", Description = "Company internal information", Regex = InternalRegex, MaxLength = 90, SeparatorRequired = true },
                new(){Code = "95", Title = "INTERNAL", Description = "Company internal information", Regex = InternalRegex, MaxLength = 90, SeparatorRequired = true },
                new(){Code = "96", Title = "INTERNAL", Description = "Company internal information", Regex = InternalRegex, MaxLength = 90, SeparatorRequired = true },
                new(){Code = "97", Title = "INTERNAL", Description = "Company internal information", Regex = InternalRegex, MaxLength = 90, SeparatorRequired = true },
                new(){Code = "98", Title = "INTERNAL", Description = "Company internal information", Regex = InternalRegex, MaxLength = 90, SeparatorRequired = true },
                new(){Code = "99", Title = "INTERNAL", Description = "Company internal information", Regex = InternalRegex, MaxLength = 90, SeparatorRequired = true },
            };

            return gs1.Union(eeu);
        }
    }
}
