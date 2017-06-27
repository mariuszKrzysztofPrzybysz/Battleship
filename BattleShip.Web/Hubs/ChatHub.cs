﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using BattleShip.Repository.Interfaces;
using BattleShip.Web.Helpers;
using Microsoft.AspNet.SignalR;
using BattleShip.Web.Infrastructure;

namespace BattleShip.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IPlayerRepository _playerRepository
            = ContainerManager.Container.Resolve<IPlayerRepository>();

        public void RedirectToBattleWebPage(string playerName, long battleId)
        {
            var targetUrl = UrlBuilder.GetUrl("Battle", "Play", new string[] {$"battleId={battleId}"});

            Clients.Groups(new List<string> {playerName, Context.User.Identity.Name}).openNewWebPage(targetUrl);
        }

        public void InviteToBattle(string addresseePlayerName)
        {
            var sender = Context.User.Identity.Name;

            Clients.Group(addresseePlayerName)
                .receiveInvitationToBattle(sender);
        }

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
            var userName = Context.User.Identity.Name;

            await Groups.Add(Context.ConnectionId, userName);

            var actualPlayerStatus = await _playerRepository.EnterChatWebPage(userName);

            Clients.Others
                .addOrUpdatePlayerPermissions(actualPlayerStatus.Login,
                    actualPlayerStatus.AllowPrivateChat,
                    actualPlayerStatus.AllowNewBattle);
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            var userName = Context.User.Identity.Name;

            var result = await _playerRepository.ExitChatWebPage(userName);

            if (result.IsSuccess)
            {
                Clients.Others
                    .removePlayerFromTheList(userName);
            }
        }
    }
}