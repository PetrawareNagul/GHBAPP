<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statistic.aspx.cs" Inherits="Silverlake.Web.Statistic" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!--Morris Chart CSS -->
    <link rel="stylesheet" href="http://bucketadmin.themebucket.net/js/morris-chart/morris.css" />
</head>
<body>

    <form id="form1" runat="server">


        <div class="container-fluid">

            <hr />

            <!--mini statistics start-->
            <div class="row">
                <div class="col-md-3" style="height: 200px">
                    <section class="panel">
                        <div class="panel-body">
                            <div class="top-stats-panel">
                                <div class="gauge-canvas">
                                    <h4 class="widget-h">SAN Drive Memory</h4>
                                    <canvas width="160" height="100" id="gauge"></canvas>
                                </div>
                                <ul class="gauge-meta clearfix">
                                    <li id="gauge-textfield" class="pull-left gauge-value"></li>
                                    <li class="pull-right gauge-title">
                                        <asp:Label ID="lbltotalSize1" runat="server" ClientIDMode="Static"></asp:Label>
                                        - GB </li>
                                </ul>
                            </div>
                        </div>
                    </section>
                </div>
                <div class="col-md-3" style="height: 200px">
                    <section class="panel">
                        <div class="panel-body">
                            <div class="top-stats-panel">
                                <div class="daily-visit">
                                    <h4 class="widget-h">Today Scan</h4>
                                    <canvas id="bar-chart-js1" height="120" width="200" style="width: 200px; height: 120px;"></canvas>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
                <div class="col-md-3" style="height: 200px">
                    <section class="panel">
                        <div class="panel-body">
                            <div class="top-stats-panel">
                                <h4 class="widget-h">Today Transfer</h4>
                                <div class="chartJS" style="height: 150px;">
                                    <canvas id="donut-chart-js1" height="150" width="150" style="width: 150px; height: 150px;"></canvas>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
                <div class="col-md-3" style="height: 200px">
                    <section class="panel">
                        <div class="panel-body">
                            <div class="top-stats-panel">
                                <h4 class="widget-h">Total Applications</h4>
                                <h1 id="fun-level" style="color: red; text-align: center" class="bg-warning"></h1>
                                <%--<img width="230px" height="80px" src="https://www.votiro.com/wp-content/uploads/2018/03/votiro.gif" />--%>

                                <hr />

                                <div id="divDepartments" runat="server">
                                
                                    </div>

                            </div>
                            <%--<div class="top-stats-panel">
                    <h4 class="widget-h">Daily Sales</h4>
                    <div class="bar-stats">
                        <ul class="progress-stat-bar clearfix">
                            <li data-percent="50%"><span class="progress-stat-percent pink"></span></li>
                            <li data-percent="90%"><span class="progress-stat-percent"></span></li>
                            <li data-percent="70%"><span class="progress-stat-percent yellow-b"></span></li>
                        </ul>
                        <ul class="bar-legend">
                            <li><span class="bar-legend-pointer pink"></span> New York</li>
                            <li><span class="bar-legend-pointer green"></span> Los Angels</li>
                            <li><span class="bar-legend-pointer yellow-b"></span> Dallas</li>
                        </ul>
                        <div class="daily-sales-info">
                            <span class="sales-count">1200 </span> <span class="sales-label">Products Sold</span>
                        </div>
                    </div>
                </div>--%>
                        </div>
                    </section>
                </div>
            </div>
            <!--mini statistics end-->

            <div class="row">
                <div class="col-sm-12">
                    <section class="panel">
                        <header class="panel-heading">
                            Bar Chart
                        <span class="tools pull-right">
                            <a href="javascript:;" class="fa fa-chevron-down"></a>
                            <a href="javascript:;" class="fa fa-cog"></a>
                            <a href="javascript:;" class="fa fa-times"></a>
                        </span>
                        </header>
                        <div class="panel-body">
                            <div class="chartJS" style="height: 266px;">
                                <canvas id="bar-chart-js" height="262" width="1049" style="width: 1049px; height: 262px;"></canvas>
                            </div>
                            <div style="display: none" id="divDayCount" runat="server"></div>

                            <div style="display: none" id="divtoDayCount" runat="server"></div>

                        </div>
                    </section>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-6">
                    <section class="panel">
                        <header class="panel-heading">
                            Total Application Chart
                        </header>
                        <div class="panel-body">
                            <div class="chartJS" style="height: 274px;">
                                <canvas id="pie-chart-js" height="270" width="495" style="width: 495px; height: 270px;"></canvas>
                            </div>
                        </div>
                    </section>
                    <div id="divTotal" runat="server">
                    </div>
                </div>

                <div class="col-sm-6">
                    <section class="panel">
                        <header class="panel-heading">
                            Last Month Application Chart
                        </header>
                        <div class="panel-body">
                            <div class="chartJS" style="height: 274px;">
                                <canvas id="donut-chart-js" height="270" width="495" style="width: 495px; height: 270px;"></canvas>
                            </div>
                        </div>
                    </section>
                    <div id="divMonth" runat="server">
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="lblTotal" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="lblTotalColors" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="lblDays" ClientIDMode="Static" runat="server" />

        <asp:HiddenField ID="lblMonth" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="lblMonthColors" ClientIDMode="Static" runat="server" />

        <%--<asp:HiddenField ID="lbltotalSize" ClientIDMode="Static" runat="server" />--%>
        <asp:HiddenField ID="lblfreeSpace" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="lblTotalApplication" ClientIDMode="Static" runat="server" />

        <asp:HiddenField ID="lblTodayCount" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="lblTodayColor" ClientIDMode="Static" runat="server" />

        <asp:HiddenField ID="lblTodayScan" ClientIDMode="Static" runat="server" />
    </form>
</body>
</html>
<script src="/Content/js/gauge/gauge.js"></script>
<!--jQuery Flot Chart-->
<script src="/Content/js/flot-chart/jquery.flot.js"></script>

<script>

    /**
 * Created by westilian on 1/19/14.
 */

    (function () {
        var t;
        function size(animate) {
            if (animate == undefined) {
                animate = false;
            }
            clearTimeout(t);
            t = setTimeout(function () {
                $("canvas").each(function (i, el) {
                    $(el).attr({
                        "width": $(el).parent().width(),
                        "height": $(el).parent().outerHeight()
                    });
                });
                redraw(animate);
                var m = 0;
                $(".chartJS").height("");
                $(".chartJS").each(function (i, el) { m = Math.max(m, $(el).height()); });
                $(".chartJS").height(m);
            }, 30);
        }
        $(window).on('resize', function () { size(false); });


        function redraw(animation) {
            var options = {};
            if (!animation) {
                options.animation = false;
            } else {
                options.animation = true;
            }

            // barChart
            var daysObj = [];
            var days = $('#lblDays').val().split(',');
            for (var i = 0; i < days.length; i++) {
                daysObj.push(days[i]);
            }
            var barChartObj = [];
            $('#tblDays tr').each(function (index, values) {
                var dayObj = [];
                var day = $(this).find('#lbldepartmentsetcount').text().split(',');
                for (var i = 0; i < day.length; i++) {
                    dayObj.push(day[i]);
                }

                barChartObj.push({
                    fillColor: "#" + $(this).find('#departmentcolor').text().trim(),
                    strokeColor: "#" + $(this).find('#departmentcolor').text().trim(),
                    data: dayObj
                });

            });

            var barChartData = {
                labels: daysObj,
                datasets: barChartObj
            }
            var myLine = new Chart(document.getElementById("bar-chart-js").getContext("2d")).Bar(barChartData);

            // end bar Chart

            // var todayChartData = [];
            debugger
            var todayChartObj = [];
            var todayChartData = [];
            $('#tbltoDays tr').each(function (index, values) {
                debugger;
                var todaysObj = [];
                var day = $(this).find('#lbldepartmentsetcount').text().split(',');
                todaysObj.push(parseInt(day));

                todayChartObj.push({
                    fillColor: "#" + $(this).find('#departmentcolor').text().trim(),
                    strokeColor: "#" + $(this).find('#departmentcolor').text().trim(),
                    data: todaysObj
                });
            });

            if (todayChartObj.length == 0) {
                todayChartData = {
                    labels: ["Today Scan"],
                    datasets: [
                        {
                            fillColor: "#E1EB5A",
                            strokeColor: "#E1EB5A",
                            data: [0]
                        },
                        {
                            fillColor: "#9DEB5A",
                            strokeColor: "#9DEB5A",
                            data: [0]
                        },
                        {
                            fillColor: "#95A5A6",
                            strokeColor: "#95A5A6",
                            data: [0]
                        },
                        {
                            fillColor: "#E75EC0",
                            strokeColor: "#E75EC0",
                            data: [0]
                        },
                        {
                            fillColor: "#5D6D7E",
                            strokeColor: "#5D6D7E",
                            data: [0]
                        }
                    ]
                }
            }
            else {
                todayChartData = {
                    labels: ["Today Scan"],
                    datasets: todayChartObj
                }
            }



            //var barChartData1 = {
            //    labels: ["Today Scan"],
            //    datasets: [
            //        {
            //            fillColor: "#E1EB5A",
            //            strokeColor: "#E1EB5A",
            //            data: [65]
            //        },
            //        {
            //            fillColor: "#EB6D5A",
            //            strokeColor: "#EB6D5A",
            //            data: [28]
            //        },
            //        {
            //            fillColor: "#9DEB5A",
            //            strokeColor: "#9DEB5A",
            //            data: [90]
            //        },
            //        {
            //            fillColor: "#98F5D3",
            //            strokeColor: "#98F5D3",
            //            data: [90]
            //        },
            //        {
            //            fillColor: "#A786F2",
            //            strokeColor: "#A786F2",
            //            data: [10]
            //        }
            //    ]

            //}
            debugger;
            var myLine1 = new Chart(document.getElementById("bar-chart-js1").getContext("2d")).Bar(todayChartData);

            // Total sets pie chat
            var totalCounts = []
            totalCounts = $('#lblTotal').val().split(',');
            var totalColors = $('#lblTotalColors').val().split(',');

            var arrayObj = [];

            for (var i = 0; i < totalCounts.length; i++) {
                arrayObj.push({
                    value: parseInt(totalCounts[i]),
                    color: '#' + totalColors[i]
                });
            }

            var myPie = new Chart(document.getElementById("pie-chart-js").getContext("2d")).Pie(arrayObj);
            //end pie chat


            // Total sets pie chat
            var monthCounts = []
            monthCounts = $('#lblMonth').val().split(',');
            var monthColors = $('#lblMonthColors').val().split(',');

            var monthObj = [];

            for (var i = 0; i < monthCounts.length; i++) {
                monthObj.push({
                    value: parseInt(monthCounts[i]),
                    color: '#' + monthColors[i]
                });
            }

            var myDonut = new Chart(document.getElementById("donut-chart-js").getContext("2d")).Doughnut(monthObj);

            //end pie chat


            // Today sets pie chat
            var todayCounts = []
            todayCounts = $('#lblTodayCount').val().split(',');
            var todayColors = $('#lblTodayColor').val().split(',');

            var todayObj = [];

            for (var i = 0; i < todayCounts.length; i++) {
                todayObj.push({ 
                    value: parseInt(todayCounts[i]),
                    color: '#' + todayColors[i]
                });
            }

            var myDonut = new Chart(document.getElementById("donut-chart-js1").getContext("2d")).Doughnut(todayObj);
            //end pie chat


            //var donutData = [
            //    {
            //        value: 30,
            //        color: "#E67A77"
            //    },
            //    {
            //        value: 50,
            //        color: "#D9DD81"
            //    },
            //    {
            //        value: 100,
            //        color: "#79D1CF"
            //    },
            //    {
            //        value: 40,
            //        color: "#95D7BB"
            //    },
            //    {
            //        value: 120,
            //        color: "#4D5360"
            //    }

            //]
            //var myDonut1 = new Chart(document.getElementById("donut-chart-js1").getContext("2d")).Doughnut(donutData);

        }
        setTimeout(size(true), 500);

        $(document).ready(function () {
            if (Gauge) {
                /*Knob*/
                var opts = {
                    lines: 12, // The number of lines to draw
                    angle: 0, // The length of each line
                    lineWidth: 0.48, // The line thickness
                    pointer: {
                        length: 0.6, // The radius of the inner circle
                        strokeWidth: 0.03, // The rotation offset
                        color: '#464646' // Fill color
                    },
                    limitMax: 'true', // If true, the pointer will not go past the end of the gauge
                    colorStart: '#fa8564', // Colors
                    colorStop: '#fa8564', // just experiment with them
                    strokeColor: '#F1F1F1', // to see which ones work best for you
                    generateGradient: true
                };


                var target = document.getElementById('gauge'); // your canvas element
                var gauge = new Gauge(target).setOptions(opts); // create sexy gauge!
                //  gauge.maxValue = 3000; // set max gauge value
                gauge.maxValue = parseInt($('#lbltotalSize1').text()); // set max gauge value

                gauge.animationSpeed = 32; // set animation speed (32 is default value)
                gauge.set(parseInt($('#lblfreeSpace').val())); // set actual value
                gauge.setTextField(document.getElementById("gauge-textfield"));

            }

            if ($.fn.plot) {

                var d1 = [
                    [0, 10],
                    [1, 20],
                    [2, 33],
                    [3, 24],
                    [4, 45],
                    [5, 96],
                    [6, 47],
                    [7, 18],
                    [8, 11],
                    [9, 13],
                    [10, 21]

                ];
                var data = ([{
                    label: "Too",
                    data: d1,
                    lines: {
                        show: true,
                        fill: true,
                        lineWidth: 2,
                        fillColor: {
                            colors: ["rgba(255,255,255,.1)", "rgba(160,220,220,.8)"]
                        }
                    }
                }]);
                var options = {
                    grid: {
                        backgroundColor: {
                            colors: ["#fff", "#fff"]
                        },
                        borderWidth: 0,
                        borderColor: "#f0f0f0",
                        margin: 0,
                        minBorderMargin: 0,
                        labelMargin: 20,
                        hoverable: true,
                        clickable: true
                    },
                    // Tooltip
                    tooltip: true,
                    tooltipOpts: {
                        content: "%s X: %x Y: %y",
                        shifts: {
                            x: -60,
                            y: 25
                        },
                        defaultTheme: false
                    },

                    legend: {
                        labelBoxBorderColor: "#ccc",
                        show: false,
                        noColumns: 0
                    },
                    series: {
                        stack: true,
                        shadowSize: 0,
                        highlightColor: 'rgba(30,120,120,.5)'

                    },
                    xaxis: {
                        tickLength: 0,
                        tickDecimals: 0,
                        show: true,
                        min: 2,

                        font: {

                            style: "normal",


                            color: "#666666"
                        }
                    },
                    yaxis: {
                        ticks: 3,
                        tickDecimals: 0,
                        show: true,
                        tickColor: "#f0f0f0",
                        font: {

                            style: "normal",


                            color: "#666666"
                        }
                    },
                    //        lines: {
                    //            show: true,
                    //            fill: true
                    //
                    //        },
                    points: {
                        show: true,
                        radius: 2,
                        symbol: "circle"
                    },
                    colors: ["#87cfcb", "#48a9a7"]
                };
                // var plot = $.plot($("#daily-visit-chart"), data, options);
            }
        });
    }());
</script>

<script>


    function animateNumber(ele, no, stepTime) {
        $({ someValue: 0 }).animate({ someValue: no }, {
            duration: stepTime,
            step: function () { // called on every step. Update the element's text with value:
                ele.text(Math.floor(this.someValue + 1));
            },
            complete: function () {
                ele.text(no);
                $('#fun-level').css("color", "#44E92B");
            }
        });
    }

    animateNumber($('#fun-level'), $('#lblTotalApplication').val(), 10000);


    //var percent_number_step = $.animateNumber.numberStepFactories.append(' %')
    //$('#fun-level').animateNumber(
    //    {
    //        number: 100,
    //        color: 'green',
    //        'font-size': '30px',

    //        easing: 'easeInQuad',

    //        numberStep: percent_number_step
    //    },
    //    15000
    //);
</script>

<%--<script src="http://bucketadmin.themebucket.net/js/dashboard.js"></script>--%>
