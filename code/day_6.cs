class Day6 {
    public static void Run() {
        const string FILE_PATH = "data/day_6.txt";

        var initialMap = new List<string>(File.ReadAllLines(FILE_PATH));
        int mapRows = initialMap.Count;
        int mapCols = initialMap[0].Length;

        int[] dx = [0, 1, 0, -1];
        int[] dy = [-1, 0, 1, 0];
        char[] directionSymbol = ['^'];
        int currentDirection = -1;

        int startX = -1, startY = -1;
        for (int i = 0; i < mapRows; i++) {
            int idx = initialMap[i].IndexOfAny(directionSymbol);
            if (idx != -1) {
                startX = idx;
                startY = i;
                currentDirection = Array.IndexOf(directionSymbol, initialMap[i][idx]);
                break;
            }
        }

        if (startX == -1 || startY == -1 || currentDirection == -1) {
            Console.WriteLine("Guard's starting position not found.");
            return;
        }

        var visitedCells = new HashSet<(int, int)> {
            (startX, startY)
        };

        int guardX = startX;
        int guardY = startY;

        while (true) {
            int nextX = guardX + dx[currentDirection];
            int nextY = guardY + dy[currentDirection];

            if (nextY < 0 || nextY >= mapRows || nextX < 0 || nextX >= mapCols)
                break;

            char nextCell = initialMap[nextY][nextX];

            if (nextCell == '#') {
                currentDirection = (currentDirection + 1) % 4;
            }
            else {
                guardX = nextX;
                guardY = nextY;
                visitedCells.Add((guardX, guardY));
            }
        }

        Console.WriteLine($"Total distinct cells visited: {visitedCells.Count}");

        var charMap = File.ReadAllLines(FILE_PATH).Select(line => line.ToCharArray()).ToList();
        char[] allDirections = ['^', '>', 'v', '<'];
        int guardDirection = -1;

        for (int row = 0; row < mapRows && guardDirection == -1; row++) {
            for (int col = 0; col < mapCols; col++) {
                char cell = charMap[row][col];
                int dirIndex = Array.IndexOf(allDirections, cell);
                if (dirIndex != -1) {
                    startX = col;
                    startY = row;
                    guardDirection = dirIndex;
                    break;
                }
            }
        }

        if (guardDirection == -1) {
            Console.WriteLine("Guard's starting position not found.");
            return;
        }

        int obstructionCount = 0;
        for (int row = 0; row < mapRows; row++) {
            for (int col = 0; col < mapCols; col++) {
                if (charMap[row][col] == '.' || charMap[row][col] == ' ') {
                    charMap[row][col] = '#';
                    if (GuardGetsStuck(charMap, startX, startY, guardDirection, dx, dy)) {
                        obstructionCount++;
                    }
                    charMap[row][col] = '.';
                }
            }
        }

        Console.WriteLine($"Number of positions to place an obstruction: {obstructionCount}");
    }

    private static bool GuardGetsStuck(List<char[]> map, int startX, int startY, int startDirection, int[] dx, int[] dy) {
        int rows = map.Count;
        int cols = map[0].Length;

        var visitedStates = new HashSet<(int x, int y, int direction)>();
        int currentX = startX;
        int currentY = startY;
        int currentDirection = startDirection;

        while (true) {
            var state = (currentX, currentY, currentDirection);
            if (visitedStates.Contains(state)) {
                return true;
            }
            visitedStates.Add(state);

            int nextX = currentX + dx[currentDirection];
            int nextY = currentY + dy[currentDirection];

            if (nextY < 0 || nextY >= rows || nextX < 0 || nextX >= cols) {
                return false;
            }

            char nextCell = map[nextY][nextX];

            if (nextCell == '#') {
                currentDirection = (currentDirection + 1) % 4;
            }
            else {
                currentX = nextX;
                currentY = nextY;
            }
        }
    }
}