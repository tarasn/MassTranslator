using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using MassTranslator.Win.Properties;
using System.Net.Http;

namespace MassTranslator.Win
{
    public class TranslatorModel
    {

        private const string GoogleTranslateUrlTemplate =
            "https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}";


        private ConcurrentDictionary<string,string> _translations = new ConcurrentDictionary<string, string>();
        private XDocument _xDoc;


        public TranslatorModel()
        {
            
        }

        public IEnumerable<Language> LoadLanguages()
        {
            var languages = new List<Language>
            {
                new Language("Afrikaans,af"),
                new Language("Irish,ga"),
                new Language("Albanian,sq"),
                new Language("Italian,it"),
                new Language("Arabic,ar"),
                new Language("Japanese,ja"),
                new Language("Azerbaijani,az"),
                new Language("Kannada,kn"),
                new Language("Basque,eu"),
                new Language("Korean,ko"),
                new Language("Bengali,bn"),
                new Language("Latin,la"),
                new Language("Belarusian,be"),
                new Language("Latvian,lv"),
                new Language("Bulgarian,bg"),
                new Language("Lithuanian,lt"),
                new Language("Catalan,ca"),
                new Language("Macedonian,mk"),
                new Language("Chinese Simplified,zh-CN"),
                new Language("Malay,ms"),
                new Language("Chinese Traditional,zh-TW"),
                new Language("Maltese,mt"),
                new Language("Croatian,hr"),
                new Language("Norwegian,no"),
                new Language("Czech,cs"),
                new Language("Persian,fa"),
                new Language("Danish,da"),
                new Language("Polish,pl"),
                new Language("Dutch,nl"),
                new Language("Portuguese,pt"),
                new Language("English,en"),
                new Language("Romanian,ro"),
                new Language("Esperanto,eo"),
                new Language("Russian,ru"),
                new Language("Estonian,et"),
                new Language("Serbian,sr"),
                new Language("Filipino,tl"),
                new Language("Slovak,sk"),
                new Language("Finnish,fi"),
                new Language("Slovenian,sl"),
                new Language("French,fr"),
                new Language("Spanish,es"),
                new Language("Galician,gl"),
                new Language("Swahili,sw"),
                new Language("Georgian,ka"),
                new Language("Swedish,sv"),
                new Language("German,de"),
                new Language("Tamil,ta"),
                new Language("Greek,el"),
                new Language("Telugu,te"),
                new Language("Gujarati,gu"),
                new Language("Thai,th"),
                new Language("Haitian Creole,ht"),
                new Language("Turkish,tr"),
                new Language("Hebrew,iw"),
                new Language("Ukrainian,uk"),
                new Language("Hindi,hi"),
                new Language("Urdu,ur"),
                new Language("Hungarian,hu"),
                new Language("Vietnamese,vi"),
                new Language("Icelandic,is"),
                new Language("Welsh,cy"),
                new Language("Indonesian,id"),
                new Language("Yiddish,yi")
            };

            return languages;
        }

        public async Task<string> Translate(string from, string to, string textFrom, CancellationToken token)
        {
            var url = string.Format(GoogleTranslateUrlTemplate, from, to, HttpUtility.UrlEncode(textFrom));

            using (var client = new HttpClient())
            using (HttpResponseMessage response = 
                await client.GetAsync(url, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (HttpContent content = response.Content)
                    {
                        var responseStr = await content.ReadAsStringAsync();

                        object[] list = new JavaScriptSerializer().Deserialize<object[]>(responseStr);

                        var sb = new StringBuilder();
                        object[] list1 = (object[])list[0];
                        for (int i = 0; i < list1.Length; i++)
                        {
                            var line = ((object[])list1[i])[0];
                            sb.Append(line);
                        }
                        return sb.ToString();
                    }
                }
            }

            return "";
        }

        

        public string Xml { get; set; }
        public string OutputXml { get; set; }

        public void TranslateXml(string from, string fromText)
        {
            var token = new CancellationToken();
            OutputXml = String.Empty;
            _translations.Clear();
            _xDoc = XDocument.Parse(string.Format("<abbr>{0}</abbr>", Xml));
            var languageAbbrs = ParseXmlTolanguageAbbrList();
            Parallel.ForEach(languageAbbrs, async (abbr) =>
            {
                _translations.TryAdd(abbr, await Translate(from, abbr, fromText, token).ConfigureAwait(false));
            });
            OutputXml =  BuildXml();
        }

        private string BuildXml()
        {
            foreach (var node in _xDoc.Root.Descendants())
            {
                var key = node.Name.LocalName.Substring(0, 2);
                node.Value = HttpUtility.HtmlEncode(_translations[key]);
            }
            return ToGogglePlayFormatedXml(_xDoc.Root.Descendants());
        }

        private IEnumerable<string> ParseXmlTolanguageAbbrList()
        {
            List<string> abbrs = new List<string>();
            foreach (var node in _xDoc.Root.Descendants())
            {
                abbrs.Add(node.Name.LocalName.Substring(0, 2));
            }
            return abbrs.Distinct();
        }


        public double WindowTop
        {
            get { return Settings.Default.Height; }
            set
            {
                Settings.Default.Height = value;
            }
        }

        public double WindowLeft
        {
            get { return Settings.Default.Width; }
            set
            {
                Settings.Default.Width = value;
            }
        }

        public void Load()
        {
            
        }

        public void Close()
        {
            Settings.Default.Save();
        }

        public string ToGogglePlayFormatedXml(IEnumerable<XElement> nodes)
        {
            var sb = new StringBuilder();
            foreach (var node in nodes)
            {
                sb.AppendFormat("<{0}>\n{1}\n<{0}/>\n", node.Name, node.Value);
            }
            return sb.ToString();
        }

    }
}