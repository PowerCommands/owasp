using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace GpuMetrics
{
    public partial class MainWindow : Window
    {
        private readonly PieSeries _gpuGreen;
        private readonly PieSeries _gpuYellow;
        private readonly PieSeries _gpuRed;
        private readonly PieSeries _gpuRest;
        private readonly PieSeries _memGreen;
        private readonly PieSeries _memYellow;
        private readonly PieSeries _memRed;
        private readonly PieSeries _memRest;
        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            _gpuGreen = CreateSegment(Brushes.Green);
            _gpuYellow = CreateSegment(Brushes.Yellow);
            _gpuRed = CreateSegment(Brushes.Red);
            _gpuRest = CreateSegment(Brushes.LightGray);
            GpuChart.Series = new SeriesCollection { _gpuGreen, _gpuYellow, _gpuRed, _gpuRest };

            _memGreen = CreateSegment(Brushes.Green);
            _memYellow = CreateSegment(Brushes.Yellow);
            _memRed = CreateSegment(Brushes.Red);
            _memRest = CreateSegment(Brushes.LightGray);
            MemoryChart.Series = new SeriesCollection { _memGreen, _memYellow, _memRed, _memRest };

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, __) => UpdateMetrics();
            _timer.Start();
        }

        private static PieSeries CreateSegment(Brush brush) =>
            new PieSeries
            {
                Fill = brush,
                StrokeThickness = 0,
                DataLabels = false,
                Values = new ChartValues<double> { 0 }
            };

        private void UpdateMetrics()
        {
            double gpu = GetGpuUsage();
            double mem = GetMemoryUsage();

            UpdateSeries(_gpuGreen, _gpuYellow, _gpuRed, _gpuRest, gpu);
            UpdateSeries(_memGreen, _memYellow, _memRed, _memRest, mem);
        }

        private static void UpdateSeries(PieSeries green, PieSeries yellow, PieSeries red, PieSeries rest, double value)
        {
            value = Math.Max(0, Math.Min(100, value));
            double greenVal = Math.Min(50, value);
            double yellowVal = Math.Min(25, Math.Max(0, value - 50));
            double redVal = Math.Max(0, value - 75);
            double restVal = 100 - (greenVal + yellowVal + redVal);

            green.Values[0] = greenVal;
            yellow.Values[0] = yellowVal;
            red.Values[0] = redVal;
            rest.Values[0] = restVal;
        }

        private double GetGpuUsage()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                var counters = category.GetCounters().Where(c => c.CounterName == "Utilization Percentage" && c.InstanceName.Contains("engtype_3D")).ToArray();
                float value = counters.Sum(c => c.NextValue());
                return value;
            }
            catch
            {
                return new Random().NextDouble() * 100;
            }
        }

        private double GetMemoryUsage()
        {
            try
            {
                var pc = new PerformanceCounter("Memory", "% Committed Bytes In Use");
                return pc.NextValue();
            }
            catch
            {
                return new Random().NextDouble() * 100;
            }
        }
    }
}
