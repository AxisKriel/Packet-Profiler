using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Packet_Profiler
{
	[ApiVersion(1, 17)]
	public class PacketProfiler : TerrariaPlugin
	{
		public override string Author
		{
			get { return "Enerdy"; }
		}

		public override string Description
		{
			get { return "Profiles all of the incoming / outgoing packets."; }
		}

		public override string Name
		{
			get { return "Packet Profiler"; }
		}

		public override Version Version
		{
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
		}

		public PacketProfiler(Main game)
			: base(game)
		{
			Order = int.MaxValue;
		}

		public static Config Config { get; private set; }
		public static NetProfiler Profiler { get; private set; }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
				ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);
				ServerApi.Hooks.NetSendData.Deregister(this, OnSendData);
			}
		}

		public override void Initialize()
		{
			ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
			ServerApi.Hooks.NetGetData.Register(this, OnGetData);
			ServerApi.Hooks.NetSendData.Register(this, OnSendData);

			//TShockAPI.Commands.ChatCommands.Add(new Command(Commands.Status, "sendstatus"));

			//Commands.ChatCommands.Add(new Command("packet.lock", DoPacketLock, "packetlock", "plk")
			//{
			//	HelpText = "Locks the profiler to profile only selected packets."
			//});
			//Commands.ChatCommands.Add(new Command("packet.reload", DoReload, "packetreload", "prl")
			//{
			//	HelpText = "Reloads the profiler's config file."
			//});
			//Commands.ChatCommands.Add(new Command(pack
		}

		void OnInitialize(EventArgs e)
		{
			string path = Path.Combine(TShock.SavePath, "PacketProfiler.json");
			Config = Config.Read(path);
			Profiler = NetProfiler.Instance;

			TShockAPI.Commands.ChatCommands.Add(new Command("packetprofiler", Commands.Lock, "plock", "pp-lock"));
			TShockAPI.Commands.ChatCommands.Add(new Command("packetprofiler", Commands.Mode, "pp-mode"));
			TShockAPI.Commands.ChatCommands.Add(new Command("packetprofiler", Commands.Settings, "pp-settings"));
		}

		void OnGetData(GetDataEventArgs args)
		{
			switch (Profiler.Mode)
			{
				case ProfileMode.Disabled:
					return;
				case ProfileMode.Lock:
					if (Profiler.Lock.Contains(args.MsgID))
					{
						Profiler.ProfileNetGet(args);
					}
					break;
				case ProfileMode.All:
					Profiler.ProfileNetGet(args);
					break;
			}
		}

		void OnSendData(SendDataEventArgs args)
		{
			switch (Profiler.Mode)
			{
				case ProfileMode.Disabled:
					return;
				case ProfileMode.Lock:
					if (Profiler.Lock.Contains(args.MsgId))
					{
						Profiler.ProfileNetSend(args);
					}
					break;
				case ProfileMode.All:
					Profiler.ProfileNetSend(args);
					break;
			}
		}
	}
}
