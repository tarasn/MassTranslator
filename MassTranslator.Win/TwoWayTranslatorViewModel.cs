using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MassTranslator.Win
{
    public class TwoWayTranslatorViewModel : ViewModelBase
    {
        private readonly TranslatorModel _model;
        private Language _selectedLanguageFrom;
        private Language _selectedLanguageTo;
        private string _textFrom;
        private string _textTo;
        private string _textCount;

        public TwoWayTranslatorViewModel(TranslatorModel model)
        {
            _model = model;
            Languages = _model.LoadLanguages().ToList();
            SelectedLanguageFrom = Languages.Find(l=>l.Name=="English");
            SelectedLanguageTo = Languages.Find(l=>l.Name=="Russian");
            TranslateCommand = new RelayCommand(c => !string.IsNullOrEmpty(TextFrom), Translate);
            SwapTranslationCommand = new RelayCommand(c=>true,SwapTranslation);
        }

        private void SwapTranslation(object obj)
        {
            var tmpLanguage = SelectedLanguageFrom;
            SelectedLanguageFrom = SelectedLanguageTo;
            SelectedLanguageTo = tmpLanguage;
        }


        private void Translate(object obj)
        {
            TextTo = _model.Translate(SelectedLanguageFrom.Abbr, SelectedLanguageTo.Abbr, TextFrom);
        }

        public ICommand TranslateCommand { get; set; }
        public ICommand SwapTranslationCommand { get; set; }

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
                    TextCount = "Text Count: " + TextFrom.Count();
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
        
        public List<Language> Languages { get; private set; }
    }
}