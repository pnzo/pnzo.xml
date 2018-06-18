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
    internal struct DynamicResult
    {
        public string ResultMessage;
        public double TimeReached;
        public bool isSuccess;
        public bool isStable; 
    }

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
            return files.FirstOrDefault(filename => Path.GetExtension(filename) == extension);
        }

        public void Load(params string[] files)
        {
            foreach (var file in files)
                _rastr.Load(RG_KOD.RG_REPL, file, FindTemplatePathWithExtension(Path.GetExtension(file)));
        }

        public DynamicResult RunDynamic()
        {
            var dynamicResult = new DynamicResult();
            _rastr.Load(RG_KOD.RG_REPL, @"", FindTemplatePathWithExtension(@".dfw"));
            var dyn = _rastr.FWDynamic();
            var result = dyn.RunEMSmode();
            if (result == RastrRetCode.AST_OK)
            {
                dynamicResult.isSuccess = true;
            } else
            {
                dynamicResult.isSuccess = false;
            }
            if (dyn.SyncLossCause == DFWSyncLossCause.SYNC_LOSS_NONE)
            {
                dynamicResult.isStable = true;
            }
            else
            {
                dynamicResult.isStable = false;
            }
            dynamicResult.ResultMessage = dyn.ResultMessage;
            dynamicResult.TimeReached = dyn.TimeReached;
            return dynamicResult;
        }
    }
}
