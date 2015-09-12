using System;
using System.Threading.Tasks;
using MudDesigner.MudEngine;
using MudDesigner.MudEngine.Actors;

namespace MudEngine.Game.Actors
{
    public abstract class MudActor : GameComponent, IActor
    {
        public string Description { get; set; }

        public IGender Gender { get; private set; }

        public IRace Race { get; private set; }

        public virtual void SetGender(IGender gender)
        {
            if (gender == null)
            {
                throw new ArgumentNullException(nameof(gender), "You can not assign a null gender to this actor.");
            }

            this.Gender = gender;
        }

        public virtual void SetRace(IRace race)
        {
            if (race == null)
            {
                throw new ArgumentNullException(nameof(race), "You can not assign a null race to this actor.");
            }

            this.Race = race;
        }
    }
}
