function Foods() {
    var self = this;
    self.Foods = ko.observableArray();
    self.Types = ko.observableArray();
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Type = ko.observable();
    self.loadingPanel = new LoadingOverlay();

    //validation warnings
    self.warningName = ko.observable();
    self.warningType = ko.observable();

    self.details = function (data) {
        self.Id(data.Id);
        self.Name(data.Name);
        self.Type(data.Type);

        self.warningName(null);
        self.warningType(null);

        $("#DeleteButton").show();
    };

    self.add = function () {
        self.Id(0);
        self.Name(null);
        self.Type(null);

        self.warningName(null);
        self.warningType(null);

        $("#DeleteButton").hide();
    };

    self.save = function () {
        if (!self.validate()) {
            self.setOKButton(false);
            return;
        }
        self.setOKButton(true);

        var url = '/Foods/Save';
        var food = JSON.stringify({
            Id: self.Id(),
            Name: self.Name(),
            Type: self.Type()
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: food,
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
        var url = '/Foods/Delete';
        var food = JSON.stringify({
            foodId: data.Id
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: food,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.deleteModal = function () {
        var data = { Id: self.Id() };
        self.delete(data);
    }

    self.refresh = function () {
        var url = '/Foods/ListRefresh';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Foods(data.Foods);
                self.Types(data.FoodTypes);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.validate = function () {
        var valid = true;

        if (self.Name._latestValue == null || self.Name._latestValue == "") {
            self.warningName("Please enter a name!");
            valid = false;
        }
        else {
            self.warningName(null);
        }

        if (self.Type._latestValue != 0 && (self.Type._latestValue == null || self.Type._latestValue == "")) {
            self.warningType("Please select a type!");
            valid = false;
        }
        else {
            self.warningType(null);
        }

        return valid;
    };

    self.setOKButton = function (value) {
        try {
            if (value) {
                var attrDataDismiss = document.createAttribute("data-dismiss");
                attrDataDismiss.value = "modal";
                document.getElementById("OKButton").attributes.setNamedItem(attrDataDismiss);
            }
            else
                document.getElementById("OKButton").removeAttribute("data-dismiss");
        }
        catch (Exception) {
            console.log(Exception);
        }
    };
}