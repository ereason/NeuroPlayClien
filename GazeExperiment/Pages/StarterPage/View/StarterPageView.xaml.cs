using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace GazeExperiment
{
    public sealed partial class StarterPage : Page
    {
        public StarterPageViewModel ViewModel { get; set; }

        public StarterPage()
        {
            InitializeComponent();
            var container = ((App)App.Current).Container;
            ViewModel = (StarterPageViewModel)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(StarterPageViewModel));
            DataContext = ViewModel;
        } 
    } 
}
