using AppSharedClasses.Models;
using AppSharedClasses.Services.Implementations;
using AppSharedClasses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GazeExperiment.Pages.Experiments
{
    public sealed partial class ShapeExperiment : Page
    {
        protected const double _discretization = 0.5;
        protected Random _rnd = new Random(DateTime.Now.Millisecond);

        private readonly NeuroPlayService _neuroPlayService = new NeuroPlayService();
        private readonly ISettingsService _settingsService = new SettingService();
        private readonly List<SettingFigures> _settings = new List<SettingFigures>();

        private DateTime _startTime;
        private MessageDialog dialogResult;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var userData = (User)e.Parameter; // get parameter

            var userSettings = new UserSettings()
            {
                ExperimentId = userData.ExperimentId,
                Name = userData.Name,
                Age = userData.Age,
                UserType = UserType.Elementary.ToString(),
                Case = "Фигуры"
            };

            _settingsService.SetUserSettings(userSettings);
            var st = Pages.Experiments.Settings.getSettings(userData.UserType);

            int countIteration = st.countIteration;
            double durationShow = st.durationShow;
            double durationPause = st.durationPause;

            var listShowValues = GetListPossibleValues(durationShow);
            var listPauseValues = GetListPossibleValues(durationPause);

            for (int i = 0; i < countIteration; i++)
            {
                _settings.Add(new SettingFigures
                {
                    DurationShow = listShowValues[_rnd.Next(0, listShowValues.Count)],
                    DurationPause = listPauseValues[_rnd.Next(0, listPauseValues.Count)],
                    IsGreenImage = _rnd.Next() % 2 == 0
                });
            }

            _settings[countIteration - 1].DurationPause = 0;

            CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await StartExperiment();
            });
        }

        public ShapeExperiment()
        {
            this.InitializeComponent();
            var container = ((App)App.Current).Container;
            dialogResult = new MessageDialog("Экспермент закончен.");
        }


        private async Task StartExperiment()
        {
            _startTime = DateTime.Now;
            //await _neuroPlayService.StartRecordAsync();
            //await _neuroPlayService.AddMarkerAsync("0", $"id:{_settingsService.GetExperimentId().Data}");
            await Task.Delay(2000);

            var greenBrush = new SolidColorBrush(Windows.UI.Colors.Green);
            var redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            for (int i = 0; i < _settings.Count; i++)
            {
                circle.Fill = _settings[i].IsGreenImage ? greenBrush : redBrush;

                string text = _settings[i].IsGreenImage ? AppSharedClasses.Resources.Messages.ShowGreenImage : AppSharedClasses.Resources.Messages.ShowRedImage;
                circle.Visibility = Visibility.Visible;
                //var result = await _neuroPlayService.AddMarkerAsync((_startTime - DateTime.Now).Milliseconds.ToString(), text);

                await Task.Delay((int)(_settings[i].DurationShow * 1000));
                circle.Visibility = Visibility.Collapsed;
                await Task.Delay((int)(_settings[i].DurationPause * 1000));
            }

            await Task.Delay(2000);
          //await _neuroPlayService.StopRecordAsync();

            dialogResult.Commands.Add(new UICommand("К началу", new UICommandInvokedHandler(this.CommandInvokedHandlerBack)));
            dialogResult.Commands.Add(new UICommand("Закрыть", new UICommandInvokedHandler(this.CommandInvokedHandler)));

            dialogResult.DefaultCommandIndex = 0;
            dialogResult.CancelCommandIndex = 1;

            await dialogResult.ShowAsync();
        }

        private void CommandInvokedHandlerBack(IUICommand command)
        {
            Frame.GoBack();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            Application.Current.Exit();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await _neuroPlayService.AddMarkerAsync((_startTime - DateTime.Now).Milliseconds.ToString(), AppSharedClasses.Resources.Messages.soundReaction);
        }

        protected List<double> GetListPossibleValues(double value)
        {
            if (value < _discretization)
            {
                return new List<double>() { value };
            }

            double variance = value / 2;
            double minValue = GetNearestMinValue(value - variance);
            double maxValue = GetNearestMaxValue(value + variance);
            var listPossibleValues = new List<double>();

            while (minValue <= maxValue)
            {
                listPossibleValues.Add(minValue);
                minValue += _discretization;
            }

            return listPossibleValues;
        }
        protected double GetNearestMinValue(double value)
        {
            double sum = 0;
            while (sum < value)
                sum += _discretization;

            return sum;
        }
        protected double GetNearestMaxValue(double value)
        {
            double sum = 0;
            while (sum <= value)
                sum += _discretization;

            return sum - _discretization;
        }
    }
}
