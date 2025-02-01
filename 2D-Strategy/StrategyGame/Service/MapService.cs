using System;
using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.View;
using TiledSharp;
using IServiceProvider = StrategyGame.Model.IService.IServiceProvider;

namespace StrategyGame.Service
{
    public class MapService : IMapService
    {
        private readonly IServiceProvider serviceProvider;

        private TmxMap map;
        private Dictionary<int, TmxTilesetTile> gidTiles;

        public int Width => map.Width;

        public int Height => map.Height;

        public int TileHeight => map.TileHeight;

        public int TileWidth => map.TileWidth;

        public MapService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            gidTiles = new Dictionary<int, TmxTilesetTile>();
        }

        public void LoadMap(string level)
        {
            IFileResourceService resourceService = serviceProvider.GetService<IFileResourceService>();
            IGraphicsService graphicsService = serviceProvider.GetService<IGraphicsService>();

            map = new TmxMap(resourceService.GetFileName("Tilemap/" + level + ".tmx"));

            foreach (var tileset in map.Tilesets)
            {
                var imageName = tileset.Name;

                if (imageName == "Objects")
                    continue;

                // open stream and add the image stream to graphics
                using (var stream = resourceService.Open(tileset.Image.Source))
                {
                    graphicsService.AddImage(imageName, stream);
                }

                var width = tileset.Columns.Value;
                var height = tileset.TileCount.Value / tileset.Columns.Value;

                for (int i = 0; i < tileset.TileCount.Value; i++)
                {
                    var gid = tileset.FirstGid + i;
                    var idOffset = i % tileset.Columns.Value;
                    var x = idOffset * (1f / width);
                    var row = (i - idOffset) / width;
                    var y = (height - 1 - row) * (1f / height);

                    graphicsService.AddTile(gid, imageName, x, y, 1f / width, 1f / height);

                    if (tileset.Tiles.ContainsKey(i))
                    {
                        var tile = tileset.Tiles[i];
                        if (tile.AnimationFrames != null && tile.AnimationFrames.Count > 0)
                        {
                            if (gidTiles.ContainsKey(gid))
                                continue;
                            gidTiles.Add(gid, tile);
                        }
                    }
                }
            }
        }

        public void ForEachLayerTile(Action<ILayerTile> func)
        {
            var layers = map.Layers;
            foreach (var layer in layers)
            {
                for (int y = 0, i = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++, i++)
                    {
                        var gid = layer.Tiles[i].Gid;
                        if (gidTiles.ContainsKey(gid))
                        {
                            var tile = gidTiles[gid];
                            int[] gids = new int[tile.AnimationFrames.Count];
                            for (int j = 0; j < tile.AnimationFrames.Count; j++)
                            {
                                gids[j] = tile.AnimationFrames[j].Id + gid;
                            }

                            func(new LayerTile(x, y, layer.Name, gids, 1f / (tile.AnimationFrames[0].Duration / 1000f)));
                        }
                        else
                        {
                            func(new LayerTile(x, y, layer.Name, gid));
                        }
                    }
                }
            }
        }

        public void ForEachObjectGroups(Action<IMapObject> func)
        {
            var objectGroups = map.ObjectGroups;
            foreach (var group in objectGroups)
            {
                foreach (var obj in group.Objects)
                {
                    var x = (float)obj.X / map.TileWidth;
                    var y = (float)obj.Y / map.TileHeight;
                    ICollection<Vector2> points = new List<Vector2>();
                    if (obj.Points != null)
                    {
                        foreach (var point in obj.Points)
                        {
                            points.Add(new Vector2((float)point.X, (float)point.Y));
                        }
                    }

                    var mapObj = new MapObject(group.Name, obj.Name, x, y, points, obj.Properties);
                    func(mapObj);
                }
            }
        }

        public class LayerTile : ILayerTile
        {
            public int X { get; }

            public int Y { get; }

            public string Layer { get; }

            public int Gid { get; }

            public bool Animated { get; }

            public int[] Gids { get; }

            public float AnimationSpeed { get; }

            internal LayerTile(int x, int y, string layer, int gid)
            {
                this.X = x;
                this.Y = y;
                this.Layer = layer;
                this.Gid = gid;
            }

            internal LayerTile(int x, int y, string layer, int[] gids, float animationSpeed)
            {
                this.X = x;
                this.Y = y;
                this.Layer = layer;
                this.Gids = gids;
                AnimationSpeed = animationSpeed;
                Animated = true;
            }
        }

        public class MapObject : IMapObject
        {
            public string Group { get; }

            public string Name { get; }

            public float X { get; }

            public float Y { get; }

            public ICollection<Vector2> Points { get; }

            public Dictionary<string, string> Properties { get; }

            internal MapObject(string @group, string name, float x, float y, ICollection<Vector2> points, Dictionary<string, string> properties)
            {
                Group = group;
                Name = name;
                X = x;
                Y = y;
                Points = points;
                Properties = properties;
            }
        }
    }
}
