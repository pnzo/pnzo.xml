using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ASTRALib;

namespace xml.task.Model.RastrManager
{
    internal class RastrOperations
    {
        private readonly Rastr _rastr;

        public RastrOperations()
        {
            _rastr = new Rastr();
        }

        public static string FindTemplatePathWithExtension(string extension)
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                  @"\RastrWIN3\SHABLON\")) return null;
            var files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\RastrWIN3\SHABLON\");
            return files.FirstOrDefault(filename => Path.GetExtension(filename).Replace(@".", @"") == extension.Replace(@".", @""));
        }

        public void Load(params string[] files)
        {
            foreach (var file in files)
                _rastr.Load(RG_KOD.RG_REPL, file, FindTemplatePathWithExtension(Path.GetExtension(file)));
        }

        public void Calc()
        {
            _rastr.Load(RG_KOD.RG_REPL, @"", FindTemplatePathWithExtension(@"dfw"));
            var dyn = _rastr.FWDynamic();
            dyn.RunEMSmode();
            Debug.Print(dyn.SyncLossCause.ToString());
        }
    }
}
