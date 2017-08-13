using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MassTranslator.Win
{
    public class Main : ViewModelBase
    {
        private readonly TranslatorModel _model;
        private TabItem _selectedTab;
        public List<TabItem> Tabs { get; set; }

        public Main(TranslatorModel model)
        {
            _model = model;
            Tabs = new List<TabItem>();
            Tabs.Add(
                new TabItem
                {
                    Header = "Two Way Translation",
                    Content = new TwoWayTranslatorView()
                });
            Tabs.Add(
                new TabItem
                {
                    Header = "XML Translation",
                    Content = new XmlTranslatorView()
                });
            SelectedTab = Tabs[0];
        }

        public TabItem SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    OnPropertyChanged("SelectedTab");
                }
            }
        }

      

    }
}
