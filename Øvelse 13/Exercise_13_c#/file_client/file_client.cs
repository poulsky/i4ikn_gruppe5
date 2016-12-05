using System;
using System.IO;
using System.Text;
using Transportlaget;
using Library;
using Linklaget;

namespace Application
{
	class file_client
	{
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;


		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// 
		/// file_client metoden opretter en peer-to-peer forbindelse
		/// Sender en forspÃ¸rgsel for en bestemt fil om denne findes pÃ¥ serveren
		/// Modtager filen hvis denne findes eller en besked om at den ikke findes (jvf. protokol beskrivelse)
		/// Lukker alle streams og den modtagede fil
		/// Udskriver en fejl-meddelelse hvis ikke antal argumenter er rigtige
		/// </summary>
		/// <param name='args'>
		/// Filnavn med evtuelle sti.
		/// </param>
	    private file_client(String[] args)
	    {
	    	// TO DO Your own code
			/*
			Transport t = new Transport (args.Length);
			byte[] file = new byte[args.Length * sizeof(char)];
			System.Buffer.BlockCopy (args.ToCharArray (), 0, file, 0, file.Length);
			//for (int i = 0; i > args.Length; i++) {
			//	file [i] = Convert.ToByte(args [i]);
			//}


			t.send (file, file.Length);

			var receivebuf = new byte[1000];
			string output;
			var t = new Transport (1000);
			while (true) {
				var n = t.receive (ref receivebuf);
				if (n != 0) {
					output = System.Text.Encoding.Default.GetString (receivebuf);
					Console.WriteLine (output);
				}
			}*/

			Console.WriteLine ("trying to connect...");

			//etabler tcp forbindelsen
			Transport TransportClient = new Transport (BUFSIZE);


			//sæt streameren til at snakke på den nu åbne tcp connection

			byte[] requestByte = new byte[BUFSIZE];
			requestByte = Encoding.ASCII.GetBytes (args [0]);

			//Skriv til server at vi ønsker den og den fil.
			TransportClient.send (requestByte, requestByte.Length);
			//string ModtagetStatus = LIB.readTextTCP (fileStream);
			receiveFile (args [1], TransportClient);
			//reager på det der kommer tilbage, hvis det ikke er null. 


			// luk forbindelsen.


	    }

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='transport'>
		/// Transportlaget
		/// </param>
		private void receiveFile (String fileName, Transport transport)
		{
			// TO DO Your own code
			//int fileSize = (int)LIB.getFileSizeTCP (io);
			byte[] ReceiveSize = new byte[BUFSIZE];
			var FileSize = transport.receive (ref ReceiveSize);
			var stringSize = ReceiveSize.ToString();
			var fileSize = int.Parse(stringSize);

			//File.WriteAllText(fileName,LIB.readTextTCP(io);
			if(fileSize > 0)
			{
				Byte[] receivingBuffer = new byte[BUFSIZE];
				Console.WriteLine (fileSize);
				FileStream DataWeWant = new FileStream(fileName,FileMode.Create,FileAccess.Write);


				int totalNumberOfReceivedBytes = 0;

				// læs mens der stadig er ting tilbage
				while (totalNumberOfReceivedBytes < fileSize)
				{
					int bytesRead = transport.receive(ref receivingBuffer);

					DataWeWant.Write(receivingBuffer, 0, bytesRead);
					totalNumberOfReceivedBytes += bytesRead;

					Array.Clear (receivingBuffer, 0, BUFSIZE);
				}

				DataWeWant.Close ();


				Console.WriteLine("File Received: {0} Bytes", fileSize);
			}
			else 
			{
				Console.WriteLine("No file found");
			}
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// First argument: Filname
		/// </param>
		public static void Main (string[] args)
		{
			
			new file_client(args);


		}
	}
}