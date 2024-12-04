using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please specify the day to run (e.g., dotnet run -- 1)");
            return;
        }

        string className = $"Day{args[0]}";
        Type? dayType = Type.GetType(className);

        if (dayType == null)
        {
            Console.WriteLine($"Class {className} not found");
            return;
        }

        MethodInfo? mainMethod = dayType.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);

        if (mainMethod == null)
        {
            Console.WriteLine($"Run method not found in class {className}");
            return;
        }

        mainMethod.Invoke(null, null);
    }
}