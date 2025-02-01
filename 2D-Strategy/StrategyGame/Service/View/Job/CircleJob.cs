using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model;

namespace StrategyGame.Service.View.Job
{
    public class CircleJob : IDrawJob
    {
        public IDrawable Drawable { get; }

        public Color Color { get; }

        public CircleJob(IDrawable drawable, Color color)
        {
            Drawable = drawable;
            Color = color;
        }

        public void Draw()
        {
            GL.Color4(Color);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(Drawable.Position);

            var corners = (int)(32 * ((Drawable.Size.X / 2) + 1));
            var delta = 2f * (float)Math.PI / corners;
            for (var i = 0; i < corners; i++)
            {
                var alpha = i * delta;
                var x = Drawable.Size.X / 2 * (float)Math.Cos(alpha);
                var y = Drawable.Size.X / 2 * (float)Math.Sin(alpha);
                GL.Vertex2(Drawable.Position.X + x, Drawable.Position.Y + y);
            }

            GL.Vertex2(Drawable.Position.X + (Drawable.Size.X / 2), Drawable.Position.Y);
            GL.End();
        }
    }
}
