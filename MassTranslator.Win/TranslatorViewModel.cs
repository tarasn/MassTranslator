using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MassTranslator.Win
{
    public class TranslatorViewModel : INotifyPropertyChanged
    {
        private readonly TranslatorModel _model;
        private Language _selectedLanguageFrom;
        private Language _selectedLanguageTo;
        private string _textFrom;
        private string _textTo;
        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        public TranslatorViewModel(TranslatorModel model)
        {
            _model = model;
            Languages = _model.LoadLanguages().ToList();
            SelectedLanguageFrom = Languages.Find(l=>l.Name=="English");
            SelectedLanguageTo = Languages.Find(l=>l.Name=="Russian");
            TranslateCommand = new RelayCommand(c => !string.IsNullOrEmpty(TextFrom), Translate);
        }

        private void Translate(object obj)
        {
            TextTo = _model.Translate(SelectedLanguageFrom.Abbr, SelectedLanguageTo.Abbr, TextFrom);
        }

        public ICommand TranslateCommand { get; set; }

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

        public Language SelectedLanguageTo
        {
            get { return _selectedLanguageTo; }
            set
            {
                if (_selectedLanguageTo != value)
                {
                    _selectedLanguageTo = value;
                    OnPropertyChanged("SelectedLanguageTo");
                }
            }
        }


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


        public string TextTo
        {
            get { return _textTo; }
            set
            {
                if (_textTo != value)
                {
                    _textTo = value;
                    OnPropertyChanged("TextTo");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public List<Language> Languages { get; private set; }
    }
}