using System;
using System.IO;
using System.Text;
using Transportlaget;
using Library;

namespace Application
{
	class file_server
	{
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		private const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// </summary>
		private file_server ()
		{
			/*
			// TO DO Your own code
			string args = "hest";
			Transport t = new Transport(args.Length);
			byte[] file = new byte[args.Length*sizeof(char)];
			System.Buffer.BlockCopy (args.ToCharArray (), 0, file,0, file.Length);
			//for (int i = 0; i > args.Length; i++) {
			//	file [i] = Convert.ToByte(args [i]);
			//}

			t.send (file, file.Length);
			*/
			Console.WriteLine ("Server started!");

			byte[] buffer = new byte[BUFSIZE];
			Transport transport = new Transport (BUFSIZE);
			while (true) 
			{
				var size = transport.receive(ref buffer);
				if (size != 0) 
				{
					var fileNamePath = System.Text.Encoding.Default.GetString (buffer);
					var fileName = LIB.extractFileName (fileNamePath);
					var fileSize = LIB.check_File_Exists (fileName);

					var mySize = fileSize.ToString ();
					byte[] sendfileSize = new byte[mySize.Length];
					sendfileSize = Encoding.ASCII.GetBytes (mySize);

					transport.send (sendfileSize, sendfileSize.Length);

					if (fileSize != 0)
						sendFile (fileName, fileSize, transport);
					else
						Console.WriteLine ("File does not exist.");
				}
			}

		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='fileSize'>
		/// File size.
		/// </param>
		/// <param name='tl'>
		/// Tl.
		/// </param>
		private void sendFile(String fileName, long fileSize, Transport transport)
		{
			// TO DO Your own code
			int totalSize = (int)fileSize;
			int totalAmountSend = 0;

			byte[] myBytes = new byte[BUFSIZE];
			FileStream Fs = new FileStream (fileName, FileMode.Open, FileAccess.Read);

			while (totalAmountSend < totalSize) 
			{
				int bytesRead = Fs.Read (myBytes, 0, BUFSIZE);

				transport.send (myBytes, bytesRead);
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
			new file_server();
		}
	}
}