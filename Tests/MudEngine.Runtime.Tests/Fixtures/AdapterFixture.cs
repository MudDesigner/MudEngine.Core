using System;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Tests.Fixtures
{
    public class AdapterFixture : AdapterBase
    {
        public override string Name => "Adapter fixture";

        public override void Configure()
        {
            throw new NotImplementedException();
        }

        public override Task Delete()
        {
            return Task.FromResult(0);
        }

        public override Task Initialize()
        {
            return Task.FromResult(0);
        }

        public override Task Start(IGame game)
        {
            return Task.FromResult(0);
        }
    }
}
