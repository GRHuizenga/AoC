using Core;

namespace _2022
{
    public class Day07 : Day<double>
    {
        private readonly Directory Dir = new Directory(null);

        public Day07(int year, string fileName) : base(year, fileName)
        {
            ConstructDirectoryTree();
            CalculateSizes(Dir);
        }

        public override double PartOne()
        {
            return SumDirectoriesUnder100K(Dir);
        }

        public override double PartTwo()
        {
            return FindSmallestDirectoryToDelete(30000000 - (70000000 - Dir.Size), Dir);
        }

        private double SumDirectoriesUnder100K(Directory dir)
        {
            return dir.Folders.Values.Select(SumDirectoriesUnder100K).Sum() + (dir.Size > 100000 ? 0 : dir.Size);
        }

        private double FindSmallestDirectoryToDelete(double requiredToFree, Directory directory)
        {
            if (directory.Folders.Count == 0) return directory.Size;
            var candidates = directory.Folders.Values
                    .Where(dir => dir.Size >= requiredToFree)
                    .Select(dir => FindSmallestDirectoryToDelete(requiredToFree, dir));

            return candidates.Count() > 0 ? candidates.Min() : directory.Size;
        }

        private void ConstructDirectoryTree()
        {
            InputLines.Skip(1).Aggregate(Dir, Parse);
        }

        private double CalculateSizes(Directory dir)
        {
            dir.Size = dir.Files.Sum(file => file.Value) + dir.Folders.Values.Sum(CalculateSizes);
            return dir.Size;
        }

        private Directory Parse(Directory currentDirectory, string line)
        {
            if (line.IsCommand())
            {
                if (line.Equals("$ ls")) return currentDirectory;
                var folder = line.Contains("..") ? currentDirectory.Parent : currentDirectory.Folders[line.Split(' ')[2]];
                return folder ?? throw new Exception("Folder should exist!");
            }

            if (line.IsDirectory())
                currentDirectory.AddFolder(line);
            else
                currentDirectory.AddFile(line);

            return currentDirectory;
        }

        private class Directory
        {
            public readonly Directory? Parent = null;
            public readonly Dictionary<string, Directory> Folders = new();
            public readonly Dictionary<string, double> Files = new();
            public double Size = 0;

            public Directory(Directory? parent)
            {
                Parent = parent;
            }

            public void AddFolder(string folder)
            {
                var x = folder.Split(' ');
                Folders.Add(x[1], new Directory(this));
            }

            public void AddFile(string file)
            {
                var x = file.Split(" ");
                var fileSize = double.Parse(x[0]);
                Files.Add(x[1], fileSize);
            }
        }
    }

    public static class Day07Extensions
    {
        public static bool IsCommand(this string @this) => @this.StartsWith("$");
        public static bool IsDirectory(this string @this) => @this.StartsWith("dir");
    }
}
