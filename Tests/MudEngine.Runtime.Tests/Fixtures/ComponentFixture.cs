using System;
using MudDesigner.MudEngine;

namespace Tests.Fixtures
{
    class ComponentFixture : IComponent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
    }
}
