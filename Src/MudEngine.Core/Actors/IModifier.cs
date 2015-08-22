namespace MudDesigner.MudEngine.Actors
{
    /// <summary>
    /// Provides properties that define how a modifier is to be adjust a stat.
    /// </summary>
    public interface IModifier : IGameComponent
    {
        /// <summary>
        /// Gets the target that this modifier is intended for.
        /// </summary>
        IActor Target { get; }

        // TODO: Change to IUseable when the interface is created
        /// <summary>
        /// Gets the source that caused this modifier to be applied.
        /// </summary>
        IActor Source { get; }

        /// <summary>
        /// Gets how much of the stat this modifier will change.
        /// </summary>
        int Amount { get; }

        /// <summary>
        /// Gets the duration in milliseconds.
        /// </summary>
        int Duration { get; }
    }
}
