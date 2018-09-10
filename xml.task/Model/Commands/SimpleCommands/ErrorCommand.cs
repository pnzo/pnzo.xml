using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xml.task.Model.Commands.SimpleCommands
{
    internal class ErrorCommand : Command
    {
        public ErrorCommand()
        {
        }

        public ErrorCommand(XElement xElement) : base(xElement)
        {
        }

        public override void Perform()
        {
            base.Perform();
            Status = @"Ошибка формата задания";
            ErrorMessage = $@"Не найдена соответствующая реализация для команды {Name}";
        }
    }
}
