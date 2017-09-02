function Menus() {
    var self = this;

    self.loadingPanel = new LoadingOverlay();
    self.Meals = ko.observableArray();
    self.MealTypes = ko.observableArray();
    self.Test = ko.observableArray();

    self.Id = ko.observable();
    self.RestaurantId = ko.observable();
    self.RestaurantName = ko.observable();
    self.StartDate = ko.observable();
    self.EndDate = ko.observable();
    self.RestaurantMeals = ko.observable();
    self.SelectedMealOd = ko.observable();

    self.RestaurantNameDisplay = ko.computed(function () {
        if (!self.RestaurantName())
            return "Error";
        return self.RestaurantName() + "'s Menu";
    })

    self.StartDateDisplay = ko.computed(function () {
        if (!self.StartDate())
            return moment(Date(), "MM/DD/YYYY", false).format();
        return moment(self.StartDate(), "MM/DD/YYYY", false).format();
    })

    // validation warning
    self.warningStartDate = ko.observable();
    self.warningEndDate = ko.observable();

    self.refresh = function () {
        var url = '/Menus/ListRefresh';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Meals(data.Meals);
                self.MealTypes(data.MealTypes);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.details = function (data) {
        self.RestaurantId(data.id);
        self.RestaurantName(data.Name);

        self.getMenu();

        self.warningStartDate(null);
        self.warningEndDate(null);
    };

    self.getMenu = function () {
        var url = '/Menus/GetMenu';

        var restaurant = JSON.stringify({
            RestaurantId: self.RestaurantId(),
        });
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data) {
                console.log(data);
                self.Id(data.Id);
                self.StartDate(new Date(data.StartDate));
                self.EndDate(new Date(data.EndDate));
                self.RestaurantMeals(data.RestaurantMeals);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

    };

    self.save = function () {
        if (!self.validate())
            return;

        var url = '/Menus/Save';
        var menu = JSON.stringify({
            Id: self.Id(),
            RestaurantId: self.RestaurantId(),
            StartDate: self.StartDate(),
            EndDate: self.EndDate(),
            meals: self.RestaurantMeals()
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: menu,
            success: function (data) {
                console.log(data);
                self.refresh();
                $("#menuItem").modal("hide");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.delete = function () {

    };

    self.addMeal = function (data) {

    };

    self.deleteMeal = function (data) {

    };

    self.validate = function () {
        var valid = true;

        if (self.nullOrEmpty(self.StartDate())) {
            self.warningStartDate("Please select the start date!");
            valid = false;
        }
        else {
            self.warningStartDate(null);
        }

        if (self.nullOrEmpty(self.EndDate())) {
            self.warningEndDate("Please select the end date!");
            valid = false;
        }
        else {
            self.warningEndDate(null);
        }

        return valid;
    };

    self.nullOrEmpty = function (data) {
        if (data == null || data == "") {
            return true;
        }
        return false;
    };
};