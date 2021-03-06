using Engine.Scenes.Components;
using Engine.Scenes.Terrain;
using GoRogue;
using NUnit.Framework;

namespace Tests.Scenes.Components
{
    class AnimateGlyphComponentTest : TestBase
    {
        DefaultTerrainFactory factory = new DefaultTerrainFactory();
        AnimateGlyphComponent _base;
        string[] _answer;

        [SetUp]
        public void SetUp()
        {
            _base = factory.Grass(new Coord()).GetComponent<AnimateGlyphComponent>();
        }
        [Test]
        public void NewAnimateGlyphComponent()
        {
            Assert.NotNull(_base);
        }

        [Test]
        public void GetDetailsTest()
        {
            _answer = _base.GetDetails();
            Assert.AreEqual(1, _answer.Length);
        }
    }
}