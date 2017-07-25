using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MassTranslator.Win
{
    public class MainViewModel
    {
        public List<TabItem> Tabs { get; set; }

        public MainViewModel()
        {
            Tabs = new List<TabItem>
            {
                new TabItem
                {
                    Header = "Two Way Translation",
                    Content = new TwoWayTranslatorView()
                },
                new TabItem
                {
                    Header = "XML Translation",
                    Content = new XmlTranslatorView()
                },
            };

        }
    }
}
