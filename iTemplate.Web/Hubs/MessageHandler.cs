using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace iTemplate.Web.SignalR
{
  public class MessageHandler
  {
    // Singleton instance
    private readonly static Lazy<MessageHandler> _instance = new Lazy<MessageHandler>(
() => new MessageHandler(GlobalHost.ConnectionManager.GetHubContext<ServerHub>()));

    private IHubContext Context { get; set; }
    
    private MessageHandler(IHubContext context)
    {
      Context = context;
    }

    public static MessageHandler Instance
    {
      get
      {
        return _instance.Value;
      }
    }

    public void BroadcastToAllUsers(string message)
    {
      //IHubContext context = GlobalHost.ConnectionManager.GetHubContext<iTemplate.Web.SignalR.ServerHub>();
      //context.Clients.All.updateStatus("99");
      //Context.Clients.All.updateStatus(message);

      Instance.BroadcastToAllUsers(message);
    }
    public void BroadcastToCurrentUser(string message)
    {
      //IHubContext context = GlobalHost.ConnectionManager.GetHubContext<iTemplate.Web.SignalR.ServerHub>();
      //context.Clients.All.updateStatus("99");
      //Context.Clients.All.updateStatus(message);

      Instance.BroadcastToCurrentUser(message);
    }
  }
}