namespace AppSharedClasses.Models.JsonResponseModels
{
    public class AddMarkerResponse {
        public string command { get; set; }
        public int pos { get; set; }
        public int pos_ms { get; set; }
        public bool result { get; set; }
        public string time { get; set; }
    }
}