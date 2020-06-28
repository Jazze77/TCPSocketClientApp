using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPSocketClientApp
{
    class Program
    {
        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public DateTime TimeStamp { get; set; }
        }
        public static void StartClient()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    //luo oli
                    Person dummy = new Person();                  
                    dummy.FirstName = "Joulu";
                    dummy.LastName = "Pukki";
                    dummy.TimeStamp = DateTime.Now;

                    Person dummy2 = new Person();
                    dummy2.FirstName = "Joulu2";
                    dummy2.LastName = "Pukki2";
                    dummy2.TimeStamp = DateTime.Now;

                    List<Person> jsonlist = new List<Person>();
                    jsonlist.Add(dummy);
                    jsonlist.Add(dummy2);

                    string jsonmessage = JsonConvert.SerializeObject(jsonlist);


                    // Encode the data string into a byte array.                     
                    byte[] msg = Encoding.Default.GetBytes(jsonmessage + "<EOF>");

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);

                    string jsonreceived = Encoding.Default.GetString(bytes, 0, bytesRec);

                    List <Person> reslist = JsonConvert.DeserializeObject<List<Person>>(jsonreceived);

                    foreach(Person iter in reslist)
                    {
                        Console.WriteLine(iter.FirstName + " " + iter.LastName);// + " " + iter.TimeStamp);
                    }

                   // Console.WriteLine("Echoed test = {0}",
                     //   Encoding.Default.GetString(bytes, 0, bytesRec));

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static int Main(String[] args)
        {
            StartClient();
            Console.ReadLine();
            return 0;
        }
    }

    
    
}
