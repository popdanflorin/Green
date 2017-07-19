function Foods()
{
    var self = this;
    self.Foods = ko.observableArray();

    self.init = function (data)
    {
        self.Foods(data);
    }
    self.details = function (data)
    {
        console.log(data);
        console.log(this);
        //self.FoodList.remove(data);
        self.FoodList()[0].Name("abc");
    }
}