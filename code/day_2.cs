class Day2 {

    private static bool IsSafe(List<int> levels) {
        bool isIncreasing = levels.Zip(levels.Skip(1), (a, b) => a < b).All(x => x);
        bool isDecreasing = levels.Zip(levels.Skip(1), (a, b) => a > b).All(x => x);
        bool isValidDifference = levels.Zip(levels.Skip(1), (a, b) => Math.Abs(a - b)).All(diff => diff >= 1 && diff <= 3);

        return (isIncreasing || isDecreasing) && isValidDifference;
    }

    public static void Run() {
        const string FILE_PATH = "data/day_2.txt";
        
        List<int> row = [];
        int count = 0;

        foreach (var line in File.ReadLines(FILE_PATH)) {
            List<int> levels = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(int.Parse)
                                   .ToList();

            if (IsSafe(levels)) {
                count++;
            } else {
                for (int i = 0; i < levels.Count; i++) {
                    var modifiedLevels = new List<int>(levels);
                    modifiedLevels.RemoveAt(i);
                    if (IsSafe(modifiedLevels)) {
                        count++;
                        break;
                    }
                }
            }
        }
        Console.WriteLine($"Number of valid rows: {count}");
    }
}