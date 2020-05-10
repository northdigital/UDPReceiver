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
  public event EventHandler onTick;

  private CancellationTokenSource tokenSource = new CancellationTokenSource();

  public async Task startAsync(int port)
  {
    var udpClient = new UdpClient(port);
    var endPointAny = new IPEndPoint(IPAddress.Any, 0);

    var token = tokenSource.Token;

    await Task.Run(() =>
    {
      try
      {
        while (true)
        {
          try
          {
            onTick?.Invoke(this, EventArgs.Empty);
            if (token.IsCancellationRequested)
              token.ThrowIfCancellationRequested();

            if (udpClient.Available > 0)
            {
              var data = udpClient.Receive(ref endPointAny);
              onReceived?.Invoke(this, new UDPReceiverEventArgs(udpClient, endPointAny, data));
            }

            Thread.Sleep(1000);
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
        }
      }
      catch (OperationCanceledException) { }
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