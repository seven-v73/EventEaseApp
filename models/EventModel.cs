namespace EventEaseApp.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public int RegisteredAttendees { get; set; }
    }
}