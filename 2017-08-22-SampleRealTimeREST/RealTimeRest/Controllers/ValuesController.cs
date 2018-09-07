using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealTimeRest.Models;

namespace RealTimeRest.Controllers {

  [Route("api/demo")]
  public class ValuesController : Controller {

    private static readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);

    private static Message _message = new Message() {
      ID = Guid.NewGuid(),
      Content = "Default"
    };

    
    [HttpGet]
    public Message Get() {
      _resetEvent.WaitOne();
      return _message;
    }


    [HttpPost]
    public void Post([FromBody] Message xMessage) {
      _message = xMessage;
      _resetEvent.Set();
    }

  }

}