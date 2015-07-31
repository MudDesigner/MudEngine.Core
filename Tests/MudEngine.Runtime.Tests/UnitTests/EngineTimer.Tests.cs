using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine;
using Tests.Fixtures;

namespace Tests.UnitTests
{
    [TestClass]
    public class EngineTimerTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception_thrown_with_null_ctor_argument()
        {
            // Act
            new EngineTimer<ComponentFixture>(null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Ctor_sets_state_property()
        {
            // Arrange
            var fixture = new ComponentFixture();

            // Act
            var engineTimer = new EngineTimer<ComponentFixture>(fixture);

            // Assert
            Assert.IsNotNull(engineTimer.StateData, "State was not assigned from the constructor.");
            Assert.AreEqual(fixture, engineTimer.StateData, "An incorrect State object was assigned to the timer.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Start_sets_is_running()
        {
            // Arrange
            var fixture = new ComponentFixture();
            var engineTimer = new EngineTimer<ComponentFixture>(fixture);

            // Act
            engineTimer.Start(0, 1, 0, (component, timer) => { });

            // Assert
            Assert.IsTrue(engineTimer.IsRunning, "Engine Timer was not started.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Callback_invoked_when_running()
        {
            // Arrange
            var fixture = new ComponentFixture();
            var engineTimer = new EngineTimer<ComponentFixture>(fixture);
            bool callbackInvoked = false;

            // Act
            engineTimer.Start(0, 1, 0, (component, timer) => { callbackInvoked = true; });
            Task.Delay(20);

            // Assert
            Assert.IsTrue(callbackInvoked, "Engine Timer did not invoke the callback as expected.");
        }
    }
}
