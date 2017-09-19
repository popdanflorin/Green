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
    self.myChart = null;
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

    // for table
    self.TableRestaurantId;
    self.TableYear;
    self.TableMonth;

    // validation warnings
    self.warningRestaurantId = ko.observable();
    self.warningReservationDate = ko.observable();
    self.warningSeats = ko.observable();

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
                self.SelectedYear(self.SelectedYear());
                self.refreshReservations();
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

    self.open = function () {
        var date = new Date().getFullYear();
        self.Years.push(date);
        for (var i = 1; i <= 5; ++i) {
            self.Years.push(date - i);
            self.Years.push(date + i);
        }
        self.Years.sort().reverse();
        document.getElementById("ChartYear").value = date;
        self.SelectedYear(date);

        self.refresh();
    }

    self.refresh = function () {
        self.refreshPercentages(self.SelectedYear());
        self.refreshChart();
    };

    self.refreshReservations = function () {
        self.loadingPanel.show();
        var url = '/Reservations/RefreshReservations';
        var info = JSON.stringify({
            restaurantId: self.TableRestaurantId,
            year: self.TableYear,
            month: self.TableMonth
        });
        $.ajax(url, {
            async: false,
            type: "post",
            data: info,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.loadingPanel.hide();
                console.log(data);
                self.Reservations(data.Reservations);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            }
        });

        $("#ReservationsTable").show();
    }

    self.refreshPercentages = function (_year) {
        var url = '/Reservations/ReservationsPercentageRefresh';
        var year = JSON.stringify({ year: _year });
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
                            max: 100,
                            callback: function (value) {
                                return value + "%"
                            }
                        },
                        scaleLabel: {
                            display: true,
                            labelString: "Percentage"
                        },
                        stacked: true
                    }],
                    xAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: "Month"
                        }
                    }]
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
        try {
            var activeElement = self.myChart.getElementAtEvent(event)[0];
            var restaurant = self.myChart.data.datasets[activeElement._datasetIndex].restaurant;
            var monthName = activeElement._model.label;
            var backgroundColor = activeElement._model.backgroundColor;
            var borderColor = activeElement._model.borderColor;
        }
        catch (e) {
            return false;
        }
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
        if (self.monthlyChart == null)
            self.monthlyChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: self.Days,
                    datasets: [{
                        label: "Percentage",
                        data: self.RestaurantPercentages(),
                        backgroundColor: backgroundColor,
                        borderColor: borderColor,
                        borderWidth: 1
                    }]
                },
                options: {
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                min: 0,
                                max: 100,
                                callback: function (value) {
                                    return value + "%"
                                }
                            },
                            scaleLabel: {
                                display: true,
                                labelString: "Percentage"
                            }
                        }],
                        xAxes: [{
                            scaleLabel: {
                                display: true,
                                labelString: "Day"
                            }
                        }]
                    }
                }
            });
        else {
            self.monthlyChart.data.labels = self.Days;
            self.monthlyChart.data.datasets[0] = {
                //label: restaurant.Name,
                data: self.RestaurantPercentages(),
                backgroundColor: backgroundColor,
                borderColor: borderColor,
                borderWidth: 1
            };
            self.monthlyChart.update();
        }
        $("#RestaurantMonthlyChartTitle").text(restaurant.Name + "'s reserved seats percentage for " + monthName + " " + year);
        $("#MonthlyChart").show();

        self.TableRestaurantId = restaurant.id;
        self.TableYear = year;
        self.TableMonth = month;
        self.refreshReservations();
    };
};