using System;
using System.Net;
using System.Net.Sockets;
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

  public void start(int port)
  {
    udpClient = new UdpClient(port);
    groupEP = new IPEndPoint(IPAddress.Any, 0);

    Task.Run(() =>
    {
      while(true)
      {
        var data = receive();
        fireOnReceived(data);
      }
    });
  }
  
  public void send(byte[] data)
  {
    udpClient.Send(data, data.Length, groupEP);
  }

  public void stop()
  {
    udpClient.Dispose();
  }
}