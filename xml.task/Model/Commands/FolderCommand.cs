using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xml.task.Model.Commands
{
    public abstract class FolderCommand
    {
        public string Name;
        public string Folder;

        protected FolderCommand(XElement xElement)
        {
            Name = xElement?.Attribute(@"name")?.Value;
            Folder = xElement?.Attribute(@"folder")?.Value;
        }

        public abstract List<Command> GenerateSimpleCommands();

        public override string ToString()
        {
            return Name;
        }
    }
}
