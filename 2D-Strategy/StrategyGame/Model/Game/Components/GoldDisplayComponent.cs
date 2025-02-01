using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using StrategyGame.Model.Components;
using StrategyGame.Model.IService;

namespace StrategyGame.Model.Game.Components
{
    public class GoldDisplayComponent : IComponent
    {
        public IEntity Parent { get; }

        private readonly IEntity ingot;
        private readonly IEntity text;

        public GoldDisplayComponent(IEntity parent)
        {
            Parent = parent;
            parent?.AddComponent(this);

            IMapService mapService = Parent.Scene.ServiceProvider.GetService<IMapService>();

            var ingotX = mapService.Width * 0.015f;
            var ingotY = mapService.Height * 0.045f;
            var ingotWidth = ((float)mapService.Width / mapService.Height) * 1.5f;
            ingot = ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateIngot(new Vector2(ingotX, ingotY), new Vector2(ingotWidth));
            Parent.AddChild(ingot);

            var textX = (mapService.Width * 0.05f) + ingotX;
            var textY = ingotY;
            var textWidth = mapService.Width * 0.05f;
            text = ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateTextDisplay(new Vector2(textX, textY), new Vector2(textWidth), "0", Color.Black);
            Parent.AddChild(text);
        }

        public void Update()
        {
            if (!text.HasComponent<DrawTextComponent>())
                return;

            text.GetComponent<DrawTextComponent>().Text =
                Parent.Scene.ServiceProvider.GetService<IGoldService>().GetGold(0) + string.Empty;
        }
    }
}
