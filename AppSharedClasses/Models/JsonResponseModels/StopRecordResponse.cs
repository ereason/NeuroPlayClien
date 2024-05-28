namespace AppSharedClasses.Models.JsonResponseModels
{
    public class StopRecordResponse {
        public string baseName { get; set; }
        public string command { get; set; }
        public File[] files { get; set; }
        public bool result { get; set; }
        public string time { get; set; }
    }

    public class File {
        public string data { get; set; }
        public string type { get; set; }
    }

}