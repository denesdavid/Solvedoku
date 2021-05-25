using System.Collections.ObjectModel;
using System.Windows.Input;
using Solvedoku.Classes;
using Solvedoku.Commands;
using Solvedoku.Properties;

namespace Solvedoku.ViewModels.OptionsWindow
{
    class OptionsWindowViewModel:ViewModelBase
    {
        #region Fields
        Language _selectedLocalization;
        ObservableCollection<Language> _localizations = new ObservableCollection<Language>
        {
            new Language{ FriendlyName = "English (en)", Name = "en"},
            new Language{ FriendlyName = "Magyar (hu)", Name = "hu"}
        };
        #endregion

        #region Properties

        public ICommand OkCommand { get; set; }
        
        public ObservableCollection<Language> Localizations
        {
            get => _localizations;
        }

        public Language SelectedLocalization
        {
            get => _selectedLocalization;
            set
            {
                _selectedLocalization = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor
        public OptionsWindowViewModel()
        {
            LoadCommands();
            foreach (var language in Localizations)
            {
                if (language.Name == Settings.Default.Localization)
                {
                    SelectedLocalization = language;
                    break;
                }
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Determines if applying the settings is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanOk() => true;

        /// <summary>
        /// Applies the settings.
        /// </summary>
        void Ok()
        {
            LocalizationHelper.Instance.CurrentCulture = new System.Globalization.CultureInfo(SelectedLocalization.Name);
            Settings.Default.Localization = SelectedLocalization.Name;
            Settings.Default.Save();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the command properties in the viewmodel.
        /// </summary>
        void LoadCommands()
        {
            OkCommand = new ParameterlessCommand(Ok, CanOk);
        }
        #endregion
    }
}