using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace server
{
	class Server
	{
		IPEndPoint ipep;
		UdpClient socket;

		private Server()
		{
			Console.WriteLine ("Starting UDP Server");
			ipep = new IPEndPoint (IPAddress.Any, 9000);
			socket = new UdpClient (ipep);
			byte[] data = new byte[1000];

			while (true) 
			{
				Console.WriteLine ("Waiting for client");
				IPEndPoint sender = new IPEndPoint (IPAddress.Any, 8000);
				data = socket.Receive (ref sender);

				string cmd = Encoding.ASCII.GetString (data, 0, data.Length);
				Console.WriteLine ("Server recieved {0} from client", cmd);

				if (cmd == "U" || cmd == "u") 
				{
					//kode
					FileStream uptime = new FileStream("/proc/uptime", FileMode.Open, FileAccess.Read);
					int bytesRead = uptime.Read (data, 0, data.Length);
					socket.Send (data, data.Length, sender);
				} 
				else if (cmd == "L" || cmd == "L") 
				{
					//kode
					FileStream loadavg = new FileStream("/proc/loadavg", FileMode.Open, FileAccess.Read);
					int bytesRead = loadavg.Read (data, 0, data.Length);
					socket.Send (data, data.Length, sender);
				}
				else 
				{
					string error = "Wrong argument";
					data = Encoding.ASCII.GetBytes (error);
					Console.WriteLine (error);
					socket.Send(data,data.Length,sender);
				}
			}
		}

		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			new Server ();
		}
	}
}
