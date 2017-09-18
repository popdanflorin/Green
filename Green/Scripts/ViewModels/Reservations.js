function Reservations() {
    var self = this;

    self.Reservations = ko.observableArray();
    self.Restaurants = ko.observableArray();
    self.Id = ko.observable();
    self.RestaurantId = ko.observable();
    self.ClientId = ko.observable();
    self.ReservationDate = ko.observable(new Date())
    self.Seats = ko.observable();
    self.UserName = ko.observable();
    self.UserId = ko.observable();
    self.isAdmin = ko.observable(false);
    self.loadingPanel = new LoadingOverlay();

    // for chart
    self.myChart;
    self.monthlyChart = null;
    self.Months = ["January", "February", "March", "April", "May", "June", "Jully", "August", "September", "October", "November", "December"];
    self.Years = [];
    self.Days = [];
    self.Percentages = ko.observableArray();
    self.RestaurantPercentages = ko.observableArray();
    self.SelectedYear = ko.observable();
    self.SelectedYearChanged = ko.computed(function () {
        if (self.SelectedYear == null || self.SelectedYear == undefined)
            return;
        //alert(self.SelectedYear());
        try {
            self.refreshPercentages(self.SelectedYear()[0]);
            self.Percentages().forEach(function (item, index) {
                self.myChart.data.datasets[index].data = item.Percentages;
            });
            self.myChart.update();
        }
        catch (e) { }
        //var object = document.getElementById("ChartYear");
        //try
        //{
        //    var tmp = object.options[object.selectedIndex].text;
        //    alert(tmp);
        //    self.refreshPercentages(tmp);
        //    self.refreshChart();
        //}
        //catch (e) {}
    });

    // validation warnings
    self.warningRestaurantId = ko.observable();
    self.warningReservationDate = ko.observable();
    self.warningSeats = ko.observable();

    self.details = function (data) {
        self.Id(data.Id);
        self.RestaurantId(data.RestaurantId);
        self.ClientId(data.ClientId);
        self.ReservationDate(data.ReservationDate);
        self.Seats(data.Seats);
        self.UserName(data.User.UserName);

        self.warningRestaurantId(null);
        self.warningReservationDate(null);
        self.warningSeats(null);
    };

    self.getMenu = function (data) {

    }

    self.add = function () {
        self.Id(0);
        self.RestaurantId(null);
        self.ReservationDate(null);
        self.Seats(null);
        self.warningRestaurantId(null);
        self.warningReservationDate(null);
        self.warningSeats(null);
    };

    self.delete = function (data) {
        if (!window.confirm("Are you sure you want to cancel the reservation?")) {
            return;
        }

        var url = '/Reservations/Delete';
        var reservation = JSON.stringify({
            reservationId: data.Id
        });
        $.ajax(url, {
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: reservation,
            success: function (data) {
                console.log(data);
                self.refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.save = function () {
        if (!self.validate()) {
            self.setOKButton(false);
            return;
        }

        var url = '/Reservations/Save';
        self.warningRestaurantId(null);
        self.warningReservationDate(null);
        self.warningSeats(null);
        if (self.ClientId()) {
            var reservation = JSON.stringify({
                Id: self.Id(),
                RestaurantId: self.RestaurantId(),
                ClientId: self.ClientId(),
                ReservationDate: self.ReservationDate(),
                Seats: self.Seats()
            });
        }
        else {
            var reservation = JSON.stringify({
                Id: self.Id(),
                RestaurantId: self.RestaurantId(),
                ClientId: self.UserId(),
                ReservationDate: self.ReservationDate(),
                Seats: self.Seats()
            });
        }
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: reservation,
            success: function (data) {
                if (data.search(new String("The selected time is not available.").valueOf()) >= 0) {
                    try {
                        self.warningReservationDate(data);
                        self.setOKButton(false);
                    } catch (Exception) {
                        console.log(Exception);
                    }
                }
                else if (data.search(new String("There are not enough seats available.").valueOf()) >= 0) {
                    try {
                        self.warningSeats(data);
                        self.setOKButton(false);
                    } catch (Exception) {
                        console.log(Exception);
                    }
                }
                else {
                    try {
                        self.setOKButton(true);
                        console.log(data);
                        self.refresh();
                    } catch (Exception) {
                        console.log(Exception);
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
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
    }

    self.refresh = function () {
        var url = '/Reservations/ListRefresh';
        self.loadingPanel.show();
        $.ajax(url, {
            async: false,
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Reservations(data.Reservations);
                self.Restaurants(data.Restaurants);
                self.UserId(data.UserId);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        var date = new Date().getFullYear();
        self.Years.push(date);
        for (var i = 1; i <= 5; ++i) {
            self.Years.push(date - i);
            self.Years.push(date + i);
        }
        self.Years.sort().reverse();
        document.getElementById("ChartYear").value = date;

        self.SelectedYear(date);

        self.refreshPercentages(self.SelectedYear());
        self.refreshChart();
    };

    self.refreshPercentages = function (_year) {
        var url = '/Reservations/ReservationsPercentageRefresh';
        var year = JSON.stringify({ year: _year })
        self.loadingPanel.show();
        $.ajax(url, {
            async: false,
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: year,
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Percentages(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
    };

    self.refreshChart = function () {
        var ctx = document.getElementById("myChart").getContext('2d');
        self.myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: self.Months,
                datasets: []
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                            min: 0,
                            max: 100
                        },
                        stacked: true
                    }],
                },
                onClick: self.refreshMonthlyChart
            }
        });

        self.Percentages().forEach(function (item, index) {
            var r = Math.floor(Math.random() * 255) % 256;
            var g = Math.floor(Math.random() * 255) % 256;
            var b = Math.floor(Math.random() * 255) % 256;
            var usedDataset = {
                label: item.Restaurant.Name,
                restaurant: item.Restaurant,
                data: item.Percentages,
                backgroundColor: 'rgba(' + r + ',' + g + ',' + b + ', 0.5)',
                borderColor: 'rgba(' + r + ',' + g + ',' + b + ', 1)',
                borderWidth: 1,
                stack: index
            };
            //var newDataset = {
            //    label: restaurant.Name,
            //    data: [12, 1, 19, 5, 3, 5, 4, 2, 3],
            //    backgroundColor: [
            //        'rgba(255, 99, 132, 0.2)',
            //        'rgba(54, 162, 235, 0.2)',
            //        'rgba(255, 206, 86, 0.2)',
            //        'rgba(75, 192, 192, 0.2)',
            //        'rgba(153, 102, 255, 0.2)',
            //        'rgba(255, 159, 64, 0.2)',
            //    ],
            //    borderColor: [
            //        'rgba(255,99,132,1)',
            //        'rgba(54, 162, 235, 1)',
            //        'rgba(255, 206, 86, 1)',
            //        'rgba(75, 192, 192, 1)',
            //        'rgba(153, 102, 255, 1)',
            //        'rgba(255, 159, 64, 1)'
            //    ],
            //    borderWidth: 1
            //};
            self.myChart.data.datasets.push(usedDataset);
        });
        self.myChart.update();
    };

    self.refreshMonthlyChart = function (event, data) {
        var activeElement = self.myChart.getElementAtEvent(event)[0];
        var restaurant = self.myChart.data.datasets[activeElement._datasetIndex].restaurant;
        var monthName = activeElement._model.label;
        var days;
        var year = self.SelectedYear()[0];

        switch (monthName) {
            case "January":
                month = 1;
                break;
            case "February":
                month = 2;
                break;
            case "March":
                month = 3;
                break;
            case "April":
                month = 4;
                break;
            case "May":
                month = 5;
                break;
            case "June":
                month = 6;
                break;
            case "Jully":
                month = 7;
                break;
            case "August":
                month = 8;
                break;
            case "September":
                month = 9;
                break;
            case "October":
                month = 10;
                break;
            case "November":
                month = 11;
                break;
            case "December":
                month = 11;
                break;
        }

        switch (month) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                days = 31;
                break;
            case 2:
                days = year % 4 == 0 ? 29 : 28;
                break;
            default:
                days = 30;
                break;
        }

        self.Days = [];
        for (var i = 1; i <= days; ++i)
            self.Days.push(i.toString());

        var url = '/Reservations/RestaurantPercentageRefresh';
        var info = JSON.stringify({
            restaurantId: restaurant.id,
            year: year,
            month: month
        })
        self.loadingPanel.show();
        $.ajax(url, {
            async: false,
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: info,
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.RestaurantPercentages(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        var ctx = document.getElementById("RestaurantMonthlyChart").getContext('2d');

        var r = Math.floor(Math.random() * 255) % 256;
        var g = Math.floor(Math.random() * 255) % 256;
        var b = Math.floor(Math.random() * 255) % 256;
        if (self.monthlyChart == null)
            self.monthlyChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: self.Days,
                    datasets: [{
                        label: restaurant.Name,
                        data: self.RestaurantPercentages(),
                        backgroundColor: 'rgba(' + r + ',' + g + ',' + b + ', 0.5)',
                        borderColor: 'rgba(' + r + ',' + g + ',' + b + ', 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                min: 0,
                                max: 100
                            }
                        }],
                    }
                }
            });
        else {
            self.monthlyChart.data.labels = self.Days;
            self.monthlyChart.data.datasets[0] = {
                label: restaurant.Name,
                data: self.RestaurantPercentages(),
                backgroundColor: 'rgba(' + r + ',' + g + ',' + b + ', 0.5)',
                borderColor: 'rgba(' + r + ',' + g + ',' + b + ', 1)',
                borderWidth: 1
            };
            self.monthlyChart.update();
        }
    };

    self.restaurantInfo = function () {
        var url = '/Reservations/RestaurantInfo';
        var restaurant = JSON.stringify({ restaurantId: self.RestaurantId() });
        var openingHour, closingHour;
        $.ajax(url, {
            async: false,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: restaurant,
            success: function (data) {
                openingHour = data.OpeningHour,
                closingHour = data.ClosingHour
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });
        var content = "Opening Hour: " + openingHour + "<br/>Closing Hour: " + closingHour;
        $("#RestaurantInfo").attr('data-content', content);
    };
};