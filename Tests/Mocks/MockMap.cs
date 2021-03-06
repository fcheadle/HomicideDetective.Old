using Engine.Scenes;
using Engine.Scenes.Areas;
using Engine.Scenes.Terrain;
using Engine.Utilities.Extensions;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Tests.Mocks
{
    public class MockMap : SceneMap
    {
        DefaultTerrainFactory _factory = new DefaultTerrainFactory();
        internal MockMap() : base(100,100)//base(100, 100, Calculate.EnumLength<MapLayer>(), Distance.MANHATTAN)
        {
            FloodWithSimpleTiles();
            TestMap();
        }
        private void TestMap()
        {
            BasicMap map = new BasicMap(42, 42, 1, Distance.EUCLIDEAN);
            int radius = 20;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    BasicTerrain t = new BasicTerrain(Color.White, Color.Black, (i + j) % 256, new Coord(i, j), true, true);
                    map.SetTerrain(t);
                }
            }

            Coord origin = new Coord(21, 21);
            Area room = new Area("wiseau's room", new Coord(30, 30), new Coord(30, 10), new Coord(10, 10), new Coord(10, 30));
            foreach (Coord c in room.InnerPoints)
                map.SetTerrain(_factory.MediumHardwoodFloor(c));
            foreach (Coord c in room.OuterPoints)
            {
                map.SetTerrain(_factory.BathroomLinoleum(c));
                if (c.X == map.Width / 2 || c.Y == map.Height / 2)
                    map.SetTerrain(_factory.ShagCarpet(c));
                else
                    map.SetTerrain(_factory.DarkHardwoodFloor(c));
            }
            this.ReplaceTiles(map, new Coord(0, 0));

            BasicMap rotated = map.Rotate(origin, radius, 45);
            this.ReplaceTiles(rotated, new Coord(0, 0));

        }

        private void FloodWithSimpleTiles()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (GetTerrain<BasicTerrain>(x, y) == null)
                        SetTerrain(new BasicTerrain(Color.White, Color.Black, '.', new Coord(x, y), true, true));
                }
            }
        }
    }
}