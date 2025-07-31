namespace WordFinder.ClI;

using WordFinder.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

public class Program {
    public static void Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            })
            .BuildServiceProvider();

        var logger = services.GetRequiredService<ILogger<WordFinder>>();

        var matrix = new List<string>
        {
            "chillwindsblowo",
            "abcdefghijklmoj",
            "coldicecreammmo",
            "mnopqrstuvwxykj",
            "weatherissunnye"
        };

        var words = new List<string> { "cold", "ice", "wind", "chill", "snow", "sun", "cold" };

        var finder = new WordFinder(matrix, logger);
        var results = finder.Find(words);

        foreach (var word in results)
        {
            Console.WriteLine(word);
        }
    }
}
