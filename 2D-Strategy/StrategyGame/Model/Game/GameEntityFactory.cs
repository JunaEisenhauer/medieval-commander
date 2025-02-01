using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using StrategyGame.Model.Components;
using StrategyGame.Model.Game.Components;
using StrategyGame.Model.IService;
using StrategyGame.Service;
using StrategyGame.Service.AI;

namespace StrategyGame.Model.Game
{
    public class GameEntityFactory : IGameEntityFactory
    {
        private readonly IScene scene;

        public GameEntityFactory(IScene scene)
        {
            this.scene = scene;
        }

        public IEntity CreateTile(Vector2 position, string layer, int gid)
        {
            GameLayer gameLayer;
            try
            {
                GameLayer.TryParse(layer, true, out gameLayer);
            }
            catch (Exception)
            {
                return null;
            }

            IEntity entity = new Entity(scene, position, new Vector2(1), false);

            new DrawTextureComponent(entity, (int)gameLayer, gid);
            return entity;
        }

        public IEntity CreateAnimatedTile(Vector2 position, string layer, int[] gids, float animationSpeed)
        {
            GameLayer gameLayer;
            try
            {
                GameLayer.TryParse(layer, true, out gameLayer);
            }
            catch (Exception)
            {
                return null;
            }

            IEntity entity = new Entity(scene, position, new Vector2(1), false);
            new DrawAnimationComponent(entity, (int)gameLayer, gids, animationSpeed);
            return entity;
        }

        public IEntity CreateArcher(Vector2 position, int owner, int goldValue)
        {
            IEntity entity = new Entity(scene, position, new Vector2(3f), true);
            new OwnerComponent(entity, owner);

            new MoveComponent(entity, 1f);
            new SelectComponent(entity);
            new PathfindingComponent(entity);

            new HealthComponent(entity, 200, 0);
            new HealthDisplayComponent(entity);
            new AttackComponent(entity, 20, 2f, 2.8f, 1.05f, AttackComponent.AttackType.Far, new Dictionary<int, float>() { { 0, 2.5f }, { 1, 3.5f }, { 2, 1.25f } });
            new HealComponent(entity);
            new CollisionComponent(entity);
            new CaptureGoldComponent(entity, goldValue);

            new DrawTextureComponent(entity, (int)GameLayer.GameObject, "Archer-Idle-" + owner);

            new AIComponent(entity, AiType.RangedArmy);

            return entity;
        }

        public IEntity CreateKnight(Vector2 position, int owner, int goldValue)
        {
            IEntity entity = new Entity(scene, position, new Vector2(3f), true);
            new OwnerComponent(entity, owner);

            new MoveComponent(entity, 1f);
            new SelectComponent(entity);
            new PathfindingComponent(entity);

            new HealthComponent(entity, 600, 1);
            new HealthDisplayComponent(entity);
            new AttackComponent(entity, 15, 2f, 0.5f, 1f, AttackComponent.AttackType.Melee, new Dictionary<int, float>() { { 0, 2.5f }, { 1, 2.5f }, { 2, 2.3f } });
            new HealComponent(entity);
            new CollisionComponent(entity);
            new CaptureGoldComponent(entity, goldValue);

            new DrawTextureComponent(entity, (int)GameLayer.GameObject, "Knight-Idle-" + owner);

            new AIComponent(entity, AiType.Army);

            return entity;
        }

        public IEntity CreateCatapult(Vector2 position, int owner, int goldValue)
        {
            IEntity entity = new Entity(scene, position, new Vector2(3f), true);
            new OwnerComponent(entity, owner);

            new MoveComponent(entity, 0.85f);
            new SelectComponent(entity);
            new PathfindingComponent(entity);

            new HealthComponent(entity, 100, 2);
            new HealthDisplayComponent(entity);
            new AttackComponent(entity, 150, 8f, 3f, 1f, AttackComponent.AttackType.Far, new Dictionary<int, float>() { { 0, 0f }, { 1, 0f } });
            new HealComponent(entity);
            new CollisionComponent(entity);
            new CaptureGoldComponent(entity, goldValue);

            new DrawTextureComponent(entity, (int)GameLayer.GameObject, "Catapult-" + owner);

            new AIComponent(entity, Service.AI.AiType.RangedArmy);

            return entity;
        }

        public IEntity CreateTower(Vector2 position, int owner, int goldValue)
        {
            IEntity entity = new Entity(scene, position, new Vector2(5f, 5f), true);
            new OwnerComponent(entity, owner);
            new SelectComponent(entity);

            new HealthComponent(entity, 1000, 3);
            new HealthDisplayComponent(entity);
            new AttackComponent(entity, 50, 2f, 3f, 1f, AttackComponent.AttackType.Far, new Dictionary<int, float>());
            new HealerComponent(entity, 1.2f, 0.1f, 3f);
            new CaptureComponent(entity);
            new CollisionComponent(entity);
            new BuyArmyComponent(entity);
            new CaptureGoldComponent(entity, goldValue);

            new DrawTextureComponent(entity, (int)GameLayer.GameObject, "Tower-" + owner);

            new AIComponent(entity, Service.AI.AiType.Tower);

            return entity;
        }

        public IEntity CreateFortress(Vector2 position, int owner, int goldValue)
        {
            IEntity entity = new Entity(scene, position, new Vector2(7f, 8f), true);
            new OwnerComponent(entity, owner);
            new SelectComponent(entity);

            new HealthComponent(entity, 1200, 4);
            new HealthDisplayComponent(entity);
            new AttackComponent(entity, 55, 2f, 3.5f, 1f, AttackComponent.AttackType.Far, new Dictionary<int, float>());
            new HealerComponent(entity, 2f, 0.1f, 3.5f);
            new WinComponent(entity, owner == 0, 2f);
            new CollisionComponent(entity);
            new BuyArmyComponent(entity);
            new CaptureGoldComponent(entity, goldValue);

            DrawTextureComponent texComp = new DrawTextureComponent(entity, (int)GameLayer.GameObject, "Fortress-" + owner);
            if (owner == 0)
                texComp.Flipped = true;

            new AIComponent(entity, Service.AI.AiType.Fortress);

            return entity;
        }

        public IEntity CreateSpawnpoint(Vector2 position, int owner)
        {
            IEntity entity = new Entity(scene, position, new Vector2(2f, 2f), false);
            new OwnerComponent(entity, owner);
            new SpawnComponent(entity, 30f);

            return entity;
        }

        public IEntity CreateSelect(IEntity target)
        {
            IEntity entity = new Entity(scene, target.Position, target.Size * 1.1f, false);
            target.AddChild(entity);
            new RelativeComponent(entity, target, Vector2.Zero);
            Color color = Color.FromArgb(80, 150, 150, 150);
            new DrawCircleComponent(entity, (int)GameLayer.Select, color);
            return entity;
        }

        public IEntity CreateRangeDisplay(IEntity target, float range)
        {
            IEntity entity = new Entity(scene, target.Position, new Vector2(range * 2), false);
            target.AddChild(entity);
            new RelativeComponent(entity, target, Vector2.Zero);

            Color color = Color.FromArgb(80, 150, 150, 150);
            new DrawCircleComponent(entity, (int)GameLayer.Range, color);

            return entity;
        }

        public IEntity CreateHealthBackground(IEntity target)
        {
            IEntity entity = new Entity(scene, target.Position, new Vector2(1.5f, 0.5f), false);
            target.AddChild(entity);
            new RelativeComponent(entity, target, new Vector2(0f, (-target.Size.Y / 2f) - 1f));

            new DrawQuadComponent(entity, (int)GameLayer.HealthBackground, Color.Black);

            return entity;
        }

        public IEntity CreateHealthDisplay(IEntity target)
        {
            IEntity entity = new Entity(scene, target.Position, new Vector2(0, 0.35f), false);
            target.AddChild(entity);
            new RelativeComponent(entity, target, new Vector2(0f, (-target.Size.Y / 2f) - 1f));

            new DrawQuadComponent(entity, (int)GameLayer.HealthBar, Color.GreenYellow);

            return entity;
        }

        public IEntity CreateProjectile(IEntity shooter, IEntity target, Vector2 size, float attackDamage)
        {
            IEntity entity = new Entity(scene, shooter.Position, size, true);
            new OwnerComponent(entity, shooter.GetComponent<OwnerComponent>().Owner);
            MoveComponent moveComponent = new MoveComponent(entity, 20f) { Destination = target.Position };
            new ProjectileComponent(entity, target, shooter, attackDamage, shooter.GetComponent<AttackComponent>().DamageMultipliers);

            new DrawCircleComponent(entity, (int)GameLayer.Projectile, Color.Black);

            return entity;
        }

        public IEntity CreateDamageAnimation(IEntity target)
        {
            IEntity entity = new Entity(scene, target.Position, target.Size, false);
            target.AddChild(entity);
            new RelativeComponent(entity, target, Vector2.Zero);

            new DrawCircleComponent(entity, (int)GameLayer.Damage, Color.FromArgb(170, 221, 112, 44));

            return entity;
        }

        public IEntity CreateMouseBox(Vector2 startPoint, Vector2 endPoint)
        {
            IEntity entity = new Entity(scene, (startPoint - endPoint) * 0.5f, startPoint - endPoint, false);
            new DrawQuadComponent(entity, (int)GameLayer.Mouse, Color.FromArgb(140, 199, 199, 199));
            return entity;
        }

        public IEntity CreateDestiantionFlag(Vector2 position)
        {
            IEntity entity = new Entity(scene, position, new Vector2(0.8f, 0.8f), false);
            new DrawTextureComponent(entity, (int)GameLayer.Flag, "Flag");
            return entity;
        }

        public IEntity CreateBuyButton(Vector2 position, Vector2 size, string texture, Action callback)
        {
            IEntity entity = new Entity(scene, position, size, false);
            new ButtonComponent(entity, callback);

            new DrawTextureComponent(entity, (int)GameLayer.Ui, texture);

            return entity;
        }

        public IEntity CreateGoldDisplay()
        {
            IEntity entity = new Entity(scene, new Vector2(0f), new Vector2(0f), false);
            new GoldDisplayComponent(entity);

            return entity;
        }

        public IEntity CreateIngot(Vector2 position, Vector2 size)
        {
            IEntity entity = new Entity(scene, position, size, false);
            new DrawTextureComponent(entity, (int)GameLayer.Ui, "Ingot");
            return entity;
        }

        public IEntity CreateTextDisplay(Vector2 position, Vector2 size, string text, Color color)
        {
            IEntity entity = new Entity(scene, position, size, false);
            new DrawTextComponent(entity, (int)GameLayer.Ui, "Berry Rotunda", text, color);

            return entity;
        }

        private Color GetOwnerColor(int owner)
        {
            return scene.ServiceProvider.GetService<IOwnerColorService>().GetOwnerColor(owner);
        }
    }
}
