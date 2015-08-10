//-----------------------------------------------------------------------
// <copyright file="GameComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    using System;
    using System.Threading.Tasks;
    using MessageBrokering;

    /// <summary>
    /// The root class for all game Types.
    /// </summary>
    public abstract class GameComponent : IGameComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameComponent"/> class.
        /// </summary>
        public GameComponent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// The Loading event is fired during initialization of the component prior to being loaded.
        /// </summary>
        public event Func<IGameComponent, Task> Loading;

        /// <summary>
        /// The Loaded event is fired upon completion of the components initialization and loading.
        /// </summary>
        public event EventHandler<EventArgs> Loaded;

        /// <summary>
        /// The Deleting event is fired immediately upon a delete request.
        /// </summary>
        public event Func<IGameComponent, Task> Deleting;

        /// <summary>
        /// The Deleted event is fired once the object has finished processing it's unloading and clean up.
        /// </summary>
        public event EventHandler<EventArgs> Deleted;

        /// <summary>
        /// Gets the name of this component.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the unique identifier for this component.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; protected set; }

        /// <summary>
        /// Gets the date that this component was instanced.
        /// </summary>
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// Gets the amount number of seconds that this component instance has been alive.
        /// </summary>
        public double TimeAlive
        {
            get
            {
                return DateTime.Now.Subtract(this.CreationDate).TotalSeconds;
            }
        }

        /// <summary>
        /// Initializes the game component.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public async Task Initialize()
        {
            MessageBrokerFactory.Instance.Publish(new InfoMessage($"Initializing {this.Name ?? "GameComponent"} ({this.GetType().Name})"));
            await this.LoadingBegan();

            this.Enable();
            await this.Load();

            this.LoadingCompleted();
            MessageBrokerFactory.Instance.Publish(new InfoMessage($"Initialization of {this.Name ?? "GameComponent"} ({this.GetType().Name}) completed."));
        }

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <para>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </para>
        /// <returns>Returns an awaitable Task</returns>
        public async Task Delete()
        {
            await this.OnDeleteRequested();

            this.Disable();
            await this.Unload();

            this.OnDeleted();
        }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        public void Disable()
        {
            this.IsEnabled = false;
        }

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public void Enable()
        {
            this.IsEnabled = true;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name)
                ? $"{this.GetType().Name}"
                : $"{this.Name} ({this.GetType().Name})";
        }

        /// <summary>
        /// Loads the component and any resources or dependencies it might have. 
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected abstract Task Load();

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected abstract Task Unload();

        /// <summary>
        /// This gets called when initialization of the component begins. 
        /// This is called prior to Load() being invoked.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadingBegan()
        {
            var handler = this.Loading;
            if (handler == null)
            {
                return;
            }

            await handler(this);
        }

        /// <summary>
        /// Called when initialization is completed. Provides last-chance initialization support.
        /// Called after Load() has completed.
        /// </summary>
        protected virtual void LoadingCompleted()
        {
            var handler = this.Loaded;
            if (handler == null)
            {
                return;
            }

            handler(this, new EventArgs());
        }

        /// <summary>
        /// This gets called when deletion of the component begins. 
        /// This is called prior to Unload() being invoked.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task OnDeleteRequested()
        {
            var handler = this.Deleting;
            if (handler == null)
            {
                return;
            }

            await handler(this);
        }

        /// <summary>
        /// Called when Deletion is completed. Provides last-chance clean-up support.
        /// Called after Unload() has completed.
        /// </summary>
        protected virtual void OnDeleted()
        {
            var handler = this.Deleted;
            if (handler == null)
            {
                return;
            }

            handler(this, new EventArgs());
        }
    }
}
