using System;
using System.Linq;
using System.Xml.Linq;
using xml.task.Model.RastrManager;

namespace xml.task.Model.Commands.SimpleCommands
{
    public class DynamicCommand : Command
    {
        public string Time { get; set; }

        public DynamicCommand()
        {
        }

        public DynamicCommand(XElement xElement) : base(xElement)
        {
            Time = xElement?.Attribute(@"time")?.Value;
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
            var result = rastr.RunDynamic();
            Status = result.IsSuccess ? (result.IsStable ? @"Устойчиво" : "Неустойчиво") : @"Ошибка расчета динамики";
            ResultMessage = $@"Сообщение Rustab: {result.ResultMessage}";
        }
    }
}
