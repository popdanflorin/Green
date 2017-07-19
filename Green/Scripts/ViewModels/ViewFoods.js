function Foods(data)
{
    var self = this;
    self.Foods = ko.observableArray(data.Foods);
    self.Types = ko.observableArray(data.FoodTypes);
    self.Name = ko.observable();
    self.Type = ko.observable();

    self.details = function (data)
    {
        self.Name(data.Name);
        self.Type(data.Type);
    }
    self.add = function ()
    {
        self.Name("");
        self.Type(null);
    }
    self.save = function () {
        self.Foods.push({ Name: ko.observable(self.Name()), Type: ko.observable(self.Type()), TypeDisplay: ko.observable("New item") });
    }
}