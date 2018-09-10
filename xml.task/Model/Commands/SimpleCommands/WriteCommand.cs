using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml.task.Model.RastrManager;

namespace xml.task.Model.Commands.SimpleCommands
{
    internal class WriteCommand : Command
    {
        public string Table;
        public string Column;
        public string Selection;
        public string Value;
        public string File;

        public WriteCommand()
        {
        }

        public WriteCommand(XElement xElement) : base(xElement)
        {
            Table = xElement?.Attribute(@"table")?.Value;
            Column = xElement?.Attribute(@"column")?.Value;
            Selection = xElement?.Attribute(@"selection")?.Value;
            Value = xElement?.Attribute(@"value")?.Value;
            File = xElement?.Attribute(@"file")?.Value;
        }

        public override void Perform()
        {
            base.Perform();
            var rastr = new RastrOperations();
            try
            {
                rastr.Load(File);
            }
            catch (Exception exception)
            {
                Status = @"Ошибка";
                ErrorMessage = $@"Ошибка загрузки {File} в Rastr. Сообщение: {exception.Message}";
                return;
            }

            try
            {
                rastr.SetValue(Table, Column, Selection, Value);
                rastr.Save(File);
            }
            catch (Exception exception)
            {
                Status = @"Ошибка";
                ErrorMessage = $@"Ошибка коррекции. Сообщение: {exception.Message}";
                return;
            }
            Status = @"Успешно";
            ResultMessage = $@"Таблица: {Table} Параметр: {Column} Выборка: {Selection} Значение: {Value}"; 
        }
    }
}
