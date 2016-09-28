using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;



namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;




		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{
			Console.WriteLine ("trying to connect...");

			//etabler tcp forbindelsen
			TcpClient NyTcpSocket = new TcpClient ();
			NyTcpSocket.Connect (args [0], PORT);

			//sæt streameren til at snakke på den nu åbne tcp connection
			NetworkStream fileStream = NyTcpSocket.GetStream ();

			//Skriv til server at vi ønsker den og den fil.
			LIB.writeTextTCP (fileStream, args [1]);
			//string ModtagetStatus = LIB.readTextTCP (fileStream);

			//reager på det der kommer tilbage, hvis det ikke er null. 
			if(LIB.getFileSizeTCP(fileStream) != null)
			{
				receiveFile (args [1], fileStream);
				Console.WriteLine("File Received: ");
			}
			else 
			{
				Console.WriteLine("No file found");
			}

			// luk forbindelsen.
			fileStream.Close();
			NyTcpSocket.Close();



			// TO DO Your own code
		}

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String fileName, NetworkStream io)
		{
			//File.WriteAllText(fileName,LIB.readTextTCP(io);

			int fileSize = (int)LIB.getFileSizeTCP (io);
			Byte[] receivingBuffer = new byte[BUFSIZE];
			Console.WriteLine (fileSize);
			



			FileStream DataWeWant = new FileStream(fileName,FileMode.Create,FileAccess.Write);
			

			int totalNumberOfReceivedBytes = 0;

				// læs mens der stadig er ting tilbage
			while (totalNumberOfReceivedBytes < fileSize)
				{
					int bytesRead = io.Read(receivingBuffer, 0, receivingBuffer.Length);

					DataWeWant.Write(receivingBuffer, 0, bytesRead);
					totalNumberOfReceivedBytes += bytesRead;

					Array.Clear (receivingBuffer, 0, BUFSIZE);
				}
			
			DataWeWant.Close ();
			io.Close ();

		}



		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
