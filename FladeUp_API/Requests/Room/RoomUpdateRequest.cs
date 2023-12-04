namespace FladeUp_API.Requests.Room
{
    public class RoomUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
