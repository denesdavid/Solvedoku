using System.Globalization;

namespace Solvedoku.Services.Localization
{
    internal interface ILocalizationService
    {
        public string this[string key] { get; }
        public CultureInfo CurrentCulture { get; set; }
    }
}