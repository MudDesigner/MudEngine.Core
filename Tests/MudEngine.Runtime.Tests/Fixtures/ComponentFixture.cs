using System;
using MudDesigner.MudEngine;

namespace MudDesigner.MudEngine.Tests.Fixture
{
    public class ComponentFixture : IComponent
    {
        public DateTime CreationDate { get; private set; } = DateTime.Now;

        public Guid Id { get; private set; } = Guid.NewGuid();

        public bool IsEnabled { get; set; }

        public double TimeAlive { get; set; }

        public void Disable()
        {
            this.IsEnabled = false;
        }

        public void Enable()
        {
            this.IsEnabled = true;
        }
    }
}
