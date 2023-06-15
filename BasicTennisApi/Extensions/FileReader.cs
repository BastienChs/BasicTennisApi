using BasicTennisApi.Interfaces;

namespace BasicTennisApi.Extensions
{
    public class FileReader : IFileReader
    {
        public string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
