using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.Pathfinding;
using StrategyGame.Model.IService.View;
using StrategyGame.Model.Menu;
using StrategyGame.Service.Sound;
using IServiceProvider = StrategyGame.Model.IService.IServiceProvider;

namespace StrategyGame.Model.Game
{
    public class GameScene : IScene
    {
        private readonly List<IEntity> addEntities = new List<IEntity>();
        private readonly List<IEntity> removeEntities = new List<IEntity>();

        private IEntity menuButtonContinue;
        private IEntity menuButtonQuit;

        public IServiceProvider ServiceProvider { get; }

        public List<IEntity> Entities { get; }

        public IEntityFactory EntityFactory { get; }

        public IEntityFactory MenuEntityFactory { get; }

        public GameScene(IServiceProvider serviceProvider, string level)
        {
            ServiceProvider = serviceProvider;

            Entities = new List<IEntity>();
            EntityFactory = new GameEntityFactory(this);
            MenuEntityFactory = new MenuEntityFactory(this);

            serviceProvider.GetService<IGoldService>().Reset();
            serviceProvider.GetService<IGameObjectService>().Reset();

            LoadImages();
            LoadFonts();
            LoadLevel(level);
            LoadSounds();

            var startGold = 300;
            ServiceProvider.GetService<IGoldService>().AddGold(0, startGold);
            ((IGameEntityFactory)EntityFactory).CreateGoldDisplay();
        }

        public void Update()
        {
            foreach (var entity in addEntities)
            {
                Entities.Add(entity);
            }

            addEntities.Clear();

            foreach (var entity in removeEntities)
            {
                Entities.Remove(entity);
            }

            removeEntities.Clear();

            foreach (var entity in Entities)
            {
                entity.Update();
            }

            if (ServiceProvider.GetService<IKeystateService>().IsKeyPressed(Key.Escape) && ServiceProvider.GetService<ITimeService>().TimeScale != 0)
            {
                float x = ServiceProvider.GetService<IMapService>().Width / 2;
                float y = ServiceProvider.GetService<IMapService>().Height / 7;
                float deltaX = (float)(ServiceProvider.GetService<IMapService>().Width * 0.35);
                float deltaY = (float)(ServiceProvider.GetService<IMapService>().Width * 0.05);
                menuButtonContinue = ((IGameEntityFactory)EntityFactory).CreateBuyButton(new Vector2(x, y * 3), new Vector2(deltaX, deltaY), "continueButton", () => ContinueButton());
                menuButtonQuit = ((IGameEntityFactory)EntityFactory).CreateBuyButton(new Vector2(x, y * 5), new Vector2(deltaX, deltaY), "menuButton", () => ExitButton());
                ServiceProvider.GetService<ITimeService>().TimeScale = 0;
            }
            else if (ServiceProvider.GetService<IKeystateService>().IsKeyPressed(Key.Escape))
            {
                ContinueButton();
            }
        }

        public void ContinueButton()
        {
            RemoveEntity(menuButtonContinue);
            RemoveEntity(menuButtonQuit);

            ServiceProvider.GetService<ITimeService>().TimeScale = 1;
        }

        public void ExitButton()
        {
            ServiceProvider.GetService<ITimeService>().TimeScale = 1;
            ServiceProvider.GetService<ISceneService>().ChangeScene("MainMenu");
        }

        public void Resize(GameWindow gameWindow)
        {
            var mapService = ServiceProvider.GetService<IMapService>();

            var windowAspect = (float)gameWindow.Width / gameWindow.Height;
            var mapAspect = (float)mapService.Width /
                    mapService.Height;
            if (windowAspect > 1)
            {
                GL.Translate(-windowAspect, 1, 0);
                GL.Scale(Math.Min(windowAspect, mapAspect) / mapService.Width, Math.Min(windowAspect, mapAspect) / mapService.Width, 1);
            }
            else
            {
                GL.Translate(-1, 1 / windowAspect, 0);
                GL.Scale(Math.Min(1 / windowAspect, 1 / mapAspect) / mapService.Height, Math.Min(1 / windowAspect, 1 / mapAspect) / mapService.Height, 1);
            }

            GL.Scale(2f, -2f, 1);

            var widthDiff = (windowAspect * mapService.Height) - mapService.Width;
            var heightDiff = ((1 / windowAspect) * mapService.Width) - mapService.Height;
            GL.Translate(Math.Max(0f, widthDiff / 2), Math.Max(0f, heightDiff / 2), 0f);
        }

        public void OnChange()
        {
            ServiceProvider.GetService<IGameStateService>().Running = true;
        }

        public void AddEntity(IEntity entity)
        {
            addEntities.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            removeEntities.Add(entity);
        }

        private void LoadSounds()
        {
            var audioService = ServiceProvider.GetService<IAudioService>();

            audioService.AddSound("selected");
            audioService.AddSound("intro");
            audioService.AddSound("dead");
            audioService.AddSound("command");
        }

        private void LoadImages()
        {
            var resourceService = ServiceProvider.GetService<IFileResourceService>();
            var graphic = ServiceProvider.GetService<IGraphicsService>();

            graphic.AddImage("CatapultButton", resourceService.Open("Catapult/Catapult-Button.png"));
            graphic.AddImage("Catapult-0", resourceService.Open("Catapult/Catapult-Player.png"));
            graphic.AddImage("Catapult-1", resourceService.Open("Catapult/Catapult-Enemy.png"));
            graphic.AddImage("Tower-0", resourceService.Open("Tower/Tower-Player.png"));
            graphic.AddImage("Tower-1", resourceService.Open("Tower/Tower-Enemy.png"));
            graphic.AddImage("Fortress-0", resourceService.Open("Fortress/Fortress-Player.png"));
            graphic.AddImage("Fortress-1", resourceService.Open("Fortress/Fortress-Enemy.png"));

            graphic.AddImage("Flag", resourceService.Open("DestinationFlag.png"));
            graphic.AddImage("Ingot", resourceService.Open("Ingot.png"));

            graphic.AddImage("continueButton", resourceService.Open("Menu/continueButton.png"));
            graphic.AddImage("menuButton", resourceService.Open("Menu/mainMenuButton.png"));

            graphic.AddImage("KnightButton", resourceService.Open("Knight/Knight-Button.png"));
            graphic.AddImage("Knight-Idle-0", resourceService.Open("Knight/Knight-Player-Idle.png"));
            for (var i = 0; i < 18; i++)
            {
                graphic.AddImage("Knight-Animation" + i + "-0", resourceService.Open("Knight/Knight-Player-" + i + ".png"));
            }

            graphic.AddImage("Knight-Idle-1", resourceService.Open("Knight/Knight-Enemy-Idle.png"));
            for (var i = 0; i < 18; i++)
            {
                graphic.AddImage("Knight-Animation" + i + "-1", resourceService.Open("Knight/Knight-Enemy-" + i + ".png"));
            }

            graphic.AddImage("ArcherButton", resourceService.Open("Archer/Archer-Button.png"));
            graphic.AddImage("Archer-Idle-0", resourceService.Open("Archer/Archer-Player-Idle.png"));
            for (var i = 0; i < 16; i++)
            {
                graphic.AddImage("Archer-Animation" + i + "-0", resourceService.Open("Archer/Archer-Player-" + i + ".png"));
            }

            graphic.AddImage("Archer-Idle-1", resourceService.Open("Archer/Archer-Enemy-Idle.png"));
            for (var i = 0; i < 16; i++)
            {
                graphic.AddImage("Archer-Animation" + i + "-1", resourceService.Open("Archer/Archer-Enemy-" + i + ".png"));
            }
        }

        private void LoadFonts()
        {
            IFileResourceService resourceService = ServiceProvider.GetService<IFileResourceService>();
            ServiceProvider.GetService<IGraphicsService>().AddFont("Berry Rotunda", resourceService.Open("Font/berry-rotunda-sprite-sheet.png"), 64, 64, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ");
        }

        private void LoadLevel(string level)
        {
            IMapService mapService = ServiceProvider.GetService<IMapService>();
            mapService.LoadMap(level);

            ServiceProvider.GetService<IPathfinderService>().ClearAreas();

            mapService.ForEachLayerTile(l =>
                {
                    if (!l.Animated)
                    {
                        ((IGameEntityFactory)EntityFactory).CreateTile(new Vector2(l.X, l.Y), l.Layer, l.Gid);
                    }
                    else
                    {
                        ((IGameEntityFactory)EntityFactory).CreateAnimatedTile(new Vector2(l.X, l.Y), l.Layer, l.Gids, l.AnimationSpeed);
                    }
                });

            mapService.ForEachObjectGroups(o =>
            {
                switch (o.Group.ToLower())
                {
                    case "gameobjects":
                        LoadGameObject(o);
                        break;
                    case "navmesh":
                        LoadNavPolygon(o);
                        break;
                    default:
                        break;
                }
            });
        }

        private void LoadGameObject(IMapObject o)
        {
            IGameEntityFactory gameFactory = (IGameEntityFactory)EntityFactory;

            var goldValue = 0;
            if (o.Properties.ContainsKey("goldValue"))
                goldValue = int.Parse(o.Properties["goldValue"]);

            switch (o.Name.ToLower())
            {
                case "army":
                    switch (int.Parse(o.Properties["type"]))
                    {
                        case 0:
                            gameFactory.CreateArcher(new Vector2(o.X, o.Y), int.Parse(o.Properties["owner"]), goldValue);
                            break;
                        case 1:
                            gameFactory.CreateKnight(new Vector2(o.X, o.Y), int.Parse(o.Properties["owner"]), goldValue);
                            break;
                        case 2:
                            gameFactory.CreateCatapult(new Vector2(o.X, o.Y), int.Parse(o.Properties["owner"]), goldValue);
                            break;
                        default:
                            break;
                    }

                    break;
                case "tower":
                    gameFactory.CreateTower(new Vector2(o.X, o.Y), int.Parse(o.Properties["owner"]), goldValue);
                    break;
                case "fortress":
                    gameFactory.CreateFortress(new Vector2(o.X, o.Y), int.Parse(o.Properties["owner"]), goldValue);
                    break;
                case "spawnpoint":
                    gameFactory.CreateSpawnpoint(new Vector2(o.X, o.Y), int.Parse(o.Properties["owner"]));
                    break;
                default:
                    break;
            }
        }

        private void LoadNavPolygon(IMapObject o)
        {
            List<Vector2> points = new List<Vector2>();

            foreach (Vector2 point in o.Points)
            {
                points.Add(new Vector2((point.X / ServiceProvider.GetService<IMapService>().TileWidth) + o.X, (point.Y / ServiceProvider.GetService<IMapService>().TileHeight) + o.Y));
            }

            if (o.Properties.ContainsKey("IsObstacle"))
            {
                ServiceProvider.GetService<IPathfinderService>().AddArea(points, true);
            }
            else
            {
                ServiceProvider.GetService<IPathfinderService>().AddArea(points, false);
            }
        }
    }
}
