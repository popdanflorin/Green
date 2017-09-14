function Menus() {
    var self = this;

    self.Menus = ko.observableArray(null);
    self.loadingPanel = new LoadingOverlay();
    self.Meals = ko.observableArray(null);
    self.MealTypes = ko.observableArray(null);

    self.Id = ko.observable(null);
    self.RestaurantId = ko.observable(null);
    self.RestaurantName = ko.observable(null);
    self.StartDate = ko.observable(null);
    self.EndDate = ko.observable(null);

    // displays
    self.RestaurantNameDisplay = ko.computed(function () {
        if (!self.RestaurantName())
            return "Error";
        return self.RestaurantName() + "'s Menus";
    });

    self.StartDateDisplay = ko.computed(function () {
        if (!self.StartDate())
            return null;
        return moment(self.StartDate(), "MM/DD/YYYY", false).format();
    });

    self.EndDateDisplay = ko.computed(function () {
        if (!self.EndDate())
            return null;
        return moment(self.EndDate(), "MM/DD/YYYY", false).format();
    });

    self.MenuTitleDisplay = ko.computed(function () {
        return self.RestaurantName() + "'s Menu\n" + self.StartDateDisplay() + " - " + self.EndDateDisplay();
    });

    // for meals
    self.MealId = ko.observable();
    self.Meal = ko.computed(function () {
        if (!self.MealId())
            return null;

        var tmp = null;
        ko.utils.arrayForEach(self.Meals(), function (element) {
            if (element.Id.valueOf() == self.MealId().valueOf()) {
                tmp = element;
                return;
            }
        });
        return tmp;
    });
    //self.RestaurantMealsName = ko.computed(function () {
    //    var tmp = "";
    //    if (self.RestaurantMeals() == null)
    //        return;
    //    ko.utils.arrayForEach(self.RestaurantMeals(), function (i) {
    //        tmp += i.Name + ", ";
    //    });
    //    if (tmp)
    //        tmp = tmp.slice(0, tmp.length - 2);
    //    return tmp;
    //});

    // 0: all, 1: selected, 2: unselected
    self.DisplayCase = ko.observable(0);
    self.DisplayedMeals = ko.computed(function () {
        if (self.Meals() != null)
            switch (self.DisplayCase()) {
                case -1:
                    return null;
                case 0:
                    return self.Meals();
                case 1:
                    return self.Meals().filter(function (meal) {
                        return meal.isSelected == true;
                    });
                case 2:
                    return self.Meals().filter(function (meal) {
                        return meal.isSelected == false;
                    });
                default:
                    return self.Meals().filter(function (meal) {
                        return meal.Name.search(new RegExp(self.DisplayCase(), "i")) != -1;
                    });
            }
        return null;
    });
    self.SearchText = ko.observable();

    // validation warning
    self.warningStartDate = ko.observable();
    self.warningEndDate = ko.observable();
    self.warningMeal = ko.observable();

    // functions
    self.open = function (data) {
        self.RestaurantId(data.id);
        self.RestaurantName(data.Name);
        self.refresh();
    };

    self.refresh = function () {
        var url = '/Menus/ListRefresh';
        var restaurant = JSON.stringify({
            restaurantId: self.RestaurantId(),
        });
        self.loadingPanel.show();
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Menus(data.Menus);
                self.MealTypes(data.MealTypes);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        if (self.Id() == null)
            self.Id(0);
        self.TemporaryMeals = [];
    };

    self.refreshMenu = function (data) {
        self.Id(data.Id);
        self.StartDate(data.StartDate);
        self.EndDate(data.EndDate);
        self.refreshMeals();

        self.warningStartDate(null);
        self.warningEndDate(null);
        self.warningMeal(null);

        if (self.nullOrEmpty(self.Id()))
            $("#MenuDeleteButton").hide();
        else
            $("#MenuDeleteButton").show();
    };

    self.ingredientsModalClose = function () {
        var url = '/Menus/GetNewMealsForMenu';
        var menu = JSON.stringify({
            menuId: self.Id(),
            selectedMeals: self.Meals()
        });
        self.loadingPanel.show();
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: menu,
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Meals(data.Meals);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        $("#MultipleSelectionButton").prop("checked", false);
    }

    self.refreshMeals = function () {
        var url = '/Menus/GetMealsForMenu';
        var menu = JSON.stringify({
            menuId: self.Id(),
        });
        self.loadingPanel.show();
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: menu,
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Meals(data.Meals);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        $("#MultipleSelectionButton").prop("checked", false);
    };

    self.add = function () {
        self.Id(0);
        self.StartDate(null);
        self.EndDate(null);

        var url = '/Menus/GetMeals';
        self.loadingPanel.show();
        $.ajax(url, {
            type: "get",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Meals(data.Meals);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        self.warningStartDate(null);
        self.warningEndDate(null);
        self.warningMeal(null);

        $("#MultipleSelectionButton").prop("checked", false);
        $("#MenuDeleteButton").hide();
    };

    self.save = function () {
        if (!self.validate())
            return;

        var url = '/Menus/Save';
        var menu = JSON.stringify({
            Id: self.Id(),
            RestaurantId: self.RestaurantId(),
            StartDate: self.StartDate(),
            EndDate: self.EndDate(),
            meals: self.Meals()
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: menu,
            success: function (data) {
                console.log(data);
                $("#menuItem").modal("hide");
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.delete = function (data) {
        var url = '/Menus/Delete';
        var menu = JSON.stringify({
            menuId: data.Id
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: menu,
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
        $('#menuItem').modal('hide');
    };

    self.onMealClicked = function (data) {
        // it it is in menu, delete. otherwise, add
        if (self.isMealInMenu(data.Id))
            self.deleteMeal(data);
        else
            self.addMeal(data);
    }

    self.addMeal = function (data) {
        self.MealId(data.Id);
        if (!self.validateMealId())
            return;

        if (self.Meals()) {
            if (self.isMealInMenu(self.MealId())) {
                self.warningMeal("Selected meal already exists in this menu!");
                return;
            }
            var index = self.Meals().findIndex(function (element) {
                return element.Id.valueOf() == self.MealId().valueOf();
            });
            self.Meals()[index].isSelected = true;
            self.refreshDisplay();
        }
    };

    self.addMealFromList = function () {
        self.addMeal(data = { Id: $('select[id=MenuMeals]').val() });
    };

    self.deleteMeal = function (data) {
        self.MealId(data.Id);
        if (!self.validateMealId())
            return;

        if (self.Meals()) {
            if (self.isMealInMenu(self.MealId()) == false) {
                self.warningMeal("Selected meal does not exist in this menu!");
                return;
            }
            var index = self.Meals().findIndex(function (element) {
                return element.Id.valueOf() == self.MealId().valueOf();
            });
            self.Meals()[index].isSelected = false;
            self.refreshDisplay();
        }
    };

    self.deleteMealFromList = function () {
        self.deleteMeal(data = { Id: $('select[id=MenuMeals]').val() });
    };

    self.onSelectMeal = function () {
        self.MealId($("#MealIngredient").val());
    };

    self.SelectAll = function () {
        var isChecked = $("#MultipleSelectionButton").prop("checked");
        self.Meals().forEach(function (meal) {
            meal.isSelected = isChecked;
        });
        self.refreshDisplay();
        return true;
    }

    // for displaying meals
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

    self.SearchMeal = ko.computed(function () {
        self.DisplayCase(self.SearchText());
    });

    // validations
    self.validate = function () {
        var valid = true;

        if (self.nullOrEmpty(self.StartDate())) {
            self.warningStartDate("Please select the start date!");
            valid = false;
        }
        else {
            self.warningStartDate(null);
        }

        if (self.nullOrEmpty(self.EndDate())) {
            self.warningEndDate("Please select the end date!");
            valid = false;
        }
        else {
            self.warningEndDate(null);
        }

        return valid;
    };

    self.validateMealId = function () {
        if (self.MealId() != 0 && self.nullOrEmpty(self.MealId())) {
            self.warningMeal("No meal choosen!");
            return false;
        }
        self.warningMeal(null);
        return true;
    };

    self.isMealInMenu = function (mealId) {
        var index = self.Meals().findIndex(function (element) {
            return element.Id.valueOf() == mealId.valueOf();
        });
        if (self.Meals()[index].isSelected == true)
            return true;
        return false;
    };

    self.nullOrEmpty = function (data) {
        if (data == null || data == "") {
            return true;
        }
        return false;
    };
};