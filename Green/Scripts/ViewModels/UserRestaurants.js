/// <reference path="../agency.min.js" />
function UserRestaurants() {
    var self = this;
    self.loadingPanel = new LoadingOverlay();

    self.UserRestaurants = ko.observableArray();
    self.UserFavorites = ko.observableArray();
    self.Types = ko.observableArray();
    self.MealTypes = ko.observableArray(null);
    self.Menu = ko.observable(null);
    self.Meals = ko.observableArray(null);
  
  
    self.RestaurantName = ko.observable();
    self.RestaurantType = ko.observable();
    self.RestaurantId = ko.observable();
    self.FavoriteRestaurantId = ko.observable();
    self.Rating = ko.observable();
    self.MealName = ko.observable();
    self.CityName = ko.observable();
    self.UserId = ko.observable();
    self.Id = ko.observable();
    self.MenuId = ko.observable();
    self.StartDate = ko.observable(null);
    self.EndDate = ko.observable(null);
    self.InitialStartDate = new Date();
    self.InitialEndDate = new Date();
    self.ActualDate = ko.observable(Date());
    self.Message = ko.observable();
    self.NoFavoritesMessage = ko.observable();

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
        var x = document.getElementById("UserRestaurantsSnackbar");
        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
    };
    // end SignalR related functions

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
                //  self.Rating(data.Rating);
                //  $('#ratingRestaurant').rateit('value', self.Rating());
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        $('#favorites').popover({
            html: true,
           // title: 'Warning<a class="close" href="#">&times;</a>',
            title: 'Warning',
            content: 'Your favorites list is empty!'
        });
    };


    self.search = function () {

        var url = '/Restaurants/UserRestaurantsSearchByName';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            data: {
                restaurantName: self.RestaurantName
            },
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
    self.searchByAll = function () {
        var currentOption = $('#dropdownTypes').val().replace(/\s+/g, '');
        if (currentOption == 'Choosetype')
            var type = 'None';
        else
            var type = self.RestaurantType;
        var url = '/Restaurants/UserRestaurantsSearch';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            data: {
                restaurantName: self.RestaurantName,
                mealName: self.MealName,
                restaurantType: type,
                cityName: self.CityName
            },
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
    self.getFavorites = function () {
        var url = '/UserFavorites/UserFavoritesGet';
        self.loadingPanel.show();

        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            data: { UserId: self.UserId },
            success: function (data) {
                async: false,
                console.log(data);
                self.UserFavorites(data.UserFavorites);
                if (data.UserFavorites.length == 0) {
                    $(this).closest('.modal').modal('hide');
                    $('#favoritesItem').modal('hide');

                 //   $('#favorites').popover('toggle');
                }
                else {
                    $('#favoritesItem').modal('show');
                }
                self.loadingPanel.hide();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.save = function (data) {

        self.details(data);

        var url = '/UserFavorites/Save';
        self.loadingPanel.show();
        var favorite = JSON.stringify({
            Id: self.Id,
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
    self.getFavoriteId = function (data) {
        var id = data.id;
        self.FavoriteRestaurantId(id);
    }
    self.deleteFavorite = function () {

        var url = '/UserFavorites/Delete';
        var restaurant = JSON.stringify({
            restaurantId: self.FavoriteRestaurantId()
        });
        var deleteSucces = false;
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data) {
                console.log(data);
                deleteSucces = true;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        if (deleteSucces)
            self.getFavorites();
    }
    self.showRating = function (data) {
        self.Rating(data.Rating);
        var nameRestaurant = data.Name.replace(/\s+/g, '');
        var rate = $('#' + data.id).rateit();
        rate.rateit('value', self.Rating());
        rate.rateit('readonly', true);
        rate.rateit('background', url(star - gold32.png));

        rate.rateit('resetable', false);
        // $(".rateit").rateit('min', self.Rating());
        //  $(".rateit").bind('rated', function () { $(this).rateit(self.Rating) });
    };


    /*Menu Display*/

    self.StartDateDisplay = ko.computed(function () {
        if (!self.StartDate())
            return null;
        return moment(self.StartDate()).format('DD MMM YYYY');
    });

    self.EndDateDisplay = ko.computed(function () {
        if (!self.EndDate())
            return null;
        return moment(self.EndDate()).format('DD MMM YYYY');
    });

    self.MenuTitleDisplay = ko.computed(function () {
        return self.RestaurantName() + "'s Menu for " + self.StartDateDisplay() + " - " + self.EndDateDisplay();
    });

    self.MealId = ko.observable();
    self.SearchText = ko.observable();


    // functions
    self.getDetails = function (data) {
        self.RestaurantId(data.id);
        self.RestaurantName(data.Name);
        self.getMenu();
    };
    self.getMenu = function () {
        var url = '/Restaurants/GetCurrentMenu';
        var restaurant = JSON.stringify({
            restaurantId: self.RestaurantId(),
        });
        self.loadingPanel.show();
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.refreshMenu(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        if (self.MenuId() == null)
            self.MenuId(0);
    };
    self.DisplayedMeals = ko.computed(function () {
        if (self.Meals() != null)
            return self.Meals();
        return null;
    });


    self.deleteModal = function () {
        var data = { Id: self.Id() };
        if (self.delete(data))
            $('#menuForUser').modal('hide');
    };

    self.refreshMeals = function () {
        var url = '/Menus/GetMealsForMenu';
        var menu = JSON.stringify({
            menuId: self.MenuId(),
        });
        self.loadingPanel.show();
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: menu,
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Meals(data.Meals);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.refreshMenu = function (data) {
        self.MenuId(data.Id);
        self.StartDate(data.StartDate);
        self.EndDate(data.EndDate);
        self.refreshMeals();
    };
    self.isActive = function (data) {
        var today = new Date();
        today.setHours(0);
        today.setMinutes(0);
        today.setSeconds(0);
        today.setMilliseconds(0);
        var startDate = new Date(data.StartDateDisplay);
        var endDate = new Date(data.EndDateDisplay);
        return startDate <= today && today <= endDate;
    };

    self.isExpired = function (data) {
        var today = new Date();
        today.setHours(0);
        today.setMinutes(0);
        today.setSeconds(0);
        today.setMilliseconds(0);
        var date = new Date(data.EndDateDisplay);
        return date < today;
    };
    self.nullOrEmpty = function (data) {
        if (data == null || data == "") {
            return true;
        }
        return false;
    };
}
 