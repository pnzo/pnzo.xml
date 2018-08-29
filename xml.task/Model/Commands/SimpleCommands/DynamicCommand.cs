using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml.task.Model.RastrManager;

namespace xml.task.Model.Commands
{
    public class DynamicCommand : Command
    {
        public DynamicCommand() : base()
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
                ResultMessage = $@"Не удалось загрузить исходные файлы в рабочее пространство RastrWIN. 
Сообщение: {exception.Message}";
                return;
            }

            var result = rastr.RunDynamic();

            Success = result.IsSuccess;
            var calculationString = Success ? (result.IsStable ? @"Устойчиво" : "Неустойчиво") : @"Ошибка расчета";
            ResultMessage = $@"Файл динамики: Rst
Файл сценария: Scn
Результат: {calculationString}
Сообщение Rustab: {result.ResultMessage}";
        }
    }
}
