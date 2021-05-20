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
                
                // Get the Name of the AssemblyFile
                var assemblyName = new AssemblyName(args.Name);
                var dllName = assemblyName.Name + ".dll";
    
                // Load from Embedded Resources - This function is not called if the Assembly is already
                // in the same folder as the app.
                var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(dllName));
                if (resources.Any())
                {

                    // 99% of cases will only have one matching item, but if you don't,
                    // you will have to change the logic to handle those cases.
                    var resourceName = resources.First();
                    using (var stream = thisAssembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream == null) return null;
                        var block = new byte[stream.Length];

                        // Safely try to load the assembly.
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

        private void BtCancelSolve_OnClick(object sender, RoutedEventArgs e)
        {
          
            //var main = App.Current.MainWindow as MainWindow; // If not a static method, this.MainWindow would work
            //if (main.TcSudokus.SelectedIndex == 0)
            //{
            //    //main.CloseClassicBusyIndicator();
            //}
            //if (main.TcSudokus.SelectedIndex == 1)
            //{
            //    main.ClosePuzzleBusyIndicator();
            //}
        }
    }
}