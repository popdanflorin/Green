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
    self.Percentages = ko.observableArray(null);
    self.RestaurantPercentages = ko.observableArray();
    self.SelectedYear = ko.observable(null);
    self.SelectedYearChanged = ko.computed(function () {
        if (!self.doNotHide) {
            $("#ReservationsTable").hide();
            $("#MonthlyChart").hide();
        }
        if (self.SelectedYear() == null || self.SelectedYear() == undefined || typeof self.SelectedYear() === 'undefined')
            return;
        try {
            self.refreshPercentages(self.SelectedYear()[0]);
            self.Percentages().forEach(function (item, index) {
                self.myChart.data.datasets[index].data = item.Percentages;
            });
            self.myChart.update();
        }
        catch (e) {
        }
        finally {
            self.doNotHide = (false);
        }
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

    // for reservations table
    self.TableRestaurant;
    self.TableYear;
    self.TableMonth;
    self.TableMonthName;

    // validation warnings
    self.warningRestaurantId = ko.observable();
    self.warningReservationDate = ko.observable();
    self.warningSeats = ko.observable();

    // SignalR
    self.doNotHide = false;
    self.Message = ko.observable();

    self.initSignalR = function () {
        $.connection.hub.start()
            .done(function () {
                console.log("SignalR initialization success!");
            })
            .fail(function () {
                console.log("SignalR initialization error!");
            })
    }

    $.connection.dataHub.client.notifyReservationChange = function (message) {
        console.log("SignalR notifyNewReservation");
        self.Message(message);
        self.showSnackbar();
        self.doNotHide = true;
        var isVisible = $("#MonthlyChart").is(':visible')
        var temp = self.SelectedYear();
        self.SelectedYear(temp);
        if (isVisible)
            self.refreshMonthlyChartAfterDelete();
    };

    self.showSnackbar = function () {
        var x = document.getElementById("ReservationsSnackbar")
        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 2000);
    };
    // end SignalR related functions

    self.delete = function (data) {
        if (!window.confirm("Are you sure you want to cancel this reservation?")) {
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
                self.refreshMonthlyChartAfterDelete();
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
        if (typeof self.SelectedYear() === 'undefined' || self.SelectedYear() == null)
            return;
        self.refreshPercentages(self.SelectedYear());
        self.refreshChart();
    };

    self.refreshReservations = function () {
        self.loadingPanel.show();
        var url = '/Reservations/RefreshReservations';
        var info = JSON.stringify({
            restaurantId: self.TableRestaurant.id,
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

        $("#ReservationsTableTitle").text(self.TableRestaurant.Name + "'s reservations for " + self.TableMonthName + " " + self.TableYear);
        $("#ReservationsTable").show();
    };

    self.refreshPercentages = function (_year) {
        if (typeof _year == "undefined" || _year == null)
            return;
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

    self.refreshRestaurantPercentages = function (restaurant, year, month) {
        self.setDays(month, year);
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

        var year = self.SelectedYear()[0];
        var month = self.getMonthByName(monthName);
        self.setDays(month, year);
        self.refreshRestaurantPercentages(restaurant, year, month);

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

        self.TableRestaurant = restaurant;
        self.TableYear = year;
        self.TableMonth = month;
        self.TableMonthName = monthName;
        self.refreshReservations();
    };

    self.refreshMonthlyChartAfterDelete = function () {
        self.refreshRestaurantPercentages(self.TableRestaurant, self.TableYear, self.TableMonth);
        self.refreshReservations();
        self.monthlyChart.data.datasets[0].data = self.RestaurantPercentages();
        self.monthlyChart.update();
        $("#ReservationsTable").show();
        $("#MonthlyChart").show();
    };

    self.isComplete = function (data) {
        var date = new Date(moment(data).format("YYYY-MM-DDTHH:mm:ss"));
        var today = new Date();
        console.log(date < today);
        return date < today;
    };

    // calculate
    self.getMonthByName = function (monthName) {
        var month;
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
        return month;
    };

    self.setDays = function (month, year) {
        var days;
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
    };
};