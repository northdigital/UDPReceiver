using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class UDPReceiverEventArgs : EventArgs
{
  public UdpClient udpClient { get; set; }
  public IPEndPoint ipEndPoint { get; set; }
  public byte[] data { get; set; }

  public UDPReceiverEventArgs(UdpClient udpClient, IPEndPoint ipEndPoint, byte[] data)
  {
    this.udpClient = udpClient;
    this.ipEndPoint = ipEndPoint;
    this.data = data;
  }
}

public class UDPReceiver
{
  public event EventHandler<UDPReceiverEventArgs> onReceived;

  private CancellationTokenSource tokenSource = new CancellationTokenSource();

  public async Task startAsync(int port)
  {
    var udpClient = new UdpClient(port);
    var ipEndPointAny = new IPEndPoint(IPAddress.Any, 0);

    var token = tokenSource.Token;

    await Task.Run(() =>
    {
      try
      {
        while (true)
        {
          Console.WriteLine(".");
          if (token.IsCancellationRequested)
            token.ThrowIfCancellationRequested();

          if (udpClient.Available > 0)
          {
            var data = udpClient.Receive(ref ipEndPointAny);
            onReceived?.Invoke(this, new UDPReceiverEventArgs(udpClient, ipEndPointAny, data));
          }

          Thread.Sleep(1000);
        }
      }
      catch(OperationCanceledException)
      {
        Console.WriteLine("good bye!");
      }
      finally
      {
        tokenSource.Dispose();
        udpClient.Dispose();
      }
    }, token);
  }

  public void stop()
  {
    tokenSource.Cancel();
  }
}