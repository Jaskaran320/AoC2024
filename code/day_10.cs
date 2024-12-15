class Day10 {
    private static readonly (int dx, int dy)[] Directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];

    public static void Run() {
        const string FILE_PATH = "data/day_10.txt";
        int[][] map = File.ReadAllLines(FILE_PATH)
            .Select(line => line.Select(c => c - '0').ToArray())
            .ToArray();
        
        var trailheads = FindTrailheads(map);
        int totalScore = 0;
        
        foreach (var (startX, startY) in trailheads) {
            int score = CalculateTrailheadScore(map, startX, startY);
            totalScore += score;
        }
        
        Console.WriteLine($"Sum of trailhead scores: {totalScore}");

        int totalRating = 0;
        foreach (var (startX, startY) in trailheads) {
            int rating = CalculateTrailheadRating(map, startX, startY);
            totalRating += rating;
        }
        Console.WriteLine($"Sum of trailhead ratings: {totalRating}");
    }

    private static List<(int x, int y)> FindTrailheads(int[][] map) {
        var trailheads = new List<(int x, int y)>();
        int height = map.Length;
        int width = map[0].Length;
        
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (map[y][x] == 0) {
                    trailheads.Add((x, y));
                }
            }
        }
        
        return trailheads;
    }

    private static int CalculateTrailheadScore(int[][] map, int startX, int startY) {
        var reachableNines = new HashSet<(int x, int y)>();
        var visited = new HashSet<(int x, int y)>();
        
        void DFS(int x, int y, int currentHeight) {
            if (map[y][x] == 9) {
                reachableNines.Add((x, y));
                return;
            }
            
            foreach (var (dx, dy) in Directions) {
                int newX = x + dx;
                int newY = y + dy;
                
                if (IsValidPosition(map, newX, newY) && 
                    !visited.Contains((newX, newY)) && 
                    map[newY][newX] == currentHeight + 1) {
                    
                    visited.Add((newX, newY));
                    DFS(newX, newY, currentHeight + 1);
                }
            }
        }
        
        visited.Add((startX, startY));
        DFS(startX, startY, 0);
        return reachableNines.Count;
    }
    
    private static bool IsValidPosition(int[][] map, int x, int y) {
        return y >= 0 && y < map.Length && x >= 0 && x < map[0].Length;
    }

    private static int CalculateTrailheadRating(int[][] map, int startX, int startY) {
        var uniquePaths = new HashSet<string>();
        var currentPath = new List<(int x, int y)>();

        void BacktrackPaths(int x, int y, int currentHeight) {
            currentPath.Add((x, y));

            if (map[y][x] == 9) {
                uniquePaths.Add(SerializePath(currentPath));
                currentPath.RemoveAt(currentPath.Count - 1);
                return;
            }

            foreach (var (dx, dy) in Directions) {
                int newX = x + dx;
                int newY = y + dy;

                if (IsValidPosition(map, newX, newY) && 
                    !currentPath.Contains((newX, newY)) && 
                    map[newY][newX] == currentHeight + 1) {
                    BacktrackPaths(newX, newY, currentHeight + 1);
                }
            }

            currentPath.RemoveAt(currentPath.Count - 1);
        }

        BacktrackPaths(startX, startY, 0);
        return uniquePaths.Count;
    }

    private static string SerializePath(List<(int x, int y)> path) {
        return string.Join(",", path.Select(p => $"{p.x},{p.y}"));
    }
}