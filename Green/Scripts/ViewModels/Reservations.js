function Reservations() {
    var self = this;
    self.Reservations = ko.observableArray();
    self.Id = ko.observable();
    self.ClientName = ko.observable();
    self.Date = ko.observable();
    self.Seats = ko.observable();
    
    self.details = function(data) {
        self.Id(data.Id);
        self.ClientName(data.ClientName);
        self.Date(data.Date);
        self.Seats(data.Seats);
    };

    self.refresh = function() {
        var url = '/Reservations/ListRefresh';
        
    }
}