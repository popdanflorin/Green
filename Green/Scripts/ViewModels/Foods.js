function Foods(data)
{
    var self = this;
    self.Foods = ko.observableArray(data.Foods);
    self.Types = ko.observableArray(data.FoodTypes);
    self.Id = ko.observable();
    self.Name = ko.observable();
    self.Type = ko.observable();

    self.details = function (data) {
        self.Id(data.Id);
        self.Name(data.Name);
        self.Type(data.Type);
    };
    self.add = function () {
        self.Id(0);
        self.Name("");
        self.Type(null);
    };
    self.save = function () {
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
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };
}