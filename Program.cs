using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
  static async Task Main(string[] args)
  {
    var udpReceiver = new UDPReceiver();

    udpReceiver.onReceived += (sender, e) =>
    {
      var receivedMessage = Encoding.UTF8.GetString(e.data);
      Console.WriteLine(receivedMessage);            
      if(receivedMessage == "stop")
        udpReceiver.stop();
      var response = Encoding.UTF8.GetBytes($"from receiver: {DateTime.Now}");  
      e.udpClient.Send(response, response.Length, e.ipEndPoint);     
    };
    
    Console.WriteLine("start listening...");   
    await udpReceiver.startAsync(7777);
  }
}
