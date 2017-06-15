using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BattleShip.Web.Hubs
{
    public class ChatHub : Hub
    {
        public void SendPublicMessage(string message)
        {
            var sender = Context.User.Identity.Name;

            Clients.All.receivePublicMessage(sender, message);
        }

        public override Task OnConnected()
        {
            JoinGroup();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            LeaveGroup();

            return base.OnDisconnected(stopCalled);
        }

        private void JoinGroup()
        {
            Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
        }

        private void LeaveGroup()
        {
            Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);
        }
    }
}