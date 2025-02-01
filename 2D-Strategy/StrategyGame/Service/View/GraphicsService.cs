using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model;
using StrategyGame.Model.IService.View;
using StrategyGame.Service.View.Job;

namespace StrategyGame.Service.View
{
    public class GraphicsService : IGraphicsService
    {
        private readonly IDictionary<int, List<IDrawJob>> drawJobs;
        private readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private readonly Dictionary<int, TileData> tileData = new Dictionary<int, TileData>();
        private readonly Dictionary<string, Dictionary<char, Rectangle>> characterBounds = new Dictionary<string, Dictionary<char, Rectangle>>();

        public GraphicsService(GameWindow gameWindow)
        {
            drawJobs = new Dictionary<int, List<IDrawJob>>();

            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);

            gameWindow.RenderFrame += (s, e) =>
            {
                Render();
                drawJobs.Clear();
                gameWindow.SwapBuffers();
            };
        }

        public void AddImage(string imageName, Stream stream)
        {
            if (textures.ContainsKey(imageName))
                textures.Remove(imageName);

            var tex = new Texture2D(stream, false);
            textures.Add(imageName, tex);
        }

        public void AddTile(int tileId, string imageName, float x, float y, float width, float height)
        {
            if (tileData.ContainsKey(tileId))
                tileData.Remove(tileId);
            tileData.Add(tileId, new TileData { ImageName = imageName, Bounds = new Rectangle(x, y, width, height) });
        }

        public void AddFont(string fontName, Stream stream, int letterWidth, int letterHeight, string characters)
        {
            if (characterBounds.ContainsKey(fontName))
                characterBounds.Remove(fontName);
            AddImage(fontName, stream);
            var tex = textures[fontName];

            var bounds = new Dictionary<char, Rectangle>();

            for (int y = tex.Height - letterHeight, i = 0; y > 0; y -= letterHeight)
            {
                for (int x = 0; x < tex.Width; x += letterWidth, i++)
                {
                    if (characters.Length <= i)
                        goto endOfLoop;

                    var bX = (float)x / tex.Width;
                    var bY = (float)y / tex.Height;
                    bounds.Add(characters[i], new Rectangle(bX, bY, (float)letterWidth / tex.Width, (float)letterHeight / tex.Height));
                }
            }

        endOfLoop:

            characterBounds.Add(fontName, bounds);
        }

        public void DrawTexture(IDrawable drawable, int layer, int textureId)
        {
            var tile = tileData[textureId];
            AddDrawJob(layer, new TextureJob(drawable, tile.Bounds, textures[tile.ImageName], Color.White));
        }

        public void DrawTexture(IDrawable drawable, int layer, string texture, bool flipped)
        {
            AddDrawJob(layer, new TextureJob(drawable, new Rectangle(flipped ? 1 : 0, 0, flipped ? -1 : 1, 1), textures[texture], Color.White));
        }

        public void DrawTexture(IDrawable drawable, int layer, string texture, Color color, bool flipped)
        {
            AddDrawJob(layer, new TextureJob(drawable, new Rectangle(flipped ? 1 : 0, 0, flipped ? -1 : 1, 1), textures[texture], color));
        }

        public void DrawCircle(IDrawable drawable, int layer, Color color)
        {
            AddDrawJob(layer, new CircleJob(drawable, color));
        }

        public void DrawQuad(IDrawable drawable, int layer, Color color)
        {
            AddDrawJob(layer, new QuadJob(drawable, color));
        }

        public void DrawCharacter(IDrawable drawable, int layer, string font, char character, Color color)
        {
            AddDrawJob(layer, new TextureJob(drawable, characterBounds[font][character], textures[font], color));
        }

        private void AddDrawJob(int layer, IDrawJob drawJob)
        {
            if (!drawJobs.ContainsKey(layer))
                drawJobs.Add(layer, new List<IDrawJob>());
            drawJobs[layer].Add(drawJob);
        }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            var sortedJobs = drawJobs.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            foreach (var jobs in sortedJobs)
            {
                var job = jobs.Value;
                var sorted = job.OrderBy(obj => obj.Drawable.Position.Y).ToList();
                foreach (var drawJob in sorted)
                {
                    drawJob.Draw();
                }
            }
        }

        private struct TileData
        {
            public string ImageName;
            public Rectangle Bounds;
        }
    }
}
