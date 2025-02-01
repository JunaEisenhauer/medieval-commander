using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.View;

namespace StrategyGame.Model.Components
{
    public class DrawAnimationComponent : IComponent
    {
        private float lastAnimation;
        private int current;

        public IEntity Parent { get; }

        public int Layer { get; }

        public int[] TextureIds { get; set; }

        public string[] Textures { get; set; }

        public Color Color { get; set; }

        public float AnimationSpeed { get; set; }

        public bool Flipped { get; set; }

        public bool Repeat { get; set; }

        public Action LoopFinished { get; set; }

        public DrawAnimationComponent(IEntity parent, int layer, int[] textureIds, float animationSpeed)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            TextureIds = textureIds;
            Color = Color.White;
            AnimationSpeed = animationSpeed;
            Repeat = true;
        }

        public DrawAnimationComponent(IEntity parent, int layer, int[] textureIds, Color color, float animationSpeed)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            TextureIds = textureIds;
            Color = color;
            AnimationSpeed = animationSpeed;
            Repeat = true;
        }

        public DrawAnimationComponent(IEntity parent, int layer, string[] textures, float animationSpeed)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            Textures = textures;
            Color = Color.White;
            AnimationSpeed = animationSpeed;
            Repeat = true;
        }

        public DrawAnimationComponent(IEntity parent, int layer, string[] textures, Color color, float animationSpeed)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            Textures = textures;
            Color = color;
            AnimationSpeed = animationSpeed;
            Repeat = true;
        }

        public void Update()
        {
            var absTime = Parent.Scene.ServiceProvider.GetService<ITimeService>().AbsoluteTime;
            if (lastAnimation + (1 / AnimationSpeed) < absTime)
            {
                lastAnimation = absTime;
                current++;
                if (current >= (Textures?.Length ?? TextureIds.Length))
                {
                    LoopFinished?.Invoke();
                    if (!Repeat)
                    {
                        Parent.RemoveComponent<DrawAnimationComponent>();
                        return;
                    }

                    current = 0;
                }
            }

            if (TextureIds != null)
            {
                Parent.Scene.ServiceProvider.GetService<IGraphicsService>()
                    .DrawTexture(Parent, Layer, TextureIds[current]);
            }
            else
            {
                Parent.Scene.ServiceProvider.GetService<IGraphicsService>()
                    .DrawTexture(Parent, Layer, Textures[current], Color, Flipped);
            }
        }
    }
}
