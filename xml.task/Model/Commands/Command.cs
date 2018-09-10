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
        public int Id;
        public List<string> Files;
        public string Status;
        public string ResultMessage;
        public string ErrorMessage;

        private readonly XElement _element;

        protected Command()
        {
        }

        protected Command(XElement xElement)
        {
            _element = xElement;
            Name = _element?.Attribute(@"name")?.Value;
        }

        public virtual void Perform()
        {

        }

        public override string ToString()
        {
            return Name ?? (Name = _element.ToString());
        }
    }
}
