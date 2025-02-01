using System.Drawing;

namespace StrategyGame.Model.IService
{
    public interface IOwnerColorService : IService
    {
        Color GetOwnerColor(int owner);
    }
}
