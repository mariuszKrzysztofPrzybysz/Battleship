$(function() {
    var chatHubProxy = $.connection.chatHub;

    chatHubProxy.client.receivePublicMessage = function (from, message) {
        console.log(from, message);
    };

    $.connection.hub.start().done(function () {
        chatHubProxy.server.sendPublicMessage("test");
    });
});