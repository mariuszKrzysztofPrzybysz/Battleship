var BattleViewModel = function() {
    var self = this;

    self.battles = ko.observableArray();

    var battlesUri = "/api/battles";

    function getAllBattles() {
        ajaxHelper(battlesUri, "GET").done(function(data) {
            self.battles(data);
        });
    }

    getAllBattles();

    self.resumeBattle = function (battleId) {
        return `${getUrlRoot()}/Battle/Play?battleId=${battleId}`;
    }
};

ko.applyBindings(new BattleViewModel());