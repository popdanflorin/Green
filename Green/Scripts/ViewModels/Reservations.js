function Reservations() {
    var self = this;

    self.Reservations = ko.observableArray();
    self.Restaurants = ko.observableArray();
    self.Id = ko.observable();
    self.RestaurantId = ko.observable();
    self.ClientName = ko.observable();
    self.ReservationDate = ko.observable(new Date())
    self.Seats = ko.observable();
    self.loadingPanel = new LoadingOverlay();

    self.details = function (data) {
        self.Id(data.Id);
        self.RestaurantId(data.RestaurantId);
        self.ClientName(data.ClientName);
        self.ReservationDate(data.ReservationDate);
        self.Seats(data.Seats);
    };

    self.add = function () {
        self.Id(0);
        self.RestaurantId(null);
        self.ClientName(null);
        self.ReservationDate(null);
        self.Seats(null);
    }


    self.delete = function (data) {
        var url = '/Reservations/Delete';
        var reservation = JSON.stringify({
            reservationId: data.Id
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: reservation,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.save = function () {
        var url = '/Reservations/Save';
        var reservation = JSON.stringify({
            Id: self.Id(),
            RestaurantId: self.RestaurantId(),
            ClientName: self.ClientName(),
            ReservationDate: self.ReservationDate(),
            Seats: self.Seats()
        });
        var message = "";
        if (self.ClientName._latestValue == null) {
            message += "Please Enter the Client's Name!\n";
        }
        if (self.Seats._latestValue <= 0) {
            message += "Please Enter the Number of Seats!\n";
        }
        //if (message.length) {
            window.alert(message);
       // }
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: reservation,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    }

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
                self.Restaurants(data.Restaurants);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
}