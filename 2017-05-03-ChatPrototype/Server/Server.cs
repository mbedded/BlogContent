using System;

namespace Server {

  class Server {
    private const int PORT = 4000;

    static void Main(string[] args) {
      Console.WriteLine("Project Chat - Server");
      Console.WriteLine("---------------------");

      Console.WriteLine("Port is set to " + PORT);

      ServerHandler handler = new ServerHandler(PORT);
      handler.StartServer();

      Console.ReadLine();
    }

  }

}