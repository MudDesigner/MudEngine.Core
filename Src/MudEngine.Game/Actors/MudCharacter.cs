using System;
using System.Collections.Generic;
using System.Linq;

namespace MudDesigner.MudEngine.Actors
{
    public abstract class MudCharacter : MudActor, ICharacter
    {
        protected List<IStat> Stats { get; set; } = new List<IStat>();

        protected List<IMountPoint> MountPoints { get; set; } = new List<IMountPoint>();

        public ICharacterClass CharacterClass { get; private set; }

        public virtual void AddAbility(IStat ability)
        {
            if (ability == null)
            {
                throw new ArgumentNullException(nameof(ability), "The ability argument can not be null.");
            }

            if (this.Stats.Contains(ability))
            {
                return;
            }

            this.Stats.Add(ability);
        }

        public void AddMountPoint(IMountPoint mountPoint)
        {
            if (mountPoint == null)
            {
                throw new ArgumentNullException(nameof(mountPoint), "The mountPoint argument can not be null.");
            }

            if (this.MountPoints.Contains(mountPoint))
            {
                return;
            }

            this.MountPoints.Add(mountPoint);
        }

        public IMountPoint FindMountPoint(string pointName)
        {
            return this.MountPoints.FirstOrDefault(point => point.Name.ToLower() == pointName.ToLower());
        }

        public IStat[] GetAbilities() => this.Stats.ToArray();

        public IMountPoint[] GetMountPoints() => this.MountPoints.ToArray();
    }
}
