using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using iTemplate.Web.Models;
using iTemplate.Web.Models.Data;

namespace iTemplate.Web.SignalR
{
  [HubName("serverHub")]
  public class ServerHub : Hub
  {
    private object _connections = new object();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override Task OnConnected()
    {
      lock (_connections)
      {
        var name = (Context.User == null) ? "Anonymous" : Context.User.Identity.Name;
        using (var db = new ApplicationDbContext())
        {
          if (db.Connections.Find(Context.ConnectionId) == null)
          {
            var newConnection = new Connection
            {
              Id = Context.ConnectionId,
              UserName = name,
              DateConnected = DateTime.Now,
              UserAgent = Context.Request.Headers["User-Agent"],
              IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
              Referrer = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"],
              CurrentPage = HttpContext.Current.Request.ServerVariables["URL"],
              IsConnected = true
            };

            db.Connections.Add(newConnection);
            db.SaveChanges();
          }
        }
        Groups.Add(Context.ConnectionId, name);
      } 
      
      return base.OnConnected();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stopCalled"></param>
    /// <returns></returns>
    public override Task OnDisconnected(bool stopCalled)
    {
      lock (_connections)
      {
        using (var db = new ApplicationDbContext())
        {
          var connection = db.Connections.Find(Context.ConnectionId);
          if (connection != null)
          {
            connection.IsConnected = false;
            connection.DateDisconnected = DateTime.Now;
            db.SaveChanges();
          }
        }
      }

      return base.OnDisconnected(stopCalled);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override Task OnReconnected()
    {
      lock (_connections)
      {
        using (var db = new ApplicationDbContext())
        {
          var connection = db.Connections.Find(Context.ConnectionId);
          if (connection != null)
          {
            connection.IsConnected = true;
            db.SaveChanges();
          }
        }
      }

      return base.OnReconnected();
    }

    /// <summary>
    /// 
    /// </summary>
    public void BroadcastToAllUsers(string message)
    {
      Clients.All.SystemMessage(message);
    }

    public void BroadcastToCurrentUser(string message)
    {
      var name = (Context.User == null) ? string.Empty : Context.User.Identity.Name;
      lock (_connections)
      {
        using (var db = new ApplicationDbContext())
        {
          var connection = db.Connections.Find(Context.ConnectionId);
          if (connection != null)
          {
            Clients.All.PersonalMessage(message, name, connection.Id);
          }
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ClientPing()
    {
      BroadcastToCurrentUser("Server Responded...");
    }
  }
}