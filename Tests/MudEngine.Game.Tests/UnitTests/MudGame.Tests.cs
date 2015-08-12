using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine;
using MudDesigner.MudEngine.Environment;
using MudDesigner.MudEngine.Game;
using MudEngine.Game.Tests.Fixtures;

namespace MudEngine.Game.Tests.UnitTests
{
    [TestClass]
    public class MudGameTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Configuring_a_game_assigns_the_game_configuration_instance()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();

            // Act
            await game.Configure(configuration);

            // Assert
            Assert.IsNotNull(game.Configuration);
            Assert.AreEqual(configuration, game.Configuration);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task BeginStart_invokes_callback()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();
            await game.Configure(configuration);
            bool callbackHit = false;

            // We must create a synchronization context as the BeginStart needs one to exist.
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            // Act
            game.BeginStart(g => callbackHit = true);

            await Task.Delay(100);

            // Assert
            Assert.IsTrue(callbackHit, "Callback was not hit.");
            Assert.IsTrue(game.IsRunning);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Start_runs_the_game()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();
            await game.Configure(configuration);

            // Act
            await game.StartAsync();

            // Assert
            Assert.IsTrue(game.IsRunning);
            Assert.IsTrue(game.IsEnabled);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Initialize_enables_the_game()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();
            await game.Configure(configuration);

            // Act
            await game.Initialize();

            // Assert
            Assert.IsTrue(game.IsEnabled);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Start_game_will_start_adapter()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>(mock => mock.GetAdapters() == new IAdapter[1] { new AdapterFixture() });
            var game = new MudGame();
            await game.Configure(configuration);

            IAdapter[] adapter = game.Configuration.GetAdapters();

            // Act
            await game.StartAsync();

            // Assert
            Assert.IsTrue(((AdapterFixture)adapter[0]).IsInitialized);
            Assert.IsTrue(((AdapterFixture)adapter[0]).IsStarted);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Stop_game_will_delete_adapter()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>(mock => mock.GetAdapters() == new IAdapter[1] { new AdapterFixture() });
            var game = new MudGame();
            await game.Configure(configuration);

            IAdapter[] adapter = game.Configuration.GetAdapters();

            // Act
            await game.StartAsync();
            await game.Stop();

            // Assert
            Assert.IsTrue(((AdapterFixture)adapter[0]).IsDeleted);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Delete_game_will_delete_adapter()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>(mock => mock.GetAdapters() == new IAdapter[1] { new AdapterFixture() });
            var game = new MudGame();
            await game.Configure(configuration);

            IAdapter[] adapter = game.Configuration.GetAdapters();

            // Act
            await game.StartAsync();
            await game.Delete();

            // Assert
            Assert.IsTrue(((AdapterFixture)adapter[0]).IsDeleted);
        }

        private async Task TestGameStartup()
        {
            // Mocks & Adapters
            IWorldFactory worldFactory = Mock.Of<IWorldFactory>();
            IAdapter server = Mock.Of<AdapterBase>();
            IAdapter worldManager = new WorldManager(worldFactory);

            // Create our game configuration
            IGameConfiguration configuration = new GameConfiguration { Name = "Sample Mud Game", };
            configuration.UseAdapter(server);
            configuration.UseAdapter(worldManager);

            // Setup and run the game.
            IGame game = new MudGame();
            await game.Configure(configuration);
            await game.StartAsync();
            await game.Delete();
        }
    }
}
