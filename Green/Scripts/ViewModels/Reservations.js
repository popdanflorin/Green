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

    // validation warnings
    self.warningRestaurantId = ko.observable();
    self.warningClientName = ko.observable();
    self.warningReservationDate = ko.observable();
    self.warningSeats = ko.observable();

    self.details = function (data) {
        self.Id(data.Id);
        self.RestaurantId(data.RestaurantId);
        self.ClientName(data.ClientName);
        self.ReservationDate(data.ReservationDate);
        self.Seats(data.Seats);
        self.warningRestaurantId(null);
        self.warningClientName(null);
         self.warningReservationDate(null);
        self.warningSeats(null);
    };

    self.add = function () {
        self.Id(0);
        self.RestaurantId(null);
        self.ClientName(null);
        self.ReservationDate(null);
        self.Seats(null);
        self.warningRestaurantId(null);
        self.warningClientName(null);
        self.warningReservationDate(null);
        self.warningSeats(null);
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
        var attrDataDismiss = document.createAttribute("data-dismiss");
        attrDataDismiss.value = "modal";
        if (!self.validate()) {
            try {
                document.getElementById("OKButton").attributes.removeNamedItem("data-dismiss");
            } catch (Exception) {
            }
            return;
        }
        try {
            document.getElementById("OKButton").attributes.setNamedItem(attrDataDismiss);
        } catch (Exception) {
        }

        var url = '/Reservations/Save';
        self.warningRestaurantId(null);
        self.warningClientName(null);
        self.warningReservationDate(null);
        self.warningSeats(null);
        var reservation = JSON.stringify({
            Id: self.Id(),
            RestaurantId: self.RestaurantId(),
            ClientName: self.ClientName(),
            ReservationDate: self.ReservationDate(),
            Seats: self.Seats()
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

    self.validate = function () {
        var valid = true;
        if (self.nullOrEmpty(self.RestaurantId._latestValue)) {
            self.warningRestaurantId("Please select a restaurant!\n");
            valid = false;
        }
        else {
            self.warningRestaurantId(null);
        }

        if (self.nullOrEmpty(self.ClientName._latestValue)) {
            self.warningClientName("Please enter your name!\n");
            valid = false;
        }
        else {
            self.warningClientName(null);
        }

        if (self.nullOrEmpty(self.ReservationDate._latestValue) || self.ReservationDate._latestValue == "Invalid date" || self.ReservationDate._latestValue[0] == "/") {
            self.warningReservationDate("Please select the date!\n");
            valid = false;
        }
        else {
            self.warningReservationDate(null);
        }

        if (self.nullOrEmpty(self.Seats._latestValue) || self.Seats._latestValue <= 0) {
            self.warningSeats("Please select the number of seats!\n");
            valid = false;
        }
        else {
            self.warningSeats(null);
        }

        return valid;
    }

    self.nullOrEmpty = function (data) {
        if (data == null || data == "") {
            return true;
        }
        return false;
    }
}