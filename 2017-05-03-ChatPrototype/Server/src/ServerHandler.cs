using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace Server {

  class ServerHandler {
    private readonly int _port;
    private volatile bool _isListening = true;

    private Dictionary<string, AcceptedClient> _clients = new Dictionary<string, AcceptedClient>();
    private static int _numberOfClients = 0;

    public ServerHandler(int xPort) {
      _port = xPort;
    }


    public async void StartServer() {
      // Start listener which accepts connections on the specific port
      TcpListener listener = new TcpListener(IPAddress.Any, _port);
      listener.Start();

      LogMessage("Start listening...");
      while (_isListening) {
        // Wait for a connection
        TcpClient clientSocket = await listener.AcceptTcpClientAsync();
        LogMessage("New Client accepted");

        // Wrap this socket into a specific class which handles requests
        AcceptedClient client = new AcceptedClient(clientSocket, this);
        client.SendWelcomeMessage();
        client.Name = GetTemporaryClientName();

        // Start thread for handling requests and messages
        Thread clientThread = new Thread(client.WaitForRequest);
        client.CurrentThread = clientThread;
        clientThread.Start();
      }
    }

    public void LogMessage(string xMessage) {
      string time = DateTime.Now.ToString("hh:mm:ss");
      Console.WriteLine("{0}: {1}", time, xMessage);
    }

    public void HandleCommand(AcceptedClient xClient, string xMessage) {
      string command = xMessage.Split(' ')[0].ToLower();
      string message = "";

      string target = "";
      switch (command) {
        case "/help":
          message = CommandHandler.GetHelpMessage();
          xClient.WriteMessage(message);
          break;

        case "/getuser":
          message = CommandHandler.GetUserMessage(_clients.Keys.ToList());
          xClient.WriteMessage(message);
          break;

        case "/login":
          string username = CommandHandler.ExtractUsernameFromLogin(xMessage);
          if (xClient.IsLoggedIn) {
            message = CommandHandler.GetErrorAlreadyLoggedIn();
            xClient.WriteMessage(message);
          } else if (IsUsernameValid(username) == false) {
            message = CommandHandler.GetErrorUsernameNotAllowed(username);
            xClient.WriteMessage(message);
          } else if (_clients.ContainsKey(username)) {
            message = CommandHandler.GetErrorUserExisting();
            xClient.WriteMessage(message);
          } else {
            _clients.Add(username, xClient);
            xClient.IsLoggedIn = true;
            xClient.Name = username;

            message = CommandHandler.GetLoginConfirmedMessage(username);
            xClient.WriteMessage(message);
          }
          break;

        case "/whisper":
          string messageTarget = CommandHandler.ExtractWhisperMessage(xClient.Name, xMessage, out target);

          if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(messageTarget)) {
            message = CommandHandler.GetErrorCommandNotRecognized();
            xClient.WriteMessage(message);
          } else if (_clients.ContainsKey(target) == false) {
            message = CommandHandler.GetErrorUserNotExisting(target);
            xClient.WriteMessage(message);
          } else {
            message = CommandHandler.GetWhisperConfirmed(target);
            _clients[target].WriteMessage(messageTarget);
            xClient.WriteMessage(message);
          }

          break;

        case "/logoff":
          xClient.IsLoggedIn = false;
          xClient.WriteMessage("Logoff received. You're logged off from the server!");
          _clients.Remove(xClient.Name);
          xClient.StopClient();
          break;

        default:
          message = CommandHandler.GetErrorCommandNotRecognized();
          xClient.WriteMessage(message);
          break;
      }
    }

    private bool IsUsernameValid(string xUsername) {
      Match result = Regex.Match(xUsername, "[a-zA-Z]+");
      return result.Success;
    }

    public void BroadcastMessage(AcceptedClient xSender, string xMessage) {
      string message = $"-> {xSender.Name}: {xMessage}";

      foreach (var iiClient in _clients.Where(x => x.Key.Equals(xSender.Name) == false)) {
        iiClient.Value.WriteMessage(message);
      }
    }

    private void StopServer() {
      _isListening = false;
    }

    private string GetTemporaryClientName() {
      _numberOfClients++;
      return "Client" + _numberOfClients;
    }


    public void RemoveClient(string xName) {
      if (_clients.ContainsKey(xName)) {
        _clients.Remove(xName);
      }
    }

  }

}