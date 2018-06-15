using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using xml.task.Model.Commands;

namespace xml.task
{
    /// <summary>
    /// Логика взаимодействия для ExecutingWindow.xaml
    /// </summary>
    public partial class ExecutingWindow : Window
    {
        public List<DynamicStabilityCommand> Commands;

        public ExecutingWindow()
        {
            InitializeComponent();
        }
    }
}
