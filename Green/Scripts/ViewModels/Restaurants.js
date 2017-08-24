function Restaurants() {
    var self = this;
    self.Restaurants = ko.observableArray();
    self.Types = ko.observableArray();
    self.id = ko.observable();
    self.Name = ko.observable();
    self.Type = ko.observable();
    self.Address = ko.observable();
    self.MaxPrice = ko.observable();
    self.OpeningHour = ko.observable();
    self.ClosingHour = ko.observable();
    self.SeatsAvailable = ko.observable();
    self.loadingPanel = new LoadingOverlay();
    self.messageText = ko.observable();

    self.showWarningNameEmpty = ko.observable();
    self.showWarningAddressEmpty = ko.observable();
    self.showWarningTypeEmpty = ko.observable();
    self.showWarningMaxPriceEmpty = ko.observable();
    self.showWarningSeatsEmpty = ko.observable();
    self.showWarningOpeningHourEmpty = ko.observable();
    self.showWarningClosingHourEmpty = ko.observable();
    
    self.details = function (data) {
        self.id(data.id);
        self.Name(data.Name);
        self.Type(data.Type);
        self.Address(data.Address);
        self.MaxPrice(data.MaxPrice);
        self.SeatsAvailable(data.SeatsAvailable);
        self.OpeningHour(data.OpeningHour);
        self.ClosingHour(data.ClosingHour);
        self.showWarningNameEmpty("");
        self.showWarningAddressEmpty("");
        self.showWarningMaxPriceEmpty("");
        self.showWarningTypeEmpty("");
        self.showWarningSeatsEmpty("");
        self.showWarningOpeningHourEmpty("");
        self.showWarningClosingHourEmpty("");
    };
    self.manage = function (data) {

    }
    self.add = function () {
        self.showWarningNameEmpty("");
        self.showWarningAddressEmpty("");
        self.showWarningTypeEmpty("");
        self.showWarningMaxPriceEmpty("");
        self.showWarningSeatsEmpty("");
        self.id(0);
        self.Name("");
        self.Type(null);
        self.Address("");
        self.MaxPrice(0);
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
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
    self.save = function () {
        var isValid = true;
        if (!$.trim($('#Name').val())) {
            self.showWarningNameEmpty("Please insert a name!");
            isValid = false;
        }
        else {
            self.showWarningNameEmpty("");
            isValid = true;
        }
        if (!$.trim($('#Address').val())) {
           self.showWarningAddressEmpty("Please insert an address!");
            isValid = false;
        }
        else {
            self.showWarningAddressEmpty("");
            isValid = true;
        }
        if (!$('#RestaurantTypes').val()) {
            self.showWarningTypeEmpty("Please select a type!");  
            isValid = false;
        }
        else {
            self.showWarningTypeEmpty("");
            isValid = true;
        }
        if (isNaN($('#MaxPrice'))|| $('#MaxPrice')==0) {
            self.showWarningMaxPriceEmpty("Please insert the max price!");       
            isValid = false;
        }
        else {
            self.showWarningMaxPriceEmpty("");
            isValid = true;
        }
        if (isNaN($('#Seats')) || $('#Seats') == 0) {
            self.showWarningSeatsEmpty("Please insert the number of seats!");
            isValid = false;
        }
        else {
            self.showWarningSeatsEmpty("");
            isValid = true;
        }
        if (isNaN($('#OpeningHour')) || $('#OpeningHour') == 0) {
            self.showWarningOpeningHourEmpty("Please insert the opening hour!");
            isValid = false;
        }
        else {
            self.showWarningOpeningHourEmpty("");
            isValid = true;
        }
        if (isNaN($('#ClosingHour')) || $('#ClosingHour') == 0) {
            self.showWarningClosingHourEmpty("Please insert the closing hour!");
            isValid = false;
        }
        else {
            self.showWarningClosingHourEmpty("");
            isValid = true;
        }
        if (isValid == true) {
            var url = '/Restaurants/Save';
            var restaurant = JSON.stringify({
                id: self.id(),
                Name: self.Name(),
                Address: self.Address(),
                Type: self.Type(),
                MaxPrice: self.MaxPrice(),
                SeatsAvailable: self.SeatsAvailable(),
                OpeningHour: self.OpeningHour(),
                ClosingHour:self.ClosingHour()
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
        }
       


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
    
}