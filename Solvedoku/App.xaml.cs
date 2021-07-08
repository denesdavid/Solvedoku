using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Reflection;
using Solvedoku.Properties;

namespace Solvedoku
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Settings.Default.Localization);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var thisAssembly = Assembly.GetExecutingAssembly();
                var assemblyName = new AssemblyName(args.Name);
                var dllName = assemblyName.Name + ".dll";
                var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(dllName));

                if (resources.Any())
                {
                    var resourceName = resources.First();
                    using (var stream = thisAssembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream == null) return null;
                        var block = new byte[stream.Length];

                        try
                        {
                            stream.Read(block, 0, block.Length);
                            return Assembly.Load(block);
                        }
                        catch (IOException)
                        {
                            return null;
                        }
                        catch (BadImageFormatException)
                        {
                            return null;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Nem sikerült betölteni a szoftver komponenseit! " + ex.Message,"Hiba!",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }
    }
}