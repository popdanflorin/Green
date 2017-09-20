function Meals() {
    var self = this;

    //for list
    self.Meals = ko.observableArray();
    self.Types = ko.observableArray();

    //for item
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Description = ko.observable();
    self.Type = ko.observable();
    self.ImageName = ko.observable();
    self.Foods = ko.observableArray();
    self.FoodTypes = ko.observableArray();
    self.ChoosenImage = ko.observable();

    self.ImageUrl = ko.computed(function () {
        return "url('../Content/images/" + self.ImageName() + "')";
    });

    //self.ImageName = ko.computed(function () {
    //    var url = '/Meals/GetImage';
    //    var meal = JSON.stringify({
    //        mealId: self.Id()
    //    });

    //    var image = null;
    //    $.ajax(url, {
    //        async: false,
    //        type: "post",
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8",
    //        data: meal,
    //        success: function (data) {
    //            image = data;
    //            console.log(data);
    //        },
    //        error: function (jqXHR, textStatus, errorThrown) {
    //            console.log(textStatus + ': ' + errorThrown);
    //        }
    //    });
    //    return image;
    //});

    // for ingredients
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
    // 0: all, 1: selected, 2: unselected
    self.DisplayCase = ko.observable(0);
    self.DisplayedFoods = ko.computed(function () {
        switch (self.DisplayCase()) {
            case -1:
                return null;
            case 0:
                return self.Foods();
            case 1:
                return self.Foods().filter(function (food) {
                    return food.isSelected == true;
                });
            case 2:
                return self.Foods().filter(function (food) {
                    return food.isSelected == false;
                });
            default:
                return self.Foods().filter(function (food) {
                    return food.Name.search(new RegExp(self.DisplayCase(), "i")) != -1;
                });
        }
    });
    self.SearchText = ko.observable();

    //for search
    self.SearchName = ko.observable(false);
    self.SearchIngredient = ko.observable(false);
    self.SearchMealsText = ko.observable("");
    self.DisplayedMeals = ko.computed(function () {
        return self.Meals().filter(function (meal) {
            return meal.Name.search(new RegExp(self.SearchMealsText(), "i")) != -1;
        });
    });

    //validation warnings
    self.warningName = ko.observable(null);
    self.warningDescription = ko.observable(null);
    self.warningType = ko.observable(null);
    self.warningIngredient = ko.observable(null);
    self.warningImage = ko.observable(null);

    self.loadingPanel = new LoadingOverlay();

    self.details = function (data) {
        self.Id(0);
        self.Id(data.Id);
        self.Name(data.Name);
        self.Description(data.Description);
        self.Type(data.Type);
        self.ImageName(data.ImageName);
        self.IngredientId(null);
        self.getIngredients();

        self.warningName(null);
        self.warningDescription(null);
        self.warningType(null);
        self.warningIngredient(null);
        self.warningImage(null);

        $("#MealDeleteButton").show();
    };

    self.add = function () {
        self.refresh();
        self.Id(0);
        self.Type(null);
        self.Name(null);
        self.Description(null);
        self.ImageName(null);
        self.IngredientId(null);
        self.ChoosenImage(null);

        self.warningName(null);
        self.warningDescription(null);
        self.warningType(null);
        self.warningIngredient(null);
        self.warningImage(null);

        $("#MealDeleteButton").hide();
    };

    self.save = function () {
        if (!self.validate()) {
            return;
        }

        // save meal
        var url = '/Meals/Save';
        var meal = JSON.stringify({
            Id: self.Id(),
            Name: self.Name(),
            Description: self.Description(),
            Type: self.Type(),
            ImageName: self.ImageName(),
            ingredients: self.Foods()
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: meal,
            success: function (data) {
                console.log(data);
                self.refresh();
                $("#mealItem").modal("hide");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.delete = function (data) {
        if (!window.confirm("Are you sure you want to delete the meal?")) {
            return false;
        }

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
        return true;
    };

    self.deleteModal = function () {
        var data = { Id: self.Id() };
        if (self.delete(data))
            $('#mealItem').modal('hide');
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
                self.FoodTypes(data.FoodTypes);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.search = function () {
        alert("Searching...");
    };

    self.onSelectIngredient = function () {
        self.IngredientId($("#MealIngredient").val());
        console.log($("#MealIngredient").val());
    }

    //self.setOKButton = function (value) {
    //    try {
    //        if (value) {
    //            var attrDataDismiss = document.createAttribute("data-dismiss");
    //            attrDataDismiss.value = "modal";
    //            document.getElementById("MealOKButton").attributes.setNamedItem(attrDataDismiss);
    //        }
    //        else
    //            document.getElementById("MealOKButton").removeAttribute("data-dismiss");
    //    }
    //    catch (Exception) {
    //        console.log(Exception);
    //    }
    //};

    // for ingredients list
    self.addIngredient = function (data) {
        self.IngredientId(data.Id);
        if (!self.validateIngredientId())
            return;

        if (self.Foods()) {
            if (self.isIngredientInMeal(self.IngredientId())) {
                self.warningIngredient("Selected ingredient already exists in this meal!");
                return;
            }
            var index = self.Foods().findIndex(function (element) {
                return element.Id.valueOf() == self.IngredientId().valueOf();
            });
            self.Foods()[index].isSelected = true;
            self.refreshDisplay();
        }
    };

    self.deleteIngredient = function (data) {
        self.IngredientId(data.Id);
        if (!self.validateIngredientId())
            return;

        if (self.Foods()) {
            if (self.isIngredientInMeal(self.IngredientId()) == false) {
                self.warningIngredient("Selected ingredient does not exist in this meal!");
                return;
            }
            var index = self.Foods().findIndex(function (element) {
                return element.Id.valueOf() == self.IngredientId().valueOf();
            });
            self.Foods()[index].isSelected = false;
            self.refreshDisplay();
        }
    };

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
                self.Foods(data);
                console.log(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.onIngredientClicked = function (data) {
        // it it is in meal, delete. otherwise, add
        if (self.isIngredientInMeal(data.Id))
            self.deleteIngredient(data);
        else
            self.addIngredient(data);
    };

    self.isIngredientInMeal = function (ingredientId) {
        var index = self.Foods().findIndex(function (element) {
            return element.Id.valueOf() == ingredientId.valueOf();
        });
        if (self.Foods()[index].isSelected == true)
            return true;
        return false;
    };

    self.SelectAll = function () {
        var isChecked = $("#MultipleSelectionButton").prop("checked");
        self.Foods().forEach(function (food) {
            food.isSelected = isChecked;
        });
        self.refreshDisplay();
        return true;
    };

    // for displaying ingredients
    self.showAll = function () {
        self.DisplayCase(0);
    };

    self.showSelected = function () {
        self.DisplayCase(1);
    };

    self.showUnselected = function () {
        self.DisplayCase(2);
    };

    self.refreshDisplay = function () {
        var tmp = self.DisplayCase();
        self.DisplayCase(-1);
        self.DisplayCase(tmp);
    };

    self.SearchIngredient = ko.computed(function () {
        self.DisplayCase(self.SearchText());
    });

    // validations
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

        if (self.Id() && self.nullOrEmpty(self.ImageName) && self.nullOrEmpty(self.ChoosenImage())) {
            self.warningImage("Please select an image!");
            valid = false;
        }
        else {
            self.warningImage(null);
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