function Ratings() {
    var self = this;

    self.Id = ko.observable();
    self.UserId = ko.observable();
    self.RestaurantId = ko.observable();
    self.UserRating = ko.observable();
    self.TotalRating = ko.observable();

    self.loadingPanel = new LoadingOverlay();

    self.set = function () {
        var tooltipvalues = ['bad', 'poor', 'ok', 'good', 'super'];
        $(".rateit").bind('over', function (event, value) { $(this).attr('title', tooltipvalues[value - 1]); });
        $(".rateit").bind('rated', self.save);

        self.getUserId();
    };

    self.getUserId = function () {
        var url = '/Ratings/GetUserId';
        self.loadingPanel.show();

        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.UserId(data.UserId);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
    
    self.changeRestaurantId = function (RestaurantId) {
        self.RestaurantId = RestaurantId.data;
        self.refresh();
    };

    self.getRatings = function () {
        var url = '/Ratings/GetRatings';
        try {
            var rating = JSON.stringify({
                Id: self.Id(),
                ClientId: self.UserId(),
                RestaurantId: self.RestaurantId(),
                Value: 0
            });
        }
        catch (e) {
            var rating = JSON.stringify({
                Id: self.Id(),
                ClientId: self.UserId(),
                RestaurantId: self.RestaurantId,
                Value: 0
            });
        }

        $.ajax(url, {
            async: false,
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: rating,
            success: function (data) {
                console.log(data);
                self.UserRating(data.UserRating);
                self.TotalRating(data.TotalRating);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.refresh = function () {
        self.getRatings();
        $("#UserRating").text(self.UserRating() + "/5");
        $(".rateit").rateit('value', self.TotalRating());
    };

    self.save = function () {
        var url = '/Ratings/Save';
        var value = $(this).rateit('value');
        try {
            var rating = JSON.stringify({
                Id: self.Id(),
                ClientId: self.UserId(),
                RestaurantId: self.RestaurantId(),
                Value: value
            });
        }
        catch (e) {
            var rating = JSON.stringify({
                Id: self.Id(),
                ClientId: self.UserId(),
                RestaurantId: self.RestaurantId,
                Value: value
            });
        }
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: rating,
            success: function (data) {
                try {
                    console.log(data);
                    self.refresh();
                } catch (Exception) {
                    console.log(Exception);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
};