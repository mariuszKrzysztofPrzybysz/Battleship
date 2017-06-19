$(function () {
    const messageInput = $('input#message-input');
    const chatMessageContainer = $('ul#js-chat-messages-container');
    const privateChat = $('button[data-event="private-chat"]');
    const navTabs = $('ul#js-nav-tabs');
    const tabPaneContainer = $('div#js-tab-pane-container');
    const playerList = $('ul#player-list-container');

    var chatHubProxy = $.connection.chatHub;

    chatHubProxy.client.removePlayerFromTheList=function(sender) {
        playerList.find(`li[data-player-name="` + sender + `"]`).remove();
    }

    chatHubProxy.client.addOrUpdatePlayerPermissions = function (sender, allowPrivateChat, allowBattle) {
        var playerOnTheList = playerList.find(`li[data-player-name="` + sender + `"]`);

        let allowPrivateChatClass = allowPrivateChat === true ? "active" : "disabled";
        let allowBattleClass = allowBattle === true ? "active" : "disabled";

        if (playerOnTheList.length ===0) {
            //TODO: Dodać nowozalogowanego użytkownika do listy
            var newPlayer = $(`<li class="list-group-item" data-player-name="`+sender+`">
                                    <div class="media">
                                        <div class="media-left">
                                            <img src="/Content/Images/Photos/`+sender+`.jpg" class="img-rounded" alt="`+sender+`" width="50" height="50">
                                        </div>
                                        <div class="media-body">
                                            <h4 class="media-heading">`+sender+`</h4>
                                            <button type="button" class="btn btn-info btn-xs `+allowPrivateChatClass+`" data-event="private-chat">Private chat</button>
                                            <button type="button" class = "btn btn-danger btn-xs `+allowBattleClass+`" data-event="battle">New battle</button>
                                        </div>
                                    </div>
                                </li>`);

            let privateChatButton = newPlayer.find('button[data-event="private-chat"]');
            privateChatButton.click(function() {
                bootbox.dialog({
                    title: 'Prywatny chat',
                    message: '<p class="text-center">Wysłano zaproszenie do ' + sender + '.</p>',
                    onEscape: true
                });

                chatHubProxy.server.inviteToPrivateChat(sender);
            });

            let battleButton = newPlayer.find('button[data-event="battle"]');
            battleButton.click(function () {
                bootbox.dialog({
                    title: 'Nowa bitwa',
                    message: '<p class="text-center">Wysłano zaproszenie do ' + sender + '.</p>',
                    onEscape: true
                });

                //chatHubProxy.server.inviteToBattle(addressee);
            });

            playerList.prepend(newPlayer);
        } else {
            playerOnTheList.find(`button[data-event="private-chat"]`)
                .removeClass('active').removeClass('disabled')
                .addClass(allowPrivateChatClass);
            playerOnTheList.find(`button[data-event="battle"]`)
                .removeClass('active').removeClass('disabled')
                .addClass(allowBattleClass);
        }
    };

    chatHubProxy.client.receivePublicMessage = function(sender, message) {
        chatMessageContainer.prepend(addNewMessage(sender, message));
    };

    chatHubProxy.client.receivePrivateMessage = function(privateChatGroupName, sender, message) {
        let messagesContainer = $(`ul[data-private-chat-group-name="` + privateChatGroupName + `"]`);

        messagesContainer.prepend(addNewMessage(sender, message));
    };

    chatHubProxy.client.receiveInvitationToPrivateChat = function (sender, privateChatGroupName) {
        bootbox.confirm({
            title: `Zaproszenie do prywatnego czatu`,
            message:
                `Użytkownik ` + sender + ` zaprasza na prywatny czat. Czy się zgadzasz?`,
            buttons: {
                cancel: {
                    label: '<i class="fa fa-times"></i> Nie'
                },
                confirm: {
                    label: '<i class="fa fa-check"></i> Tak'
                }
            },
            callback: function(result) {
                if (result === true) {
                    chatHubProxy.server.openNewTab(sender, privateChatGroupName);
                } else {
                    
                }
            }
        });
    };

    function initClosePrivateChat(navTab, playerName, privateChatGroupName) {
        navTab.find('span.close').click(function () {
            navTab.remove();
            let tabPane = $('div#' + playerName);
            tabPane.remove();

            navTabs.find('li').first().addClass('active');
            $('div#chat').addClass('active in');

            chatHubProxy.server.sendPrivateMessage(privateChatGroupName,
                "Użytkownik " + playerName + " zakończył prywaną rozmowę");
        });
    }

    chatHubProxy.client.openNewTab = function (playerName, privateChatGroupName) {
        let navTab = $(`<li data-player-name="` +
            playerName +
            `" data-private-chat-group-name=` +
            privateChatGroupName +
            ` class=""><a href="#` +
            playerName +
            `" data-toggle="tab"><span class="badge"></span>&nbsp;` +
            playerName +
            `&nbsp;<span class="close" aria-hidden="true">&times;</span></a></li>`);

        initClosePrivateChat(navTab, playerName, privateChatGroupName);

        navTabs.append(navTab);

        tabPaneContainer.append(`<div class="tab-pane fade" id="` +
            playerName +
            `" style="overflow-y: auto;"><ul class = "list-group" id="js-` +
            playerName +
            `-messages-container" data-private-chat-group-name="` +
            privateChatGroupName +
            `"></ul>
                                </div>`);
    };

    $.connection.hub.start().done(function () {
        privateChat.each(function () {
            if ($(this).hasClass('active')) {
                //TODO: click vs on('click', ...
                $(this).click(function () {
                    let addresseePlayerName = $(this).closest('li').data('player-name');

                    bootbox.dialog({
                        title: 'Prywatny chat',
                        message: '<p class="text-center">Wysłano zaproszenie do ' + addresseePlayerName + '.</p>',
                        onEscape: true
                    });

                    chatHubProxy.server.inviteToPrivateChat(addresseePlayerName);
                });
            }
        });

        messageInput.keyup(function (event) {
            if (event.key === "Enter") {
                let encodedMessage = htmlEncode(messageInput.val());

                let addresee = $('ul.nav-tabs').find('li.active');

                let encodedAddreseePlayerName = htmlEncode(addresee.data('player-name'));

                if (typeof encodedAddreseePlayerName === "undefined" || encodedAddreseePlayerName==="") {
                    chatHubProxy.server.sendPublicMessage(encodedMessage);
                } else {
                    let encodedPrivateChatGroupName = htmlEncode(addresee.data('private-chat-group-name'));
                    chatHubProxy.server.sendPrivateMessage(encodedPrivateChatGroupName, encodedMessage);
                }

                messageInput.val("");
            }
            if (event.key === "Escape") {
                messageInput.val("");
            }
        });
    });
});

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

function addNewMessage(sender, message) {
    var encodedSender = htmlEncode(sender);
    var encodedMessage = htmlEncode(message);

    var date = new Date();

    return `<li class="list-group-item">
                <div class="media">
                    <div class="well well-sm">
                        <div class="media-left">
                            <img src= "/Content/Images/Photos/` + encodedSender + `.jpg" class ="img-rounded" alt= "` + encodedSender + `" width="75" height="75">
                        </div>
                    <div class="media-body">
                        <h3 class="media-heading">&nbsp; <small>&nbsp; <i>&nbsp; Published&nbsp; ` + date + ` </i></small></h3>
                        <p> ` + encodedMessage + ` </p>
                    </div>
                 </div>
                </div>
            </li>`;
}