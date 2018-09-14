using OxyPlot;
using OxyPlot.Axes;
//using OxyPlot.Wpf;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using xml.task.Model.Commands;
using xml.task.Model.Commands.SimpleCommands;

namespace xml.task.Windows
{
    /// <summary>
    /// Логика взаимодействия для PlotWindow.xaml
    /// </summary>
    public partial class PlotWindow : Window
    {
        public PlotCommand command;
        public PlotWindow(Command command)
        {
            this.command = command as PlotCommand;
            InitializeComponent();
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < command.Plots.Count; i++)
            {
                var plot = command.Plots[i];
                MainGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star),
                });
                var plotView = new OxyPlot.Wpf.PlotView();
                plotView.Model = GetPlotModel(plot);
                Grid.SetRow(plotView, i);
                MainGrid.Children.Add(plotView);

            }

        }

        private PlotModel GetPlotModel(Model.Commands.SimpleCommands.Plot plot)
        {
            var model = new PlotModel
            {
                Title = plot.Name,
                Subtitle = plot.Curves.Count.ToString(),
            };

            var xAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                IsZoomEnabled = true,
                MajorGridlineStyle = LineStyle.Solid,
            };
            var yAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                IsZoomEnabled = true,
                MajorGridlineStyle = LineStyle.Solid,
            };



            foreach (var curve in plot.Curves)
            {
                if (curve.points.Count == 0)
                    continue;               
                var series = new LineSeries
                {
                    Title = curve.Name,
                };
                foreach (var point in curve.points)
                {
                    series.Points.Add(new DataPoint(point.X, point.Y));
                }
                model.Series.Add(series);
                var max = curve.points.Max(k => k.X);
                var min = curve.points.Min(k => k.X);
                xAxis.AbsoluteMinimum = min > xAxis.AbsoluteMinimum ? min : xAxis.AbsoluteMinimum;
                xAxis.AbsoluteMaximum = max < xAxis.AbsoluteMaximum ? max : xAxis.AbsoluteMaximum;
            }
            model.Axes.Add(xAxis);
            model.Axes.Add(yAxis);
            return model;
        }
    }
}
