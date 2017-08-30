function Meals() {
    var self = this;

    //for list
    self.Meals = ko.observableArray();
    self.Types = ko.observableArray();
    self.Foods = ko.observableArray();
    self.Ratings = ko.observableArray();

    //for item
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Description = ko.observable();
    self.Type = ko.observable();
    self.Rating = ko.observable();

    self.ImageName = ko.computed(function () {
        var url = '/Meals/GetImage';
        var meal = JSON.stringify({
            mealId: self.Id()
        });

        var image = null;
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: meal,
            success: function (data) {
                image = data;
                console.log(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        return image;
    });

    self.IngredientId = ko.observable();
    self.Ingredient = ko.computed(function () {
        if (!self.IngredientId())
            return null;

        var tmp = null;
        ko.utils.arrayForEach(self.Foods(), function (element) {
            if (element.Id.valueOf() == self.IngredientId().valueOf()) {
                tmp = element;
                return;
            }
        });
        return tmp;
    });
    self.Ingredients = ko.observableArray();
    self.IngredientsName = ko.computed(function () {
        var tmp = "";
        if (self.Ingredients == null)
            return;
        ko.utils.arrayForEach(self.Ingredients(), function (i) {
            tmp += i.Name + ", ";
        });
        if (tmp)
            tmp = tmp.slice(0, tmp.length - 2);
        return tmp;
    });

    //for search
    self.SearchName = ko.observable(false);
    self.SearchIngredient = ko.observable(false);

    //validation warnings
    self.warningName = ko.observable(null);
    self.warningDescription = ko.observable(null);
    self.warningType = ko.observable(null);
    self.warningIngredient = ko.observable(null);

    self.loadingPanel = new LoadingOverlay();

    self.details = function (data) {
        self.Id(null);
        self.Id(data.Id);
        self.Name(data.Name);
        self.Description(data.Description);
        self.Type(data.Type);
        self.Rating(data.Rating);
        self.IngredientId(null);
        self.getIngredients();

        self.warningName(null);
        self.warningDescription(null);
        self.warningType(null);
        self.warningIngredient(null);
    };

    self.add = function () {
        self.Id(0);
        self.Type(null);
        self.Name(null);
        self.Description(null);
        self.Rating(null);
        self.IngredientId(null);
        self.Ingredients([]);

        self.warningName(null);
        self.warningDescription(null);
        self.warningType(null);
        self.warningIngredient(null);
    };

    self.save = function () {
        if (!self.validate()) {
            self.setOKButton(false);
            return;
        }
        self.setOKButton(true);

        var url = '/Meals/Save';
        var meal = JSON.stringify({
            Id: self.Id(),
            Name: self.Name(),
            Description: self.Description(),
            Type: self.Type(),
            Rating: self.Rating(),
            ingredients: self.Ingredients()
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

    self.deleteModal = function () {
        var data = { Id: self.Id() };
        self.delete(data);
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
                self.Foods(data.Foods);
                self.Ratings(data.Ratings);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.addIngredient = function () {
        if (!self.validateIngredientId())
            return;

        if (self.Ingredients()) {
            var index = self.Ingredients().findIndex(function (element) {
                return element.Id.valueOf() == self.IngredientId().valueOf();
            });
            if (index != -1) {
                self.warningIngredient("Selected ingredient already exists in this meal!");
                return;
            }
        }

        self.Ingredients.push(self.Ingredient());
    };

    self.deleteIngredient = function () {
        if (!self.validateIngredientId())
            return;

        var index = self.Ingredients().findIndex(function (element) {
            return element.Id.valueOf() == self.IngredientId().valueOf();
        });
        if (index == -1) {
            self.warningIngredient("Selected ingredient does not exist in this meal!");
            return;
        }

        self.Ingredients.splice(index, 1);
    }

    self.getIngredients = function () {
        var url = '/Meals/GetIngredients';
        var meal = JSON.stringify({
            mealId: self.Id()
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: meal,
            success: function (data) {
                self.Ingredients(data);
                console.log(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    }

    self.search = function () {
        alert("Searching...");
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

    self.validate = function () {
        var valid = true;
        if (self.nullOrEmpty(self.Name._latestValue)) {
            self.warningName("Please enter a name!");
            valid = false;
        }
        else {
            self.warningName(null);
        }

        if (self.nullOrEmpty(self.Description._latestValue)) {
            self.warningDescription("Please enter a description!");
            valid = false;
        }
        else {
            self.warningDescription(null);
        }

        if (self.Type._latestValue != 0 && self.nullOrEmpty(self.Type._latestValue)) {
            self.warningType("Please select the type!");
            valid = false;
        }
        else {
            self.warningType(null);
        }

        return valid;
    };

    self.validateIngredientId = function () {
        if (self.IngredientId() != 0 && self.nullOrEmpty(self.IngredientId())) {
            self.warningIngredient("No ingredient choosen!");
            return false;
        }
        self.warningIngredient(null);
        return true;
    };

    self.nullOrEmpty = function (data) {
        if (data == null || data == "") {
            return true;
        }
        return false;
    };
};