namespace WordFinder.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using WordFinder.Core;
using Xunit;

public class WordFinderTests
{
    private static List<string> ValidMatrix => new List<string>
    {
        "chillwindsblow",     // 14
        "abcdefghijklm1",      // 14
        "coldicecreammm",     // 14
        "mnopqrstuvwxyZ",     // 14
        "weatherissunny"      // 14
    };

    [Fact]
    public void Finds_Words_Present_In_Matrix()
    {
        var finder = new WordFinder(ValidMatrix);
        var stream = new List<string> { "cold", "wind", "chill", "sun", "ice" };

        var result = finder.Find(stream).ToList();

        Assert.Contains("cold", result);
        Assert.Contains("wind", result);
        Assert.Contains("chill", result);
        Assert.Contains("ice", result);
        Assert.DoesNotContain("video", result);
    }

    [Fact]
    public void Returns_Empty_When_No_Words_Match()
    {
        var finder = new WordFinder(ValidMatrix);
        var stream = new List<string> { "banana", "kiwi", "mango" };

        var result = finder.Find(stream);

        Assert.Empty(result);
    }

    [Fact]
    public void Ignores_Duplicates_In_Wordstream()
    {
        var finder = new WordFinder(ValidMatrix);
        var stream = new List<string> { "cold", "cold", "cold", "wind" };

        var result = finder.Find(stream).ToList();

        Assert.Equal(2, result.Count); // only "cold" and "wind"
    }

    [Fact]
    public void Returns_Top_10_Matches_If_More_Found()
    {
        // Create a 64x16 matrix where each row includes a known word like "word00", "word01"
        var matrix = Enumerable.Range(0, 20)
            .Select(i => $"word{i:D2}".PadRight(16, 'x')) // Ensure all rows are 16 chars
            .Concat(Enumerable.Repeat("xxxxxxxxxxxxxxxx", 44)) // Pad up to 64 rows
            .ToList();

        // Add all 20 known words to the wordstream
        var stream = Enumerable.Range(0, 20)
            .Select(i => $"word{i:D2}")
            .ToList();

        var finder = new WordFinder(matrix);
        var result = finder.Find(stream).ToList();

        // We should only get 10 matches (limit)
        Assert.Equal(10, result.Count);

        // All results must be from the known word list
        foreach (var word in result)
        {
            Assert.Contains(word, stream);
        }
    }

    [Fact]
    public void Throws_If_Matrix_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new WordFinder(null));
    }

    [Fact]
    public void Throws_If_Matrix_Has_Inconsistent_Row_Lengths()
    {
        var badMatrix = new List<string>
        {
            "abcd",
            "efg",   // shorter row
            "hijk"
        };

        Assert.Throws<ArgumentException>(() => new WordFinder(badMatrix));
    }

    [Fact]
    public void Throws_If_Wordstream_Is_Null()
    {
        var finder = new WordFinder(ValidMatrix);
        Assert.Throws<ArgumentNullException>(() => finder.Find(null));
    }

    [Fact]
    public void Is_Case_Insensitive()
    {
        var finder = new WordFinder(ValidMatrix);
        var stream = new List<string> { "COLD", "Wind", "ChIlL" };

        var result = finder.Find(stream).ToList();

        Assert.Contains("COLD", result);
        Assert.Contains("Wind", result);
        Assert.Contains("ChIlL", result);
    }
}