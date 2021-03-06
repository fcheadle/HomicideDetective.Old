using Engine.Scenes;
using Engine.Scenes.Areas;
using GoRogue;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Scenes.Areas
{
    [TestFixture]
    class AreaTests
    {
        private readonly Coord _sw = new Coord(3, 4);
        private readonly Coord _nw = new Coord(1, 1);
        private readonly Coord _ne = new Coord(5, 0);
        private readonly Coord _se = new Coord(7, 3);
        Area _area;
        [SetUp]
        public void SetUp()
        {
            _area = new Area("forbidden zone", _se, _ne, _nw, _sw);
        }
        [Test]
        public void AreaTest()
        {
            Assert.AreEqual(14, _area.OuterPoints.Count);
            Assert.AreEqual(23, _area.InnerPoints.Count);
            Assert.AreEqual(5, _area.NorthBoundary.Count);
            Assert.AreEqual(4, _area.WestBoundary.Count);
            Assert.AreEqual(5, _area.SouthBoundary.Count);
            Assert.AreEqual(4, _area.EastBoundary.Count);
        }
        [Test]
        public void ToStringOverrideTest()
        {
            Assert.AreEqual("forbidden zone", _area.ToString());
        }
        [Test]
        public void ContainsTest()
        {
            Area area = new Area("forbidden zone", _se, _ne, _nw, _sw);
            Assert.IsFalse(area.Contains(new Coord(-5, -5)));
            Assert.IsFalse(area.Contains(new Coord(1, 2)));
            Assert.IsFalse(area.Contains(new Coord(9, 8)));
            Assert.IsFalse(area.Contains(new Coord(6, 15)));
            Assert.IsTrue(area.Contains(new Coord(2, 1)));
            Assert.IsTrue(area.Contains(new Coord(4, 1)));
            Assert.IsTrue(area.Contains(new Coord(6, 3)));
            Assert.IsTrue(area.Contains(new Coord(3, 3)));
        }
        [Test]
        public void OverlapTest()
        {
            Coord tl = new Coord(0, 0);
            Coord tr = new Coord(2, 0);
            Coord br = new Coord(2, 2);
            Coord bl = new Coord(0, 2);
            Area a2 = new Area("zone of terror", br, tr, tl, bl);
            List<Coord> answer = _area.Overlap(a2).ToList();

            foreach (Coord c in answer)
            {
                Assert.IsTrue(_area.Contains(c));
                Assert.IsTrue(a2.Contains(c));
            }
        }
        [Test]
        public void LeftAtTest()
        {
            Assert.AreEqual(_nw.X, _area.LeftAt(_nw.Y));
            Assert.AreEqual(3, _area.LeftAt(_area.Top)); //the rise/run of the top left-right meetx y=0 at x=3
            Assert.AreEqual(3, _area.LeftAt(_area.Bottom));
        }
        [Test]
        public void RightAtTest()
        {
            Assert.AreEqual(_ne.X, _area.RightAt(_ne.Y));
            Assert.AreEqual(5, _area.RightAt(_area.Top));
            Assert.AreEqual(5, _area.RightAt(_area.Bottom));
        }
        [Test]
        public void TopAtTest()
        {
            Assert.AreEqual(_ne.Y, _area.TopAt(_ne.X));
            Assert.AreEqual(_nw.Y, _area.TopAt(_nw.X));
            Assert.AreEqual(0, _area.TopAt(5));
        }
        [Test]
        public void BottomAtTest()
        {
            Assert.AreEqual(_se.Y, _area.BottomAt(_se.X));
            Assert.AreEqual(_sw.Y, _area.BottomAt(_sw.X));
            Assert.AreEqual(4, _area.BottomAt(5));
        }
        [Test]
        public void TopTest()
        {
            Assert.AreEqual(0, _area.Top);
        }
        [Test]
        public void BottomTest()
        {
            Assert.AreEqual(4, _area.Bottom);
        }
        [Test]
        public void LeftTest()
        {
            Assert.AreEqual(1, _area.Left);
        }
        [Test]
        public void RightTest()
        {
            Assert.AreEqual(7, _area.Right);
        }
        [Test]
        public void ShiftWithParametersTest()
        {
            Coord two = new Coord(2, 2);
            Area a1 = _area;
            Area a2 = _area.Shift(two);

            foreach (Coord inner in a2.InnerPoints)
                Assert.IsTrue(a1.Contains(inner - two));
        }

        [Test]
        public void DistinguishSubAreasTest()
        {
            /* 0123456
             * XXXXXXX
             * X     X
             * X     X
             * X     X
             * X     Xxxx
             * X     X  x
             * XXXXXXX  x
             *    x     x
             *    xxxxxxx
             */
            Coord nw = new Coord(0, 0);
            Coord sw = new Coord(0, 9);
            Coord se = new Coord(9, 9);
            Coord ne = new Coord(9, 0);
            Area mainArea = new Area("parent area", ne: ne, nw: nw, se: se, sw: sw);

            nw = new Coord(1, 1);
            se = new Coord(5, 5);
            sw = new Coord(1, 5);
            ne = new Coord(5, 1);
            Area imposingSubArea = new Area("imposing sub area", ne: ne, nw: nw, se: se, sw: sw);

            nw = new Coord(4, 4);
            se = new Coord(8, 8);
            sw = new Coord(4, 8);
            ne = new Coord(8, 4);
            Area hostSubArea = new Area("host sub area", ne: ne, nw: nw, se: se, sw: sw);

            mainArea.SubAreas.Add(RoomType.Parlor, hostSubArea);
            mainArea.SubAreas.Add(RoomType.GuestBathroom, imposingSubArea);

            mainArea.DistinguishSubAreas();
            hostSubArea = mainArea[RoomType.Parlor];
            imposingSubArea = mainArea[RoomType.GuestBathroom];
            foreach (Coord c in imposingSubArea.InnerPoints)
            {
                Assert.IsTrue(mainArea.Contains(c), "Main area somehow had a coord removed.");
                Assert.IsFalse(hostSubArea.Contains(c), "sub area contains a coordinate in imposing area.");
            }
            foreach (Coord c in hostSubArea.InnerPoints)
            {
                Assert.IsTrue(mainArea.Contains(c), "Main area somehow lost a coord.");
                Assert.IsFalse(imposingSubArea.Contains(c), "a coord should have been removed from the sub area but was not.");
            }
        }

        [Test]
        public void AddConnectionBetweenTest()
        {
            Area a = AreaFactory.Rectangle("test-tangle", new Coord(0, 0), 6, 6);
            Area b = AreaFactory.Rectangle("rest-ert", new Coord(a.Right, a.Top), 5, 5);
            int aCountBefore = a.OuterPoints.Count();
            int bCountBefore = b.OuterPoints.Count();
            Area.AddConnectionBetween(a, b);
            Assert.AreEqual(1, a.Connections.Count());
            Assert.AreEqual(1, b.Connections.Count());

            Assert.AreEqual(aCountBefore - 1, a.OuterPoints.Count());
            Assert.AreEqual(bCountBefore - 1, b.OuterPoints.Count());

        }
        [Test]
        public void RemoveOverlappingOuterpointsTest()
        {
            Area a = AreaFactory.FromRectangle("Area A", new Rectangle(new Coord(1, 1), new Coord(3, 4)));
            Area b = AreaFactory.FromRectangle("Area B", new Rectangle(new Coord(3, 0), new Coord(6, 5)));

            int aCountBefore = a.OuterPoints.Count();
            int bCountBefore = b.OuterPoints.Count();
            a.RemoveOverlappingOuterpoints(b);
            Assert.Less(a.OuterPoints.Count(), aCountBefore, "No connecting points were removed from Area A");
            Assert.AreEqual(b.OuterPoints.Count(), bCountBefore, "Connecting points were unexpectedly removed from Area b");
        }
        [Test]
        public void RemoveOverlappingInnerpointsTest()
        {
            Area a = AreaFactory.FromRectangle("Area A", new Rectangle(new Coord(1, 1), new Coord(3, 4)));
            Area b = AreaFactory.FromRectangle("Area B", new Rectangle(new Coord(3, 0), new Coord(6, 5)));

            int aCountBefore = a.OuterPoints.Count();
            int bCountBefore = b.OuterPoints.Count();
            a.RemoveOverlappingInnerpoints(b);
            Assert.Less(a.OuterPoints.Count(), aCountBefore, "No connecting points were removed from Area A");
            Assert.AreEqual(b.OuterPoints.Count(), bCountBefore, "Connecting points were unexpectedly removed from Area b");
        }
        [Test]
        public void RemoveOverlappingPointsTest()
        {
            Area a = AreaFactory.FromRectangle("Area A", new Rectangle(new Coord(3, 3), new Coord(5, 5)));
            Area b = AreaFactory.FromRectangle("Area B", new Rectangle(new Coord(1, 1), new Coord(7, 7)));

            
            int bCountBefore = b.OuterPoints.Count();
            a.RemoveOverlappingPoints(b);

            Assert.AreEqual(0, a.OuterPoints.Count());
            Assert.AreEqual(0, a.InnerPoints.Count());
            Assert.AreEqual(b.OuterPoints.Count(), bCountBefore, "Connecting points were unexpectedly removed from Area b");
        }
        [Test]
        public void RotateTest()
        {
            /* (0,0 & 0,1)
             * ###
             * #  ##
             * #    ## 
             *  #     ##
             *  #       ##
             *   #        ##
             *   #          ## (14, 6) 
             *    #         #
             *    #        #
             *     #      #
             *     #     #
             *      #   #
             *      #  #
             *       ##
             *       # (6, 14)
             */ 
            float degrees = 45.0f;
            
            Area prior = new Area("bermuda triangle", new Coord(14, 6), new Coord(0, 1), new Coord(0, 0), new Coord(6, 14));
            
            Area post = prior.Rotate(degrees, false, new Coord(6,14));
            Assert.AreEqual(prior.Bottom, post.Bottom);
            Assert.AreEqual(prior.SouthWestCorner, post.SouthWestCorner);
            Assert.Less(prior.Left, post.Left);
            Assert.Less(prior.Right, post.Right);
            Assert.Less(prior.SouthEastCorner.X, post.SouthEastCorner.X);
            Assert.Less(prior.SouthEastCorner.Y, post.SouthEastCorner.Y);

            //Assert.AreEqual(copyOfPrior, prior);

            //prior.Rotate(degrees, true, new Coord(6, 14));

            //Assert.AreEqual(prior, post);
        }
    }
}
