using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml.task.Model.Commands;
using xml.task.Model.Commands.SimpleCommands;

namespace xml.task
{
    internal static class CommandClient
    {
        public static List<Command> GetCommands(XDocument xmlDocument)
        {
            var commands = new List<Command>();
            var id = 0;
            if (xmlDocument.Root == null) return commands;
            foreach (var xmlElement in xmlDocument.Root.Elements())
            {
                var filesInformation = GetFilesInformation(xmlElement);
                switch (xmlElement.Name.LocalName)
                {
                    case @"stab_d":
                        foreach (var filesList in filesInformation)
                        {
                            var command = new DynamicCommand(xmlElement)
                            {
                                ID = id,
                                Files = filesList                             
                            };
                            commands.Add(command);
                            id++;
                        }
                        break;
                    case @"correction":
                        commands.Add(new WriteCommand(xmlElement)); break;
                    default:
                        break;
                }
            }
            foreach (var comm in commands)
            {
                Console.WriteLine(comm.Name);
                Console.WriteLine(comm.ID);
                foreach (var list in comm.Files)
                {
                    Console.WriteLine(list);
                }
            }
            return commands;
        }

        public static List<List<string>> GetFilesInformation(XElement xmlElement)
        {
            var finalList = new List<List<string>>();

            foreach (var subElement in xmlElement.Elements())
            {
                var list = new List<string>();
                var type = subElement.Name.LocalName;
                if (type == @"file")
                {
                    var path = subElement?.Attribute(@"path")?.Value;
                    if (path != null)
                    {
                        list.Add(path);
                    }
                    if (list.Count > 0)
                        finalList.Add(list);
                }
                else if (type == @"files")
                {
                    foreach (var fileElement in subElement.Elements())
                    {
                        var path = fileElement?.Attribute(@"path")?.Value;
                        if (path != null)
                        {
                            list.Add(path);
                        }
                    }
                    if (list.Count > 0)
                        finalList.Add(list);
                }
                else if (type == @"folder")
                {
                    var path = subElement?.Attribute(@"path")?.Value;
                    var varString = subElement?.Attribute(@"var")?.Value;
                    if (varString == null)
                        continue;
                    var varExtensionsList = varString.Split(',');
                    if (varExtensionsList.Length == 0)
                        continue;

                }
            }

            return finalList;
        }

        public static string GetHeader(XDocument xmlDocument)
        {
            return xmlDocument.Root?.Attribute(@"name")?.Value ?? @"noname";
        }
    }
}
