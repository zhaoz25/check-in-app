using CheckInApp.controller;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    class OxyExData
    {
        public PlotModel PieModel { get; set; }
        public PlotModel AreaModel { get; set; }
        public PlotModel BarModel { get; set; }
        public PlotModel StackedBarModel { get; set; }
        public PlotModel ChartModel { get; set; }

        public OxyExData()
        {

        }
        public async Task<bool> CreatePieChart(string title,string token,int month)
        {
            var model = new PlotModel { Title = title };

            var ps = new PieSeries
            {
                StrokeThickness = .25,
                InsideLabelPosition = .25,
                AngleSpan = 360,
                StartAngle = 0
            };
            // gửi request lấy dữ liệu
            ResultAndChartData rs = new ResultAndChartData();
            UserHandler handler = new UserHandler();

            rs = await handler.GetLogForChart(token, month);
            if (rs.result.error == true)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < rs.chartDataList.Count; i++)
                {
                    ps.Slices.Add(new PieSlice(rs.chartDataList[i].lastName, rs.chartDataList[i].countCheckIn));
                }
                model.Series.Add(ps);
                ChartModel = model;

                return true;
            }
            
        }

        public async Task<bool> CreateBarChart(bool stacked, string title,string token,int month)
        {
            var model = new PlotModel
            {
                Title = title,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            // gửi request lấy dữ liệu
            ResultAndChartData rs = new ResultAndChartData();
            UserHandler handler = new UserHandler();

            rs = await handler.GetLogForChart(token,month);
            if(rs.result.error == true)
            {
                return false;
            }
            else
            {
                var s1 = new ColumnSeries { Title = "Check In", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
                for (int i = 0; i < rs.chartDataList.Count; i++)
                {
                    s1.Items.Add(new ColumnItem { Value = rs.chartDataList[i].countCheckIn });
                }

                var s2 = new ColumnSeries { Title = "Check Out", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
                for (int i = 0; i < rs.chartDataList.Count; i++)
                {
                    s2.Items.Add(new ColumnItem { Value = rs.chartDataList[i].countCheckOut });
                }

                var categoryAxis = new CategoryAxis { Position = CategoryAxisPosition() };
                for (int i = 0; i < rs.chartDataList.Count; i++)
                {
                    categoryAxis.Labels.Add(rs.chartDataList[i].lastName);
                }

                var valueAxis = new LinearAxis { Position = ValueAxisPosition(), MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
                model.Series.Add(s1);
                model.Series.Add(s2);
                model.Axes.Add(categoryAxis);
                model.Axes.Add(valueAxis);

                ChartModel = model;

                return true;
            }
            
        }

        private AxisPosition CategoryAxisPosition()
        {
            if (typeof(BarSeries) == typeof(ColumnSeries))
            {
                return AxisPosition.Bottom;
            }

            return AxisPosition.Bottom;
        }

        private AxisPosition ValueAxisPosition()
        {
            if (typeof(BarSeries) == typeof(ColumnSeries))
            {
                return AxisPosition.Left;
            }

            return AxisPosition.Left;
        }
    }
}
