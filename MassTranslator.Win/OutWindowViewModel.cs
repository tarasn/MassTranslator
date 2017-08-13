using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MassTranslator.Win
{
    public class OutWindowViewModel : ViewModelBase
    {
        private readonly TranslatorModel _model;
        public ICommand CopyCommand { get; set; }

        public OutWindowViewModel(TranslatorModel model)
        {
            _model = model;
            OnPropertyChanged("OutXml");
            CopyCommand = new RelayCommand(c => true, Copy);
        }



        private void Copy(object obj)
        {
            Clipboard.SetText(OutXml);
        }

        public string OutXml
        {
            get
            {
                return _model.OutputXml;
            }
            set
            {
                
            }
        }
    }
}
