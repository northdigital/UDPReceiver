using System.Net;
using System.Net.Sockets;

public class UDPReceiver
{
  private UdpClient udpClient;
  IPEndPoint groupEP;

  public UDPReceiver(int port)
  {
    init(port);
  }

  private void init(int port)
  {
    udpClient = new UdpClient(port);
    groupEP = new IPEndPoint(IPAddress.Any, 0);
  }
  
  public byte[] receive()
  {
    return udpClient.Receive(ref groupEP);
  }

  public void send(byte[] data)
  {
    udpClient.Send(data, data.Length, groupEP);
  }

  public void close()
  {
    udpClient.Dispose();
  }
}