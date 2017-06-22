$(function () {
    const root = window.location.origin;
    const battleId = getUrlParameter("id");

    $('div#player table').append(printBoardForPlayer());
    $('div#opponent table').append(printBoardForOpponent());

    var tbody = $('div#player tbody');

    const occupied = 'occupied';

    tbody.find('td.cell').each(function () {
        $(this).on('click', function () {
            const button = $(this).find('button');
            const cellPosition = $(this).data('cell');
            if ($(this).hasClass(occupied)) {
                $(this).removeClass(occupied);

                button.removeClass('btn-success');
                button.addClass('btn-info');

                removeConstraints(cellPosition);
            } else {
                let counter = parseInt($(this).attr('data-counter'));
                if (counter === 0) {
                    $(this).addClass(occupied);

                    button.removeClass('btn-info');
                    button.addClass('btn-success');

                    addConstraints(cellPosition);
                }
            }
        });
    });

    const columns = ["", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", ""];

    function addConstraints(cellPosition) {
        let column = cellPosition.split("-")[0];
        let row = parseInt(cellPosition.split("-")[1]);

        let leftTopCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) - 1]}-${row - 1}"]`);
        increaseCellCounter(leftTopCell);

        let rightTopCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) + 1]}-${row - 1}"]`);
        increaseCellCounter(rightTopCell);

        let rightBottomCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) + 1]}-${row + 1}"]`);
        increaseCellCounter(rightBottomCell);

        let leftBottomCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) - 1]}-${row + 1}"]`);
        increaseCellCounter(leftBottomCell);
    }
    ;

    function increaseCellCounter(cell) {
        let cellCounter = parseInt(cell.attr("data-counter"));
        if (!isNaN(cellCounter)) {
            cellCounter++;
            cell.attr("data-counter", cellCounter.toString());
            cell.find("button").addClass("disabled");
        }
    }
    ;

    function removeConstraints(cellPosition) {
        let column = cellPosition.split("-")[0];
        let row = parseInt(cellPosition.split("-")[1]);

        let leftTopCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) - 1]}-${row - 1}"]`);
        decreaseCellCounter(leftTopCell);

        let rightTopCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) + 1]}-${row - 1}"]`);
        decreaseCellCounter(rightTopCell);

        let rightBottomCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) + 1]}-${row + 1}"]`);
        decreaseCellCounter(rightBottomCell);

        let leftBottomCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) - 1]}-${row + 1}"]`);
        decreaseCellCounter(leftBottomCell);
    }
    ;

    function decreaseCellCounter(cell) {
        let cellCounter = parseInt(cell.attr("data-counter"));
        if (!isNaN(cellCounter)) {
            cellCounter--;
            cell.attr("data-counter", cellCounter.toString());
            if (cellCounter <= 0) {
                cell.find("button").removeClass("disabled");
            }
        }
    }
    ;

    const battleHubProxy = $.connection.battleHub;

    battleHubProxy.client.opponentHasGivenIn = function (opponent, targetLocation) {
        bootbox.alert({
            title: "Game over",
            message: `You are a winner! ${opponent} has given in.`,
            size: "small",
            callback: function (r) {
                battleHubProxy.server.leaveBattle(battleId);
                window.location = targetLocation;
            }
        });
    }

    $.connection.hub.start().done(function () {

        battleHubProxy.server.joinBattle(battleId);

        initOpponentBoard();
        
        const giveInButton = $("button#js-give-in");
        const clearBoard = $("button#js-clear-board");
        const readyButton = $("button#js-ready");

        giveInButton.click(function () {
            bootbox.confirm({
                title: "Confirmation",
                message: "Do you want to give in? This cannot be undone.",
                buttons: {
                    cancel: {
                        label: '<i class="fa fa-times"></i> Cancel'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> Confirm'
                    }
                },
                callback: function (result) {
                    if (result) {
                        
                        $.ajax({
                            url: root + "/Battle/GiveInAsync?battleId=" + battleId,
                            method: "POST",
                            success: function (data) {
                                bootbox.alert({
                                    title: "Game over",
                                    message: "You have given in.",
                                    callback: function (r) {
                                        let targetLocation = data.Data;

                                        battleHubProxy.server.giveIn(battleId, targetLocation);
                                        battleHubProxy.server.leaveBattle(battleId);
                                        window.location = targetLocation;
                                    }
                                });
                            },
                            error: function (message) {
                                console.log("error", message);
                            }
                        });
                    }
                }
            });
        });

        clearBoard.click(function () {
            let cells = $("div#player td.cell");

            cells.each(function () {
                $(this).attr("data-counter", 0);
                $(this).removeClass("occupied");

                let button = $(this).find("button");
                button.removeClass("btn-success");
                button.addClass("btn-info");
                button.removeClass("disabled");
            });
        });

        readyButton.click(function () {
            const totalNumberOfDecks = 5 * 1 + 4 * 1 + 3 * 2 + 2 * 2 + 2 * 1;
            let currentNumberOfDecks = parseInt($("div#player td.occupied").length);
            if (currentNumberOfDecks < totalNumberOfDecks) {
                bootbox.alert({
                    message: `<center>Zakreśl jeszcze ${totalNumberOfDecks - currentNumberOfDecks} statki</center>`,
                    size: "small"
                });
            } else if (currentNumberOfDecks > totalNumberOfDecks) {
                bootbox.alert({
                    closeButton: false,
                    message: `<center>Wykreśl jeszcze ${currentNumberOfDecks - totalNumberOfDecks} statki</center>`,
                    size: "small"
                });
            } else {
                bootbox.dialog({
                    message: '<div class="text-center"><i class="fa fa fa-spin fa-spinner"></i> Loading...</div>'
                });


                $.ajax({
                    url: root,//TODO
                    contentType: "application/json",
                    method: "POST",
                    success: function (result) {
                        //TODO
                        bootbox.hideAll();
                        clearBoard.closest('div').remove();
                        readyButton.closest('div').remove();
                    },
                    error: function (message) {
                        //TODO
                        bootbox.hideAll();
                        bootbox.dialog({
                            message: `<div class="text-center">` + message + `</div>`
                        });
                    }
                });
            }
        });
    });

    var initOpponentBoard = function () {
        const opponentCells = $('div#opponent td.cell');

        opponentCells.each(function () {
            $(this).click(function () {
                let cell = $(this);
                $.ajax({
                    url: root + "/Battle/AttackAsync",
                    method: "POST",
                    data: { battleId: battleId, cell: $(this).data("cell") },
                    success: function (result) {
                        if (result.IsSuccess) {
                            if (result.Data.isGameOver === true) {
                                bootbox.alert({
                                    title: "Game over",
                                    message: "You have won",
                                    callback: function (r) {
                                        battleHubProxy.server.leaveBattle(battleId);
                                        window.location = root + "/Chat/Index";
                                    }
                                });
                            } else {
                                if (result.Data.result === "missed")
                                    cell.find("button").addClass("js-miss");
                                else
                                    cell.find("button").addClass("js-hit");

                            }
                            cell.off("click");
                        }
                    },
                    error: function (message) {
                        console.log(message);
                    }
                });
            });
        });
    }
});

getUrlParameter = function(sParam) {
    var sPageUrl = decodeURIComponent(window.location.search.substring(1)),
        sUrlVariables = sPageUrl.split("&"),
        sParameterName,
        i;

    for (i = 0; i < sUrlVariables.length; i++) {
        sParameterName = sUrlVariables[i].split("=");

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
    return null;
};