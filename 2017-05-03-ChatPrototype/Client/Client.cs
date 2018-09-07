using System;
using System.Net.Sockets;

namespace Client {

  class Client {

    private const int PORT = 4000;

    static void Main(string[] args) {
      Console.WriteLine("Project Chat - Client");
      Console.WriteLine("---------------------");

      Console.WriteLine("Please input the server you want to connect to:");
      string serverAddress = Console.ReadLine();

      TcpClient client = new TcpClient(serverAddress, PORT);

      ClientHandler handler = new ClientHandler(client);
      handler.Start();
    }

  }

}