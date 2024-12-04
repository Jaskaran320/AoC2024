class Day1 {
    public static void Run() {
        const string FILE_PATH = "data/day_1.txt";
        List<int> firstColumn = [];
        List<int> secondColumn = [];

        foreach (var line in File.ReadLines(FILE_PATH)) {
            var numbers = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
            if (numbers.Length == 2) {
                firstColumn.Add(int.Parse(numbers[0]));
                secondColumn.Add(int.Parse(numbers[1]));
            }
        }

        firstColumn.Sort();
        secondColumn.Sort();

        int sumOfAbsoluteDifferences = 0;
        for (int i = 0; i < firstColumn.Count; i++) {
            sumOfAbsoluteDifferences += Math.Abs(firstColumn[i] - secondColumn[i]);
        }

        Console.WriteLine($"Sum of absolute differences: {sumOfAbsoluteDifferences}");

        Dictionary<int, int> secondColumnFrequency = secondColumn.GroupBy(x => x)
                                                                 .ToDictionary(g => g.Key, g => g.Count());

        int totalSimilarityScore = 0;
        foreach (var number in firstColumn) {
            if (secondColumnFrequency.TryGetValue(number, out int count)) {
                totalSimilarityScore += number * count;
            }
        }

        Console.WriteLine($"Total similarity score: {totalSimilarityScore}");

    }
}