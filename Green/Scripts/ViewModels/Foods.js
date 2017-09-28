function Foods() {
    var self = this;
    self.Foods = ko.observableArray();
    self.Types = ko.observableArray();
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Type = ko.observable();
    self.loadingPanel = new LoadingOverlay();

    self.SelectedItems = ko.observableArray();
    self.Message = ko.observable(); // for snackbar

    // validation warnings
    self.warningName = ko.observable();
    self.warningType = ko.observable();

    self.categoriesState = ko.observableArray();

    // SignalR
    self.initSignalR = function () {
        $.connection.hub.start()
            .done(function () {
                console.log("SignalR initialization success!");
            })
            .fail(function () {
                console.log("SignalR initialization error!");
            })
    }

    $.connection.dataHub.client.refreshFoods = function (temp) {
        console.log("SignalR refreshFoods");
        self.refresh();
    };
    // end SignalR related functions

    self.details = function (data) {
        self.Id(data.Id);
        self.Name(data.Name);
        self.Type(data.Type);

        self.warningName(null);
        self.warningType(null);

        $("#FoodDeleteButton").show();
    };

    self.add = function () {
        self.Id(0);
        self.Name(null);
        self.Type(null);

        self.warningName(null);
        self.warningType(null);

        $("#FoodDeleteButton").hide();
    };

    self.save = function () {
        if (!self.validate()) {
            return;
        }

        var isNew = (self.Id() == 0 || self.Id() == null);

        var url = '/Foods/Save';
        var food = JSON.stringify({
            Id: self.Id(),
            Name: self.Name(),
            Type: self.Type()
        });
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: food,
            success: function (data) {
                console.log(data);
                self.refresh();
                self.Message(data);
                $("#foodItem").modal("hide");
                $.connection.dataHub.server.refreshFoods();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Message(textStatus);
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        self.showSnackbar();
    };

    self.delete = function (data) {
        if (!window.confirm("Are you sure you want to delete this food?")) {
            return false;
        }

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
                self.Message(data);
                $.connection.dataHub.server.refreshFoods();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                self.Message(textStatus);
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        self.showSnackbar();
        return true;
    };

    self.deleteModal = function () {
        var data = { Id: self.Id() };
        if (self.delete(data))
            $("#foodItem").modal("hide");
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

    self.setCategoryState = function (data, event) {
        var id = event.currentTarget.hash;
        var category = $(id)['0'];
        var isActive = (category.className.search("in") == -1);
        var index = parseInt(category.id.match("[0-9]*$")[0]);
        self.categoriesState[index] = isActive;
    };

    self.showSnackbar = function () {
        var x = document.getElementById("FoodSnackbar")
        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
    };

    self.validate = function () {
        var valid = true;

        if (self.Name() == null || self.Name() == "") {
            self.warningName("Please enter a name!");
            valid = false;
        }
        else {
            self.warningName(null);
        }

        if (self.Type() != 0 && (self.Type() == null || self.Type() == "")) {
            self.warningType("Please select a type!");
            valid = false;
        }
        else {
            self.warningType(null);
        }

        return valid;
    };

    self.sort = function (key) {
        switch (key) {
            case "Name":
                break;
            case "Type":
                break;
        }
    };
}