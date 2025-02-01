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
using StrategyGame.Model.Menu.Components;
using StrategyGame.Service;

namespace StrategyGame.Model.Menu
{
    public class MenuEntityFactory : IMenuEntityFactory
    {
        private readonly IScene scene;

        public MenuEntityFactory(IScene scene)
        {
            this.scene = scene;
        }

        public IEntity CreateBackground(string name)
        {
            IEntity entity = new Entity(scene, new Vector2(0f), new Vector2(2f), false);
            new DrawTextureComponent(entity, (int)MenuLayer.Background, name);

            return entity;
        }

        public IEntity CreateButton(string text, string name, Vector2 position, Vector2 size, string sceneType)
        {
            IEntity entity = new Entity(scene, position, size, false);
            new DrawTextureComponent(entity, (int)MenuLayer.Button, name);
            new ButtonComponent(entity, () => scene.ServiceProvider.GetService<ISceneService>().ChangeScene(sceneType));

            entity.AddChild(CreateText(text, entity));

            return entity;
        }

        private IEntity CreateText(string text, IEntity button)
        {
            var height = button.Size.Y * 0.8f;
            var x = button.Position.X - ((height * (text.Length - 1)) / 2f);

            IEntity entity = new Entity(scene, new Vector2(x, button.Position.Y), new Vector2(height), false);
            new DrawTextComponent(entity, (int)MenuLayer.ButtonText, "Berry Rotunda", text, Color.Black);

            return entity;
        }
    }
}
