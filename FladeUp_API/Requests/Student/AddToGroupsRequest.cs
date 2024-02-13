namespace FladeUp_API.Requests.Student
{
    public class AddToGroupsRequest
    {
        public int[] GroupIds { get; set; }
        public int StudentId { get; set; }
    }
}
