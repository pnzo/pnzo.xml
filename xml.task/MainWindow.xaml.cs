using System;
using ICSharpCode.AvalonEdit.Folding;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using xml.task.Model.Commands;
using xml.task.Model.RastrManager;
using Microsoft.Win32;

namespace xml.task
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FoldingManager foldingManager;
        XmlFoldingStrategy foldingStrategy;

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


            textEditor.Load(@"..\..\default\xmltext.xml");
        }

        void UpdateFoldings()
        {
            if (foldingStrategy is XmlFoldingStrategy)
            {
                ((XmlFoldingStrategy)foldingStrategy).UpdateFoldings(foldingManager, textEditor.Document);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            const string fileName = @"..\..\default\xmltext.xml";
            var doc = XDocument.Parse(System.IO.File.ReadAllText(fileName, Encoding.Default));
            foreach (var element in doc.Root.Elements())
            {
                var command = new DynamicStabilityCommand(element);
                command.Perform();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var rastr = new RastrOperations();
            Debug.Print((RastrOperations.FindTemplatePathWithExtension(@"111") == null).ToString());
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            textEditor.Save(@"..\..\default\xmltext.xml");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            textEditor.Text = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
            fileComboBox.Text = openFileDialog.FileName;

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var taskWindow = new ExecutingWindow
            {
                Owner = this,
                Title = @"test",
                Topmost = true
            };
            taskWindow.Show();
        }
    }
}
