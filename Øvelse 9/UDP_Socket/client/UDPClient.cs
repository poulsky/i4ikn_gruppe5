using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
	public class UDPClient
	{
		private const int Port=9000;

		public static int Main()
		{
			bool done = true;
			UdpClient listener = new UdpClient (Port);
			IPEndPoint groupEP = new IPEndPoint (IPAddress.Parse("10.0.0.1"), Port);
			string receivedData;
			byte[] receiveByteArray;
			byte[] request = new byte[10];
			string command;
		




		

			try
			{
				while(done)
				{
					Console.WriteLine("Enter command 'U' or 'L'");
					command = Console.ReadLine();
					request = Encoding.ASCII.GetBytes(command);
					listener.Send (request, request.Length, groupEP);
					done = false;


					while(!done)
					{
						Console.WriteLine("Waiting for broadcast");

						receiveByteArray = listener.Receive(ref groupEP);
						Console.WriteLine("Received a broadcast from {0}", groupEP.ToString() );
						receivedData = Encoding.ASCII.GetString(receiveByteArray, 0, receiveByteArray.Length);
						Console.WriteLine("Data received: \n{0}\n\n", receivedData);
						done = true;
					}
				}
			}
			catch(Exception e) {
				Console.WriteLine(e.ToString());
			}
			listener.Close ();
			return 0;
		}

		
	}
}

