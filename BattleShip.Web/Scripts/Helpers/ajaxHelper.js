var self = this;

self.error = ko.observable();

function ajaxHelper(uri, method, data) {
    self.error("");
    return $.ajax({
            type: method,
            url: uri,
            dataType: "json",
            contentType: "application/json",
            data: data ? JSON.stringify(data) : null
        })
        .fail(function(jqXhr, textStatus, errorThrown) {
            self.error(errorThrown);
        });
}