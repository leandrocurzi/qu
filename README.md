# WordFinder

**WordFinder** is a high-performance, case-insensitive word search engine built in C#. It scans a character matrix (up to 64x64) to find words from a given stream. Words may appear horizontally (left-to-right) or vertically (top-to-bottom). It returns the top 10 most frequently matched words, counting each word only once regardless of how many times it appears in the stream.

## Features

- Supports large word streams with efficient lookups
- Horizontal and vertical word matching
- Case-insensitive matching
- Top 10 most repeated matches returned
- Safe handling of invalid or malformed input
- Well-tested with xUnit
- Benchmark-ready structure

## Project Structure

WordFinder/
├── WordFinder.Core/ # Class library with main implementation
├── WordFinder.Tests/ # xUnit test project
├── WordFinder.CLI/ # Console runner (optional)
├── WordFinder.sln # Solution file
└── README.md # Project documentation

## Constraints
- Matrix size must not exceed 64 rows or 64 characters per row.
- All matrix rows must be non-null and of equal length.
- Only horizontal (left-to-right) and vertical (top-to-bottom) searches are supported.

## Running the Solution

Build the project:
dotnet build

Run tests:
dotnet test

Run the CLI:
dotnet run --project WordFinder.CLI

## Performance Considerations
Efficient Matrix Search Strategy
The core performance optimization in this project comes from preprocessing the matrix once, and scanning it efficiently during lookups.

Preprocessing:
Lines are stored as strings in a HashSet<string>
This is done only once, during class initialization

Word Lookup:
The wordstream is first deduplicated using HashSet<string> to avoid redundant work
Each word is then checked using .Contains(...) against the matrix lines
The comparison uses StringComparison.OrdinalIgnoreCase for performance and correctness