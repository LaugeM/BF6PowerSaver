using BF6PowerSaver.Stores;
using BF6PowerSaver.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BF6PowerSaver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NavigationStore navigationStore;
        private SearchStore searchStore;
        protected override void OnStartup(StartupEventArgs e)
        {
            navigationStore = new();
            searchStore = new();
            navigationStore.CurrentViewModel = new HomeViewModel(searchStore, navigationStore);
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}
