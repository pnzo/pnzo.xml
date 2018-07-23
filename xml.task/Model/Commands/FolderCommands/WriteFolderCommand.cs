using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml.task.Model.Commands.SimpleCommands;

namespace xml.task.Model.Commands
{
    internal class WriteFolderCommand : FolderCommand
    {
        public string Table;
        public string Column;
        public string Selection;
        public string Value;

        public WriteFolderCommand(XElement xElement) : base(xElement)
        {
            Table = xElement?.Attribute(@"table")?.Value;
            Column = xElement?.Attribute(@"column")?.Value;
            Selection = xElement?.Attribute(@"selection")?.Value;
            Value = xElement?.Attribute(@"value")?.Value;
        }

        public override List<Command> GenerateSimpleCommands()
        {
            var commands = new List<Command>();
            if (Directory.Exists(Folder) == false)
            {
                return commands;
            }
            var files = Directory.GetFiles(Folder, @"*.rst");
            foreach (var file in files)
            {
                var command = new WriteCommand
                {
                    Table = Table,
                    Column = Column,
                    Selection = Selection,
                    Value = Value,
                    File = file,
                    Name = $@"{Name} [{commands.Count}]"
                };

                commands.Add(command);
            }
            return commands;
        }
    }
}
