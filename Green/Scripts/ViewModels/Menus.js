function Menus() {
    var self = this;

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
        return self.RestaurantName() + "'s Menu";
    });

    self.StartDateDisplay = ko.computed(function () {
        if (!self.StartDate())
            return moment(Date(), "MM/DD/YYYY", false).format();
        return moment(self.StartDate(), "MM/DD/YYYY", false).format();
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

        self.warningStartDate(null);
        self.warningEndDate(null);
        self.warningMeal(null);
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
                self.Id(data.Id);
                self.StartDate(new Date(data.StartDate));
                self.EndDate(new Date(data.EndDate));
                self.Meals(data.Meals);
                self.MealTypes(data.MealTypes);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        if (self.Id() == null)
            self.Id(0);
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
                self.refresh();
                $("#menuList").modal("hide");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.delete = function () {
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