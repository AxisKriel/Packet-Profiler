using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Profiler.Extensions
{
	public static class EnumExtensions
	{
		public static string Name(this PacketTypes value)
		{
			return value.ToString();
		}
	}
}
