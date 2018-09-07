using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Client {

  class ClientHandler {

    private readonly TcpClient _client;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;

    private volatile bool _isConnected = true;

    public ClientHandler(TcpClient xClient) {
      _client = xClient;

      var stream = _client.GetStream();
      _reader = new StreamReader(stream);
      _writer = new StreamWriter(stream);
    }

    public void Start() {
      Thread receiveThread = new Thread(ReceiveMessages);
      receiveThread.Start();

      while (_isConnected) {
        string input = Console.ReadLine();

        _writer.WriteLine(input);
        _writer.Flush();
      }
    }

    private async void ReceiveMessages() {
      while (_isConnected) {
        string result = await _reader.ReadLineAsync();
        Console.WriteLine(result);

        if (result.StartsWith("Logoff")) {
          _isConnected = false;
          Console.WriteLine("Press any key to quit!");
        }
      }
    }

  }

}