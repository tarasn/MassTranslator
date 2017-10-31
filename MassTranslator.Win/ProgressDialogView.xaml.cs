using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MassTranslator.Win
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialogView : Window
    {
        public ProgressDialogView(CancellationTokenSource cts, string title, string text)
        {
            InitializeComponent();
            var viewModel = ((App)Application.Current).ModelFactory.CreateProgressDialogViewModel(cts, title, text);
            this.DataContext = viewModel;
            this.Closing += viewModel.ViewClosingEvent;
        }
    }
}
