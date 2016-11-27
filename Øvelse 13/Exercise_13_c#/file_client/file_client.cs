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
	    private file_client(String args)
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
			*/
			var receivebuf = new byte[1000];
			string output;
			var t = new Transport (1000);
			while (true) {
				var n = t.receive (ref receivebuf);
				if (n != 0) {
					output = System.Text.Encoding.Default.GetString (receivebuf);
					Console.WriteLine (output);
				}
			}

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
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// First argument: Filname
		/// </param>
		public static void Main (string[] args)
		{
			var kf = "hest";
			new file_client(kf);


		}
	}
}