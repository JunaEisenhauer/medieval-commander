using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace StrategyGame.Model.Menu
{
    public interface IMenuEntityFactory : IEntityFactory
    {
        IEntity CreateBackground(string name);

        IEntity CreateButton(string text, string name, Vector2 position, Vector2 size, string scene);
    }
}
