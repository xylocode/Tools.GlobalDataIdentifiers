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
