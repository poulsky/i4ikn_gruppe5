using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_server
	{

		/// <summary>
		/// The PORT
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
 		/// </summary>
		private file_server ()
		{
			
			TcpListener welcomeScoket = new TcpListener(PORT);
			while (true) 
			{
				welcomeScoket.Start ();	
				Console.WriteLine ("Server started");
				TcpClient clientSocket = default(TcpClient);
				clientSocket = welcomeScoket.AcceptTcpClient ();

				//Modtager besked fra client
				NetworkStream inFromClient = clientSocket.GetStream ();


				//Besked læses, og data lægges i bytesFrom
				string fileName = tcp.LIB.readTextTCP (inFromClient);

				Console.WriteLine ("Client request: " + fileName);

				long fileSize = tcp.LIB.check_File_Exists (fileName);

				string mySize = fileSize.ToString ();

				tcp.LIB.writeTextTCP (inFromClient, mySize);

				if (fileSize != 0)
					sendFile (fileName, fileSize, inFromClient);
				else
					Console.WriteLine ("File doesn't exist");

				clientSocket.Close ();
				welcomeScoket.Stop ();

			}
		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{
			int totalSize = (int)fileSize;
			int totalAmountSend = 0;

			byte[] myBytes = new byte[BUFSIZE];
			FileStream Fs = new FileStream (fileName, FileMode.Open, FileAccess.Read);

			while (totalAmountSend < totalSize) 
			{
				int bytesRead = Fs.Read (myBytes, 0, BUFSIZE);

				io.Write (myBytes, 0, bytesRead);
				totalAmountSend += bytesRead;

				Console.WriteLine (bytesRead);
			}

			Fs.Close();

		
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
			new file_server();

		}
	}
}
