namespace temp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryName = Path.GetFileName(currentDirectory);
            var relativePath = directoryName.StartsWith("net6.0") ? @"../../../" : string.Empty;
        }
    }
}
