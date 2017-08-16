function Reservations() {
    var self = this;
    self.Reservations = ko.observableArray();
    self.Id = ko.observable();
    self.ClientName = ko.observable();
    self.ReservationDate = ko.observable();
    self.Seats = ko.observable();
    self.loadingPanel = new LoadingOverlay();

    self.details = function (data) {
        self.Id(data.Id);
        self.ClientName(data.ClientName);
        self.Date(data.ReservationDate);
        self.Seats(data.Seats);
    };

    self.refresh = function () {
        var url = '/Reservations/ListRefresh';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Reservations(data.Reservations);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
}