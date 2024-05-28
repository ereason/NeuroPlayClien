
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using static System.Net.Mime.MediaTypeNames;

namespace GazeExperiment
{
    public class ExperimentNumberService : IExperimentNumberService
    {
        string key = "experimentNumber";
        public int number { get; private set; }

        public ExperimentNumberService() 
        {
            object storedValue = ApplicationData.Current.LocalSettings.Values[key];
            if (storedValue == null)
            {
                ApplicationData.Current.LocalSettings.Values[key] = 1;
                number = 1;
            }
            else
            {
                number = (int)storedValue;
            }
        }

        public void increaseNumber()
        {
            number++;
            ApplicationData.Current.LocalSettings.Values[key] = number;
        }
    }
}