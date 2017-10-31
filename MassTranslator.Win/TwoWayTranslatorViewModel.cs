using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
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
            var cts = new CancellationTokenSource();

            // TODO: Consider moving view creation outside the view model.
            var dialog = new ProgressDialogView(cts, "Translation status", "Translation ongoing...");
            dialog.Loaded += async (object sender, RoutedEventArgs e) =>
            {
                try
                {
                    TextTo = await _model.Translate(SelectedLanguageFrom.Abbr, SelectedLanguageTo.Abbr, TextFrom, cts.Token);
                    dialog.Close();
                }
                catch (OperationCanceledException)
                {
                    // Operation cancelled, no further action required.
                }
            };

            dialog.ShowDialog();
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

     


        public List<Language> Languages { get; private set; }
    }
}