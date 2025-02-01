using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.View;
using IServiceProvider = StrategyGame.Model.IService.IServiceProvider;

namespace StrategyGame.Model.Menu
{
    public class GameOverScene : IScene
    {
        private readonly List<IEntity> addEntities = new List<IEntity>();
        private readonly List<IEntity> removeEntities = new List<IEntity>();

        public IService.IServiceProvider ServiceProvider { get; }

        public List<IEntity> Entities { get; }

        public IEntityFactory EntityFactory { get; }

        public GameOverScene(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Entities = new List<IEntity>();

            EntityFactory = new MenuEntityFactory(this);

            LoadImages();
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
            LoadMenu();
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

            ServiceProvider.GetService<IGraphicsService>().AddImage("GameOver", resourceService.Open("Menu/gameOver.png"));
            ServiceProvider.GetService<IGraphicsService>().AddImage("Win", resourceService.Open("Menu/youWin.png"));
            ServiceProvider.GetService<IGraphicsService>().AddImage("OkButton", resourceService.Open("Menu/okButton.png"));
        }

        private void LoadMenu()
        {
            bool won = ServiceProvider.GetService<IGameStateService>().Won;
            IMenuEntityFactory menuFactory = (IMenuEntityFactory)EntityFactory;
            menuFactory.CreateBackground(won ? "Win" : "GameOver");
            menuFactory.CreateButton(string.Empty, "OkButton", new Vector2(0f, 0f), new Vector2(0.8f, 0.22f), "MainMenu");
        }
    }
}
