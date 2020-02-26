using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace iTemplate.Web.SignalR
{
  public class BackgroundServerTimer : IRegisteredObject
  {
    private readonly DateTime _internetBirthDate = DateTime.Now.AddSeconds(30);
    private readonly IHubContext hubContext;
    private Timer taskTimer;

    public BackgroundServerTimer()
    {
      hubContext = GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
      var delayStartby = 2000;
      var repeatEvery = 1000;

      taskTimer = new Timer(BroadcastUptimeToClients, null, delayStartby, repeatEvery);
    }

    private void BroadcastUptimeToClients(object state)
    {
      //Sending the server time to all the connected clients on the client method SendServerTime()
      hubContext.Clients.All.SendServerTime(string.Format("{0:dd-MMM-yyyy hh:mm:ss}", DateTime.Now));
    }

    public void Stop(bool immediate)
    {
      HostingEnvironment.UnregisterObject(this);
    }
  }
}