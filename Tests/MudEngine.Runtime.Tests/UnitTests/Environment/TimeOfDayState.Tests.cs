using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class TimeOfDayStateTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Ctor_sets_id_and_creation_date()
        {
            // Act
            var state = new TimeOfDayState();

            // Act
            Assert.IsNotNull(state.Id, "No ID was generated.");
            Assert.IsFalse(state.CreationDate == default(DateTime));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Can_set_and_retrieve_name()
        {
            // Arrange
            string name = "Test";

            // Act
            var state = new TimeOfDayState { Name = name };

            // Act
            Assert.AreEqual(name, state.Name);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Can_set_and_retrieve_start_time()
        {
            // Arrange
            var startTime = Mock.Of<ITimeOfDay>();

            // Act
            var state = new TimeOfDayState { StateStartTime = startTime };

            // Act
            Assert.AreEqual(startTime, state.StateStartTime);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public async Task Can_get_the_time_state_has_been_alive()
        {
            // Arrange
            var state = new TimeOfDayState();

            // Act
            await Task.Delay(TimeSpan.FromSeconds(1));
            int aliveTime = (int)state.TimeAlive;

            // Act
            Assert.IsTrue(aliveTime == 1);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [ExpectedException(typeof(ArgumentNullException))]
        [Owner("Johnathon Sullinger")]
        public void Initialize_throws_exception_with_null_start_time()
        {
            // Arrange
            var state = new TimeOfDayState();

            // Act
            state.Initialize(null, 2);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [ExpectedException(typeof(InvalidTimeOfDayException))]
        [Owner("Johnathon Sullinger")]
        public void Initialize_throws_exception_with_time_of_day_containing_no_hoursPerDay()
        {
            // Arrange
            var day = Mock.Of<ITimeOfDay>();
            var state = new TimeOfDayState();

            // Act
            state.Initialize(day, 2);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Initialize_assigns_time_of_day()
        {
            // Arrange
            var day = Mock.Of<ITimeOfDay>(m => m.Hour == 5 && m.HoursPerDay == 24);

            // Mock out the cloning of the ITimeOfDay instance.
            // The state initialization performs two clones at the moment.
            Mock.Get(day).Setup(mock => mock.Clone()).Returns(day);
            Mock.Get(day.Clone()).Setup(mock => mock.Clone()).Returns(day);

            var state = new TimeOfDayState();

            // Act
            state.Initialize(day, 0.05);

            // Assert
            Assert.IsNotNull(state.StateStartTime);
            Assert.AreEqual(5, state.StateStartTime.Hour);
            Assert.AreEqual(0, state.StateStartTime.Minute);
            Assert.AreEqual(24, state.StateStartTime.HoursPerDay);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public async Task State_clock_increments_by_the_hour()
        {
            // Arrange
            var day = Mock.Of<ITimeOfDay>(m => m.Hour == 5 && m.HoursPerDay == 24);

            // Mock out the cloning of the ITimeOfDay instance.
            // The state initialization performs two clones at the moment.
            Mock.Get(day).Setup(mock => mock.Clone()).Returns(day);
            Mock.Get(day.Clone()).Setup(mock => mock.Clone()).Returns(day);

            var state = new TimeOfDayState();

            // Act
            state.Initialize(day, 0.005);
            await Task.Delay(TimeSpan.FromSeconds(10));

            // Assert
            Assert.AreEqual(6, state.StateStartTime.Hour);
        }
    }
}
