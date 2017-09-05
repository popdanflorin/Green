function UserRestaurants() {
    var self = this;
    self.UserRestaurants = ko.observableArray();
    self.Types = ko.observableArray();

    self.loadingPanel = new LoadingOverlay();
    self.TotalRating = ko.observable();
    self.RestaurantName = ko.observable();
    self.RestaurantType = ko.observable();
    self.RestaurantId = ko.observable();
    self.showWarningInputEmpty = ko.observable();

    self.getRatings = function (data) {
        var url = '/Restaurants/GetRatings';
        $.ajax(url, {
            async: false,
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: { restaurantId: data.id },
            success: function (data) {
                console.log(data);
                self.TotalRating(data.TotalRating);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        $(".rateit").rateit('value', TotalRating);
    };
   
    self.refresh = function () {
        var url = '/Restaurants/UserRestaurantsRefresh';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.UserRestaurants(data.UserRestaurants);
                self.Types(data.Types);

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
    self.search = function () {
        var isValid = true;
        if (!$.trim($('#Name').val())) {
            self.showWarningInputEmpty("Please insert a restaurant name!");
            isValid = false;
        }
        else {
            self.showWarningInputEmpty("lalala");

        }
        if (isValid == true) {
            var url = '/Restaurants/UserRestaurantsSearch';
            self.loadingPanel.show();
            $.ajax(url, {
                type: "get",
                contentType: "application/json; charset=utf-8",
                data: { restaurantName: self.RestaurantName },
                success: function (data) {
                    self.loadingPanel.hide();
                    console.log(data);
                    self.UserRestaurants(data.UserRestaurants);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(textStatus + ': ' + errorThrown);
                }
            });
        }
    };
}
