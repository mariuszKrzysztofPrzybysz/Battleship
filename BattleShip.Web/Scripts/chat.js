$(function() {
    const messageInput = $('input#message-input');
    const messageButton = $('button#message-button');
    const chatMessageContainer = $('ul#js-chat-messages-container');
    const privateChat = $('button[data-event="private-chat"]');
    const battle = $('button[data-event="battle"]');
    const navTabs = $('ul#js-nav-tabs');
    const tabPaneContainer = $('div#js-tab-pane-container');
    const playerList = $('ul#player-list-container');

    var chatHubProxy = $.connection.chatHub;

    chatHubProxy.client.openNewWebPage = function(targetPage) {
        bootbox.alert({
            message: "Kliknij, aby przejść na nową stronę",
            size: "small",
            callback: function() {
                window.open(targetPage);
            }
        });
    }

    chatHubProxy.client.removePlayerFromTheList = function(playerName) {
        playerList.find(`li[data-player-name="` + playerName + `"]`).remove();
    }

    chatHubProxy.client.addOrUpdatePlayerPermissions = function (data) {
        var playerOnTheList = playerList.find(`li[data-player-name="` + data.Login + `"]`);

        let allowPrivateChatClass = data.AllowPrivateChat === true ? "active" : "disabled";
        let allowBattleClass = data.AllowNewBattle === true ? "active" : "disabled";

        if (playerOnTheList.length === 0) {
            let login = data.Login;
            var newPlayer = $(`<li class="list-group-item" data-player-name="` +
                login +
                `">
                <div class="media">
                <div class="media-left">
                <img src="Content/Images/Photos/` +
                login +
                `.jpg" class="img-rounded" alt="` +
                login +
                `" width="50" height="50">
                </div>
                <div class="media-body">
                <h4 class="media-heading">` +
                login +
                `</h4>
                <button type="button" class="btn btn-info btn-xs ` +
                allowPrivateChatClass +
                `" data-event="private-chat">Private chat</button>
                <button type="button" class = "btn btn-danger btn-xs ` +
                allowBattleClass +
                `" data-event="battle">New battle</button>
                </div>
                </div>
                </li>`);

            let privateChatButton = newPlayer.find('button.active[data-event="private-chat"]');
            privateChatButton.click(function() {
                chatHubProxy.server.inviteToPrivateChat(login);
            });

            let battleButton = newPlayer.find('button.active[data-event="battle"]');
            battleButton.click(function() {
                chatHubProxy.server.inviteToBattle(login);
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

    chatHubProxy.client.receiveInvitationToPrivateChat = function(sender, privateChatGroupName) {
        bootbox.confirm({
            title: `Zaproszenie do prywatnego czatu`,
            message:
                `Użytkownik ${sender} zaprasza na prywatny czat. Czy się zgadzasz?`,
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
                    chatHubProxy.server.startPrivateChat(sender, privateChatGroupName);
                }
            }
        });
    };

    chatHubProxy.client.receiveInvitationToBattle = function(playerName) {
        bootbox.confirm({
            title: `Gracz ${playerName} wyzywa cię na bitwę. Podejmiesz wyzwanie?`,
            size: "small",
            message:
                `<center><img src="/Content/Images/battle.jpg" class="img-rounded" alt="battle" width="100%"></center>`,
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
                    $.ajax({
                        url: "/Battle/CreateAsync",
                        type: "POST",
                        data: { playerName: playerName },
                        success: function(result) {
                            if (result.IsSuccess === true) {
                                chatHubProxy.server.redirectToBattleWebPage(playerName, result.Data.battleId);
                            }
                        },
                        error: function(message) {
                            console.log(message);
                        }
                    });
                }
            }
        });
    };

    chatHubProxy.client.startPrivateChat = function (playerName, privateChatGroupName) {
        let navTab = $(`<li data-player-name="` +
            playerName +
            `" data-private-chat-group-name=` +
            privateChatGroupName +
            ` class=""><a href="#` +
            playerName +
            `" data-toggle="tab"><span class="badge"></span>&nbsp;` +
            playerName +
            `&nbsp;<span class="close" aria-hidden="true">&times;</span></a></li>`);

        initClosePrivateChatButton(navTab, playerName, privateChatGroupName);

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

    function initClosePrivateChatButton(navTab, playerName, privateChatGroupName) {
        navTab.find("span.close").click(function () {
            navTab.remove();
            let tabPane = $(`div#${playerName}`);
            tabPane.remove();

            navTabs.find("li").first().addClass("active");
            $("div#chat").addClass("active in");
            messageInput.focus();

            chatHubProxy.server.sendPrivateMessage(privateChatGroupName,
                "Użytkownik zamknął okno prywanej rozmowy");
        });
    }

    $.connection.hub.start().done(function() {
        privateChat.each(function() {
            if ($(this).hasClass("active")) {
                $(this).click(function() {
                    let addresseePlayerName = $(this).closest("li").data("player-name");

                    chatHubProxy.server.inviteToPrivateChat(addresseePlayerName);
                });
            }
        });

        battle.each(function() {
            if ($(this).hasClass("active")) {
                $(this).click(function() {
                    let addresseePlayerName = $(this).closest("li").data("player-name");

                    chatHubProxy.server.inviteToBattle(addresseePlayerName);
                });
            }
        });

        messageInput.keyup(function(event) {
            if (event.key === "Enter") {
                sendMessage();
            }
            if (event.key === "Escape") {
                messageInput.val("");
            }
        });

        messageButton.on("click", sendMessage);

        function sendMessage() {
            let encodedMessage = htmlEncode(messageInput.val());
            let addresee = $("ul.nav-tabs").find("li.active");
            let encodedAddreseePlayerName = htmlEncode(addresee.data("player-name"));

            if (encodedAddreseePlayerName === "") {
                chatHubProxy.server.sendPublicMessage(encodedMessage);
            } else {
                let encodedPrivateChatGroupName = htmlEncode(addresee.data("private-chat-group-name"));
                chatHubProxy.server.sendPrivateMessage(encodedPrivateChatGroupName, encodedMessage);
            }

            messageInput.val("");
        }

        window.onbeforeunload = function() {
            chatHubProxy.server.leaveChat();
        }
    });
});

function htmlEncode(value) {
    var encodedValue = $("<div />").text(value).html();
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
                        <h3 class="media-heading">&nbsp; <small>&nbsp; <i>&nbsp; Published&nbsp; ` + date.toUTCString() + ` </i></small></h3>
                        <p> ` + encodedMessage + ` </p>
                    </div>
                 </div>
                </div>
            </li>`;
};