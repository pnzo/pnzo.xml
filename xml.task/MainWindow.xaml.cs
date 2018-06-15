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

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FoldingManager foldingManager;
        XmlFoldingStrategy foldingStrategy;
        string filePath;

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(0.5);
            foldingUpdateTimer.Tick += delegate { UpdateFoldings(); };
            foldingUpdateTimer.Start();
            textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();

            foldingManager = FoldingManager.Install(textEditor.TextArea);

            foldingStrategy = new XmlFoldingStrategy();
            foldingStrategy.ShowAttributesWhenFolded = true;
            foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
        }

        void UpdateFoldings()
        {
            if (foldingStrategy is XmlFoldingStrategy)
            {
                ((XmlFoldingStrategy)foldingStrategy).UpdateFoldings(foldingManager, textEditor.Document);
            }
        }

        private void LoadFileToTextEditor(string filename)
        {
            filePath = @"";
            textEditor.Text = File.ReadAllText(filename, Encoding.UTF8);
            filePath = Path.GetFullPath(filename);
            fileLabel.Content = filePath;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Save(filePath);
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() != true) return;
            LoadFileToTextEditor(openFileDialog.FileName);
        }

        private void executingButton_Click(object sender, RoutedEventArgs e)
        {
            var doc = XDocument.Parse(textEditor.Text);
            var commands = new List<DynamicStabilityCommand>();
            foreach (var element in doc.Root.Elements())
            {
                var command = new DynamicStabilityCommand(element);
                commands.Add(command);
            }

            var taskWindow = new ExecutingWindow
            {
                Owner = this,
                Commands = commands,
                Title = doc.Root.Attribute(@"name")?.Value ?? @"noname",
            Topmost = true
            };
            taskWindow.Show();
        }

        private void textEditor_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;

        }

        private void textEditor_PreviewDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            foreach (string f in s)
                LoadFileToTextEditor(f);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            filePath = @"";
            fileLabel.Content = filePath;
            textEditor.Text = @"<task name=""new task"">

</task>
";
        }

        private void exampleButton_Click(object sender, RoutedEventArgs e)
        {
            LoadFileToTextEditor(@"default\xmltext.xml");
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() != true) return;
            textEditor.Save(saveFileDialog.FileName);
        }
    }
}
