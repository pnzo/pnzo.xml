using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml.task.Model.Commands;

namespace xml.task
{
    internal static class CommandClient
    {
        public static List<Command> GetCommands(XDocument xmlDocument)
        {
            var commands = new List<Command>();
            if (xmlDocument.Root == null) return commands;
            foreach (var xmlElement in xmlDocument.Root.Elements())
            {
                switch (xmlElement.Name.LocalName)
                {
                    case @"dynamic":
                        commands.Add(new DynamicStabilityCommand(xmlElement)); break;
                    default:
                        break;
                }
            }
            return commands;
        }

        public static string GetHeader(XDocument xmlDocument)
        {
            return xmlDocument.Root?.Attribute(@"name")?.Value ?? @"noname";
        }
    }
}
