//-----------------------------------------------------------------------
// <copyright file="MudGame.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The Default engine implementation of the IGame interface. This implementation provides validation support via ValidationBase.
    /// </summary>
    public class MudGame : GameComponent, IGame
    {
        /// <summary>
        /// Gets information pertaining to the game.
        /// </summary>
        public IGameConfiguration Configuration { get; protected set; }

        /// <summary>
        /// Gets a value indicating that the initialized or not.
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Gets or sets the last saved.
        /// </summary>
        public DateTime LastSaved { get; private set; }

        /// <summary>
        /// Configures the game using the provided game configuration.
        /// </summary>
        /// <param name="config">The configuration the game should use.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task Configure(IGameConfiguration config)
        {
            this.Configuration = config;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Starts the game using the begin/end async pattern. This method requires the caller to handle the process life-cycle management as a loop is not generated internally.
        /// </summary>
        /// <param name="startCompletedCallback">The delegate to invoke when the game startup has completed.</param>
        public void BeginStart(Action<IGame> startCompletedCallback)
        {
            Task.Run(this.StartAsync)
            .ContinueWith(task => startCompletedCallback(this), TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Starts game asynchronously. This will start a game loop that can be awaited. The loop will run until stopped.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public async Task StartAsync()
        {
            if (!this.IsEnabled)
            {
                await this.Initialize();
            }

            IConfigurationComponent[] components = this.Configuration.GetConfigurationComponents();
            foreach (IConfigurationComponent component in components)
            {
                await component.Initialize();
            }

            this.IsRunning = true;
        }

        /// <summary>
        /// Stops the game from running.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public Task Stop()
        {
            if (!this.IsEnabled && !this.IsRunning)
            {
                this.IsEnabled = false;
                this.IsRunning = false;
                foreach (IConfigurationComponent component in this.Configuration.GetConfigurationComponents())
                {
                    component.Disable();
                }
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Loads the component and any resources or dependencies it might have.
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected override Task Load()
        {
            this.IsEnabled = true;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected override Task Unload()
        {
            return this.Stop();
        }
    }
}
