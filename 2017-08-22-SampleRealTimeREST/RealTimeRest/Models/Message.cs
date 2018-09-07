using System;

namespace RealTimeRest.Models {

  /// <summary>
  ///   This data class is a wrapper for sent and received messages.
  /// </summary>
  public class Message {

    /// <summary>
    ///   ID of this message
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    ///   The message content
    /// </summary>
    public string Content { get; set; }

  }

}