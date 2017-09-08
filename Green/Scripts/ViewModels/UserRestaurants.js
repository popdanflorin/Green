function UserRestaurants() {
    var self = this;
    self.UserRestaurants = ko.observableArray();
    self.Types = ko.observableArray();

    self.loadingPanel = new LoadingOverlay();
    self.RestaurantName = ko.observable();
    self.RestaurantType = ko.observable();
    self.RestaurantId = ko.observable();

    self.UserId = ko.observable();
    self.Id = ko.observable();

  

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
            isValid = false;
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
    self.searchByType = function () {
        var isValid = true;
        var currentOption = $('#selectMenu').val();
        if (currentOption =='') {
  
            isValid = false;
        }
        if (isValid == true) {
            var url = '/Restaurants/UserRestaurantsSearchByType';
            self.loadingPanel.show();
            $.ajax(url, {
                type: "get",
                contentType: "application/json; charset=utf-8",
                data: { restaurantType: self.RestaurantType },
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
        else {
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
        }

    };
    self.getUserId = function () {
        var url = '/UserFavorites/GetUserId';
        self.loadingPanel.show();

        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.UserId(data.UserId);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
  
}
