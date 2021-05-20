using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Data;

namespace Solvedoku.Classes
{
    public class LocalizationHelper : INotifyPropertyChanged
    {
        #region Static fields

        static readonly LocalizationHelper instance = new LocalizationHelper();
        public static LocalizationHelper Instance => instance;

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

    public class LocExtension : Binding
    {
        public LocExtension(string name) : base("[" + name + "]")
        {
            Mode = BindingMode.OneWay;
            Source = LocalizationHelper.Instance;
        }
    }
}