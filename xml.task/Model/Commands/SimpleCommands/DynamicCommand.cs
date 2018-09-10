using System;
using System.Linq;
using System.Xml.Linq;
using xml.task.Model.RastrManager;

namespace xml.task.Model.Commands.SimpleCommands
{
    public class DynamicCommand : Command
    {
        public DynamicCommand()
        {
        }

        public DynamicCommand(XElement xElement) : base(xElement)
        {
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

            var result = rastr.RunDynamic();
            Status = result.IsSuccess ? (result.IsStable ? @"Устойчиво" : "Неустойчиво") : @"Ошибка расчета динамики";
            ResultMessage = $@"Сообщение Rustab: {result.ResultMessage}";
        }
    }
}
