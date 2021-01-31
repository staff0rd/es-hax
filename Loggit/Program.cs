using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Loggit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var delay = 1000;
            if (args.Length > 0) {
                delay = int.Parse(args[0]);
            }
            var loggerConfig = new LoggerConfiguration()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200") ){
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        EmitEventFailure =
                            EmitEventFailureHandling.WriteToSelfLog |
                            EmitEventFailureHandling.RaiseCallback |
                            EmitEventFailureHandling.ThrowException,
                        FailureCallback = (e) => {
                            Console.WriteLine("Fail");
                            if (e.Exception == null) {
                                Console.WriteLine("No exception");
                            } else
                            Console.WriteLine($"Failed to write: {e.Exception.Message}");
                        
            }
                        
                 })
                 .WriteTo.Console();
            
            var logger = loggerConfig.CreateLogger();
            var random = new Random();
            do
            {
                var number = random.Next();
                var word = "bubble";
                logger.Information("This is the number {number} and here is a {word}", number, word);
                await Task.Delay(delay);
            } while(true);
        }
    }
}
