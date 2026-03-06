using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Reflection;
using Solvedoku.Properties;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solvedoku.Services.Repository;
using Solvedoku.Classes;
using Solvedoku.Views.Options;
using Solvedoku.ViewModels.OptionsWindow;
using Solvedoku.Views.AboutBox;
using Solvedoku.ViewModels.AboutBoxWindow;
using Solvedoku.ViewModels.ClassicSudoku;
using Solvedoku.ViewModels;
using Solvedoku.ViewModels.JigsawSudoku;
using Solvedoku.Services.MessageBox;

namespace Solvedoku
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        IHostApplicationBuilder builder = Host.CreateApplicationBuilder();
        public static IHost AppHost { get; private set; }
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register DbContext, repositories, and services
                    services.AddTransient(typeof(IRepositoryService<SudokuFile>), typeof(FileRepositoryService));
                    services.AddTransient(typeof(IMessageBoxService), typeof(MessageBoxService));
                    //services.AddScoped(typeof(RepositoryService<>));

                    // Register WPF windows and viewmodels
                    services.AddSingleton<ViewModelBase>();
                    services.AddSingleton<BaseSudokuViewModel>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<OptionsWindow>();
                    services.AddTransient<OptionsWindowViewModel>();
                    services.AddTransient<AboutBoxWindow>();
                    services.AddTransient<AboutBoxViewModel>();
                    services.AddTransient<ClassicSudokuViewModel>();
                    services.AddTransient<JigsawSudokuViewModel>();

                })
                .Build();

            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Settings.Default.Localization);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await AppHost.StartAsync();
            // Resolve MainWindow from DI and show it
            var main = AppHost.Services.GetRequiredService<MainWindow>();
            main.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (AppHost != null)
            {
                await AppHost.StopAsync();
                AppHost.Dispose();
            }
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