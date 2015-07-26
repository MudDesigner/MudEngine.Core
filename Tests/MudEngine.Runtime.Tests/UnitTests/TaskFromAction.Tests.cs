using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Core;

namespace Tests.Engine.Runtime.Tests
{
    /// <summary>
    /// Tests for ensuring an awaitable Task is returned when wrapping an Action.
    /// </summary>
    [TestClass]
    public class TaskFromActionTests
    {
        /// <summary>
        /// Tests that an Action is invoked with an awaitable Task returned.
        /// </summary>
        [TestMethod]
        [TestCategory("Mud Engine")]
        [TestCategory("Mud Engine - Runtime")]
        public async Task Returns_awaitable_task_when_invoked()
        {
            // Arrange
            bool isCallbackFinished = false;

            // Act
            await TaskFromAction.Invoke(() => isCallbackFinished = true);

            // Assert
            Assert.IsTrue(isCallbackFinished, "Callback was not called.");
        }

        /// <summary>
        /// Tests that an ArgumentNulLException is thrown when a 
        /// null argument is provided.
        /// </summary>
        [TestMethod]
        [TestCategory("Mud Engine")]
        [TestCategory("Mud Engine - Runtime")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception_thrown_when_action_is_null()
        {
            // Act
            Task task = TaskFromAction.Invoke(null);

            // Assert
            Assert.Fail();
        }
    }
}
