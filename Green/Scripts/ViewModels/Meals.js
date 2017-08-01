function Meals() {
    var self = this;

    //for list
    self.Meals = ko.observableArray();
    self.Types = ko.observableArray();
    self.Statuses = ko.observableArray();
    self.Ratings = ko.observableArray();

    //for item
    self.Id = ko.observable();
    self.UserId = ko.observable();
    self.Details = ko.observable();
    self.PreparationDetails = ko.observable();
    self.PlannedTime = ko.observable();
    self.ActualTime = ko.observable();
    self.Type = ko.observable();
    self.Status = ko.observable();
    self.Rating = ko.observable();

    self.loadingPanel = new LoadingOverlay();

    self.details = function (data) {
        self.Id(data.Id);
        self.UserId(data.UserId);
        self.Details(data.Details);
        self.PreparationDetails(data.PreparationDetails);
        self.PlannedTime(data.PlannedTime);
        self.ActualTime(data.ActualTime);
        self.Type(data.Type);
        self.Status(data.Status);
        self.Rating(data.Rating);
    };
    self.add = function () {
        self.Id(0);
        self.Type(null);
        self.Details(null);
        self.PreparationDetails(null);
        self.Status(null);
        self.Rating(null);
    };
    self.save = function () {
        var url = '/Meals/Save';
        var meal = JSON.stringify({
            Id: self.Id(),
            UserId: self.UserId(),
            Details: self.Details(),
            PreparationDetails: self.PreparationDetails(),
            PlannedTime: self.PlannedTime(),
            ActualTime: self.ActualTime(),
            Type: self.Type(),
            Status: self.Status(),
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
                self.Statuses(data.Statuses);
                self.Ratings(data.Ratings);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
}