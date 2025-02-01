using OpenTK;

namespace StrategyGame.Model
{
    /// <summary>
    /// Interface for the view holds position and size of an drawable entity.
    /// </summary>
    public interface IDrawable
    {
        Vector2 Position { get; set; }

        Vector2 Size { get; set; }
    }
}
