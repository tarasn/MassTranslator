using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;

namespace MassTranslator.Win
{
    public class XmlTranslatorViewModel : INotifyPropertyChanged
    {
        private readonly TranslatorModel _model;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private Language _selectedLanguageFrom;
        private string _textFrom;
        private string _xmlFileName;



        public XmlTranslatorViewModel(TranslatorModel model)
        {
            _model = model;
            Languages = _model.LoadLanguages().ToList();
            SelectedLanguageFrom = Languages.Find(l => l.Name == "English");
            TranslateCommand = new RelayCommand(c => !string.IsNullOrEmpty(TextFrom) && 
                !string.IsNullOrEmpty(XmlFileName), Translate);
            LoadXmlCommand = new RelayCommand(c => !string.IsNullOrEmpty(TextFrom), LoadXml);
        }

        private void Translate(object obj)
        {
            var xml = _model.TranslateXml(SelectedLanguageFrom.Abbr, TextFrom);
        }

        private void LoadXml(object obj)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _model.Xml = File.ReadAllText(openFileDialog.FileName);
                XmlFileName = openFileDialog.FileName;
            }
        }


        public ICommand TranslateCommand { get; set; }
        public ICommand LoadXmlCommand { get; set; }

        public string TextFrom
        {
            get { return _textFrom; }
            set
            {
                if (_textFrom != value)
                {
                    _textFrom = value;
                    OnPropertyChanged("TextFrom");
                }
            }
        }


        public string XmlFileName
        {
            get { return _xmlFileName; }
            set
            {
                if (_xmlFileName != value)
                {
                    _xmlFileName = value;
                    OnPropertyChanged("XmlFileName");
                }
            }
        }




        public Language SelectedLanguageFrom
        {
            get { return _selectedLanguageFrom; }
            set
            {
                if (_selectedLanguageFrom != value)
                {
                    _selectedLanguageFrom = value;
                    OnPropertyChanged("SelectedLanguageFrom");
                }
            }
        }

        public List<Language> Languages { get; private set; }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}