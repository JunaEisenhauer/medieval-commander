using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model.Game;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.View;
using IServiceProvider = StrategyGame.Model.IService.IServiceProvider;

namespace StrategyGame.Model.Menu
{
    public class StartMenuScene : IScene
    {
        private readonly List<IEntity> addEntities = new List<IEntity>();
        private readonly List<IEntity> removeEntities = new List<IEntity>();

        public IService.IServiceProvider ServiceProvider { get; }

        public List<IEntity> Entities { get; }

        public IEntityFactory EntityFactory { get; }

        public StartMenuScene(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Entities = new List<IEntity>();

            EntityFactory = new MenuEntityFactory(this);

            LoadImages();
            LoadMenu();
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
        }

        public void Resize(GameWindow gameWindow)
        {
            var windowAspect = (float)gameWindow.Width / gameWindow.Height;

            if (windowAspect > 1)
            {
                GL.Scale((float)gameWindow.Width / gameWindow.Height, -1f, 1f);
            }
            else
            {
                GL.Scale(1f, -(float)gameWindow.Height / gameWindow.Width, 1f);
            }
        }

        public void OnChange()
        {
            ServiceProvider.GetService<IGameStateService>().Running = false;
        }

        public void AddEntity(IEntity entity)
        {
            addEntities.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            removeEntities.Add(entity);
        }

        private void LoadImages()
        {
            IFileResourceService resourceService = ServiceProvider.GetService<IFileResourceService>();

            ServiceProvider.GetService<IGraphicsService>().AddImage("MenuBackground", resourceService.Open("Menu/MenuBackground.png"));
            ServiceProvider.GetService<IGraphicsService>().AddImage("StartButton", resourceService.Open("Menu/StartButton.png"));
            ServiceProvider.GetService<IGraphicsService>().AddImage("ExitButton", resourceService.Open("Menu/ExitButton.png"));

            ServiceProvider.GetService<IGraphicsService>().AddFont("Berry Rotunda", resourceService.Open("Font/berry-rotunda-sprite-sheet.png"), 64, 64, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ");
        }

        private void LoadMenu()
        {
            IMenuEntityFactory menuFactory = (IMenuEntityFactory)EntityFactory;
            menuFactory.CreateBackground("MenuBackground");

            menuFactory.CreateButton(string.Empty, "StartButton", new Vector2(0f, 0f), new Vector2(0.8f, 0.22f), "LevelSelection");

            menuFactory.CreateButton(string.Empty, "ExitButton", new Vector2(0f, 0.55f), new Vector2(0.8f, 0.22f), null);
        }
    }
}
