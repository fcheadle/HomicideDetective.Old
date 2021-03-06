using System.Collections.Generic;
using Engine.Utilities.Extensions;
using GoRogue;
using NUnit.Framework;

namespace Tests.Utilities.Extensions
{
    class CoordExtensionTests
    {
        [Test]
        [Category("NonGraphical")]
        public void NeighborsTest()
        {
            Coord c = new Coord(0, 0);
            List<Coord> neighbors = c.Neighbors();
            Assert.AreEqual(4, neighbors.Count);
            Assert.IsTrue(neighbors.Contains(new Coord(-1, 0)));
            Assert.IsTrue(neighbors.Contains(new Coord(1, 0)));
            Assert.IsTrue(neighbors.Contains(new Coord(0, 1)));
            Assert.IsTrue(neighbors.Contains(new Coord(0, -1)));
        }
    }
}
