using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DivergenceMeter.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace DivergenceMeter.ViewModels
{
    public class SettingsWindowViewModel: BindableBase
    {
        private Settings _settings;
        public Settings Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        public DelegateCommand SaveSettingsCommand { get; set; }

        public SettingsWindowViewModel(Settings settings)
        {
            Settings = settings;
            SaveSettingsCommand = new DelegateCommand(SaveSettings);
        }

        private void SaveSettings()
        {
            var path = $@"{AppDomain.CurrentDomain.BaseDirectory}Settings.json";
            Settings.SaveSettings(path, this.Settings);
        }
    }
}
