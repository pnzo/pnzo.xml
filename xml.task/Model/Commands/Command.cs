using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTRALib;

namespace xml.task.Model.Commands
{
    class TestCommand
    {
        public string Rst;
        public string Scn;
        public string Name;
        public bool Success;
        public string ResultMessage;

        public TestCommand(XElement xElement)
        {
            Rst = xElement.Attribute(@"rst").Value;
            Scn = xElement.Attribute(@"scn").Value;
            Name = xElement.Attribute(@"name").Value;
        }

        //public XElement ToXElement()
        //{

        //}s

        public void Perform()
        {
            Console.WriteLine(@"{0} {1} {2}", Name, Rst, Scn);
            Rastr rastr = new Rastr();

        }
    }
}
