using Windows.UI.Xaml.Controls;

namespace GazeExperiment
{
    public class NavigationService: INavigationService
    {
        private Frame _frame;
        public Frame Frame => _frame;


        public NavigationService()
        {
            this._frame = new Frame();
        }

        public bool CanGoBack => this.Frame.CanGoBack;

        public void GoBack() => this.Frame.GoBack();

        public void Navigate<T>(object args)
        {
            this.Frame.Navigate(typeof(T), args);
        }
    }
}