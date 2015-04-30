using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Packet_Profiler
{
	public static class Commands
	{
		private static NetProfiler Profiler = PacketProfiler.Profiler;

		public static void Lock(CommandArgs args)
		{
			Profiler.SetLock(args.Parameters.ToArray());
			if (args.Parameters.Count < 1)
				args.Player.SendInfoMessage("Removed the packet lock.");
			else
			{
				args.Player.SendInfoMessage("Locked on packets:");
				for (int i = 0; i < Profiler.Lock.Count; i++)
					args.Player.SendInfoMessage(" - Packet {0} @ {1}", (int)Profiler.Lock[i], Profiler.Lock[i].ToString());
			}
		}

		public static void Mode(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}pp-mode <all|lock|off>");
			else
			{
				string mode = args.Parameters[0].ToLowerInvariant();
				switch (mode)
				{
					case "all":
						Profiler.Mode = ProfileMode.All;
						args.Player.SendInfoMessage("Enabled [all] profiling.");
						return;
					case "lock":
						Profiler.Mode = ProfileMode.Lock;
						args.Player.SendInfoMessage("Enabled [lock] profiling.");
						return;
					case "off":
					case "disable":
						Profiler.Mode = ProfileMode.Disabled;
						args.Player.SendInfoMessage("Disabled profiling.");
						return;
					default:
						args.Player.SendErrorMessage("Invalid mode!");
						return;
				}
			}
		}

		public static void Settings(CommandArgs args)
		{
			var regex = new Regex(@"^\S+ (.+?)(?: (true|false))?$");
			Match match = regex.Match(args.Message);
			if (!match.Success)
			{
				args.Player.SendInfoMessage("Syntax: {0}pp-settings <setting> [true|false]", TShock.Config.CommandSpecifier);
				args.Player.SendInfoMessage("Available settings: showpacketnames, showwhoami, showremoteclient, showignoreclient");
			}
			else
			{
				string setting = match.Groups[1].Value.ToLowerInvariant();
				bool toggle = string.IsNullOrWhiteSpace(match.Groups[2].Value);
				switch (setting)
				{
					case "showpacketnames":
						if (toggle)
							Profiler.ShowPacketNames = !Profiler.ShowPacketNames;
						else if (!bool.TryParse(match.Groups[2].Value, out toggle))
						{
							args.Player.SendErrorMessage("Invalid value!");
							return;
						}
						else
							Profiler.ShowPacketNames = toggle;

						args.Player.SendInfoMessage("{0}abled ShowPacketNames!", Profiler.ShowPacketNames ? "En" : "Dis");
						return;
					case "showwhoami":
						if (toggle)
							Profiler.ShowWhoAmI = !Profiler.ShowWhoAmI;
						else if (!bool.TryParse(match.Groups[2].Value, out toggle))
						{
							args.Player.SendErrorMessage("Invalid value!");
							return;
						}
						else
							Profiler.ShowWhoAmI = toggle;

						args.Player.SendInfoMessage("{0}abled ShowWhoAmI!", Profiler.ShowWhoAmI ? "En" : "Dis");
						return;
					case "showremoteclient":
						if (toggle)
							Profiler.ShowRemoteClient = !Profiler.ShowRemoteClient;
						else if (!bool.TryParse(match.Groups[2].Value, out toggle))
						{
							args.Player.SendErrorMessage("Invalid value!");
							return;
						}
						else
							Profiler.ShowRemoteClient = toggle;

						args.Player.SendInfoMessage("{0}abled ShowRemoteClient!", Profiler.ShowRemoteClient ? "En" : "Dis");
						return;
					case "showignoreclient":
						if (toggle)
							Profiler.ShowIgnoreClient = !Profiler.ShowIgnoreClient;
						else if (!bool.TryParse(match.Groups[2].Value, out toggle))
						{
							args.Player.SendErrorMessage("Invalid value!");
							return;
						}
						else
							Profiler.ShowIgnoreClient = toggle;

						args.Player.SendInfoMessage("{0}abled ShowIgnoreClient!", Profiler.ShowIgnoreClient ? "En" : "Dis");
						return;
					default:
						args.Player.SendErrorMessage("Invalid setting!");
						return;
				}
			}
		}
	}
}
