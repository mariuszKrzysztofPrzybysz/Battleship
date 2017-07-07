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
};

ko.applyBindings(new BattleViewModel());