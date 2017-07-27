using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MassTranslator.Win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ModelFactory ModelFactory { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ModelFactory = new ModelFactory();
            ModelFactory.Model.Load();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ModelFactory.Model.Close();
        }
    }
}
