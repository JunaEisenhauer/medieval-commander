using StrategyGame.Model;

namespace StrategyGame.Service.View.Job
{
    public interface IDrawJob
    {
        IDrawable Drawable { get; }

        void Draw();
    }
}
