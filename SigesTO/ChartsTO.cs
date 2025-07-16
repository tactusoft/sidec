using System.Collections.Generic;

namespace SigesTO
{
    public class ChartDataset
    {
        public string label { get; set; }
        public decimal[] data { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public int borderWidth { get; set; }
        public int hoverOffset { get; set; }
        public string stack { get; set; }
    }

    public class ChartTicks
    {
        public ChartFont font { get; set; }
    }

    public class ChartAxes
    {
        public bool beginAtZero { get; set; }
        public bool stacked { get; set; }
        public ChartTicks ticks { get; set; }
    }
    public class ChartScales
    {
        public ChartAxes x { get; set; }
        public ChartAxes y { get; set; }
    }

    public class ChartData
    {
        public string[] labels { get; set; }
        public ChartDataset[] datasets { get; set; }
    }
    public class ChartTitle
    {
        public bool display { get; set; }
        public string text { get; set; }
        public string color { get; set; }
    }

    public class ChartFont
    {
        public string size { get; set; }
    }

    public class ChartLabels
    {
        public ChartFont font { get; set; }
    }

    public class ChartLegend
    {
        public ChartLabels labels { get; set; }
    }

    public class ChartPlugins
    {
        public ChartLegend legend{ get; set; }
    }

    public class ChartOptions
    {
        public ChartScales scales { get; set; }
        public bool responsive { get; set; }
        public ChartTitle title { get; set; }
        public char? indexAxis { get; set; }
        public ChartPlugins plugins { get; set; }
    }

    public class Chart
    {
        public string type { get; set; }
        public ChartData data { get; set; }
        public ChartOptions options { get; set; }
    }
}
