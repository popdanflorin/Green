function Restaurants() {
    var self = this;
    self.Restaurants = ko.observableArray();
    self.Types = ko.observableArray();
    self.Images = ko.observableArray();
    self.id = ko.observable();
    self.Name = ko.observable();
    self.Type = ko.observable();
    self.ImageName = ko.observable();
    self.Address = ko.observable();
    self.City = ko.observable();
    self.OpeningHour = ko.observable();
    self.ClosingHour = ko.observable();
    self.SeatsAvailable = ko.observable();
    self.loadingPanel = new LoadingOverlay();
    self.messageText = ko.observable();

    //for Image SlideShow
    self.RestaurantImages = ko.observableArray();

    // for Menu
    self.Restaurant = ko.computed(function () {
        return { Id: self.id, Name: self.Name };
    });

    // validation warnings
    self.showWarningNameEmpty = ko.observable();
    self.showWarningAddressEmpty = ko.observable();
    self.showWarningCityEmpty = ko.observable();
    self.showWarningTypeEmpty = ko.observable();
    self.showWarningSeatsEmpty = ko.observable();
    self.showWarningOpeningHourEmpty = ko.observable();
    self.showWarningClosingHourEmpty = ko.observable();

    self.details = function (data) {
        self.id(data.id);
        self.Name(data.Name);
        self.Type(data.Type);
        self.Address(data.Address);
        self.City(data.City);
        self.SeatsAvailable(data.SeatsAvailable);
        self.OpeningHour(data.OpeningHour);
        self.ClosingHour(data.ClosingHour);
        self.showWarningNameEmpty("");
        self.showWarningAddressEmpty("");
        self.showWarningCityEmpty("");
        self.showWarningTypeEmpty("");
        self.showWarningSeatsEmpty("");
        self.showWarningOpeningHourEmpty("");
        self.showWarningClosingHourEmpty("");
    };

    self.openShowImage = function (data) {
        var url = '/Restaurants/GetRestaurantImages';
        var restaurant = JSON.stringify({
            restaurantId: data.id
        });
        $.ajax(url, {
            type: "post",
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data2) {
                self.RestaurantImages(data2.Images);
                console.log(data2);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    }
    self.add = function () {
        self.showWarningNameEmpty("");
        self.showWarningAddressEmpty("");
        self.showWarningCityEmpty("");
        self.showWarningTypeEmpty("");
        self.showWarningSeatsEmpty("");
        self.id(0);
        self.Name("");
        self.Type(null);
        self.Address("");
        self.City("");
        self.SeatsAvailable(0);
        self.OpeningHour(0);
        self.ClosingHour(0);
    };

    self.refresh = function () {
        var url = '/Restaurants/ListRefresh';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Restaurants(data.Restaurants);
                self.Types(data.RestaurantTypes);
                self.Images(data.Images);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
    self.save = function () {
        if (!self.validate())
            return;

        var url = '/Restaurants/Save';
        var restaurant = JSON.stringify({
            id: self.id(),
            Name: self.Name(),
            Address: self.Address(),
            City: self.City(),
            Type: self.Type(),
            SeatsAvailable: self.SeatsAvailable(),
            OpeningHour: self.OpeningHour(),
            ClosingHour: self.ClosingHour()
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data) {
                console.log(data);
                self.messageText(data.message);
                self.refresh();
                $('#restaurantItem').modal('hide');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.deleteRestaurant = function (data) {
        var url = '/Restaurants/Delete';
        var restaurant = JSON.stringify({
            restaurantId: self.id()
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });


    };
    self.openDeleteDialog = function (data) {
        self.id(data.id);
    };

    self.getRestaurant = function (data) {
        self.id(data.id);
        self.Name(data.Name);
    };

    // Images
    self.setCoverImage = function () {
        var i, iamgeId, isCover = false;
        var checkboxes = document.getElementsByClassName("checkbox");
        for (i = 0; i < checkboxes.length && !isCover; ++i) {
            imageId = checkboxes[i].value;
            isCover = checkboxes[i].checked;
        }

        var url = '/Restaurants/SetCoverImage';
        var ids = JSON.stringify({
            restaurantId: self.id(),
            imageId: imageId
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: ids,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        click: $('#imageItem').modal('hide');
    };

    self.disableOthers = function (data) {
        $("#ApplyCoverChangeButton").prop('disabled', false);
        var imageId = data.Id;
        var isCover = data.isCover;
        var checkboxes = document.getElementsByClassName("checkbox");
        for (i = 0; i < checkboxes.length; ++i)
            if (checkboxes[i].value != imageId) {
                checkboxes[i].checked = false;
            }
        return true;
    }

    self.deleteImage = function (data) {
        if (!window.confirm("Are you sure you want to delete the image?")) {
            return;
        }

        var imageId = data.Id;
        var restaurantId = data.RestaurantId;

        var url = '/Restaurants/DeleteImage';
        var image = JSON.stringify({
            imageId: imageId
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: image,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        $("#imageItem").modal('hide');
        //self.openShowImage({ id: restaurantId });
    };

    self.validate = function () {
        var isValid = true;

        if (!$.trim($('#Name').val())) {
            self.showWarningNameEmpty("Please insert a name!");
            isValid = false;
        }
        else {
            self.showWarningNameEmpty("");
        }

        if (!$.trim($('#Address').val())) {
            self.showWarningAddressEmpty("Please insert an address!");
            isValid = false;
        }
        else {
            self.showWarningAddressEmpty("");
        }

        if (!$.trim($('#City').val())) {
            self.showWarningCityEmpty("Please insert a city!");
            isValid = false;
        }
        else {
            self.showWarningCityEmpty("");
        }

        if (!$('#RestaurantTypes').val()) {
            self.showWarningTypeEmpty("Please select a type!");
            isValid = false;
        }
        else {
            self.showWarningTypeEmpty("");
        }

        var seats = $('#Seats').val();
        if (isNaN(seats) || seats == 0) {
            self.showWarningSeatsEmpty("Please insert the number of seats!");
            isValid = false;
        }
        else {
            self.showWarningSeatsEmpty("");
        }

        var openingHour = $('#OpeningHour').val();
        var closingHour = $('#ClosingHour').val();
        if (isNaN(openingHour) || openingHour > 24 || openingHour < 0) {
            self.showWarningOpeningHourEmpty("Please insert the opening hour!");
            isValid = false;
        }
        else {
            self.showWarningOpeningHourEmpty("");

        }

        if (isNaN(closingHour) || closingHour > 24 || closingHour < 0) {
            self.showWarningClosingHourEmpty("Please insert the closing hour!");
            isValid = false;
        }
        else {
            self.showWarningClosingHourEmpty("");
        }

        if (closingHour < openingHour) {
            self.showWarningClosingHourEmpty("Please review the opening and closing hours!");
            isValid = false;
        }
        else {
            self.showWarningClosingHourEmpty("");
        }

        return isValid;
    };
}