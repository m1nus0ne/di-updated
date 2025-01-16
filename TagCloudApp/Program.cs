using CommandLine;
using TagCloud;

namespace TagCloudApplication;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(option =>
            {
                if (TagCloudServicesFactory.ConfigureServiceAndTryGet<TagCloudGenerator>(option, out var generator))
                    generator.Generate();
                else
                {
                    Console.WriteLine("Can't configure service");
                    Console.Read();
                }
            });
    }
}