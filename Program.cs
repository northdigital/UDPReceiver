using System;
using System.Text;
using System.Threading;

class Program
{
  static void Main(string[] args)
  {
    var udpReceiver = new UDPReceiver();
    udpReceiver.onReceived += (sender, data) =>
    {
      var receivedMessage = Encoding.UTF8.GetString(data);
      Console.WriteLine(receivedMessage);            
      if(receivedMessage == "stop")
        udpReceiver.stop();
      udpReceiver.send(Encoding.UTF8.GetBytes($"from receiver: {DateTime.Now}"));     
    };
    
    udpReceiver.start(7777);
    Console.WriteLine("start listening...");   
    Console.ReadLine();
  }
}
