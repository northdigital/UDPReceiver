using System;
using System.Text;
using System.Threading.Tasks;

class Program
{
  const int PORT = 7777;

  static async Task Main(string[] args)
  {
    var udpReceiver = new UDPReceiver();

    udpReceiver.onTick += (s, e) =>
    {
      Console.WriteLine(".");
    };

    udpReceiver.onReceived += (s, e) =>
    {
      var message = Encoding.UTF8.GetString(e.data);
      Console.WriteLine($"{e.ipEndPoint.Address} {message}");            
      if(message == "stop")
        udpReceiver.stop();
      var response = Encoding.UTF8.GetBytes($"{DateTime.Now}");  
      e.udpClient.Send(response, response.Length, e.ipEndPoint);     
    };
    
    Console.WriteLine("start listening...");   
    await udpReceiver.startAsync(PORT);
    Console.WriteLine("good bye!");
  }
}
