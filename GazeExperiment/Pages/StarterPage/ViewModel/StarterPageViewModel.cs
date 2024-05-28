using AppSharedClasses.Services.Interfaces;
using GazeExperiment;
using System.Threading.Tasks;
using System;
using Windows.UI.Popups;
using GazeExperiment.Pages.Experiments;
using System.ComponentModel;

public class StarterPageViewModel : INotifyPropertyChanged
{
    protected INeuroPlayService NeuroPlayService { get; }
    protected INavigationService NavigationService { get; }
    protected IExperimentNumberService ExperimentNumberService { get; }

    public StarterPageModel Model;

    public string AgeField
    {
        get
        {
            return Model.age.ToString();
        }
        set
        {
            int n;
            bool isNumeric = int.TryParse(value.ToString(), out n);
            if (isNumeric && n > 0)
            {
                Model.age = n;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AgeField"));
        }
    }

    public StarterPageViewModel(
        INeuroPlayService neuroPlayService,
        INavigationService navigationService, 
        IExperimentNumberService experimentNumberService)
    {
        NeuroPlayService = neuroPlayService;
        NavigationService = navigationService;
        ExperimentNumberService = experimentNumberService;
        Model = new StarterPageModel();
        Model.experimentNumber = ExperimentNumberService.number;
    }

    public async void startExp()
    {
        if (!Model.IsValid)
        {
            var messageDialog = new MessageDialog("Ошибка ввода данных");
            await messageDialog.ShowAsync();
            return;
        }

        //if (await NeuroPlayIsConected())
        {
            var userData = new AppSharedClasses.Models.User(
                Model.experimentNumber.ToString(),
                Model.name,
                (uint)Model.age,
                Model.userTypeOptions[Model.userTypeOptionIndex].ComboBoxOption);

            ExperimentNumberService.increaseNumber();

            if (Model.expetipemtOptionIndex == 0)
            {
                NavigationService.Navigate<ShapeExperiment>(userData);
            }

            if (Model.expetipemtOptionIndex == 1)
            {
                NavigationService.Navigate<ImagesExperimentPage>(userData);
            }
        }
    }

    private async Task<bool> NeuroPlayIsConected()
    {
        try
        {
            return await NeuroPlayService.IsConnectedAsync();
        }
        catch
        {
            var messageDialog = new MessageDialog(AppSharedClasses.Resources.Messages.IsNotConnected);
            await messageDialog.ShowAsync();
            return false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string property)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(property));
    }
}