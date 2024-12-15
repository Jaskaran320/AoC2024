class Day9 {
    public static void Run() {
        const string FILE_PATH = "data/day_9.txt";
        var input = File.ReadAllText(FILE_PATH).Trim();
        var lengths = input.Select(c => int.Parse(c.ToString())).ToList();
        var disk = CreateInitialDisk(lengths);
        var compactedDisk = CompactDisk(disk);
        long checksum = CalculateChecksum(compactedDisk);
        Console.WriteLine($"Checksum: {checksum}");

        disk = CreateInitialDisk(lengths);
        var newCompactedDisk = CompactDiskWholeFiles(disk);
        long newChecksum = CalculateChecksum(newCompactedDisk);
        Console.WriteLine($"New Checksum: {newChecksum}");

    }

    private static List<int> CreateInitialDisk(List<int> lengths) {
        var disk = new List<int>();
        int fileId = 0;
        
        for (int i = 0; i < lengths.Count; i++) {
            int length = lengths[i];
            if (i % 2 == 0) {
                for (int j = 0; j < length; j++) {
                    disk.Add(fileId);
                }
                fileId++;
            }
            else {
                for (int j = 0; j < length; j++) {
                    disk.Add(-1);
                }
            }
        }
        return disk;
    }

    private static List<int> CompactDisk(List<int> disk) {
        bool moved;

        while (true) {
            moved = false;
            for (int i = disk.Count - 1; i > 0; i--) {
                if (disk[i] >= 0) {
                    int freeSpaceIndex = FindLeftmostFreeSpace(disk, i);
                    if (freeSpaceIndex != -1) {
                        disk[freeSpaceIndex] = disk[i];
                        disk[i] = -1;
                        moved = true;
                        break;
                    }
                }
            }
            if (!moved) {
                break;
            }
        }
        
        return disk;
    }

    private static int FindLeftmostFreeSpace(List<int> disk, int beforeIndex) {
        for (int i = 0; i < beforeIndex; i++) {
            if (disk[i] == -1) {
                return i;
            }
        }
        return -1;
    }

    private static long CalculateChecksum(List<int> disk) {
        long sum = 0;
        for (int i = 0; i < disk.Count; i++) {
            if (disk[i] >= 0) {
                sum += (long)i * disk[i];
            }
        }
        return sum;
    }

    private static List<int> CompactDiskWholeFiles(List<int> disk) {
        int maxFileId = disk.Max();
        
        for (int fileId = maxFileId; fileId >= 0; fileId--) {
            int fileStart = disk.IndexOf(fileId);
            if (fileStart == -1) continue;
            
            int fileSize = GetFileSize(disk, fileStart, fileId);
            int bestFreeSpace = FindBestFreeSpace(disk, fileStart, fileSize);
            
            if (bestFreeSpace != -1) {
                MoveFile(disk, fileStart, fileSize, bestFreeSpace);
            }
        }
        
        return disk;
    }

    private static int GetFileSize(List<int> disk, int start, int fileId) {
        int size = 0;
        for (int i = start; i < disk.Count && disk[i] == fileId; i++) {
            size++;
        }
        return size;
    }

    private static int FindBestFreeSpace(List<int> disk, int fileStart, int neededSize) {
        int currentFreeStart = -1;
        int currentFreeSize = 0;
        
        for (int i = 0; i < fileStart; i++) {
            if (disk[i] == -1) {
                if (currentFreeStart == -1) {
                    currentFreeStart = i;
                }
                currentFreeSize++;
                
                if (currentFreeSize >= neededSize) {
                    return currentFreeStart;
                }
            } else {
                currentFreeStart = -1;
                currentFreeSize = 0;
            }
        }
        return -1;
    }
    private static void MoveFile(List<int> disk, int start, int size, int newStart) {
        int fileId = disk[start];

        for (int i = 0; i < size; i++) {
            disk[newStart + i] = fileId;
        }

        for (int i = 0; i < size; i++) {
            disk[start + i] = -1;
        }
    }
}