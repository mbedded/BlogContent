using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server {

  class AcceptedClient {
    private readonly ServerHandler _server;
    private readonly TcpClient _client;

    // Read and write messages
    private readonly StreamWriter _writer;
    private readonly StreamReader _reader;

    private volatile bool _isListening = true;

    // Current state of the Client
    public string Name { get; set; }

    public bool IsLoggedIn { get; set; }
    public Thread CurrentThread { get; set; }


    public AcceptedClient(TcpClient xClient, ServerHandler xServer) {
      _client = xClient;
      _server = xServer;

      var stream = _client.GetStream();
      _reader = new StreamReader(stream);
      _writer = new StreamWriter(stream);
    }

    public async void WaitForRequest() {
      while (_isListening) {
        string message = "";
        try {
          message = await _reader.ReadLineAsync();
        } catch (Exception ex) {
          _server.LogMessage($"Exception at {Name}-waitForRequest(). {ex.Message}");
          _isListening = false;
          _server.RemoveClient(Name);
        }

        _server.LogMessage($"{Name} received: {message}");

        if (IsLoggedIn == false && message.StartsWith("/login") == false && message.StartsWith("/help") == false) {
          message = CommandHandler.GetErrorNotLoggedIn();
          WriteMessage(message);
          continue;
        }

        if (message.StartsWith("/")) {
          _server.HandleCommand(this, message);
        } else {
          _server.BroadcastMessage(this, message);
        }
      }

      _server.LogMessage(Name + " has stopped the thread!");
    }

    public void WriteMessage(string xMessage) {
      try {
        _writer.WriteLine(xMessage);
        _writer.Flush();
      } catch (Exception ex) {
        _server.LogMessage($"Exception at {Name}-writeMessage(). {ex.Message}");
      }
    }

    public void StopClient() {
      _isListening = false;
    }

    public void SendWelcomeMessage() {
      WriteMessage("Hello - You are connected to the Server. Please login with /login [USERNAME] or call /help.");
    }
  }

}