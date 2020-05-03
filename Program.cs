using System;
using System.Text;

class Program
{
  static void Main(string[] args)
  {
    var udpReceiver = new UDPReceiver();
    udpReceiver.onReceived += (sender, data) =>
    {
      var receivedMessage = Encoding.UTF8.GetString(data);
      Console.WriteLine(receivedMessage);
      udpReceiver.send(Encoding.UTF8.GetBytes($"from receiver: {DateTime.Now}"));
    };
    udpReceiver.start(7777);

    Console.ReadLine();
  }
}
