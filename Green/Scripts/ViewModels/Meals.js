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
    self.Ingredient = "";
    self.IngredientsName = ko.observable();
    self.IngredientsId = [];

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
        self.Id(data.Id);
        self.Name(data.Name);
        self.Description(data.Description);
        self.Type(data.Type);
        self.Rating(data.Rating);

        self.IngredientsId = [];
        self.IngredientsName(null);
        if (data.Ingredients != null) {
            var tmp = data.Ingredients.split(",");
            tmp.sort();
            tmp.forEach(function (ingredient) {
                self.Ingredient = ingredient;
                self.IngredientsId.push(self.Ingredient);
            });
        }
        self.getIngredientsName();
        self.Ingredient = null;

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
        self.Ingredient = null;
        self.IngredientsId = [];
        self.IngredientsName(null);

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
        var meal;
        try {
            meal = JSON.stringify({
                Id: self.Id(),
                Name: self.Name(),
                Description: self.Description(),
                Type: self.Type(),
                Rating: self.Rating(),
                Ingredients: self.IngredientsId().join(",").toString()
            });
        }
        catch (e) {
            meal = JSON.stringify({
                Id: self.Id(),
                Name: self.Name(),
                Description: self.Description(),
                Type: self.Type(),
                Rating: self.Rating(),
                Ingredients: self.IngredientsId.join(",").toString()
            });
        }
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
                self.Foods(data.Foods);
                self.Ratings(data.Ratings);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.addIngredient = function () {
        if (!self.validateIngredient())
            return;

        var index = self.IngredientsId.findIndex(function (element) {
            return element.valueOf() == self.Ingredient.valueOf();
        });
        if (index != -1) {
            self.warningIngredient("Selected ingredient already exists in this meal!");
            return;
        }

        self.warningIngredient(null);
        self.IngredientsId.push(self.Ingredient);
        self.getIngredientsName();
    };

    self.deleteIngredient = function () {
        if (!self.validateIngredient())
            return;

        var index = self.IngredientsId.findIndex(function (element) {
            return element.valueOf() == self.Ingredient.valueOf();
        });
        if (index == -1) {
            self.warningIngredient("Selected ingredient does not exist in this meal!");
            return;
        }

        self.warningIngredient(null);
        self.IngredientsId.splice(index, 1);
        self.getIngredientsName();
    }

    self.getIngredientsName = function () {
        var url = '/Meals/GetIngredientsName';
        var foods = "";
        try {
            foods = JSON.stringify({
                ingredientsId: ko.toJS(self.IngredientsId())
            });
        }
        catch (e) {
            foods = JSON.stringify({
                ingredientsId: ko.toJS(self.IngredientsId)
            });
        }

        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: foods,
            success: function (data) {
                console.log(data);
                self.IngredientsName(data);
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

    self.validateIngredient = function () {
        try {
            if (self.Ingredient._latestValue != 0 && self.nullOrEmpty(self.Ingredient)) {
                self.warningIngredient("No ingredient choosen!");
                return false;
            }
            else {
                self.warningIngredient(null);
                return true;
            }
        }
        catch (e) {
            if (self.Ingredient != 0 && self.nullOrEmpty(self.Ingredient)) {
                self.warningIngredient("No ingredient choosen!");
                return false;
            }
            else {
                self.warningIngredient(null);
                return true;
            }
        }
    }

    self.nullOrEmpty = function (data) {
        if (data == null || data == "") {
            return true;
        }
        return false;
    };
};