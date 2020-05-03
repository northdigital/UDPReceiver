using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class UDPReceiver
{
  private UdpClient udpClient;
  private IPEndPoint groupEP;

  private byte[] receive()
  {
    return udpClient.Receive(ref groupEP);    
  }

  public event EventHandler<byte[]> onReceived;

  private void fireOnReceived(byte[] data)
  {
    onReceived?.Invoke(this, data);
  }
  
  private CancellationTokenSource tokenSource = new CancellationTokenSource();

  public void start(int port)
  {
    udpClient = new UdpClient(port);
    groupEP = new IPEndPoint(IPAddress.Any, 0);
    
    var token = tokenSource.Token;

    Task.Run(() =>
    {
      while(true)
      {
        Console.WriteLine(".");
        if (token.IsCancellationRequested)
          token.ThrowIfCancellationRequested();
       
        if(udpClient.Available > 0)
        {
          var data = receive();
          fireOnReceived(data);
        }

        Thread.Sleep(1000);
      }
    }, token);
  }
  
  public void send(byte[] data)
  {
    udpClient.Send(data, data.Length, groupEP);
  }

  public void stop()
  {
    tokenSource.Cancel();
    // udpClient.Dispose();
  }
}