﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.Environment;
using MudEngine.Game.Environment;

namespace MudEngine.Game.Tests.UnitTests.Environment
{
    [TestClass]
    public class MudWorldTests
    {
        [TestMethod]
        public void Ctor_sets_default_hours_per_day()
        {
            // Act
            var world = new MudWorld(Mock.Of<IRealmFactory>());

            // Assert
            Assert.IsTrue(world.HoursPerDay > 0);
        }

        [TestMethod]
        public void Ctor_sets_default_GameDayToRealHourRatio()
        {
            // Act
            var world = new MudWorld(Mock.Of<IRealmFactory>());

            // Assert
            Assert.IsTrue(world.GameDayToRealHourRatio > 0);
        }

        [TestMethod]
        public void Ctor_creates_time_manager_with_time_periods()
        {
            // Arrange
            var timePeriods = new List<ITimePeriod>
            {
                Mock.Of<ITimePeriod>(mock => mock.StateStartTime == Mock.Of<ITimeOfDay>())
            };

            // Act
            var world = new MudWorld(Mock.Of<IRealmFactory>(), timePeriods);

            // Assert
            Assert.IsNotNull(world.TimePeriodManager.GetTimePeriodForDay(
                Mock.Of<ITimeOfDay>(mock => mock.Hour == 0 && mock.Minute == 0)));
        }

        [TestMethod]
        public void World_returns_available_time_periods()
        {
            // Arrange
            var timePeriods = new List<ITimePeriod>
            {
                Mock.Of<ITimePeriod>(mock => mock.StateStartTime == Mock.Of<ITimeOfDay>())
            };

            // Act
            var world = new MudWorld(Mock.Of<IRealmFactory>(), timePeriods);

            // Assert
            Assert.AreEqual(1, world.GetTimePeriodsForWorld().Count());
        }

        [TestMethod]
        public async Task World_can_create_a_realm()
        {
            // Arrange
            IRealmFactory factory = Mock.Of<IRealmFactory>(
                mock => mock.CreateRealm(It.IsAny<string>(), It.IsAny<IWorld>()) == Task.FromResult(Mock.Of<IRealm>()));

            var world = new MudWorld(factory);
            var realmName = "Test Realm";

            // Act
            IRealm realm = await world.CreateRealm(realmName);

            // Assert
            Assert.IsNotNull(realm);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRealmException))]
        public async Task Adding_realm_to_world_without_a_name_throws_exception()
        {
            // Arrange
            IRealmFactory factory = Mock.Of<IRealmFactory>(
                mock => mock.CreateRealm(It.IsAny<string>(), It.IsAny<IWorld>()) == Task.FromResult(Mock.Of<IRealm>()));

            var world = new MudWorld(factory);
            var realmName = "Test Realm";
            IRealm realm = await world.CreateRealm(realmName);

            // Act
            await world.AddRealmToWorld(realm);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public async Task World_can_add_new_realm()
        {
            // Arrange
            IRealmFactory factory = Mock.Of<IRealmFactory>(
                mock => mock.CreateRealm(It.IsAny<string>(), It.IsAny<IWorld>()) == Task.FromResult(
                    Mock.Of<IRealm>(r => r.Name == "Unit Test")));

            var world = new MudWorld(factory);
            var realmName = "Test Realm";
            IRealm realm = await world.CreateRealm(realmName);

            // Act
            await world.AddRealmToWorld(realm);

            // Assert
            Assert.AreEqual(1, world.GetRealmsInWorld().Count());
        }

        [TestMethod]
        public async Task World_can_add_collection_of_realms()
        {
            // Arrange
            var world = new MudWorld(Mock.Of<IRealmFactory>());
            var realms = new List<IRealm>() { Mock.Of<IRealm>(r => r.Name == "R1"), Mock.Of<IRealm>(r => r.Name == "R2") };

            // Act
            await world.AddRealmsToWorld(realms);

            // Assert
            Assert.AreEqual(2, world.GetRealmsInWorld().Count());
        }

        [TestMethod]
        public async Task World_does_not_add_same_realm_twice()
        {
            // Arrange
            IRealmFactory factory = Mock.Of<IRealmFactory>(
                mock => mock.CreateRealm(It.IsAny<string>(), It.IsAny<IWorld>()) == Task.FromResult(
                    Mock.Of<IRealm>(r => r.Name == "Unit Test")));

            var world = new MudWorld(factory);
            var realmName = "Test Realm";
            IRealm realm = await world.CreateRealm(realmName);

            // Act
            await world.AddRealmToWorld(realm);
            await world.AddRealmToWorld(realm);

            // Assert
            Assert.AreEqual(1, world.GetRealmsInWorld().Count());
        }

        [TestMethod]
        public async Task Adding_realm_sets_its_enabled_flag()
        {
            // Arrange
            IRealmFactory factory = Mock.Of<IRealmFactory>(mock => 
            mock.CreateRealm(It.IsAny<string>(), It.IsAny<IWorld>()) == Task.FromResult(
                    Mock.Of<IRealm>(r => r.Name == "Unit Test" && r.IsEnabled == true)));

            var world = new MudWorld(factory);
            var realmName = "Test Realm";
            IRealm realm = await world.CreateRealm(realmName);

            // Act
            await world.AddRealmToWorld(realm);

            // Assert
            Assert.IsTrue(realm.IsEnabled);
        }
    }
}
