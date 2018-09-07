using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server {

  class CommandHandler {
    // Threadsafe by documentation:
    //  https://msdn.microsoft.com/de-de/library/system.text.stringbuilder(v=vs.110).aspx
    private static readonly StringBuilder _builder = new StringBuilder();

    public static string GetHelpMessage() {
      _builder.Clear();

      _builder.AppendLine("To communicate with the server, there are the following commands available:");
      _builder.AppendLine("  /help - Displays this help dialog");
      _builder.AppendLine("  /login [USERNAME] - Logs you on into the server. You have to be logged in to join the chat");
      _builder.AppendLine("  /logoff - Logs you off from the server and closes the session");
      _builder.AppendLine("  /getUser - Displays a list of all currently available users");
      _builder.AppendLine("  /whisper [USERNAME] [MESSAGE] - Sends a private message to the target user");
      _builder.AppendLine("Every normal written text will be handled as broadcast-message to all connected users.");
      _builder.AppendLine();

      return _builder.ToString();
    }

    public static string GetUserMessage(List<string> xClientNames) {
      _builder.Clear();
      _builder.AppendLine("Following users are logged in:");

      foreach (string iiClient in xClientNames) {
        _builder.AppendFormat("  {0}\n", iiClient);
      }

      return _builder.ToString();
    }

    public static string ExtractUsernameFromLogin(string xMessage) {
      string[] lParts = xMessage.Split(' ');
      return lParts.Last();
    }

    public static string GetLoginConfirmedMessage(string xUsername) {
      return $"You are now logged in as '{xUsername}'.";
    }

    /// <summary>
    ///   Extracts the Target and the Message from the whisper-command.
    ///   Returns the target by out-parameter.
    /// </summary>
    /// <param name="xSender">The name of the sender of the message</param>
    /// <param name="xMessage">The message, the user has sent</param>
    /// <param name="xTarget">Contains the target of the message</param>
    /// <returns>The message which should be send to the recipient</returns>
    public static string ExtractWhisperMessage(string xSender, string xMessage, out string xTarget) {
      // Use regex to have two groups. The first contains the username. The second
      // contains the message.
      Match match = Regex.Match(xMessage, @"/whisper\s+(\w+)\s+(.*)");
      string message = "";

      // There have to be 3 Groups. Always! Otherwise there is an error in the using
      if (match.Groups.Count != 3) {
        xTarget = "";
      } else {
        xTarget = match.Groups[1].Value;
        message = $"From {xSender}: {match.Groups[2].Value}";
      }

      return message;
    }

    public static string GetWhisperConfirmed(string xTarget) {
      return $"Message has been sent to '{xTarget}'";
    }

    public static string GetErrorCommandNotRecognized() {
      return "ERROR: Your command is not existing or has a typo. Please refer to /help if you need some!";
    }

    public static string GetErrorNotLoggedIn() {
      return "ERROR: You have to be logged in to be able to write and receive Messages. To login use the /login command!";
    }

    public static string GetErrorUsernameNotAllowed(string xUsername) {
      return $"ERROR: The username '{xUsername}' is not allowed. Please use just letters from A-Z.";
    }

    public static string GetErrorUserExisting() {
      return "ERROR: A user with this name is already existing!";
    }

    public static string GetErrorUserNotExisting(string xTarget) {
      return $"ERROR: The user '{xTarget}' is not existing or logged in!";
    }

    public static string GetErrorAlreadyLoggedIn() {
      return "ERROR: You are already logged in!";
    }

  }

}