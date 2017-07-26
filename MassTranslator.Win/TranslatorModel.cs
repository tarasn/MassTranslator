using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace MassTranslator.Win
{
    public class TranslatorModel
    {

        private const string GoogleTranslateUrlTemplate =
            "https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}";


        private ConcurrentDictionary<string,string> _translations = new ConcurrentDictionary<string, string>();

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

        public string Translate(string from, string to, string textFrom)
        {
            var url = string.Format(GoogleTranslateUrlTemplate, from, to, HttpUtility.UrlEncode(textFrom));
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var responseStream = response.GetResponseStream())
                {
                    var responseText = new StreamReader(responseStream).ReadToEnd();
                    var list = new JavaScriptSerializer().Deserialize<object[]>(responseText);
                    //TODO: Doesn't work when in response few lines
                    return (string)((object[]) ((object[]) (list[0]))[0])[0];
                }
            }
            return "";
        }


        public string Xml { get; set; }

        public string TranslateXml(string from, string fromText)
        {
            _translations.Clear();
            var languageAbbrs = ParseXmlTolanguageAbbrList();
            Parallel.ForEach(languageAbbrs, (abbr) =>
            {
                _translations.TryAdd(abbr, Translate(from, abbr, fromText));
            });

            return BuildXml();
        }

        //TODO: XDocument can be loaded once 
        private string BuildXml()
        {
            XDocument xDoc = XDocument.Parse(string.Format("<abbr>{0}</abbr>", Xml));
            foreach (var node in xDoc.Root.Descendants())
            {
                var key = node.Name.LocalName.Substring(0, 2);
                node.Value = HttpUtility.HtmlEncode(_translations[key]);
            }
            return xDoc.Root.ToString();
        }

        private IEnumerable<string> ParseXmlTolanguageAbbrList()
        {
            XDocument xDoc = XDocument.Parse(string.Format("<abbr>{0}</abbr>",Xml));
            List<string> abbrs = new List<string>();
            foreach (var node in xDoc.Root.Descendants())
            {
                abbrs.Add(node.Name.LocalName.Substring(0, 2));
            }
            return abbrs.Distinct();
        }
    }
}