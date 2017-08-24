function Meals() {
    var self = this;

    //for list
    self.Meals = ko.observableArray();
    self.Types = ko.observableArray();
    self.Ratings = ko.observableArray();

    //for item
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Description = ko.observable();
    self.Type = ko.observable();
    self.Rating = ko.observable();

    //for search
    self.SearchName = ko.observable(false);
    self.SearchIngredient = ko.observable(false);

    self.loadingPanel = new LoadingOverlay();

    self.details = function (data) {
        self.Id(data.Id);
        self.Name(data.Details);
        self.Description(data.Description);
        self.Type(data.Type);
        self.Rating(data.Rating);
    };
    self.add = function () {
        self.Id(0);
        self.Type(null);
        self.Name(null);
        self.Description(null);
        self.Rating(null);
    };
    self.save = function () {
        var url = '/Meals/Save';
        var meal = JSON.stringify({
            Id: self.Id(),
            Name: self.Name(),
            Description: self.Description(),
            Type: self.Type(),
            Rating: self.Rating(),
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: meal,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
    self.delete = function (data) {
        var url = '/Meals/Delete';
        var meal = JSON.stringify({
            mealId: data.Id
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: meal,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
    self.refresh = function () {
        var url = '/Meals/ListRefresh';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Meals(data.Meals);
                self.Types(data.Types);
                self.Ratings(data.Ratings);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.search = function () {
        alert("Searching..."); 
    };
}