using System.Windows.Data;
using Solvedoku.Services.Localization;

namespace Solvedoku.UI.Extensions
{
    public class LocalizeExtension : Binding
    {
        public LocalizeExtension(string name) : base("[" + name + "]")
        {
            Mode = BindingMode.OneWay;
            Source = LocalizationService.Instance;
        }
    }
}
