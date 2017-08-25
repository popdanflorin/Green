function Reservations() {
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
    self.isAdmin = ko.observable(false);
    self.loadingPanel = new LoadingOverlay();

    // validation warnings
    self.warningRestaurantId = ko.observable();
    self.warningReservationDate = ko.observable();
    self.warningSeats = ko.observable();

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
    };

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
        if (!window.confirm("Are you sure you want to cancel the reservation?")) {
            return;
        }

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
        if (!self.validate()) {
            self.setOKButton(false);
            return;
        }

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

    self.setOKButton = function(value) {
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
                self.isAdmin(data.isAdmin);
                self.UserId(data.UserId);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.validate = function () {
        var valid = true;
        if (self.nullOrEmpty(self.RestaurantId._latestValue)) {
            self.warningRestaurantId("Please select a restaurant!");
            valid = false;
        }
        else {
            self.warningRestaurantId(null);
        }

        if (self.nullOrEmpty(self.ReservationDate._latestValue) || self.ReservationDate._latestValue == "Invalid date" || self.ReservationDate._latestValue[0] == "/") {
            self.warningReservationDate("Please select the date!");
            valid = false;
        }
        else {
            self.warningReservationDate(null);
        }

        if (self.nullOrEmpty(self.Seats._latestValue) || self.Seats._latestValue <= 0) {
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
        var restaurant = JSON.stringify({ restaurantId: self.RestaurantId()});
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

    //hides the element for non-admin users
    ko.bindingHandlers.allowAccess = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            try {
                var value = ko.unwrap(valueAccessor());
            }
            catch (e) {
                var value = valueAccessor();
            }
            // admin user
            if (value) {
                element.hidden = false;
            }
                // normal user
            else {
                element.hidden = true;
            }
        }
    };
};