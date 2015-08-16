using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine
{
    public interface IConfigurable
    {
        void Configure();
    }

    public interface IConfigurable<TConfiguration> : IConfigurable where TConfiguration : IConfiguration
    {
        void Configure(TConfiguration configuration);
    }
}
