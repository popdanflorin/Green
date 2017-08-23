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
    }

    self.getRestaurantRating = function () {

    }
    
    self.changeRestaurantId = function (RestaurantId) {
        self.RestaurantId = RestaurantId.data;
    };

    self.refresh = function () {
        var url = '/Ratings/Refresh';
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

        //if (self.UserRating == 0)
        //    $(".rateit").rateit('value', self.UserRating);
        //else
        //    $(".rateit").rateit('value', self.TotalRating);
    };

    self.save = function () {
        var url = '/Ratings/Save';
        var value = $(this).rateit('value');
        var rating = JSON.stringify({
            Id: self.Id(),
            ClientId: self.UserId(),
            RestaurantId: self.RestaurantId(),
            Value: value
        });
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