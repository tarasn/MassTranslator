namespace MassTranslator.Win
{
    public class ModelFactory
    {
        private readonly TranslatorModel _model = new TranslatorModel();

        public MainViewModel CreateMainViewModel()
        {
            return new MainViewModel();
        }

        public TwoWayTranslatorViewModel CreateTwoWayTranslatorViewModel()
        {
            return new TwoWayTranslatorViewModel(_model);
        }

        public XmlTranslatorViewModel CreateXmlTranslatorViewModel()
        {
            return new XmlTranslatorViewModel(_model);
        }
    }
}