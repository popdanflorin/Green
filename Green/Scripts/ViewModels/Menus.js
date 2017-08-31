function Menus() {
    var self = this;

    self.Menus = ko.observableArray();
    self.Meals = ko.observableArray();

    self.RestaurantId = ko.observable();
    self.RestaurantName = ko.observable();
    self.StartDate = ko.observable();
    self.EndDate = ko.observable();
    self.RestaurantMeals = ko.observable();
    self.loadingPanel = new LoadingOverlay();

    self.RestaurantNameDisplay = ko.computed(function () {
        if (!self.RestaurantName())
            return "Error";
        return self.RestaurantName() + "'s Menu";
    })

    self.StartDateDisplay = ko.computed(function () {
        if (!self.StartDate())
            return Date();
        return self.StartDate();
    })

    // validation warning
    self.warningStartDate = ko.observable();
    self.warningEndDate = ko.observable();

    self.refresh = function () {
        self.loadingPanel.show();
        self.loadingPanel.hide();

    };

    self.details = function (data) {
        self.RestaurantId(data.id);
        self.RestaurantName(data.Name);

        // get meals
    };

    self.save = function () {

    };

    self.delete = function () {

    };

    self.validate = function () {

    };
};