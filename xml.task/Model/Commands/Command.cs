using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xml.task.Model.Commands
{
    class TestCommand
    {
        public string Rst;
        public string Scn;
        public bool Success;
        public string ResultMessage;

        public TestCommand(XElement xElement)
        {
            Rst = xElement.Attribute(@"rst").Value;
            Scn = xElement.Attribute(@"scn").Value;
        }

        //public XElement ToXElement()
        //{

        //}

        public void Perform()
        {
            Console.WriteLine(@"{0} {1}", Rst, Scn);
        }
    }
}
