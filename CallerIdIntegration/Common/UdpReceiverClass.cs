using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CallerIdIntegration.Common
{

    // -----------------------UDP Receiver Class----------------------------------------
    // This is absolutely vital in order to receive UPD packets into an application 
    // If you are not familar with these functions, you may want to consider simply pasting into your code.

    public class UdpReceiverClass
    {
        // Declare variables
        public static Boolean Done;
        public static string ReceivedMessage;
        public static byte[] ReceviedMessageByte;
        public static int NListenPort = 3520;

        // Define event
        public delegate void UdpEventHandler(UdpReceiverClass u, EventArgs e);
        public event UdpEventHandler DataReceived;

        // Idle listening
        public void UdpIdleReceive()
        {

            // Set done to false so loop will begin
            Done = false;

            // Setup filter for too small messages
            const int filterMessageSmallerThan = 4;

            // Setup socket for listening
            UdpClient listener = new UdpClient(NListenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, NListenPort);

            // Wait for broadcast
            try
            {
                while (!Done)
                {
                    // Receive broadcast
                    ReceviedMessageByte = listener.Receive(ref groupEP);
                    ReceivedMessage = Encoding.UTF7.GetString(ReceviedMessageByte, 0, ReceviedMessageByte.Length);

                    // Filter smaller messages););
                    if (ReceviedMessageByte.Length > filterMessageSmallerThan)
                    {
                        DataReceived(this, EventArgs.Empty);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            listener.Close();
        }

    }
}
