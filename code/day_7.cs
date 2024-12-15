class Day7 {
    private enum Operator { Add, Multiply, Concatenate }

    public static void Run() {
        string FILE_PATH = "data/day_7.txt";
        var lines = File.ReadAllLines(FILE_PATH);
        long sum = 0;
        long sumWithConcat = 0;

        foreach (var line in lines) {
            var parts = line.Split(':');
            var testValue = long.Parse(parts[0].Trim());
            var numbers = parts[1].Trim().Split(' ').Select(long.Parse).ToList();
            
            if (CanMakeTarget(numbers, testValue)) {
                sum += testValue;
            }
            if (CanMakeTargetWithConcat(numbers, testValue)) {
                sumWithConcat += testValue;
            }
        }

        Console.WriteLine($"Sum of valid test values: {sum}");
        Console.WriteLine($"Sum with concatenation: {sumWithConcat}");

    }

    private static bool CanMakeTarget(List<long> numbers, long target) {
        int operatorCount = numbers.Count - 1;
        int combinations = 1 << operatorCount;

        for (int i = 0; i < combinations; i++) {
            long result = numbers[0];
            
            for (int j = 0; j < operatorCount; j++) {
                bool isMultiply = (i & (1 << j)) != 0;
                long nextNum = numbers[j + 1];
                
                if (isMultiply) {
                    result *= nextNum;
                } else {
                    result += nextNum;
                }
            }

            if (result == target) {
                return true;
            }
        }

        return false;
    }

    private static bool CanMakeTargetWithConcat(List<long> numbers, long target) {
        int operatorCount = numbers.Count - 1;
        int operatorTypes = 3; // +, *, ||
        long maxPower = (long)Math.Pow(operatorTypes, operatorCount);

        for (long i = 0; i < maxPower; i++) {
            long result = numbers[0];
            long temp = i;
            
            for (int j = 0; j < operatorCount; j++) {
                int operatorType = (int)(temp % operatorTypes);
                temp /= operatorTypes;
                long nextNum = numbers[j + 1];

                switch ((Operator)operatorType) {
                    case Operator.Add:
                        result += nextNum;
                        break;
                    case Operator.Multiply:
                        result *= nextNum;
                        break;
                    case Operator.Concatenate:
                        result = ConcatenateNumbers(result, nextNum);
                        break;
                }
            }

            if (result == target) return true;
        }
        
        return false;
    }

    private static long ConcatenateNumbers(long a, long b) {
        long bCopy = b;
        while (bCopy > 0) {
            a *= 10;
            bCopy /= 10;
        }
        return a + b;
    }
}