public class Rule(int before, int after) {
    public int Before { get; } = before;
    public int After { get; } = after;
}

public class Update(string line) {
    public List<int> Pages { get; } = line.Split(',').Select(int.Parse).ToList();

    public int GetMiddleNumber() {
        return Pages[Pages.Count / 2];
    }
}

class Day5 {

    private static List<Rule> ParseRules(string[] lines) {
        var rules = new List<Rule>();
        int lineIndex = 0;
        
        while (!string.IsNullOrEmpty(lines[lineIndex])) {
            var parts = lines[lineIndex].Split('|');
            rules.Add(new Rule(int.Parse(parts[0]), int.Parse(parts[1])));
            lineIndex++;
        }
        return rules;
    }

    private static List<Update> ParseUpdates(string[] lines) {
        var updates = new List<Update>();
        int lineIndex = 0;

        while (!string.IsNullOrEmpty(lines[lineIndex])) {
            lineIndex++;
        }
        lineIndex++;

        while (lineIndex < lines.Length) {
            if (!string.IsNullOrEmpty(lines[lineIndex])) {
                updates.Add(new Update(lines[lineIndex]));
            }
            lineIndex++;
        }
        return updates;
    }

    private static bool IsValidUpdate(Update update, List<Rule> rules) {
        foreach (var rule in rules) {
            if (update.Pages.Contains(rule.Before) && update.Pages.Contains(rule.After)) {
                int beforeIndex = update.Pages.IndexOf(rule.Before);
                int afterIndex = update.Pages.IndexOf(rule.After);
                if (beforeIndex > afterIndex) {
                    return false;
                }
            }
        }
        return true;
    }

    private static List<Update> CorrectInvalidUpdates(List<Update> updates, List<Rule> rules) {
        
        var orderedUpdates = new List<Update>();
        foreach (var update in updates) {
            if (!IsValidUpdate(update, rules)) {

                for (int i = 0; i < update.Pages.Count - 1; i++) {
                    for (int j = 0; j < update.Pages.Count - i - 1; j++) {
                        foreach (var rule in rules) {
                            if (update.Pages[j] == rule.Before && update.Pages[j + 1] == rule.After) {
                                (update.Pages[j], update.Pages[j + 1]) = (update.Pages[j + 1], update.Pages[j]);
                            }
                        }
                    }
                }
                orderedUpdates.Add(update);
            }
        }
        return orderedUpdates;
    }

    public static void Run() {
        const string FILE_PATH = "data/day_5.txt";
        string[] lines = File.ReadAllLines(FILE_PATH);
        
        var rules = ParseRules(lines);
        var updates = ParseUpdates(lines);

        Console.WriteLine($"Number of rules: {rules.Count}");
        Console.WriteLine($"Number of updates: {updates.Count}");
        
        int sum1 = updates
            .Where(update => IsValidUpdate(update, rules))
            .Sum(update => update.GetMiddleNumber());
            
        Console.WriteLine($"Sum of middle numbers of valid updates: {sum1}");

        int sum2 = CorrectInvalidUpdates(updates, rules)
            .Sum(update => update.GetMiddleNumber());

        Console.WriteLine($"Sum of middle numbers of invalid updates: {sum2}");
    }
}
