using System.Threading;

namespace MassTranslator.Win
{
    public class ModelFactory
    {
        private readonly TranslatorModel _model = new TranslatorModel();

        public Main CreateMainViewModel()
        {
            return new Main(_model);
        }

        public TwoWayTranslatorViewModel CreateTwoWayTranslatorViewModel()
        {
            return new TwoWayTranslatorViewModel(_model);
        }

        public XmlTranslatorViewModel CreateXmlTranslatorViewModel()
        {
            return new XmlTranslatorViewModel(_model);
        }

        public TranslatorModel Model
        {
            get { return _model; }
        }

        public OutWindowViewModel CreateOutWindowViewModel()
        {
            return new OutWindowViewModel(_model);
        }

        public ProgressDialogViewModel CreateProgressDialogViewModel(CancellationTokenSource cts, string title, string text)
        {
            return new ProgressDialogViewModel(cts, title, text);
        }
    }
}