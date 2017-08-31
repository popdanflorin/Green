function Menus() {
    var self = this;

    self.Menus = ko.observableArray();
    self.Restaurants = ko.observableArray();

    self.RestaurantId = ko.observable();
    self.RestaurantName = ko.observable();
    self.StartDate = ko.observable();
    self.EndDate = ko.observable();
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
       // alert("Menu Refresh");
        self.loadingPanel.hide();

    };

    self.details = function (data) {
        self.RestaurantId(data.id);
        self.RestaurantName(data.Name);
    };

    self.save = function () {

    };

    self.delete = function () {

    };
};