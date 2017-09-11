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
    self.Message = ko.observable();
    self.details = function (data) {
        self.RestaurantId(data.id);
        self.Id(null);
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
                self.UserId(data.UserId);
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
        if (currentOption == '') {

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
  
   self.save= function (data) {
     
        self.details(data);
       
        var url = '/UserFavorites/Save';
        self.loadingPanel.show();
        var favorite = JSON.stringify({
            Id:self.Id,
            RestaurantId: self.RestaurantId(),
            ClientId: self.UserId(),
        });

        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: favorite, 
            success: function (data) {
                console.log(data);
                self.loadingPanel.hide();
                self.Message(data);
                $("#succes").modal('show');
             
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

}
