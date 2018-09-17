using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using xml.task.Model.RastrManager;

namespace xml.task.Model.Commands.SimpleCommands
{
    public class PlotCommand : Command
    {
        public List<Plot> Plots = new List<Plot>();

        public PlotCommand()
        {
        }

        public PlotCommand(XElement xElement) : base(xElement)
        {
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
                    if (curveElement.Name.LocalName != @"curve")
                        continue;
                    var curve = new Curve
                    {
                        Name = curveElement.Attribute(@"name")?.Value,
                        Table = curveElement.Attribute(@"table")?.Value,
                        Column = curveElement.Attribute(@"column")?.Value,
                        Selection = curveElement.Attribute(@"selection")?.Value,
                    };
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

            rastr.SetExitFileTemplate($@"""{Id}_{Name}""");
            rastr.SetExitFilesDirectory($@"{Environment.CurrentDirectory}\exitfiles\");
            var result = rastr.RunDynamicWithExitFile();
            Status = result.IsSuccess ? (result.IsStable ? @"Устойчиво" : "Неустойчиво") : @"Ошибка расчета динамики";
            if (!result.IsSuccess)
                return;
            ResultMessage = $@"Сообщение Rustab: {result.ResultMessage}";

            Status = "Успешно";
            foreach (var plot in Plots)
            {
                foreach (var curve in plot.Curves)
                {
                    try
                    {
                        curve.Points = rastr.GetPointsFromExitFile(curve.Table, curve.Column, curve.Selection);
                        if (curve.Points.Count != 0) continue;
                        Status = "Ошибка";
                        ErrorMessage = @"Выведены не все зависимости";
                    }
                    catch (Exception exception)
                    {
                        Status = "Ошибка";
                        ErrorMessage = $@"Ошибка вывода зависимости. {exception.Message}";
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
        public string Table;
        public string Column;
        public string Selection;
        public List<Point> Points;
    }
}
