using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public void InviteToPrivateChat(string addresseePlayerName)
        {
            var privateChatGroupName = Guid.NewGuid().ToString();
            var sender = Context.User.Identity.Name;

            Groups.Add(Context.ConnectionId, privateChatGroupName);

            Clients.Group(addresseePlayerName)
                .receiveInvitationToPrivateChat(sender, privateChatGroupName);
        }

        public void OpenNewTab(string sender, string privateChatGroupName)
        {
            Groups.Add(Context.ConnectionId, privateChatGroupName);

            Clients.Group(Context.User.Identity.Name).openNewTab(sender, privateChatGroupName);

            Clients.Group(sender).openNewTab(Context.User.Identity.Name, privateChatGroupName);
        }

        public void SendPrivateMessage(string privateChatGroupName, string message)
        {
            var sender = Context.User.Identity.Name;

            Clients.Group(privateChatGroupName)
                .receivePrivateMessage(privateChatGroupName, sender, message);
        }

        public override Task OnConnected()
        {
            Groups.Add(Context.ConnectionId, Context.User.Identity.Name);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);

            return base.OnDisconnected(stopCalled);
        }
    }
}