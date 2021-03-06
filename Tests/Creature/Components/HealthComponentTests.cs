using Engine.Creatures.Components;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace Tests.Creature.Components
{
    class HealthComponentTests : TestBase
    {

        //sometimes you just need to store values for checking during updates
        HealthComponent _component;
        string[] _answer;
        float _breath;
        float _maximum;
        float _minimumHeartStatus;
        float _currentHeartStatus;
        float _maximumHeartStatus;
        DateTime _start;
        DateTime _previous;

        public HealthComponentTests()
        {
            _maximumHeartStatus = 0;
        }

        [Test]
        public void NewHealthComponentTest()
        {
            Assert.NotNull(_component);
            Assert.NotNull(_component.SystoleBloodPressure);
            Assert.Less(0, _component.SystoleBloodPressure);
            Assert.NotNull(_component.DiastoleBloodPressure);
            Assert.Less(0, _component.DiastoleBloodPressure);
            Assert.NotNull(_component.Pulse);
            Assert.Less(0, _component.Pulse);
            Assert.NotNull(_component.BreathRate);
            Assert.Less(0, _component.BreathRate);
            Assert.NotNull(_component.NormalBodyTemperature);
            Assert.Less(0, _component.NormalBodyTemperature);
            Assert.NotNull(_component.CurrentBodyTemperature);
            Assert.Less(0, _component.CurrentBodyTemperature);
            Assert.NotNull(_component.LungCapacity);
            Assert.Less(0, _component.LungCapacity);
            Assert.NotNull(_component.CurrentBreathVolume);
            Assert.Less(0, _component.CurrentBreathVolume);
            Assert.NotNull(_component.BloodVolume);
            Assert.Less(0, _component.BloodVolume);
            Assert.NotNull(_component.TypicalBloodVolume);
            Assert.Less(0, _component.TypicalBloodVolume);
            _component.ProcessTimeUnit();
            _breath = _component.CurrentBreathVolume;
        }
        [Test]
        public void GetDetailsTest()
        {
            _component = (HealthComponent)Game.Player.GetComponent<HealthComponent>();
            _answer = _component.GetDetails();
            _maximum = _component.LungCapacity;
            Assert.Less(4, _answer.Length);
        }

        [Test]
        public void BreathingTest()
        {
            Game.SwapUpdate(JustBreathe);
            _start = DateTime.Now;
            _previous = _start;
            for (int i = 0; i < 33; i++)
            {
                Game.RunOnce();
            }
        }
        private void JustBreathe(GameTime time)
        {
            if (DateTime.Now - _previous > TimeSpan.FromSeconds(2))
            {
                _previous = DateTime.Now;
                _component = (HealthComponent)Game.Player.GetComponent<HealthComponent>();
                Assert.AreNotEqual(_breath, _component.CurrentBreathVolume);
                _breath = _component.CurrentBreathVolume;
                Assert.Less(0, _breath);
                Assert.Greater(_maximum, _breath);
            }
        }
        [Test]
        public void HeartBeatTest()
        {
            Game.SwapUpdate(BeatHeart);
            _start = DateTime.Now;
            _previous = _start;
            for (int i = 0; i < 33; i++)
            {
                Game.RunOnce();
            }
            Assert.LessOrEqual(-2, Math.Round(_minimumHeartStatus));
            Assert.GreaterOrEqual(2, Math.Round(_maximumHeartStatus));
        }
        private void BeatHeart(GameTime time)
        {
            _previous = DateTime.Now;
            _component = (HealthComponent)Game.Player.GetComponent<HealthComponent>();
            _currentHeartStatus = _component.MonitorHeart().Y;
            if (_minimumHeartStatus > _currentHeartStatus)
                _minimumHeartStatus = _currentHeartStatus;
            if (_maximumHeartStatus < _currentHeartStatus)
                _maximumHeartStatus = _currentHeartStatus;

        }
    }
}
