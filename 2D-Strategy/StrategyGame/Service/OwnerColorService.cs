using System.Drawing;
using StrategyGame.Model.IService;

namespace StrategyGame.Service
{
    public class OwnerColorService : IOwnerColorService
    {
        public Color GetOwnerColor(int owner)
        {
            switch (owner)
            {
                case 0:
                    return Color.Blue;
                case 1:
                    return Color.Red;
                default:
                    return Color.White;
            }
        }
    }
}
