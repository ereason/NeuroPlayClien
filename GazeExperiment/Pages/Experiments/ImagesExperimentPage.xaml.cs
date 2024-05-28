using AppSharedClasses.Services.Implementations;
using AppSharedClasses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppSharedClasses.Models;
using Windows.Storage;
using System.Collections.Concurrent;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace GazeExperiment
{
    public sealed partial class ImagesExperimentPage : Page
    {
        protected const double _discretization = 0.5;
        protected Random _rnd = new Random(DateTime.Now.Millisecond);
        
        private readonly NeuroPlayService _neuroPlayService = new NeuroPlayService();
        private readonly ISettingsService _settingsService = new SettingService();
        private readonly List<SettingImagesAndSoud> _settings = new List<SettingImagesAndSoud>();

        private DateTime _startTime;
        private MessageDialog dialogResult;

        static Random rnd = new Random();
        MediaElement PlayMusic = new MediaElement();
        StorageFolder Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

        public ImagesExperimentPage()
        {
            this.InitializeComponent();
            dialogResult = new MessageDialog("Экспермент закончен.");
        }
     
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var userData = (User)e.Parameter; // get parameter
      
            var userSettings = new UserSettings()
            {
                ExperimentId = userData.ExperimentId,
                Name = userData.Name,
                Age = userData.Age,
                UserType = userData.UserType.ToString(),
                Case = "Изображения и звук"
            };

            _settingsService.SetUserSettings(userSettings);

            int countIteration = 50;
            double durationShow = 2;
            double durationPause = 2;

            var listShowValues = GetListPossibleValues(durationShow);
            var listPauseValues = GetListPossibleValues(durationPause);

            for (int i = 0; i < countIteration; i++)
            {
                _settings.Add(new SettingImagesAndSoud
                {
                    DurationShow = listShowValues[_rnd.Next(0, listShowValues.Count)],
                    DurationPause = listPauseValues[_rnd.Next(0, listPauseValues.Count)],
                    IsShowImage = _rnd.Next() % 2 == 0
                });
            }

            _settings[countIteration - 1].DurationPause = 0;

            CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await StartExperiment();

            });
        }

        private async Task StartExperiment() {
            Folder = await Folder.GetFolderAsync("Resources");
            Folder = await Folder.GetFolderAsync("Sounds");
            StorageFile sf = await Folder.GetFileAsync("MyFile.mp3");
            PlayMusic.AutoPlay = false;
            PlayMusic.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);

            _startTime = DateTime.Now;
            //await _neuroPlayService.StartRecordAsync();
            //await _neuroPlayService.AddMarkerAsync("0", $"id:{_settingsService.GetExperimentId().Data}");
          
            var images = await GetImages();
            circle.Visibility = Visibility.Collapsed;
            await Task.Delay(2000);
            for (int i = 0; i < 2 ; i++) {
            
                string text = _settings[i].IsShowImage ?
                    AppSharedClasses.Resources.Messages.imgShowed : AppSharedClasses.Resources.Messages.playedSound;

                if (_settings[i].IsShowImage)
                {
                    circle.Source = randomElem(images);
                    circle.Visibility = Visibility.Visible;
                }
                else
                {
                    PlayMusic.Play();
                }  

              //var result = await _neuroPlayService.AddMarkerAsync((_startTime - DateTime.Now).Milliseconds.ToString(), text);

                await Task.Delay((int)(_settings[i].DurationShow * 1000));
                PlayMusic.Stop();
                circle.Visibility = Visibility.Collapsed;
                circle.Source = null;
                await Task.Delay((int)(_settings[i].DurationPause * 1000));
            }
            
            await Task.Delay(2000);
           // await _neuroPlayService.StopRecordAsync();
 
            dialogResult.Commands.Add(new UICommand("К началу",   new UICommandInvokedHandler(this.CommandInvokedHandlerBack)));
            dialogResult.Commands.Add(new UICommand("Закрыть",  new UICommandInvokedHandler(this.CommandInvokedHandler)));
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

        private async void Button_Click2(object sender, RoutedEventArgs e)
        {
            await _neuroPlayService.AddMarkerAsync((_startTime - DateTime.Now).Milliseconds.ToString(), AppSharedClasses.Resources.Messages.imgReaction);
        }
     
        BitmapImage randomElem(List<BitmapImage> list)
        {
            int r = rnd.Next(list.Count);
     
            return list[r];
        }

        protected async Task<List<BitmapImage>> GetImages()
        {
            var humansImages = new ConcurrentBag<Windows.UI.Xaml.Media.Imaging.BitmapImage>();
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync(@"Resources\Images\");
            var files = await folder.GetFilesAsync();
            
            foreach (var file in files)
            {
                humansImages.Add(await ResizedImage(file, 4096, 2160  ));
            }

            return humansImages.ToList();
        }

        public  async Task<BitmapImage> ResizedImage(StorageFile ImageFile, int maxWidth, int maxHeight)
        {
            IRandomAccessStream inputstream = await ImageFile.OpenReadAsync();
            BitmapImage sourceImage = new BitmapImage();
            sourceImage.SetSource(inputstream);
            var origHeight = sourceImage.PixelHeight;
            var origWidth = sourceImage.PixelWidth;
            var ratioX = maxWidth / (float)origWidth;
            var ratioY = maxHeight / (float)origHeight;
            var ratio = Math.Min(ratioX, ratioY);
            var newHeight = (int)(origHeight * ratio);
            var newWidth = (int)(origWidth * ratio);

            sourceImage.DecodePixelWidth = newWidth;
            sourceImage.DecodePixelHeight = newHeight;

            return sourceImage;
        }

        protected List<double> GetListPossibleValues(double value) {
            if (value < _discretization) {
                return new List<double>() { value };
            }

            double variance = value / 2;
            double minValue = GetNearestMinValue(value - variance);
            double maxValue = GetNearestMaxValue(value + variance);
            var listPossibleValues = new List<double>();

            while (minValue <= maxValue) {
                listPossibleValues.Add(minValue);
                minValue += _discretization;
            }

            return listPossibleValues;
        }
        protected double GetNearestMinValue(double value) {
            double sum = 0;
            while (sum < value)
                sum += _discretization;

            return sum;
        }
        protected double GetNearestMaxValue(double value) {
            double sum = 0;
            while (sum <= value)
                sum += _discretization;

            return sum - _discretization;
        }
    }
}