using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using TShockAPI;

namespace Packet_Profiler
{
	public class NetProfiler
	{
		public static NetProfiler Instance = new NetProfiler();

		public ProfileMode Mode { get; set; }

		public List<PacketTypes> Lock { get; private set; }

		public bool ShowPacketNames
		{
			get { return PacketProfiler.Config.ShowPacketNames; }
			set { PacketProfiler.Config.SetShowPacketNames(value); }
		}

		public bool ShowWhoAmI
		{
			get { return PacketProfiler.Config.Get_ShowWhoAmI; }
			set { PacketProfiler.Config.SetShowWhoAmI(value); }
		}

		public bool ShowRemoteClient
		{
			get { return PacketProfiler.Config.Send_ShowRemoteClient; }
			set { PacketProfiler.Config.SetShowRemoteClient(value); }
		}

		public bool ShowIgnoreClient
		{
			get { return PacketProfiler.Config.Send_ShowIgnoreClient; }
			set { PacketProfiler.Config.SetShowIgnoreClient(value); }
		}

		public NetProfiler()
		{
			Mode = ProfileMode.Disabled;
			Lock = new List<PacketTypes>();
		}

		public void SetLock(params string[] args)
		{
			Lock.Clear();
			for (int i = 0; i < args.Length; i++)
			{
				int p;
				if (int.TryParse(args[i], out p))
					Lock.Add((PacketTypes)p);
			}
		}

		public void ProfileNetGet(GetDataEventArgs data)
		{
			PacketTypes packet = data.MsgID;
			string s = string.Format("GET: Packet {0}{1}{2}",
				ShowPacketNames ? packet.ToString() + " " : "",
				string.Format("[{0}]", (int)packet),
				ShowWhoAmI ? string.Format(" (Sender: {0})", data.Msg.whoAmI) : "");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(s);
			Console.ResetColor();
		}

		public void ProfileNetSend(SendDataEventArgs data)
		{
			PacketTypes packet = data.MsgId;
			var sb = new StringBuilder();
			sb.Append("SEND: Packet ");
			if (ShowPacketNames)
				sb.Append(packet.ToString()).Append(' ');
			sb.AppendFormat("[{0}]", (int)packet);
			if (ShowRemoteClient && ShowIgnoreClient) sb.AppendFormat(
				" (Sender: {0} | IgnoreClient: {1})", data.remoteClient, data.ignoreClient);
			else if (ShowRemoteClient)
				sb.AppendFormat(" (Sender: {0})", data.remoteClient);
			else if (ShowIgnoreClient)
				sb.AppendFormat(" (IgnoreClient: {0})", data.ignoreClient);
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine(sb.ToString());
			Console.ResetColor();
		}
	}
}
