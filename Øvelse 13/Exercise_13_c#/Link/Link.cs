using System;
using System.IO.Ports;

/// <summary>
/// Link.
/// </summary>
namespace Linklaget
{
	/// <summary>
	/// Link.
	/// </summary>
	public class Link
	{
		/// <summary>
		/// The DELIMITE for slip protocol.
		/// </summary>
		const byte DELIMITER = (byte)'A';
		/// <summary>
		/// The buffer for link.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The serial port.
		/// </summary>
		SerialPort serialPort;

		/// <summary>
		/// Initializes a new instance of the <see cref="link"/> class.
		/// </summary>
		public Link (int BUFSIZE)
		{
			// Create a new SerialPort object with default settings.
			serialPort = new SerialPort("/dev/ttyS1",115200,Parity.None,8,StopBits.One);

			if(!serialPort.IsOpen)
				serialPort.Open();

			buffer = new byte[(BUFSIZE*2)];

			//serialPort.ReadTimeout = 200;
			serialPort.DiscardInBuffer ();
			serialPort.DiscardOutBuffer ();
		}

		/// <summary>
		/// Send the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public void send (byte[] buf, int size)
		{
	    	// TO DO Your own code
			int sendBufSize = 0;
			buffer [0] = DELIMITER;
			int j = 0;
			for (int i = 0; i < size; i++) {
				
				if (buf [i] == Convert.ToByte('A')) {
					buffer [j + 1] = Convert.ToByte('B');
					buffer [j + 2] = Convert.ToByte('C');
					j += 2;
				} else if (buf [i] == Convert.ToByte('B')) {
					buffer [j + 1] = Convert.ToByte('B');
					buffer [j + 2] = Convert.ToByte('D');
					j += 2;
				} else {
					buffer [j+1] = buf [i];
					j++;
				}
				
			}
			buffer [j+1] = DELIMITER;

			serialPort.Write (buffer,0,buffer.Length);

		}

		/// <summary>
		/// Receive the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public int receive (ref byte[] buf)
		{
			Array.Clear (buffer, 0, buffer.Length);
			serialPort.Read (buf, 0, buf.Length);

			int j = 0;
	    	// TO DO Your own code
			for (int i = 1; i < buf.Length; i++) {
				if (buf [i] == Convert.ToByte('B')) {
					if (buf [i + 1] == Convert.ToByte('C')) {
						buffer [j] = Convert.ToByte ('A');
						i++;
					} else if (buf [i + 1] == Convert.ToByte('D')) {
						buffer [j] = Convert.ToByte ('B');
						i++;
					}


				} else if (buf [i] == Convert.ToByte('A'))
					break;
				else
					buffer [j] = buf [i];
				j++;


			}
			//buffer [j] = Convert.ToByte('A');
			//transport.send (buffer);
			return buffer.Length;
		}
	}
}
