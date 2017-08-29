function UserRestaurants() {
    var self = this;
    self.UserRestaurants = ko.observableArray();
    self.loadingPanel = new LoadingOverlay();
    self.RatingValue = ko.observable();

    self.getRating = function (data) {
        var tooltipvalues = ['Bad', 'Decent', 'Good', 'Very good', 'Excellent'];
        var rows = self.UserRestaurants().length;
        for (var i = 0; i < rows; i++) {
            if (self.UserRestaurants()[i].RestaurantId == data.id) {
                self.RatingValue(self.UserRestaurants()[i].RatingValue);
                break;
            }
        }
        $(".rateit").bind('over', function (event, value) { $(this).attr('title', tooltipvalues[value - 1]); });
        $(".rateit").bind('rated', self.RatingValue);
    }

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

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
}
