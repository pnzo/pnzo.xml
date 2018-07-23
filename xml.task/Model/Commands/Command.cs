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
    public abstract class Command
    {
        public string Name;
        public bool Success;
        public string Summary;
        public string ResultMessage;
        private XElement element;

        protected Command()
        {
        }

        protected Command(XElement xElement)
        {
            element = xElement;
            Name = element?.Attribute(@"name")?.Value;
        }

        public virtual void Perform()
        {

        }

        public override string ToString()
        {
            return Name ?? (Name = element.ToString());
        }
    }
}
