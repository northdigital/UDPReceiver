using System;
using System.Text;

class Program
{
  static void Main(string[] args)
  {
    var udpReceiver = new UDPReceiver(7777);

    while(true)
    {
      var receivedMessage = Encoding.UTF8.GetString(udpReceiver.receive());
      Console.WriteLine(receivedMessage);
            
      udpReceiver.send(Encoding.UTF8.GetBytes($"from receiver: {DateTime.Now}"));
    }
  }
}
