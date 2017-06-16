using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using BattleShip.Repository.Interfaces;
using Microsoft.AspNet.SignalR;
using BattleShip.Web.Infrastructure;

namespace BattleShip.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IAccountRepository _repository
            = ContainerManager.Container.Resolve<IAccountRepository>();

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

        public override async Task OnConnected()
        {
            await Groups.Add(Context.ConnectionId, Context.User.Identity.Name);

            var actualPlayerStatus = await _repository.EnterChatWebPage(Context.User.Identity.Name);

            Clients.Others
                .addOrUpdatePlayerPermissions(actualPlayerStatus.Login,
                    actualPlayerStatus.AllowPrivateChat,
                    actualPlayerStatus.AllowNewBattle);
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            await Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);
        }
    }
}