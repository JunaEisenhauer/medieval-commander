using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace StrategyGame.Model.Game
{
    public interface IGameEntityFactory : IEntityFactory
    {
        IEntity CreateTile(Vector2 position, string layer, int gid);

        IEntity CreateAnimatedTile(Vector2 position, string layer, int[] gids, float animationSpeed);

        IEntity CreateArcher(Vector2 position, int owner, int goldValue);

        IEntity CreateKnight(Vector2 position, int owner, int goldValue);

        IEntity CreateCatapult(Vector2 position, int owner, int goldValue);

        IEntity CreateTower(Vector2 position, int owner, int goldValue);

        IEntity CreateFortress(Vector2 position, int owner, int goldValue);

        IEntity CreateSpawnpoint(Vector2 position, int owner);

        IEntity CreateSelect(IEntity target);

        IEntity CreateRangeDisplay(IEntity entity, float range);

        IEntity CreateHealthBackground(IEntity target);

        IEntity CreateHealthDisplay(IEntity target);

        IEntity CreateProjectile(IEntity shooter, IEntity target, Vector2 size, float attackDamage);

        IEntity CreateDamageAnimation(IEntity entity);

        IEntity CreateDestiantionFlag(Vector2 position);

        IEntity CreateMouseBox(Vector2 startPoint, Vector2 endPoint);

        IEntity CreateBuyButton(Vector2 position, Vector2 size, string texture, Action callback);

        IEntity CreateGoldDisplay();

        IEntity CreateIngot(Vector2 position, Vector2 size);

        IEntity CreateTextDisplay(Vector2 position, Vector2 size, string text, Color color);
    }
}
