using System;
using System.Text;
using Linklaget;
using System.Text;

/// <summary>
/// Transport.
/// </summary>
namespace Transportlaget
{
	/// <summary>
	/// Transport.
	/// </summary>
	public class Transport
	{
		/// <summary>
		/// The link.
		/// </summary>
		private Link link;
		/// <summary>
		/// The 1' complements checksum.
		/// </summary>
		private Checksum checksum;
		/// <summary>
		/// The buffer.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The seq no.
		/// </summary>
		private byte seqNo;
		/// <summary>
		/// The old_seq no.
		/// </summary>
		private byte old_seqNo;
		/// <summary>
		/// The error count.
		/// </summary>
		private int errorCount;
		/// <summary>
		/// The DEFAULT_SEQNO.
		/// </summary>
		private const int DEFAULT_SEQNO = 2;

		/// <summary>
		/// Initializes a new instance of the <see cref="Transport"/> class.
		/// </summary>
		public Transport (int BUFSIZE)
		{
			link = new Link(BUFSIZE+(int)TransSize.ACKSIZE);
			checksum = new Checksum();
			buffer = new byte[BUFSIZE+(int)TransSize.ACKSIZE];
			seqNo = 0;
			old_seqNo = DEFAULT_SEQNO;
			errorCount = 0;
		}

		/// <summary>
		/// Receives the ack.
		/// </summary>
		/// <returns>
		/// The ack.
		/// </returns>
		private bool receiveAck()
		{
			byte[] buf = new byte[(int)TransSize.ACKSIZE];
			int size = link.receive(ref buf);
			if (size != (int)TransSize.ACKSIZE) return false;
			if(!checksum.checkChecksum(buf, (int)TransSize.ACKSIZE) ||
					buf[(int)TransCHKSUM.SEQNO] != seqNo ||
					buf[(int)TransCHKSUM.TYPE] != (int)TransType.ACK)
				return false;
			
			seqNo = (byte)((buf[(int)TransCHKSUM.SEQNO] + 1) % 2);


			return true;
		}

		/// <summary>
		/// Sends the ack.
		/// </summary>
		/// <param name='ackType'>
		/// Ack type.
		/// </param>
		private void sendAck (bool ackType)
		{
			byte[] ackBuf = new byte[(int)TransSize.ACKSIZE];
			ackBuf [(int)TransCHKSUM.SEQNO] = (byte)
					(ackType ? (byte)buffer [(int)TransCHKSUM.SEQNO] : (byte)(buffer [(int)TransCHKSUM.SEQNO] + 1) % 2);
			ackBuf [(int)TransCHKSUM.TYPE] = (byte)(int)TransType.ACK;
			checksum.calcChecksum (ref ackBuf, (int)TransSize.ACKSIZE);

			/*if (errorCount == 4) {
				ackBuf [0]++;
			}
			errorCount++;*/


			link.send(ackBuf, (int)TransSize.ACKSIZE);
		}

		/// <summary>
		/// Send the specified buffer and size.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='size'> 
		/// Size.
		/// </param>
		public void send(byte[] buf, int size)
		{
			if (buffer != null)
				Array.Clear (buffer, 0, buffer.Length);
<<<<<<< HEAD

=======
>>>>>>> a7cc1fa92c65bb4921730215646917d258cab5a7
			do
			{
			Array.Copy (buf, 0, buffer, 4, size);
			buffer [2] = seqNo;
			buffer [3] = (byte)0;
			checksum.calcChecksum (ref buffer, buffer.Length);

			var line = Encoding.ASCII.GetString (buffer);
			Console.WriteLine (line);
				/*if(errorCount == 3)
				{
					for(int k = 5; k<100;k++)	
						buffer[k] = (byte)'A';
				}*/

<<<<<<< HEAD
			Console.WriteLine (line);
				link.send (buffer, size+4);
=======
			
			link.send (buffer, buffer.Length);
>>>>>>> a7cc1fa92c65bb4921730215646917d258cab5a7

				errorCount++;

			
			}while(!receiveAck());

			old_seqNo = DEFAULT_SEQNO;
		}

		/// <summary>
		/// Receive the specified buffer.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		public int receive (ref byte[] buf)
		{
			if (buffer != null)
				Array.Clear (buffer, 0, buffer.Length);
<<<<<<< HEAD

=======
			
>>>>>>> a7cc1fa92c65bb4921730215646917d258cab5a7
			int n = 0;
			// TO DO Your own code
			bool ack;

			do {
				
					
				 	n = link.receive (ref buffer);
				ack = checksum.checkChecksum (buffer, buffer.Length);
				sendAck (ack);

			} while(!ack);

<<<<<<< HEAD
			if (!(old_seqNo == buffer [2])) 
			{
				Array.Copy (buffer, 4, buf, 0, n-4);
=======
			if (old_seqNo != buffer [2]) {
				Array.Copy (buffer, 4, buf, 0, buf.Length);
>>>>>>> a7cc1fa92c65bb4921730215646917d258cab5a7
				old_seqNo = buffer [2];
			}


			return n-(int)TransSize.ACKSIZE;
		}
	}
}