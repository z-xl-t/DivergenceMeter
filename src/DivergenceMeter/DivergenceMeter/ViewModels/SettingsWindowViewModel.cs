using System;
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
        public SettingsWindowViewModel(Settings settings)
        {
            Settings = settings;
        }
        public void SaveSettings()
        {
            Settings.SaveSettings(Settings.Default_File_Path, this.Settings);
        }
    }
}
