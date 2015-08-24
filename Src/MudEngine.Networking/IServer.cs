//-----------------------------------------------------------------------
// <copyright file="IServer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Networking
{
    /// <summary>
    /// Provides members that can be used to interact with a server
    /// </summary>
    public interface IServer : IAdapter
    {
        /// <summary>
        /// Gets or sets the owner of the server.
        /// </summary>
        string Owner { get; set; }

        /// <summary>
        /// Gets the port that the server is running on.
        /// </summary>
        int RunningPort { get; }

        /// <summary>
        /// Gets the status of the server.
        /// </summary>
        ServerStatus Status { get; }

        /// <summary>
        /// Configures the server using a given configuration.
        /// </summary>
        /// <param name="configuration">The server configuration used to setup the server.</param>
        void Configure(IServerConfiguration configuration);
    }
}