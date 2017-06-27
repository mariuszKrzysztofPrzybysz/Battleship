printBoardForPlayer = function () {
    return printBoard(tdCellsForPlayer);
};
printBoardForOpponent = function () {
    return printBoard(tdCellsForOpponent);
};

printBoard = function(tdCells) {
    let columns = ["", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J"];

    const tbody = $("<tbody/>");

    let tr = $("<tr/>");
    for (let c = 0; c < columns.length; c++) {
        tr.append(`<td>${columns[c]}</td>`);
    }
    tbody.append(tr);

    for (let r = 1; r <= 10; r++) {
        let tr = $("<tr/>");
        tr.append(`<td>${r}</td>`);

        for (let c = 1; c < columns.length; c++) {
            tr.append(tdCells);
        }

        tbody.append(tr);
    }

    return tbody;
};

tdCellsForPlayer = function(column, row) {
    return `<td class="cell" data-counter="0" data-cell="${column}-${row}"><button class="btn btn-info"></button></td>`;
};

tdCellsForOpponent = function (column, row) {
    return `<td class="cell" data-cell="${column}-${row}"><button class="btn btn-info"></button></td>`;
};