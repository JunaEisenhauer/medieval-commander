using System.IO;
using System.Runtime.CompilerServices;
using StrategyGame.Model.IService;

namespace StrategyGame.Service
{
    public class FileResourceService : IFileResourceService
    {
        private readonly string root;

        public FileResourceService(string root)
        {
            this.root = root;
        }

        public static string GetSourceDirectory([CallerFilePath] string sourcePath = "")
        {
            return Path.GetFullPath(@"..\..\");
        }

        public bool Exists(string name)
        {
            return File.Exists(GetFileName(name));
        }

        public Stream Open(string name)
        {
            return File.OpenRead(GetFileName(name));
        }

        public string GetFileName(string name)
        {
            return Path.Combine(root, name);
        }
    }
}
