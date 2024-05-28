using Windows.UI.Xaml.Controls;

namespace GazeExperiment
{
    public interface INavigationService
    {
        Frame Frame { get;}
        bool CanGoBack { get; }
        void GoBack();
        void Navigate<T>(object args = null);
    }
}