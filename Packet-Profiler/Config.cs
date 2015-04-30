using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TShockAPI;

namespace Packet_Profiler
{
	public class Config
	{
		private string path;

		public bool ShowPacketNames { get; private set; }

		public bool Get_ShowWhoAmI { get; private set; }

		public bool Send_ShowRemoteClient { get; private set; }

		public bool Send_ShowIgnoreClient { get; private set; }

		public Config()
		{
			ShowPacketNames = true;
			Get_ShowWhoAmI = true;
			Send_ShowRemoteClient = true;
			Send_ShowIgnoreClient = true;
		}

		public void SetShowPacketNames(bool value)
		{
			ShowPacketNames = value;
			Write(path);
		}

		public void SetShowWhoAmI(bool value)
		{
			Get_ShowWhoAmI = value;
			Write(path);
		}

		public void SetShowRemoteClient(bool value)
		{
			Send_ShowRemoteClient = value;
			Write(path);
		}

		public void SetShowIgnoreClient(bool value)
		{
			Send_ShowIgnoreClient = value;
			Write(path);
		}

		public void Write(string path)
		{
			try
			{
				File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
			}
			catch (Exception ex)
			{
				Log.ConsoleError(ex.ToString());
			}
		}

		public static Config Read(string path)
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				Config config = new Config();
				if (!File.Exists(path))
					File.WriteAllText(path, JsonConvert.SerializeObject(config, Formatting.Indented));
				else
					config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
				config.path = path;
				return config;
			}
			catch (Exception)
			{
				return new Config();
			}
		}
	}
}
