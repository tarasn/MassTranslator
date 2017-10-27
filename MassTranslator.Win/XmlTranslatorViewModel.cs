using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MassTranslator.Win.Properties;
using Microsoft.Win32;

namespace MassTranslator.Win
{
    public class XmlTranslatorViewModel : ViewModelBase
    {
        private readonly TranslatorModel _model;
        private Language _selectedLanguageFrom;
        private string _textFrom;
        private string _xmlFileName;
        private string _textCount;



        public XmlTranslatorViewModel(TranslatorModel model)
        {
            _model = model;
            Languages = _model.LoadLanguages().ToList();
            SelectedLanguageFrom = Languages.Find(l => l.Name == "English");
            TranslateCommand = new RelayCommand(c => !string.IsNullOrEmpty(TextFrom) && 
                !string.IsNullOrEmpty(XmlFileName), Translate);
            LoadXmlCommand = new RelayCommand(c => !string.IsNullOrEmpty(TextFrom), LoadXml);
            OpenOutputXmlCommand = new RelayCommand(c => !string.IsNullOrEmpty(_model.OutputXml), OpenOutputXml);
        }

        private void OpenOutputXml(object obj)
        {
            var outWindow = new OutWindow();
            outWindow.Owner = Application.Current.MainWindow;
            outWindow.ShowDialog();
        }

        private void Translate(object obj)
        {
            _model.TranslateXml(SelectedLanguageFrom.Abbr, TextFrom);
            var targetFilename = XmlFileName + ".translated";
            //MessageBox.Show(string.Format("{0}", targetFilename), "Saved");
            File.WriteAllText(targetFilename, _model.OutputXml);
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
        public ICommand OpenOutputXmlCommand { get; set; }

        public string TextFrom
        {
            get { return _textFrom; }
            set
            {
                if (_textFrom != value)
                {
                    _textFrom = value;
                    TextCount = "Text Count: " + TextFrom.Count();
                    OnPropertyChanged("TextFrom");
                }
            }
        }

        public string TextCount
        {
            get { return _textCount; }
            set
            {
                if (_textCount != value)
                {
                    _textCount = value;
                    OnPropertyChanged("TextCount");
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

       

    }
}