using MudDesigner.MudEngine.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudEngine.Game.Environment
{
    public class MudWorldFactory : IWorldFactory
    {
        readonly IRealmFactory realmFactory;
        public MudWorldFactory(IRealmFactory realmFactory)
        {
            this.realmFactory = realmFactory;
        }

        public Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay) 
            => this.CreateWorld(name, gameDayToRealWorldHoursRatio, hoursPerDay, Enumerable.Empty<ITimePeriod>(), Enumerable.Empty<IRealm>());

        public Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<ITimePeriod> timePeriods)
            => this.CreateWorld(name, gameDayToRealWorldHoursRatio, hoursPerDay, timePeriods, Enumerable.Empty<IRealm>());

        public Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<IRealm> realms)
            => this.CreateWorld(name, gameDayToRealWorldHoursRatio, hoursPerDay, Enumerable.Empty<ITimePeriod>(), realms);

        public async Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<ITimePeriod> timePeriods, IEnumerable<IRealm> realms)
        {
            var world = new MudWorld(this.realmFactory, timePeriods);

            world.SetName(name);
            world.GameDayToRealHourRatio = gameDayToRealWorldHoursRatio;
            world.SetHoursPerDay(hoursPerDay);

            await world.Initialize();

            if (realms.Count() > 0)
            {
                await world.AddRealmsToWorld(realms);
            }

            return world;
        }
    }
}
