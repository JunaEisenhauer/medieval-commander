using System.Drawing;
using System.IO;
using StrategyGame.Model;

namespace StrategyGame.Model.IService.View
{
    public interface IGraphicsService : IService
    {
        void DrawTexture(IDrawable entity, int layer, int textureId);

        void DrawTexture(IDrawable entity, int layer, string texture, bool flipped);

        void DrawTexture(IDrawable entity, int layer, string texture, Color color, bool flipped);

        void DrawCircle(IDrawable entity, int layer, Color color);

        void DrawQuad(IDrawable entity, int layer, Color color);

        void DrawCharacter(IDrawable entity, int layer, string font, char character, Color color);

        void AddImage(string imageName, Stream stream);

        void AddTile(int tileId, string imageName, float x, float y, float width, float height);

        void AddFont(string fontName, Stream stream, int letterWidth, int letterHeight, string characters);
    }
}
