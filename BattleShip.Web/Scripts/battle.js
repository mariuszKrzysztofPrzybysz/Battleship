$(function() {
    const battleHubProxy = $.connection.battleHub;

    const root = window.location.origin;
    const battleId = getUrlParameter("battleId");

    const playerTable = $("div#player table");
    const opponentTable = $("div#opponent table");

    playerTable.append(printBoardForPlayer());
    opponentTable.append(printBoardForOpponent());


    function initializePlayerBoard() {
        var tbody = $("div#player tbody");

        const occupied = "occupied";

        tbody.find("td.cell").each(function() {
            $(this).on("click",
                function() {
                    const button = $(this).find("button");
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

            let leftTopCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) - 1]
                }-${row - 1}"]`);
            increaseCellCounter(leftTopCell);

            let rightTopCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) + 1]}-${row - 1
                }"]`);
            increaseCellCounter(rightTopCell);

            let rightBottomCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) + 1]}-${row + 1
                }"]`);
            increaseCellCounter(rightBottomCell);

            let leftBottomCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) - 1]
                }-${row + 1}"]`);
            increaseCellCounter(leftBottomCell);
        };

        function increaseCellCounter(cell) {
            let cellCounter = parseInt(cell.attr("data-counter"));
            if (!isNaN(cellCounter)) {
                cellCounter++;
                cell.attr("data-counter", cellCounter.toString());
                cell.find("button").addClass("disabled");
            }
        };

        function removeConstraints(cellPosition) {
            let column = cellPosition.split("-")[0];
            let row = parseInt(cellPosition.split("-")[1]);

            let leftTopCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) - 1]}-${row - 1
                }"]`);
            decreaseCellCounter(leftTopCell);

            let rightTopCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) + 1]
                }-${row - 1
                }"]`);
            decreaseCellCounter(rightTopCell);

            let rightBottomCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) + 1]}-${row + 1
                }"]`);
            decreaseCellCounter(rightBottomCell);

            let leftBottomCell = tbody.find(`td[data-cell="${columns[columns.indexOf(column) - 1]
                }-${row + 1
                }"]`);
            decreaseCellCounter(leftBottomCell);
        };

        function decreaseCellCounter(cell) {
            let cellCounter = parseInt(cell.attr("data-counter"));
            if (!isNaN(cellCounter)) {
                cellCounter--;
                cell.attr("data-counter", cellCounter.toString());
                if (cellCounter <= 0) {
                    cell.find("button").removeClass("disabled");
                }
            }
        };
    };

    function uninitializePlayerBoard() {
        playerTable.find("td.cell").off('click');
    }

    function initializeOpponentBoard() {
        const opponentCells = $('div#opponent td.cell');

        opponentCells.each(function() {
            $(this).click(function() {
                let cell = $(this);
                $.ajax({
                    url: root + "/Battle/AttackAsync",
                    method: "POST",
                    data: { battleId: battleId, cell: $(this).data("cell") },
                    success: function(result) {
                        if (result.IsSuccess) {
                            uninitializeOpponentBoard();
                            $("div#action").text("Action: defend");

                            if (result.Data.result === "missed") {
                                cell.find("button").addClass("js-miss");
                                battleHubProxy.server.updateCellStatus(battleId, cell.data("cell"), false);
                            }
                            else {
                                cell.find("button").addClass("js-hit");
                                battleHubProxy.server.updateCellStatus(battleId, cell.data("cell"), true);

                                if (result.Data.isGameOver === true) {
                                    bootbox.alert({
                                        title: "Game over",
                                        message: "You have won",
                                        callback: function(r) {
                                            battleHubProxy.server.sendMessageToDefeated(battleId);
                                            window.location = root + "/Chat/Index";
                                        }
                                    });
                                }

                                cell.off("click");
                            }
                        }
                    },
                    error: function(message) {
                        console.log(message);
                    }
                });
            });
        });
    };

    function uninitializeOpponentBoard() {
        opponentTable.find("td.cell").off("click");
    }

    function initializeControlPanel() {
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
                            url: root + "/Battle/GiveInAsync",
                            method: "POST",
                            data: {battleId: battleId},
                            success: function (data) {
                                bootbox.alert({
                                    title: "Game over",
                                    message: "You have given in.",
                                    callback: function (r) {
                                        let targetLocation = data.Data;

                                        battleHubProxy.server.giveIn(battleId, targetLocation);
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
            //const totalNumberOfDecks = 5 * 1 + 4 * 1 + 3 * 2 + 2 * 2 + 2 * 1;
            const totalNumberOfDecks = 5;
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
                let playerBoard = loadPlayerBoard();
                
                if (playerBoard.length === 0) {
                    bootbox.alert({
                        closeButton: false,
                        title: "Błąd",
                        message: "<center>Wypełnij poprawnie planszę!</center>",
                        size: "small"
                    });
                    return;
                }
                $.ajax({
                    url: root + "/Battle/UploadBoardAsync",
                    method: "POST",
                    data: { battleId: battleId, board: playerBoard },
                    beforeSend: function() {
                        bootbox.dialog({
                            message: '<div class="text-center"><i class="fa fa fa-spin fa-spinner"></i> Loading...</div>'
                        });
                    },
                    success: function (result) {
                        console.log(r.Data);
                        bootbox.hideAll();
                        clearBoard.closest("div").remove();
                        readyButton.closest("div").remove();
                        uninitializePlayerBoard();
                        initializeOpponentBoard();
                        battleHubProxy.server.changePlayerStatusToReady(battleId);
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
    }

    function loadPlayerBoard() {
        let result = [];

        playerTable.find("td.occupied").each(function() {
            result.push($(this).data("cell"));
        });

        if (isValidBoard(result))
            return result.join("");

        result = [];
        return result;
    }

    function isValidBoard(board) {
        return true;
    }

    $.connection.hub.start().done(function() {
        battleHubProxy.server.joinBattle(battleId);

        initializePlayerBoard();

        initializeControlPanel();
    });

    battleHubProxy.client.updateOpponentStatus=function() {
        $("div#status").text("Status: ready");
    }

    battleHubProxy.client.receiveMessageFromWinner = function(winner) {
        bootbox.alert({
            title: "Game over",
            message: `${winner} has won the battle. Click the button to return to chat`,
            callback: function(r) {
                window.location = root + "/Chat/Index";
            }
        });
    }

    battleHubProxy.client.opponentHasGivenIn = function (opponent, targetLocation) {
        bootbox.alert({
            title: "Game over",
            message: `You are a winner! ${opponent} has given in.`,
            size: "small",
            callback: function (r) {
                window.location = targetLocation;
            }
        });
    }

    battleHubProxy.client.updateCellStatus = function (cell, isHitted) {
        let playerButton = playerTable.find(`td[data-cell="${cell}"]`).find("button");

        if (isHitted)
            playerButton.addClass("js-hit");
        else
            playerButton.addClass("js-miss");

        opponentTable.on("click", "td.cell", initializeOpponentBoard);
        $("div#action").text("Action: attack");
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