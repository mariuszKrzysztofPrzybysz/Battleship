$(function () {
    const messageInput = $('input#message-input');
    const chatMessageContainer = $('ul.js-chat-messages-container');
    const privateChat = $('button[data-event="private-chat"]');

    var chatHubProxy = $.connection.chatHub;

    chatHubProxy.client.receivePublicMessage = function(sender, message) {
        chatMessageContainer.prepend(addNewMessage(sender, message));
    };

    chatHubProxy.client.answerToInviteToPrivateChat = function (sender) {
        console.log(sender);
    };

    $.connection.hub.start().done(function () {
        privateChat.each(function () {
            if ($(this).hasClass('active')) {
                //TODO: click vs on('click', ...
                $(this).on('click', function () {
                    let addresseePlayerName = $(this).closest('li').data('player-name');

                    chatHubProxy.server.inviteToPrivateChat(addresseePlayerName);
                });
            }
        });

        messageInput.keyup(function (event) {
            if (event.key === "Enter") {
                let addresee = $('ul.nav-tabs').find('li.active');

                let addreseePlayerName = addresee.data('player-name');

                if (typeof addreseePlayerName === "undefined") {
                    chatHubProxy.server.sendPublicMessage(messageInput.val());
                } else {
                    
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