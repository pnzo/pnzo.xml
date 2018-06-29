using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml.task.Model.RastrManager;

namespace xml.task.Model.Commands.SimpleCommands
{
    class WriteCommand : Command
    {
        public string Table;
        public string Column;
        public string Selection;
        public string Value;
        public string File;

        public WriteCommand() : base()
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
                ResultMessage = $@"Не удалось загрузить исходный файл в рабочее пространство RastrWIN. 
Сообщение: {exception.Message}";
                return;
            }

            try
            {
                rastr.SetValue(Table, Column, Selection, Value);
                rastr.Save(File);
            }
            catch (Exception exception)
            {
                ResultMessage = $@"Не удалось выполнить коррекцию файла. 
Сообщение: {exception.Message}";
                return;
            }
            ResultMessage = $@"Коррекция файла выполнена успешно.
Таблица: {Table}
Параметр: {Column}
Выборка: {Selection}
Значение: {Value}"; 
        }
    }
}
