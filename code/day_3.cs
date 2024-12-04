using System.Text.RegularExpressions;

class Day3 {
    public static void Run() {
        
        int res = 0;
        bool flag = true;
        const string FILE_PATH = "data/day_3.txt";
        string fileContent = File.ReadAllText(FILE_PATH);

        string pattern = @"(do\(\))|(don't\(\))|mul\((\d{1,3}),(\d{1,3})\)";
        Regex regex = new(pattern);

        MatchCollection matches = regex.Matches(fileContent);

        foreach (Match match in matches) {
            if (match.Value == "do()") {
                flag = true;
            }
            else if (match.Value == "don't()") {
                flag = false;
            }
            else if (flag) {
                int a = int.Parse(match.Groups[3].Value);
                int b = int.Parse(match.Groups[4].Value);
                res += a * b;
            }
        }

        Console.WriteLine($"Result: {res}");
    }
}