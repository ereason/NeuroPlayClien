using System;
using AppSharedClasses.Models;
using AppSharedClasses.Services.Interfaces;

namespace AppSharedClasses.Services.Implementations
{
public class SettingService : ISettingsService {
        private UserSettings _userSettings;

        public ServiceResult<string> GetExperimentId() {
            return new ServiceResult<string>(true, _userSettings.ExperimentId);
        }

        public ServiceResult<UserSettings> GetSettings() {
            return new ServiceResult<UserSettings>(true, _userSettings);
        }

        public ServiceResult<string> GetSettingsToString() {
            return new ServiceResult<string>(true, $"{_userSettings.ExperimentId} {_userSettings.Name} {_userSettings.Age} {_userSettings.UserType} {_userSettings.Case} {DateTime.Now.ToLongDateString()} {DateTime.Now.ToShortTimeString()}");
        }
        
        public ServiceResult IncrementIdExperiment() {
            _userSettings.ExperimentId = (int.Parse(_userSettings.ExperimentId)+1).ToString();
            return new ServiceResult(true);
        }

        public ServiceResult SetUserSettings(UserSettings settings) {
            _userSettings = settings;
            return new ServiceResult(true);
        }
    }
}