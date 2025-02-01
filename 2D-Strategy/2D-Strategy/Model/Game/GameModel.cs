using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Input;
using StrategyGame.Model.MapData;
using StrategyGame.Model.MapData.MapComponents;
using StrategyGame.Model.MapData.Pathfinding;

namespace StrategyGame.Model.Game
{
    public class GameModel : IModel
    {
        public List<IEntity> Entities { get; }

        public Map gameMap;

        private readonly Random random;

        public Vector2? StartPosition { get; private set; }

        public IGameObject entity;

        // public Health healthBar = new Health();
        public GameModel()
        {
            random = new Random();
            Entities = new List<IEntity>();
            gameMap = new Map();

            StartPosition = null;

            InitializeEntities();
            InitializeNavMesh();
        }

        public void InitializeNavMesh()
        {
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.65f, 0.90f), new Vector2(-0.86f, 0.50f), new Vector2(-0.30f, 0.50f), new Vector2(-0.16f, 0.83f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.86f, 0.50f), new Vector2(-0.89f, 0.13f), new Vector2(-0.33f, 0.24f), new Vector2(-0.30f, 0.50f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.89f, 0.13f), new Vector2(-0.37f, -0.02f), new Vector2(-0.33f, 0.24f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.37f, 0.02f), new Vector2(-0.22f, -0.23f), new Vector2(-0.16f, 0.24f), new Vector2(-0.33f, 0.24f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.22f, -0.23f), new Vector2(0.03f, -0.20f), new Vector2(0.20f, 0.06f), new Vector2(-0.16f, 0.24f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.20f, -0.53f), new Vector2(-0.25f, -0.92f), new Vector2(0.22f, -0.90f), new Vector2(0.30f, -0.50f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.22f, -0.90f), new Vector2(0.80f, -0.60f), new Vector2(0.47f, -0.40f), new Vector2(0.30f, -0.50f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.80f, -0.60f), new Vector2(0.86f, 0.05f), new Vector2(0.42f, -0.27f), new Vector2(0.47f, -0.40f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.86f, 0.05f), new Vector2(0.38f, 0.02f), new Vector2(0.42f, -0.27f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.38f, 0.02f), new Vector2(0.86f, 0.05f), new Vector2(0.66f, 0.10f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.03f, -0.2f), new Vector2(0.22f, -0.27f), new Vector2(0.20f, 0.06f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.22f, -0.27f), new Vector2(0.42f, -0.27f), new Vector2(0.38f, 0.02f), new Vector2(0.20f, 0.06f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.43f, 0.31f), new Vector2(0.70f, 0.40f), new Vector2(0.80f, 0.77f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.43f, 0.31f), new Vector2(0.80f, 0.77f), new Vector2(0.20f, 0.37f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.20f, 0.37f), new Vector2(0.80f, 0.77f), new Vector2(0.50f, 0.90f), new Vector2(0.15f, 0.70f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.20f, 0.06f), new Vector2(0.10f, 0.25f), new Vector2(-0.16f, 0.24f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.10f, 0.25f), new Vector2(-0.06f, 0.46f), new Vector2(-0.16f, 0.24f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.16f, 0.24f), new Vector2(-0.33f, 0.24f), new Vector2(-0.30f, 0.50f), new Vector2(-0.06f, 0.46f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.30f, 0.50f), new Vector2(-0.06f, 0.46f), new Vector2(-0.12f, 0.55f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.30f, 0.50f), new Vector2(-0.12f, 0.55f), new Vector2(-0.16f, 0.83f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(-0.12f, 0.55f), new Vector2(-0.16f, 0.83f), new Vector2(0.07f, 0.95f), new Vector2(0.15f, 0.70f) });
            gameMap.CreateArea(new List<Vector2> { new Vector2(0.15f, 0.70f), new Vector2(0.07f, 0.95f), new Vector2(0.50f, 0.90f) });

            gameMap.AddComponent(0, new Neighbors());
            gameMap.AddComponent(1, new Neighbors());
            gameMap.AddComponent(2, new Neighbors());
            gameMap.AddComponent(3, new Neighbors());
            gameMap.AddComponent(4, new Neighbors());
            gameMap.AddComponent(5, new Neighbors());
            gameMap.AddComponent(6, new Neighbors());
            gameMap.AddComponent(7, new Neighbors());
            gameMap.AddComponent(8, new Neighbors());
            gameMap.AddComponent(9, new Neighbors());
            gameMap.AddComponent(10, new Neighbors());
            gameMap.AddComponent(11, new Neighbors());
            gameMap.AddComponent(12, new Neighbors());
            gameMap.AddComponent(13, new Neighbors());
            gameMap.AddComponent(14, new Neighbors());
            gameMap.AddComponent(15, new Neighbors());
            gameMap.AddComponent(16, new Neighbors());
            gameMap.AddComponent(17, new Neighbors());
            gameMap.AddComponent(18, new Neighbors());
            gameMap.AddComponent(19, new Neighbors());
            gameMap.AddComponent(20, new Neighbors());
            gameMap.AddComponent(21, new Neighbors());

            gameMap.GetComponent<Neighbors>(0).AddNeighbor(gameMap.GetNode(0), gameMap.GetNode(1), 0);
            gameMap.GetComponent<Neighbors>(1).AddNeighbor(gameMap.GetNode(1), gameMap.GetNode(0), 0);

            gameMap.GetComponent<Neighbors>(0).AddNeighbor(gameMap.GetNode(0), gameMap.GetNode(19), 0);
            gameMap.GetComponent<Neighbors>(19).AddNeighbor(gameMap.GetNode(19), gameMap.GetNode(0), 0);

            gameMap.GetComponent<Neighbors>(1).AddNeighbor(gameMap.GetNode(1), gameMap.GetNode(2), 0);
            gameMap.GetComponent<Neighbors>(2).AddNeighbor(gameMap.GetNode(2), gameMap.GetNode(1), 0);

            gameMap.GetComponent<Neighbors>(1).AddNeighbor(gameMap.GetNode(1), gameMap.GetNode(17), 0);
            gameMap.GetComponent<Neighbors>(17).AddNeighbor(gameMap.GetNode(17), gameMap.GetNode(1), 0);

            gameMap.GetComponent<Neighbors>(2).AddNeighbor(gameMap.GetNode(2), gameMap.GetNode(3), 0);
            gameMap.GetComponent<Neighbors>(3).AddNeighbor(gameMap.GetNode(3), gameMap.GetNode(2), 0);

            gameMap.GetComponent<Neighbors>(3).AddNeighbor(gameMap.GetNode(3), gameMap.GetNode(4), 0);
            gameMap.GetComponent<Neighbors>(4).AddNeighbor(gameMap.GetNode(4), gameMap.GetNode(3), 0);

            gameMap.GetComponent<Neighbors>(3).AddNeighbor(gameMap.GetNode(3), gameMap.GetNode(17), 0);
            gameMap.GetComponent<Neighbors>(17).AddNeighbor(gameMap.GetNode(17), gameMap.GetNode(3), 0);

            gameMap.GetComponent<Neighbors>(4).AddNeighbor(gameMap.GetNode(4), gameMap.GetNode(10), 0);
            gameMap.GetComponent<Neighbors>(10).AddNeighbor(gameMap.GetNode(10), gameMap.GetNode(4), 0);

            gameMap.GetComponent<Neighbors>(4).AddNeighbor(gameMap.GetNode(4), gameMap.GetNode(15), 0);
            gameMap.GetComponent<Neighbors>(15).AddNeighbor(gameMap.GetNode(15), gameMap.GetNode(4), 0);

            gameMap.GetComponent<Neighbors>(10).AddNeighbor(gameMap.GetNode(10), gameMap.GetNode(11), 0);
            gameMap.GetComponent<Neighbors>(11).AddNeighbor(gameMap.GetNode(11), gameMap.GetNode(10), 0);

            gameMap.GetComponent<Neighbors>(11).AddNeighbor(gameMap.GetNode(11), gameMap.GetNode(8), 0);
            gameMap.GetComponent<Neighbors>(8).AddNeighbor(gameMap.GetNode(8), gameMap.GetNode(11), 0);

            gameMap.GetComponent<Neighbors>(8).AddNeighbor(gameMap.GetNode(8), gameMap.GetNode(9), 0);
            gameMap.GetComponent<Neighbors>(9).AddNeighbor(gameMap.GetNode(9), gameMap.GetNode(8), 0);

            gameMap.GetComponent<Neighbors>(8).AddNeighbor(gameMap.GetNode(8), gameMap.GetNode(7), 0);
            gameMap.GetComponent<Neighbors>(7).AddNeighbor(gameMap.GetNode(7), gameMap.GetNode(8), 0);

            gameMap.GetComponent<Neighbors>(7).AddNeighbor(gameMap.GetNode(7), gameMap.GetNode(6), 0);
            gameMap.GetComponent<Neighbors>(6).AddNeighbor(gameMap.GetNode(6), gameMap.GetNode(7), 0);

            gameMap.GetComponent<Neighbors>(6).AddNeighbor(gameMap.GetNode(6), gameMap.GetNode(5), 0);
            gameMap.GetComponent<Neighbors>(5).AddNeighbor(gameMap.GetNode(5), gameMap.GetNode(6), 0);

            gameMap.GetComponent<Neighbors>(15).AddNeighbor(gameMap.GetNode(15), gameMap.GetNode(16), 0);
            gameMap.GetComponent<Neighbors>(16).AddNeighbor(gameMap.GetNode(16), gameMap.GetNode(15), 0);

            gameMap.GetComponent<Neighbors>(16).AddNeighbor(gameMap.GetNode(16), gameMap.GetNode(17), 0);
            gameMap.GetComponent<Neighbors>(17).AddNeighbor(gameMap.GetNode(17), gameMap.GetNode(16), 0);

            gameMap.GetComponent<Neighbors>(17).AddNeighbor(gameMap.GetNode(17), gameMap.GetNode(18), 0);
            gameMap.GetComponent<Neighbors>(18).AddNeighbor(gameMap.GetNode(18), gameMap.GetNode(17), 0);

            gameMap.GetComponent<Neighbors>(18).AddNeighbor(gameMap.GetNode(18), gameMap.GetNode(19), 0);
            gameMap.GetComponent<Neighbors>(19).AddNeighbor(gameMap.GetNode(19), gameMap.GetNode(18), 0);

            gameMap.GetComponent<Neighbors>(19).AddNeighbor(gameMap.GetNode(19), gameMap.GetNode(20), 0);
            gameMap.GetComponent<Neighbors>(20).AddNeighbor(gameMap.GetNode(20), gameMap.GetNode(19), 0);

            gameMap.GetComponent<Neighbors>(20).AddNeighbor(gameMap.GetNode(20), gameMap.GetNode(21), 0);
            gameMap.GetComponent<Neighbors>(21).AddNeighbor(gameMap.GetNode(21), gameMap.GetNode(20), 0);

            gameMap.GetComponent<Neighbors>(21).AddNeighbor(gameMap.GetNode(21), gameMap.GetNode(14), 0);
            gameMap.GetComponent<Neighbors>(14).AddNeighbor(gameMap.GetNode(14), gameMap.GetNode(21), 0);

            gameMap.GetComponent<Neighbors>(14).AddNeighbor(gameMap.GetNode(14), gameMap.GetNode(13), 0);
            gameMap.GetComponent<Neighbors>(13).AddNeighbor(gameMap.GetNode(13), gameMap.GetNode(14), 0);

            gameMap.GetComponent<Neighbors>(13).AddNeighbor(gameMap.GetNode(13), gameMap.GetNode(12), 0);
            gameMap.GetComponent<Neighbors>(12).AddNeighbor(gameMap.GetNode(12), gameMap.GetNode(13), 0);
        }

        public void InitializeEntities()
        {
            // Add fortress
            Tower playerFortress = new Tower(PlayerType.Player, new Vector2(0f, -0.8f), 0.1f, 0.3f, 200, 15, 1.2f)
            {
                IsMain = true
            };
            Entities.Add(playerFortress);

            Tower enemyFortress = new Tower(PlayerType.Enemy, new Vector2(0.5f, 0.6f), 0.1f, 0.3f, 200, 15, 1.2f)
            {
                IsMain = true
            };
            Entities.Add(enemyFortress);

            // Add tower
            Entities.Add(new Tower(PlayerType.Player, new Vector2(0.7f, -0.2f), 0.07f, 0.3f, 150, 12, 1.2f));
            Entities.Add(new Tower(PlayerType.Enemy, new Vector2(-0.5f, 0.7f), 0.07f, 0.3f, 150, 12, 1.2f));

            Entities.Add(new Archer(PlayerType.Player, new Vector2(0.4f, -0.6f)));
            Entities.Add(new Knight(PlayerType.Player, new Vector2(0f, 0f)));
            Entities.Add(new Cavalry(PlayerType.Player, new Vector2(0.3f, -0.1f)));
            Entities.Add(new Archer(PlayerType.Enemy, new Vector2(-0.56f, 0.45f)));
            Entities.Add(new Knight(PlayerType.Enemy, new Vector2(-0.05f, 0.73f)));
            Entities.Add(new Cavalry(PlayerType.Enemy, new Vector2(0.25f, 0.8f)));

            Entities.Add(new Button(new Vector2(-1.5f, 0f), new Vector2(0.5f, 0.2f), "test"));
        }

        public void Update(IKeyboard keyboard, float time, Action<SceneType> sceneChangeAction)
        {
            if (keyboard.IsButtonPressed(MouseButton.Left))
            {
                StartPosition = keyboard.MousePosition;

                foreach (IGameObject e in Entities.OfType<IGameObject>())
                {
                    if ((keyboard.MousePosition - e.Position).Length < e.Radius)
                    {
                        if (e.Selected == true)
                            e.Selected = false;
                        else
                            e.Selected = true;
                    }
                    else
                    {
                        e.Selected = false;
                    }
                }
            }

            if (keyboard.IsButtonDown(MouseButton.Left) && StartPosition.HasValue && (StartPosition.Value - keyboard.MousePosition).Length > 0.05f)
            {
                float aMinX;
                float aMaxX;
                float aMinY;
                float aMaxY;

                foreach (IArmy a in Entities.OfType<IArmy>())
                {
                    if (a.Owner == PlayerType.Enemy)
                        continue;

                    a.Selected = true;

                    if (StartPosition.Value.X < keyboard.MousePosition.X)
                    {
                        aMinX = StartPosition.Value.X;
                        aMaxX = keyboard.MousePosition.X;
                    }
                    else
                    {
                        aMinX = keyboard.MousePosition.X;
                        aMaxX = StartPosition.Value.X;
                    }

                    if (StartPosition.Value.Y < keyboard.MousePosition.Y)
                    {
                        aMinY = StartPosition.Value.Y;
                        aMaxY = keyboard.MousePosition.Y;
                    }
                    else
                    {
                        aMinY = keyboard.MousePosition.Y;
                        aMaxY = StartPosition.Value.Y;
                    }

                    if ((aMinX > (a.Position.X + a.Radius) || (a.Position.X - a.Radius) > aMaxX) || ((aMinY > a.Position.Y + a.Radius) || (a.Position.Y - a.Radius) > aMaxY))
                    {
                        a.Selected = false;
                    }
                }
            }

            if (keyboard.IsButtonReleased(MouseButton.Left))
            {
                StartPosition = null;
            }

            if (keyboard.IsButtonPressed(MouseButton.Right))
            {
                foreach (IEntity next in Entities)
                {
                    if (next is IArmy tmp)
                    {
                        if (tmp.Selected)
                        {
                            int node1 = gameMap.GetNodeWithPoint(next.Position);
                            int node2 = gameMap.GetNodeWithPoint(keyboard.MousePosition);

                            if (node1 == -1 || node2 == -1)
                                break;

                            tmp.Path = Pathfinder.RefinePath(gameMap, Pathfinder.AStarSearch(gameMap, node1, node2), keyboard.MousePosition);
                        }
                    }
                }
            }

            foreach (IEntity entity in Entities.ToList())
            {
                if (entity is Projectile projectile)
                {
                    if (!projectile.UpdatePosition())
                        Entities.Remove(projectile);
                }

                IGameObject gameObj = entity as IGameObject;
                if (gameObj == null)
                    continue;

                if (gameObj is IArmy tmp)
                    tmp.UpdatePosition();

                List<IEntity> newEntities = gameObj.UpdateAttack(time, Entities.FindAll(e => e is IGameObject).Cast<IGameObject>().ToList());
                if (newEntities != null)
                    newEntities.ForEach(e => Entities.Add(e));

                    foreach (IEntity entity2 in Entities)
                    {
                        if (gameObj is Tower tow)
                        {
                            List<IEntity> newEntities2 = gameObj.UpdateHealing(time, Entities.FindAll(e => e is IGameObject).Cast<IGameObject>().ToList());
                            if (newEntities2 != null)
                                newEntities2.ForEach(e => Entities.Add(e));
                        }
                    }

                    gameObj.Update(time);
            }
        }

        public void SceneChangedEvent(SceneType type)
        {
        }
    }
}