$(function () {
    const jsChatContainer = $('div.js-chat-container');
    const navbarHeader = $('div.navbar-header');
    const divPanel = $('div.panel');
    const divPanelHeading = $('div.panel-heading');
    const divPanelBody = $('div.panel-body');
    const hr = $('hr');
    const footer = $('footer');

    jsChatContainer.height($(window).height() -
        navbarHeader.outerHeight() -
        hr.outerHeight());

    divPanel.outerHeight(jsChatContainer.height() - divPanelHeading.outerHeight());

    divPanelBody.innerHeight(divPanel.innerHeight() - divPanelHeading.outerHeight());
});