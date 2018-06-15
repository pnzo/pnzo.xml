using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using xml.task.Model.RastrManager;
using System.Diagnostics;

namespace xml.task.Model.Commands
{
    public class DynamicStabilityCommand
    {
        public string Rst;
        public string Scn;
        public string Name;
        public string Folder;
        public bool Success;
        public string ResultMessage;

        public DynamicStabilityCommand(XElement xElement)
        {
            Rst = xElement?.Attribute(@"rst")?.Value;
            Scn = xElement?.Attribute(@"scn")?.Value;
            Name = xElement?.Attribute(@"name")?.Value;
            Folder = xElement?.Attribute(@"folder")?.Value;
        }

        public void Perform()
        {
            if (Folder == null)
            {
                var rastr = new RastrOperations();
                rastr.Load(Rst, Scn);
                rastr.Calc();
            }
            else
            {
                var rstFiles = Directory.GetFiles(Folder, @"*.rst");
                var scnFiles = Directory.GetFiles(Folder, @"*.scn");
            }
        }
    }
}
