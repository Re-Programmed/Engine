using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.DiscordRPC
{
    static class RPCManager
    {
		static DiscordRpcClient client;

		/// <summary>
		/// Run on initilization to start the RPC.
		/// </summary>
		public static void Initialize()
		{
			client = new DiscordRpcClient("995784691179331584");

			client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

			client.OnReady += (sender, e) =>
			{
				Console.WriteLine("Received Ready from user {0}", e.User.Username);
			};

			client.OnPresenceUpdate += (sender, e) =>
			{
				Console.WriteLine("Received Update! {0}", e.Presence);
			};

			client.Initialize();

			UpdateState(new RichPresence()
			{
				Timestamps = new Timestamps(DateTime.UtcNow),
				Details = "Testing.",
				State = "Test.",
				Assets = new Assets()
				{
					LargeImageKey = "testing",
					LargeImageText = "Test.",
					SmallImageKey = "testing"
				}
			});
		}

		/// <summary>
		/// Updates the status displayed on discord.
		/// </summary>
		/// <param name="presence">Information to show.</param>
		public static void UpdateState(RichPresence presence)
        {
			client.SetPresence(presence);
		}

		/// <summary>
		/// Run on window close to release data. FAILURE TO RUN THIS MAY CAUSE A MEMORY LEAK.
		/// </summary>
		public static void Deinitialize()
		{
			client.Dispose();
		}
	}
}
