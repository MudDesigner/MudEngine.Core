using MudDesigner.MudEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Fixtures
{
    class ComponentFixture : IComponent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
    }
}
