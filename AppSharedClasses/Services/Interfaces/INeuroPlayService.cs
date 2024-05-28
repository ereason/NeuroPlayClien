using System.Threading.Tasks;

namespace AppSharedClasses.Services.Interfaces
{
    public interface INeuroPlayService {
        Task<bool> IsConnectedAsync();
        Task StartRecordAsync();
        Task StopRecordAsync();
        //Task<bool> AddMarkerAsync(string position, string text);
        Task<string> AddMarkerAsync(string position, string text);
    }
}