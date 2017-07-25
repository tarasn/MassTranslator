namespace MassTranslator.Win
{
    public class Language
    {
        public Language(string csv)
        {
            var pair = csv.Split(',');
            Name = pair[0];
            Abbr = pair[1];
        }

        public string Name { get; private set; }
        public string Abbr { get; private set; }
    }
}