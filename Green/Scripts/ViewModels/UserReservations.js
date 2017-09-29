function UserReservations(vmRatings) {
    var self = this;

    self.Reservations = ko.observableArray();
    self.Restaurants = ko.observableArray();
    self.Id = ko.observable();
    self.RestaurantId = ko.observable();
    self.ClientId = ko.observable();
    self.ReservationDate = ko.observable(new Date())
    self.Seats = ko.observable();
    self.UserName = ko.observable();
    self.UserId = ko.observable();
    self.loadingPanel = new LoadingOverlay();

    // validation warnings
    self.warningRestaurantId = ko.observable();
    self.warningReservationDate = ko.observable();
    self.warningSeats = ko.observable();

    // SignalR
    self.Message = ko.observable();

    self.initSignalR = function () {
        $.connection.hub.start()
            .done(function () {
                console.log("SignalR initialization success!");
            })
            .fail(function () {
                console.log("SignalR initialization error!");
            })
    }

    $.connection.dataHub.client.notifyNewReservation = function (restaurantName) {
        console.log("SignalR notifyNewReservation");
        self.Message("A new reservation has been made for " + restaurantName + "!");
        self.showSnackbar();
    };

    self.showSnackbar = function () {
        var x = document.getElementById("UserReservationsSnackbar")
        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
    };
    // end SignalR related functions

    self.details = function (data) {
        self.Id(data.Id);
        self.RestaurantId(data.RestaurantId);
        self.ClientId(data.ClientId);
        self.ReservationDate(data.ReservationDate);
        self.Seats(data.Seats);
        self.UserName(data.User.UserName);

        self.warningRestaurantId(null);
        self.warningReservationDate(null);
        self.warningSeats(null);

        vmRatings.changeRestaurantId(self.RestaurantId());
    };

    self.getMenu = function (data) {

    }

    self.add = function () {
        self.Id(0);
        self.RestaurantId(null);
        self.ReservationDate(null);
        self.Seats(null);
        self.warningRestaurantId(null);
        self.warningReservationDate(null);
        self.warningSeats(null);
    };

    self.delete = function (data) {
        if (!window.confirm("Are you sure you want to cancel this reservation?")) {
            return;
        }

        var url = '/Reservations/Delete';
        var restaurantId = data.RestaurantId;
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
                $.connection.dataHub.server.notifyReservationChange(restaurantId, "A reservation has been canceled at ");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.save = function () {
        if (!self.validate()) {
            self.setOKButton(false);
            return;
        }

        var isNew = self.Id() == 0 || self.Id() == null;

        var url = '/Reservations/Save';
        self.warningRestaurantId(null);
        self.warningReservationDate(null);
        self.warningSeats(null);
        if (self.ClientId()) {
            var reservation = JSON.stringify({
                Id: self.Id(),
                RestaurantId: self.RestaurantId(),
                ClientId: self.ClientId(),
                ReservationDate: self.ReservationDate(),
                Seats: self.Seats()
            });
        }
        else {
            var reservation = JSON.stringify({
                Id: self.Id(),
                RestaurantId: self.RestaurantId(),
                ClientId: self.UserId(),
                ReservationDate: self.ReservationDate(),
                Seats: self.Seats()
            });
        }
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: reservation,
            success: function (data) {
                if (data.search(new String("The selected time is not available.").valueOf()) >= 0) {
                    try {
                        self.warningReservationDate(data);
                        self.setOKButton(false);
                    } catch (Exception) {
                        console.log(Exception);
                    }
                }
                else if (data.search(new String("There are not enough seats available.").valueOf()) >= 0) {
                    try {
                        self.warningSeats(data);
                        self.setOKButton(false);
                    } catch (Exception) {
                        console.log(Exception);
                    }
                }
                else {
                    try {
                        self.setOKButton(true);
                        console.log(data);
                        self.refresh();
                        if (isNew)
                            $.connection.dataHub.server.notifyNewReservation(self.RestaurantId());
                        else
                            $.connection.dataHub.server.notifyReservationChange(self.RestaurantId(), "Details have been changed for a reservation at ");
                    } catch (Exception) {
                        console.log(Exception);
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.setOKButton = function (value) {
        try {
            if (value) {
                var attrDataDismiss = document.createAttribute("data-dismiss");
                attrDataDismiss.value = "modal";
                document.getElementById("OKButton").attributes.setNamedItem(attrDataDismiss);
            }
            else
                document.getElementById("OKButton").removeAttribute("data-dismiss");
        }
        catch (Exception) {
            console.log(Exception);
        }
    }

    self.refresh = function () {
        var url = '/Reservations/ListRefreshUser';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Reservations(data.Reservations);
                self.Restaurants(data.Restaurants);
                self.UserId(data.UserId);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.isComplete = function (data) {
        var date = new Date(moment(data).format("YYYY-MM-DDTHH:mm:ss"));
        var today = new Date();
        console.log(date < today);
        return date < today;
    };

    self.validate = function () {
        var valid = true;
        if (self.nullOrEmpty(self.RestaurantId())) {
            self.warningRestaurantId("Please select a restaurant!");
            valid = false;
        }
        else {
            self.warningRestaurantId(null);
        }

        //if (self.nullOrEmpty(self.ReservationDate()) || self.ReservationDate() == "Invalid date" || self.ReservationDate()[0] == "/") {
        if (self.nullOrEmpty(self.ReservationDate())) {
            self.warningReservationDate("Please select the date!");
            valid = false;
        }
        else {
            self.warningReservationDate(null);
        }

        if (self.nullOrEmpty(self.Seats()) || self.Seats() <= 0) {
            self.warningSeats("Please select the number of seats!");
            valid = false;
        }
        else {
            self.warningSeats(null);
        }

        return valid;
    };

    self.nullOrEmpty = function (data) {
        if (data == null || data == "") {
            return true;
        }
        return false;
    };

    self.restaurantInfo = function () {
        var url = '/Reservations/RestaurantInfo';
        var restaurant = JSON.stringify({ restaurantId: self.RestaurantId() });
        var openingHour, closingHour;
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data) {
                openingHour = data.OpeningHour,
                closingHour = data.ClosingHour
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        var content = "Opening Hour: " + openingHour + "<br/>Closing Hour: " + closingHour;
        $("#RestaurantInfo").attr('data-content', content);
    }
};