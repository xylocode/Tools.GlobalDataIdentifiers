# GlobalDataIdentifiers library

Library for working with Data and Application Identifiers defined by GS1 and ASC MH10 Standards.

- [NuGet](https://www.nuget.org/packages/XyloCode.Tools.GlobalDataIdentifiers) (.NET library)
- [GitHub](https://github.com/xylocode/Tools.GlobalDataIdentifiers) (source code)

#### Supported Platforms

- .NET 6.0 LTS;
- .NET 7.0;
- .NET 8.0 LTS.

## Supported Standards

### GS1

GS1 Application Identifiers (AIs) are prefixes used in barcodes and EPC/RFID-tags to define the meaning and format of data attributes. This tool was developed in response to the growing use of AIs in the various industry sectors to include product data beyond the GTIN, such as the batch/lot number, serial number, best before date and expiration date.

- [GS1 Application Identifiers](https://ref.gs1.org/ai/)

### ASC MH10

ASC MH10 is the American National "Continuous Operation" Standard. ASC MH10 Data Identifiers may be used with any alphanumeric data carrier and are designed to ensure
cross-industry commonality of data identifiers used in automatic identification technologies.

- [MHI](https://my.mhi.org/s/store#/store/browse/detail/a153h000005lJuR)
- [American National Standards Institute](https://webstore.ansi.org/standards/mhia/ansimh102010)

### Chestny ZNAK

Chestny ZNAK — the Unified national track & trace digital system, based on GS1, created by the Center for Research in Perspective Technologies to implement projects in the digital economy on the territory of the Eurasian Economic Union (Armenia, Belarus, Kazakhstan, Kyrgyzstan, Russia).

- [Состав кода маркировки](https://markirovka.ru/knowledge/tovarnye-gruppy/shini-pokrishki/sostav-koda-markirovki-shiny)

## How to use

```cs
using XyloCode.Tools.GlobalDataIdentifiers.Generic;
using XyloCode.Tools.GlobalDataIdentifiers.GS1;
using XyloCode.Tools.GlobalDataIdentifiers.MH10;

namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IGlobalDataIdentifiersProvider provider;
            string input;
            IList<KeyValuePair<Identifier, string>> decodedData;
            
            
            provider = new GS1Provider();
            //provider = new EurasianEconomicUnionGS1Provider();

            foreach (var item in provider.Identifiers)
            {
                Console.WriteLine("AI{0}: {1}", item.Key, item.Value.Title);
            }

            input = GS1Provider.FNC1
                + "010481034700322221omczdcsfv8vz8" + GS1Provider.GroupSeparator
                + "918023" + GS1Provider.GroupSeparator
                + "92YInk1wZffMEBZ7nNGQGccH/mMyjxqb6SQH3ebMXVZBw+HK+uHdACsiK6pm1v+2gtXQ+aXpw9y03iimVeDUtgaw==" + GS1Provider.GroupSeparator
                + "3643123456";
            decodedData = provider.Decode(input);
            foreach (var item in decodedData)
            {
                Console.WriteLine("AI: {0}", item.Key.Code);
                Console.WriteLine(item.Key.Title);
                //Console.WriteLine(item.Key.Description + "\r\n" + item.Key.Note);
                Console.WriteLine(item.Value);
                Console.WriteLine();
            }


            provider = new MH10Provider();
            input = MH10Provider.FNC1
                + "Y123" + MH10Provider.GroupSeparator
                + "1YABC" + MH10Provider.GroupSeparator
                + "12YABC123" + MH10Provider.GroupSeparator
                + "123Y123ABC" + MH10Provider.GroupSeparator
                + MH10Provider.EndOfTransmission;

            decodedData = provider.Decode(input);
            foreach (var item in decodedData)
            {
                Console.WriteLine("DI: {0}", item.Key.Code);
                Console.WriteLine(item.Key.Title);
                //Console.WriteLine(item.Key.Description + "\r\n" + item.Key.Note);
                Console.WriteLine(item.Value);
                Console.WriteLine();
            }

            Console.Beep();
            Console.ReadLine();
        }
    }
}
```

## License

MIT License