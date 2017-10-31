using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MassTranslator.Win
{
    public class ProgressDialogViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;

        public ProgressDialogViewModel(CancellationTokenSource cts, string title, string text)
        {
            this.cts = cts;
            this.Title = title;
            this.Text = text;
        }

        public string Title { get; private set; }

        public string Text { get; private set; }

        private void Cancel(Object obj)
        {
            cts.Cancel();
        }

        public void ViewClosingEvent(object sender, EventArgs e)
        {
            Cancel(null);
        }
    }
}
