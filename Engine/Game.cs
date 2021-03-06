using Engine.Creatures;
using Engine.Items;
using Engine.Scenes;
using Engine.Scenes.Components;
using Engine.Scenes.Terrain;
using Engine.UI;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine
{
    public class Game
    {
        public const double TimeIncrement = 100;
        public static Settings Settings => _settings; 
        public static ICreatureFactory CreatureFactory => _creatureFactory; 
        public static IItemFactory ItemFactory => _itemFactory; 
        public static DefaultTerrainFactory TerrainFactory => _terrainFactory; 
        public static CrimeSceneInvestigationUi UiManager => _csi;
        public static MenuUi Menu => _menu;
        public static SceneMap Map => UiManager.Map;
        public BasicEntity Player => UiManager.Player;

        private static Settings _settings;
        private static ICreatureFactory _creatureFactory;
        private static DefaultTerrainFactory _terrainFactory;
        private static IItemFactory _itemFactory;
        private static CrimeSceneInvestigationUi _csi;
        private static MenuUi _menu;

        public bool IsPaused { get => Global.CurrentScreen.IsPaused; set => Global.CurrentScreen.IsPaused = value; }

        public Game(Settings settings, ICreatureFactory creatureFactory, IItemFactory itemFactory, DefaultTerrainFactory terrainFactory) 
        {
            ApplySettings(settings);
            SetCreatureFactory(creatureFactory);
            SetItemFactory(itemFactory);
            SetTerrainFactory(terrainFactory);
            Setup();
        }

        protected Game()
        {
            SadConsole.Themes.Library.Default.SetControlTheme(typeof(TextArea), new PaperButtonTheme());
        }

        protected void ApplySettings(Settings settings)
        {
            _settings = settings;
        }

        protected void SetCreatureFactory(ICreatureFactory creatureFactory)
        {
            _creatureFactory = creatureFactory;
        }

        public static void SwitchUserInterface()
        {
            if (Global.CurrentScreen == _csi)
            {
                Global.CurrentScreen = _menu;
                _csi.Hide();
                _menu.Show();
                _menu.Player.IsFocused = true;
            }
            else
            {
                Global.CurrentScreen = _csi;
                _menu.Hide();
                _csi.Show();
                _csi.Player.IsFocused = true;
            }
        }

        protected void SetItemFactory(IItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }

        protected void SetTerrainFactory(DefaultTerrainFactory terrainFactory)
        {
            _terrainFactory = terrainFactory;
        }
        protected void Setup()
        {
            SadConsole.Themes.Library.Default.SetControlTheme(typeof(TextArea), new PaperButtonTheme());
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
        }
        public virtual void Init()
        {
            _csi = new CrimeSceneInvestigationUi();
            _csi.Components.Add(new WeatherComponent());
            _menu = new MenuUi();
            Global.CurrentScreen = UiManager;
        }
        public virtual void Start()
        {
            SadConsole.Game.Instance.Run();
        }
        public virtual void Stop()
        {
            SadConsole.Game.Instance.Dispose();
        }
        public virtual void Update(GameTime time)
        {
        }
    }
}