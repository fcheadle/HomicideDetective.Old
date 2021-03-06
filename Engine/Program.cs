using Engine.Creatures;
using Engine.Items;
using Engine.Scenes.Terrain;

namespace Engine
{
    public class Program
    {
        public static Game CurrentGame { get; private set; }
        internal static void Main()
        {

        }
        public static void Start(
            Settings settings = null,
            ICreatureFactory creatureFactory = null, 
            IItemFactory itemFactory = null, 
            DefaultTerrainFactory terrainFactory = null
            )
        {
            settings = settings ?? new Settings();
            creatureFactory = creatureFactory ?? new DefaultCreatureFactory();
            terrainFactory = terrainFactory ?? new DefaultTerrainFactory();
            itemFactory = itemFactory ?? new DefaultItemFactory();
            CurrentGame = new Game(settings, creatureFactory, itemFactory, terrainFactory);
            CurrentGame.Start();
            CurrentGame.Stop();
        }
    }
}
