﻿using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    class SettingsTests
    {
        [Test]
        public void GetSettingsTest()
        {
            Assert.AreEqual(360, Engine.Settings.MapWidth);
            Assert.AreEqual(360, Engine.Settings.MapHeight);
            Assert.AreEqual(120, Engine.Settings.GameWidth);
            Assert.AreEqual(40, Engine.Settings.GameHeight);
            Assert.AreEqual(false, Engine.Settings.IsPaused);
            Assert.AreEqual(false, Engine.Settings.ShowingMenu);
            Assert.Less(0, Engine.Settings.Random.Next(5,10));
            Assert.AreEqual(Radius.CIRCLE, Engine.Settings.FOVRadius);
            Assert.AreEqual(16, Engine.Settings.MovementKeyBindings.Count());
        }
        [Test]
        public void ToggleMenuTest()
        {
            Assert.AreEqual(false, Engine.Settings.IsPaused);
            Assert.AreEqual(false, Engine.Settings.ShowingMenu);
            Engine.Settings.ToggleMenu();
            Assert.AreEqual(true, Engine.Settings.IsPaused);
            Assert.AreEqual(true, Engine.Settings.ShowingMenu);
            Engine.Settings.ToggleMenu();
        }
        [Test]
        public void TogglePauseTest()
        {
            Assert.AreEqual(false, Engine.Settings.IsPaused);
            Assert.AreEqual(false, Engine.Settings.ShowingMenu);
            Engine.Settings.TogglePause();
            Assert.AreEqual(true, Engine.Settings.IsPaused);
            Assert.AreEqual(false, Engine.Settings.ShowingMenu);
        }
    }
}
