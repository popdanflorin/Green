function Restaurants() {
    var self = this;
    self.Restaurants = ko.observableArray();
    self.Types = ko.observableArray();
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Type = ko.observable();
    self.Address = ko.observable();
    self.MaxPrice = ko.observable();
    self.loadingPanel = new LoadingOverlay();

    self.add = function () {
        self.Id(0);
        self.Name("");
        self.Type(null);
        self.Address("");
        self.MaxPrice(0);
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
                self.Types(data.RestaurantsTypes);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
    self.save = function () {
        
    };
}