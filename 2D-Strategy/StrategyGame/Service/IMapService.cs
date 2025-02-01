using System;
using System.Collections.Generic;
using OpenTK;

namespace StrategyGame.Model.IService
{
    public interface IMapService : IService
    {
        int Width { get; }

        int Height { get; }

        int TileHeight { get; }

        int TileWidth { get; }

        void LoadMap(string level);

        void ForEachLayerTile(Action<ILayerTile> func);

        void ForEachObjectGroups(Action<IMapObject> func);
    }

    public interface ILayerTile
    {
        int X { get; }

        int Y { get; }

        string Layer { get; }

        int Gid { get; }

        bool Animated { get; }

        int[] Gids { get; }

        float AnimationSpeed { get; }
    }

    public interface IMapObject
    {
        string Group { get; }

        string Name { get; }

        float X { get; }

        float Y { get; }

        ICollection<Vector2> Points { get; }

        Dictionary<string, string> Properties { get; }
    }
}
