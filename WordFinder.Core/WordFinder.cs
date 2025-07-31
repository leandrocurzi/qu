namespace WordFinder.Core;

using Microsoft.Extensions.Logging;

public class WordFinder
{
    private readonly char[][] matrix;
    private readonly int rows;
    private readonly int cols;
    private readonly HashSet<string> horizontalLines;
    private readonly HashSet<string> verticalLines;
    private readonly ILogger<WordFinder>? logger;

    public WordFinder(IEnumerable<string> matrix, ILogger<WordFinder>? logger = null)
    {
        this.logger = logger;

        if (matrix == null) throw new ArgumentNullException(nameof(matrix));

        var rowsArray = matrix.ToArray();

        if (rowsArray.Length == 0)
            throw new ArgumentException("Matrix must contain at least one row.");

        int expectedLength = rowsArray[0].Length;

        if (rowsArray.Any(row => row == null || row.Length != expectedLength))
            throw new ArgumentException("All rows in the matrix must be non-null and have the same length.");

        this.matrix = matrix.Select(row => row.ToCharArray()).ToArray();
        this.rows = this.matrix.Length;
        this.cols = this.matrix[0].Length;

        this.horizontalLines = new HashSet<string>();
        this.verticalLines = new HashSet<string>();

        logger?.LogDebug("Preprocessing matrix: {Rows}x{Cols}", rows, cols);
        Preprocess();
    }

    public IEnumerable<string> Find(IEnumerable<string> wordstream)
    {
        if (wordstream == null)
        {
            logger?.LogError("Wordstream is null.");
            throw new ArgumentNullException(nameof(wordstream));
        }

        logger?.LogDebug("Starting word search with {WordCount} unique words.", wordstream.Count());

        var foundWords = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var uniqueWords = new HashSet<string>(wordstream, StringComparer.OrdinalIgnoreCase);

        try
        {
            foreach (var word in uniqueWords)
            {
                if (horizontalLines.Any(line => line.Contains(word, StringComparison.OrdinalIgnoreCase)) ||
                    verticalLines.Any(line => line.Contains(word, StringComparison.OrdinalIgnoreCase)))
                {
                    if (string.IsNullOrWhiteSpace(word))
                    {
                        continue;
                    }

                    foundWords[word] = foundWords.ContainsKey(word) ? foundWords[word] + 1 : 1;
                }
            }

            var result = foundWords
                .OrderByDescending(kvp => kvp.Value)
                .ThenBy(kvp => kvp.Key)
                .Take(10)
                .Select(kvp => kvp.Key)
                .ToList();

            logger?.LogInformation("Search completed. {Count} matches found.", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An unexpected error occurred during word search.");
            throw;
        }
    }

    private void Preprocess()
    {
        // Store all horizontal lines
        for (int i = 0; i < rows; i++)
        {
            horizontalLines.Add(new string(matrix[i]));
        }

        // Store all vertical lines
        for (int col = 0; col < cols; col++)
        {
            var colBuilder = new char[rows];
            for (int row = 0; row < rows; row++)
            {
                colBuilder[row] = matrix[row][col];
            }
            verticalLines.Add(new string(colBuilder));
        }
    }
}
