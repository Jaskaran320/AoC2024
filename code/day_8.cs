class Point {
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y) {
        X = x;
        Y = y;
    }

    public override bool Equals(object? obj) {
        if (obj is Point other)
            return X == other.X && Y == other.Y;
        return false;
    }

    public override int GetHashCode() {
        return HashCode.Combine(X, Y);
    }
}

class Antenna {
    public Point Position { get; set; }
    public char Frequency { get; set; }

    public Antenna(Point position, char frequency) {
        Position = position;
        Frequency = frequency;
    }
}

class Day8 {
    public static void Run() {
        const string FILE_PATH = "data/day_8.txt";
        var lines = File.ReadAllLines(FILE_PATH);
        var antennas = ParseInput(lines);
        var maxX = lines[0].Length;
        var maxY = lines.Length;
        var antinodes = CalculateAntinodes(antennas, maxX, maxY);
        Console.WriteLine($"Number of antinodes: {antinodes.Count}");
        var antinodesPartTwo = CalculateAntinodesforCollinear(antennas, maxX, maxY);
        Console.WriteLine($"Number of antinodes now: {antinodesPartTwo.Count}");
    }

    private static List<Antenna> ParseInput(string[] lines) {
        var antennas = new List<Antenna>();
        for (int y = 0; y < lines.Length; y++) {
            for (int x = 0; x < lines[y].Length; x++) {
                char c = lines[y][x];
                if (char.IsLetterOrDigit(c)) {
                    antennas.Add(new Antenna(new Point(x, y), c));
                }
            }
        }
        return antennas;
    }

    private static bool IsWithinBounds(Point p, int maxX, int maxY) {
        return p.X >= 0 && p.X < maxX && p.Y >= 0 && p.Y < maxY;
    }

    private static HashSet<Point> CalculateAntinodes(List<Antenna> antennas, int maxX, int maxY) {
        var antinodes = new HashSet<Point>();
        var frequencyGroups = antennas.GroupBy(a => a.Frequency);

        foreach (var group in frequencyGroups) {
            var sameFreqAntennas = group.ToList();
            for (int i = 0; i < sameFreqAntennas.Count; i++) {
                for (int j = i + 1; j < sameFreqAntennas.Count; j++) {
                    var a1 = sameFreqAntennas[i];
                    var a2 = sameFreqAntennas[j];

                    int dx = a2.Position.X - a1.Position.X;
                    int dy = a2.Position.Y - a1.Position.Y;

                    var antinode1 = new Point(a1.Position.X - dx, a1.Position.Y - dy);
                    var antinode2 = new Point(a2.Position.X + dx, a2.Position.Y + dy);

                    if (IsWithinBounds(antinode1, maxX, maxY))
                        antinodes.Add(antinode1);
                    if (IsWithinBounds(antinode2, maxX, maxY))
                        antinodes.Add(antinode2);
                }
            }
        }
        return antinodes;
    }

    private static bool IsCollinear(Point p1, Point p2, Point p3) {
        int area = p1.X * (p2.Y - p3.Y) + 
                   p2.X * (p3.Y - p1.Y) + 
                   p3.X * (p1.Y - p2.Y);
        return area == 0;
    }

    private static HashSet<Point> CalculateAntinodesforCollinear(List<Antenna> antennas, int maxX, int maxY) {
        var antinodes = new HashSet<Point>();
        var frequencyGroups = antennas.GroupBy(a => a.Frequency);

        foreach (var group in frequencyGroups) {
            var sameFreqAntennas = group.ToList();
            if (sameFreqAntennas.Count < 2) continue;

            for (int y = 0; y < maxY; y++) {
                for (int x = 0; x < maxX; x++) {
                    var point = new Point(x, y);

                    for (int i = 0; i < sameFreqAntennas.Count; i++) {
                        for (int j = i + 1; j < sameFreqAntennas.Count; j++) {
                            if (IsCollinear(point, 
                                          sameFreqAntennas[i].Position,
                                          sameFreqAntennas[j].Position)) {
                                antinodes.Add(point);
                                goto NextPoint;
                            }
                        }
                    }
                    NextPoint: continue;
                }
            }
        }
        return antinodes;
    }
}