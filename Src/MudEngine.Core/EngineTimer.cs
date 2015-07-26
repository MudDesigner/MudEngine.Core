//-----------------------------------------------------------------------
// <copyright file="EngineTimer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <para>
    /// The Engine Timer allows for starting a timer that will execute a callback at a given interval.
    /// </para>
    /// <para>
    /// The timer may fire:
    ///  - infinitely at the given interval
    ///  - fire once
    ///  - fire _n_ number of times.
    /// </para>
    /// <para>
    /// The Engine Timer will stop its self when it is disposed of.
    /// </para>
    /// <para>
    /// The Timer requires you to provide it an instance that will have an operation performed against it.
    /// The callback will be given the generic instance at each interval fired.
    /// </para>
    /// <para>
    /// In the following example, the timer is given an instance of an IPlayer. 
    /// It starts the timer off with a 30 second delay before firing the callback for the first time.
    /// It tells the timer to fire every 60 seconds with 0 as the number of times to fire. When 0 is provided, it will run infinitely.
    /// Lastly, it is given a callback, which will save the player every 60 seconds.
    /// @code
    /// var timer = new EngineTimer<IPlayer>(new DefaultPlayer());
    /// timer.StartAsync(30000, 6000, 0, (player, timer) => player.Save());
    /// @endcode
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type that will be provided when the timer callback is invoked.</typeparam>
    public sealed class EngineTimer<T> : CancellationTokenSource, IDisposable
    {
        /// <summary>
        /// The timer task
        /// </summary>
        private Task timerTask;

        /// <summary>
        /// How many times we have fired the timer thus far.
        /// </summary>
        private long fireCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineTimer{T}"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        public EngineTimer(T state)
        {
            this.StateData = state;
        }

        /// <summary>
        /// Gets the object that was provided to the timer when it was instanced.
        /// This object will be provided to the callback at each interval when fired.
        /// </summary>
        public T StateData { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the engine timer is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// <para>
        /// Starts the timer, firing a synchronous callback at each interval specified until `numberOfFires` has been reached.
        /// If `numberOfFires` is 0, then the callback will be called indefinitely until the timer is manually stopped.
        /// </para>
        /// <para>
        /// The following example shows how to start a timer, providing it a callback.
        /// </para>
        /// @code
        /// var timer = new EngineTimer<IPlayer>(new DefaultPlayer());
        /// double startDelay = TimeSpan.FromSeconds(30).TotalMilliseconds;
        /// double interval = TimeSpan.FromMinutes(10).TotalMilliseconds;
        /// int numberOfFires = 0;
        /// 
        /// timer.Start(
        ///     startDelay, 
        ///     interval, 
        ///     numberOfFires, 
        ///     (player, timer) => player.Save());
        /// @endcode
        /// </summary>
        /// <param name="startDelay">
        /// <para>
        /// The `startDelay` is used to specify how much time must pass before the timer can invoke the callback for the first time.
        /// If 0 is provided, then the callback will be invoked immediately upon starting the timer.
        /// </para>
        /// <para>
        /// The `startDelay` is measured in milliseconds.
        /// </para>
        /// </param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="numberOfFires">Specifies the number of times to invoke the timer callback when the interval is reached. Set to 0 for infinite.</param>
        public void Start(double startDelay, double interval, int numberOfFires, Action<T, EngineTimer<T>> callback)
        {
            this.IsRunning = true;

            this.timerTask = Task
                .Delay(TimeSpan.FromMilliseconds(startDelay), this.Token)
                .ContinueWith(
                    (task, state) => RunTimer(task, (Tuple<Action<T, EngineTimer<T>>, T>)state, interval, numberOfFires),
                    Tuple.Create(callback, this.StateData),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
        }

        /// <summary>
        /// Starts the specified start delay.
        /// </summary>
        /// <param name="startDelay">The start delay in milliseconds.</param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="numberOfFires">Specifies the number of times to invoke the timer callback when the interval is reached. Set to 0 for infinite.</param>
        public void StartAsync(double startDelay, double interval, int numberOfFires, Func<T, EngineTimer<T>, Task> callback)
        {
            this.IsRunning = true;

            this.timerTask = Task
                .Delay(TimeSpan.FromMilliseconds(startDelay), this.Token)
                .ContinueWith(
                    async (task, state) => await RunTimerAsync(task, (Tuple<Func<T, EngineTimer<T>, Task>, T>)state, interval, numberOfFires),
                    Tuple.Create(callback, this.StateData),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
        }

        /// <summary>
        /// Stops the timer for this instance.
        /// Stopping the timer will not dispose of the EngineTimer, allowing you to restart the timer if you need to.
        /// </summary>
        public void Stop()
        {
            if (!this.IsCancellationRequested)
            {
                this.Cancel();
            } 
            this.IsRunning = false;
        }

        /// <summary>
        /// Stops the timer and releases the unmanaged resources used by the <see cref="T:System.Threading.CancellationTokenSource" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.IsRunning = false;
                this.Cancel();
            }

            base.Dispose(disposing);
        }

        private async Task RunTimer(Task task, Tuple<Action<T, EngineTimer<T>>, T> state, double interval, int numberOfFires)
        {
            while (!this.IsCancellationRequested)
            {
                // Only increment if we are supposed to.
                if (numberOfFires > 0)
                {
                    this.fireCount++;
                }

                state.Item1(state.Item2, this);
                await PerformTimerCancellationCheck(interval, numberOfFires);
            }
        }

        private async Task RunTimerAsync(Task task, Tuple<Func<T, EngineTimer<T>, Task>, T> state, double interval, int numberOfFires)
        {
            while (!this.IsCancellationRequested)
            {
                // Only increment if we are supposed to.
                if (numberOfFires > 0)
                {
                    this.fireCount++;
                }

                await state.Item1(state.Item2, this);
                await PerformTimerCancellationCheck(interval, numberOfFires);
            }
        }

        private async Task PerformTimerCancellationCheck(double interval, int numberOfFires)
        {
            // If we have reached our fire count, stop. If set to 0 then we fire until manually stopped.
            if (numberOfFires > 0 && this.fireCount >= numberOfFires)
            {
                this.Stop();
            }

            await Task.Delay(TimeSpan.FromMilliseconds(interval), this.Token).ConfigureAwait(false);
        }
    }
}
