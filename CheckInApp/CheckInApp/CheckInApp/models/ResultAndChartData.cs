using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInApp.models
{
    public class ResultAndChartData
    {
        public Result result { get; set; }
        public List<ChartData> chartDataList { get; set; }

        public ResultAndChartData()
        {
            result = new Result();
            chartDataList = new List<ChartData>();
        }
    }
}
