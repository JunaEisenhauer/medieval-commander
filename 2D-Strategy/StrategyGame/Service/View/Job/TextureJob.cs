using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model;

namespace StrategyGame.Service.View.Job
{
    public class TextureJob : IDrawJob
    {
        private Rectangle bounds;
        private Texture2D texture2D;
        private Color color;

        public IDrawable Drawable { get; }

        public TextureJob(IDrawable drawable, Rectangle bounds, Texture2D texture2D, Color color)
        {
            Drawable = drawable;
            this.bounds = bounds;
            this.texture2D = texture2D;
            this.color = color;
        }

        public void Draw()
        {
            var size = Drawable.Size / 2;

            texture2D.Bind();
            GL.Color4(color);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(bounds.Min);
            GL.Vertex2(Drawable.Position + new Vector2(-size.X, size.Y));
            GL.TexCoord2(bounds.Min.X, bounds.Max.Y);
            GL.Vertex2(Drawable.Position - size);
            GL.TexCoord2(bounds.Max);
            GL.Vertex2(Drawable.Position + new Vector2(size.X, -size.Y));
            GL.TexCoord2(bounds.Max.X, bounds.Min.Y);
            GL.Vertex2(Drawable.Position + size);
            GL.End();
            texture2D.Unbind();
        }
    }
}
