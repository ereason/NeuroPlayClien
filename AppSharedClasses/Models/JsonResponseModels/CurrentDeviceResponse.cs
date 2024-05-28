namespace AppSharedClasses.Models.JsonResponseModels
{
    public class CurrentDeviceResponse {
        public int BSF { get; set; }
        public int HPF { get; set; }
        public int LPF { get; set; }
        public string command { get; set; }
        public int currentChannels { get; set; }
        public object[] currentChannelsNames { get; set; }
        public int currentFrequency { get; set; }
        public Device device { get; set; }
        public bool isRecording { get; set; }
        public object[] quality { get; set; }
        public bool result { get; set; }
        public bool searching { get; set; }
        public string time { get; set; }
    }

    public class Device {
        public int battery { get; set; }
        public int maxChannels { get; set; }
        public string model { get; set; }
        public string name { get; set; }
        public int preferredChannelCount { get; set; }
        public string serialNumber { get; set; }
    }
}