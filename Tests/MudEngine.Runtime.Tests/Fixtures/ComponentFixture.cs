using System;
using MudDesigner.MudEngine;

namespace Tests.Fixtures
{
    class ComponentFixture : IComponent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public bool IsEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Disable()
        {
            throw new NotImplementedException();
        }

        public void Enable()
        {
            throw new NotImplementedException();
        }
    }
}
