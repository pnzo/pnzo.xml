using System;
using ICSharpCode.AvalonEdit.Folding;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using xml.task.Model.Commands;
using xml.task.Model.RastrManager;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace xml.task
{

    //class FileList : ObservableCollection<string>
    //{
    //    public FileList(params string[] extensions)
    //    {
    //        this.extensions = extensions.ToList<string>();

    //    }

    //    public List<string> extensions;
    //    public void AddFile(string filename)
    //    {
    //        foreach (string s in this)
    //        {
    //            if (String.Compare(s, filename, true) == 0) return;
    //        }
    //        if (!File.Exists(filename)) return;
    //        if (extensions.Count(s => s == System.IO.Path.GetExtension(filename)) > 0 || extensions.Count == 0) base.Insert(0,filename);
    //    }
    //}

    public partial class MainWindow
    {
        private readonly FoldingManager _foldingManager;
        private readonly XmlFoldingStrategy _foldingStrategy;
        private string _filePath;

        public MainWindow()
        {
            InitializeComponent();
            var foldingUpdateTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.5)};
            foldingUpdateTimer.Tick += delegate { UpdateFoldings(); };
            foldingUpdateTimer.Start();
            TextEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();

            _foldingManager = FoldingManager.Install(TextEditor.TextArea);

            _foldingStrategy = new XmlFoldingStrategy
            {
                ShowAttributesWhenFolded = true
            };
            _foldingStrategy.UpdateFoldings(_foldingManager, TextEditor.Document);
        }

        private void UpdateFoldings()
        {
            _foldingStrategy?.UpdateFoldings(_foldingManager, TextEditor.Document);
        }

        private void LoadFileToTextEditor(string filename)
        {
            _filePath = @"";
            TextEditor.Text = File.ReadAllText(filename, Encoding.UTF8);
            _filePath = Path.GetFullPath(filename);
            FileLabel.Content = _filePath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TextEditor.Save(_filePath);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() != true) return;
            LoadFileToTextEditor(openFileDialog.FileName);
        }

        private void ExecutingButton_Click(object sender, RoutedEventArgs e)
        {
            var doc = XDocument.Parse(TextEditor.Text);
            var commands = doc.Root?.Elements().Select(element => new DynamicStabilityCommand(element)).ToList();

            var taskWindow = new ExecutingWindow
            {
                Owner = this,
                Commands = commands,
                Title = doc.Root?.Attribute(@"name")?.Value ?? @"noname",
            Topmost = true
            };
            taskWindow.Show();
        }

        private void TextEditor_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;

        }

        private void TextEditor_PreviewDrop(object sender, DragEventArgs e)
        {
            var s = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            if (s == null) return;
            foreach (var f in s)
                LoadFileToTextEditor(f);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _filePath = @"";
            FileLabel.Content = _filePath;
            TextEditor.Text = @"<task name=""new task"">

</task>
";
        }

        private void ExampleButton_Click(object sender, RoutedEventArgs e)
        {
            LoadFileToTextEditor(@"default\xmltext.xml");
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog {Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*"};
            if (saveFileDialog.ShowDialog() != true) return;
            TextEditor.Save(saveFileDialog.FileName);
        }
    }
}
