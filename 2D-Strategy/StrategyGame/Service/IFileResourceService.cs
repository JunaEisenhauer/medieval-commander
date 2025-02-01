using System.IO;

namespace StrategyGame.Model.IService
{
    public interface IFileResourceService : IService
    {
        bool Exists(string name);

        Stream Open(string name);

        string GetFileName(string name);
    }
}
