using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml.task.Model.RastrManager;

namespace xml.task.Model.Commands
{
    class DynamicFolderCommand : FolderCommand
    {
        public DynamicFolderCommand(XElement xElement) : base(xElement)
        {
        }

        public override List<Command> GenerateSimpleCommands()
        {
            var commands = new List<Command>();
            if (Directory.Exists(Folder) == false)
            {
                return commands;
            }
            string[] rstFiles = Directory.GetFiles(Folder,@"*.rst");
            string[] scnFiles = Directory.GetFiles(Folder, @"*.scn");
            foreach (var rstFile in rstFiles)
            {
                foreach (var scnFile in scnFiles)
                {
                    var command = new DynamicCommand();
                    command.Rst = rstFile;
                    command.Scn = scnFile;
                    command.Name = $@"{Name} [{commands.Count}]";
                    commands.Add(command);
                }
            }
            return commands;
        }
    }
}
