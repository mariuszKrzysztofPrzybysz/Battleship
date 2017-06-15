$(function() {
    const chatMessageContainer = $('ul.js-chat-messages-container');
    const messageInput = $('input#message-input');


    var chatHubProxy = $.connection.chatHub;

    chatHubProxy.client.receivePublicMessage = function(sender, message) {
        chatMessageContainer.prepend(addNewMessage(sender, message));
    };

    $.connection.hub.start().done(function() {
        messageInput.keyup(function(event) {
            if (event.key === "Enter") {
                let addresee = $('ul.nav-tabs').find('li.active');

                let addreseePlayerName = addresee.data('player-name');

                if (typeof addreseePlayerName === "undefined" || addreseePlayerName === "chat") {
                    chatHubProxy.server.sendPublicMessage(messageInput.val());
                } else {
                    //$('div.tab-content div.active ul.messages-list').prepend(newMessage($('#caller-login').val(), $('#messageInput').val()));

                    //chatHubProxy.server.sendPrivateMessage(addreseeConnectionnId, $('#messageInput').val());
                }

                messageInput.val("");
            }
            if (event.key === "Esc") {
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