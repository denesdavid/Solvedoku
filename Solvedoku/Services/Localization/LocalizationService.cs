using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Data;

namespace Solvedoku.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        #region Static fields

        static readonly LocalizationService instance = new LocalizationService();
        public static LocalizationService Instance => instance;

        #endregion

        #region Fields

        readonly ResourceManager resManager = Properties.Resources.ResourceManager;
        CultureInfo currentCulture = null;

        #endregion

        #region Properties

        public string this[string key] => resManager.GetString(key, currentCulture);

        public CultureInfo CurrentCulture
        {
            get => currentCulture;
            set
            {
                if (currentCulture != value)
                {
                    currentCulture = value;
                    CultureInfo.DefaultThreadCurrentCulture = value;
                    CultureInfo.DefaultThreadCurrentUICulture = value;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = value;
                    System.Threading.Thread.CurrentThread.CurrentCulture = value;
                    var @event = PropertyChanged;
                    if (@event != null)
                    {
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}