class Day4 {
    public static void Run() {
        const string FILE_PATH = "data/day_4.txt";
        string[] lines = File.ReadAllLines(FILE_PATH);
        int count = CountOccurrences(lines, "XMAS");
        Console.WriteLine($"Total occurrences of 'XMAS': {count}");
        count = CountXMasOccurrences(lines);
        Console.WriteLine($"Total occurrences of X-MAS patterns: {count}");
    }

    private static int CountOccurrences(string[] lines, string word) {
        int count = 0;
        int rows = lines.Length;
        int cols = lines[0].Length;

        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++) {
                if (lines[r][c] == word[0]) {
                    count += CountWordInAllDirections(lines, word, r, c);
                }
            }
        }
        return count;
    }

    private static int CountWordInAllDirections(string[] lines, string word, int row, int col) {
        int count = 0;
        int[][] directions = [
            [0, 1],
            [1, 0],
            [1, 1],
            [1, -1],
            [0, -1],
            [-1, 0],
            [-1, -1],
            [-1, 1]
        ];

        foreach (var dir in directions) {
            if (IsWordInDirection(lines, word, row, col, dir[0], dir[1])) {
                count++;
            }
        }
        return count;
    }

    private static bool IsWordInDirection(string[] lines, string word, int row, int col, int rowDir, int colDir) {
        int wordLength = word.Length;
        int rows = lines.Length;
        int cols = lines[0].Length;

        for (int i = 0; i < wordLength; i++) {
            int newRow = row + i * rowDir;
            int newCol = col + i * colDir;

            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || lines[newRow][newCol] != word[i]) {
                return false;
            }
        }
        return true;
    }

    private static int CountXMasOccurrences(string[] lines) {
        int count = 0;
        int rows = lines.Length;
        int cols = lines[0].Length;

        for (int r = 1; r < rows - 1; r++) {
            for (int c = 1; c < cols - 1; c++) {
                if (IsXMasPattern(lines, r, c)) {
                    count++;
                }
            }
        }
        return count;
    }

    private static bool IsXMasPattern(string[] lines, int row, int col) {
        return CheckDiagonalPair(lines, row, col, 1, 1, -1, 1) || 
               CheckDiagonalPair(lines, row, col, -1, -1, 1, -1) ||
               CheckDiagonalPair(lines, row, col, 1, 1, 1, -1) ||
               CheckDiagonalPair(lines, row, col, -1, -1, -1, 1);
    }

    private static bool CheckDiagonalPair(string[] lines, int row, int col, 
        int dir1Row, int dir1Col, int dir2Row, int dir2Col) {

        const string PATTERN = "MAS";
        const string REVERSE_PATTERN = "SAM";

        string diag1 = GetDiagonalString(lines, row, col, dir1Row, dir1Col);
        string diag2 = GetDiagonalString(lines, row, col, dir2Row, dir2Col);
        
        return (diag1 == PATTERN && diag2 == PATTERN) ||
               (diag1 == PATTERN && diag2 == REVERSE_PATTERN) ||
               (diag1 == REVERSE_PATTERN && diag2 == PATTERN) ||
               (diag1 == REVERSE_PATTERN && diag2 == REVERSE_PATTERN);
    }

    private static string GetDiagonalString(string[] lines, int row, int col, 
        int rowDir, int colDir) {
        char[] result = new char[3];
        for (int i = -1; i <= 1; i++) {
            int r = row + (i * rowDir);
            int c = col + (i * colDir);
            if (r < 0 || r >= lines.Length || c < 0 || c >= lines[0].Length) {
                return "";
            }
            result[i + 1] = lines[r][c];
        }
        return new string(result);
    }

}