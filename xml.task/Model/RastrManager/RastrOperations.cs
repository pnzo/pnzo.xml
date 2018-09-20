using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ASTRALib;

namespace xml.task.Model.RastrManager
{
    internal struct DynamicResult
    {
        public string ResultMessage;
        public double TimeReached;
        public bool IsSuccess;
        public bool IsStable;

        public override string ToString()
        {
            return $@"Результат: {ResultMessage}
Расчитанное время: {TimeReached}
Успешно: {IsSuccess} 
Устойчиво: {IsStable}";
        }
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
            {
                _rastr.Load(RG_KOD.RG_REPL, file, FindTemplatePathWithExtension(Path.GetExtension(file)));
            }

        }

        public void Save(string file)
        {
            _rastr.Save(file, FindTemplatePathWithExtension(Path.GetExtension(file)));
        }

        public void SetValue(string tableName, string columnName, string selection, string value)
        {
            table table = _rastr.Tables.Item(tableName);
            col column = table.Cols.Item(columnName);
            table.SetSel(selection);
            column.Calc(value);
        }

        public void SetExitFilesDirectory(string path)
        {
            SetValue(@"com_dynamics", @"SnapPath", @"1", @"""" + path + @"""");
        }

        public void SetExitFileTemplate(string template)
        {
            SetValue(@"com_dynamics", @"SnapTemplate", @"1", template);
        }

        public void SetDynamicTime(string time)
        {
            SetValue(@"com_dynamics", @"Tras", @"1", time);
        }

        public DynamicResult RunDynamic()
        {
            var dynamicResult = new DynamicResult();
            _rastr.Load(RG_KOD.RG_REPL, @"", FindTemplatePathWithExtension(@".dfw"));
            var dyn = _rastr.FWDynamic();
            var result = dyn.RunEMSmode();
            dynamicResult.IsSuccess = result == RastrRetCode.AST_OK;
            dynamicResult.IsStable = dyn.SyncLossCause == DFWSyncLossCause.SYNC_LOSS_NONE;
            dynamicResult.ResultMessage = dyn.ResultMessage == @"" ? @" - " : dyn.ResultMessage;
            dynamicResult.TimeReached = dyn.TimeReached;
            return dynamicResult;
        }

        public DynamicResult RunDynamicWithExitFile()
        {
            var dynamicResult = new DynamicResult();
            _rastr.Load(RG_KOD.RG_REPL, @"", FindTemplatePathWithExtension(@".dfw"));

            SetValue(@"com_dynamics", @"SnapAutoLoad", @"1", 1.ToString());
            var dyn = _rastr.FWDynamic();
            var result = dyn.Run();

            dynamicResult.IsSuccess = result == RastrRetCode.AST_OK;
            dynamicResult.IsStable = dyn.SyncLossCause == DFWSyncLossCause.SYNC_LOSS_NONE;
            dynamicResult.ResultMessage = dyn.ResultMessage == @"" ? @" - " : dyn.ResultMessage;
            dynamicResult.TimeReached = dyn.TimeReached;
            return dynamicResult;
        }

        public List<Point> GetPointsFromExitFile(string tableName, string columnName, string selection)
        {
            table table = _rastr.Tables.Item(tableName);
            table.SetSel(selection);
            var points = new List<Point>();
            var index = table.FindNextSel[-1];
            if (index < 0)
                return points;
            double[,] v = _rastr.GetChainedGraphSnapshot(tableName, columnName, index, 0);

            for (var i = 0; i < v.GetLength(0); i++)
            {
                points.Add(new Point(v[i, 1], v[i, 0]));
            }
            return points;
        }
    }
}
