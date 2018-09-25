using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using xml.task.Model.RastrManager;
using org.mariuszgromada.math.mxparser;

namespace xml.task.Model.Commands.SimpleCommands
{
    public class PlotCommand : Command
    {
        public List<Plot> Plots = new List<Plot>();
        public string Time;

        public PlotCommand()
        {
        }

        public PlotCommand(XElement xElement) : base(xElement)
        {
            Time = xElement?.Attribute(@"time")?.Value;
            foreach (var plotElement in xElement.Elements())
            {
                if (plotElement.Name.LocalName != @"plot")
                    continue;
                var plot = new Plot
                {
                    Name = plotElement.Attribute(@"name")?.Value
                };
                foreach (var curveElement in plotElement.Elements())
                {
                    if (curveElement.Name.LocalName != @"curve" && curveElement.Name.LocalName != @"arg")
                        continue;
                    var curve = new Curve
                    {
                        Name = curveElement.Attribute(@"name")?.Value,
                        Table = curveElement.Attribute(@"table")?.Value,
                        Column = curveElement.Attribute(@"column")?.Value,
                        Selection = curveElement.Attribute(@"selection")?.Value,
                        Formula = curveElement.Attribute(@"formula")?.Value,
                    };
                    if (curveElement.Name.LocalName == @"curve")
                        curve.Printable = true;
                    if (curveElement.Name.LocalName == @"arg")
                        curve.Printable = false;

                    plot.Curves.Add(curve);
                }
                Plots.Add(plot);
            }
        }

        public override void Perform()
        {
            base.Perform();
            var rastr = new RastrOperations();
            try
            {
                rastr.Load(Files.ToArray<string>());
            }
            catch (Exception exception)
            {
                Status = "Ошибка";
                ErrorMessage = $@"Ошибка загрузки файлов в Rastr. Сообщение: {exception.Message}";
                return;
            }
            if (Time != null)
                rastr.SetDynamicTime(Time);
            rastr.SetExitFileTemplate($@"""{Id}_{Name}_<count>.sna""");
            rastr.SetExitFilesDirectory($@"{Environment.CurrentDirectory}\exitfiles\");
            var result = rastr.RunDynamicWithExitFile();
            Status = result.IsSuccess ? (result.IsStable ? @"Устойчиво" : "Неустойчиво") : @"Ошибка расчета динамики";
            if (!result.IsSuccess)
                return;
            ResultMessage = $@"Сообщение Rustab: {result.ResultMessage}";

            Status = "Успешно";
            foreach (var plot in Plots)
            {
                var arguments = new List<Argument>();
                foreach (var curve in plot.Curves)
                {
                    if (curve.Printable == false)
                        arguments.Add(new Argument(curve.Name));
                    if (curve.Table != null && curve.Column != null && curve.Selection != null)
                        curve.Points = rastr.GetPointsFromExitFile(curve.Table, curve.Column, curve.Selection);
                    //if (curve.Points.Count != 0) continue;
                    //Status = "Ошибка";
                    //ErrorMessage = @"Выведены не все зависимости";
                }
                foreach (var formulaCurve in plot.Curves.Where(curve => curve.Formula != null))
                {
                    var pointsCount = plot.Curves.FirstOrDefault().Points.Count;
                    for (int i = 0; i < pointsCount; i++)
                    {
                        var x = plot.Curves.FirstOrDefault().Points[i].X;
                        foreach (var argument in arguments)
                            argument.setArgumentValue(plot.Curves.Where(curve => curve.Name == argument.getArgumentName()).FirstOrDefault().Points[i].Y);
                        var e = new org.mariuszgromada.math.mxparser.Expression(formulaCurve.Formula,arguments.ToArray());
                        formulaCurve.Points.Add(new Point(x, e.calculate()));
                    }
                }
            }

        }
    }

    public class Plot
    {
        public string Name;
        public List<Curve> Curves = new List<Curve>();
    }

    public class Curve
    {
        public string Name;
        public bool Printable;
        public string Table;
        public string Formula;
        public string Column;
        public string Selection;
        public List<Point> Points = new List<Point>();
    }
}
