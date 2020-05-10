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

    udpReceiver.onReceived += async (s, e) =>
    {
      try
      {
        var message = Encoding.UTF8.GetString(e.data);
        Console.WriteLine($"{e.ipEndPoint.Address} {message}");
        if (message == "ping")
        {
          var response = Encoding.UTF8.GetBytes("pong");
          e.udpClient.Send(response, response.Length, e.ipEndPoint);
        }
        else if (message == "stop")
        {
          udpReceiver.stop();                  
        }
        else
        {
          var response = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
          e.udpClient.Send(response, response.Length, e.ipEndPoint);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);        
      }
    };

    Console.WriteLine("start listening...");
    await udpReceiver.startAsync(PORT);
    Console.WriteLine("good bye!");
  }
}
