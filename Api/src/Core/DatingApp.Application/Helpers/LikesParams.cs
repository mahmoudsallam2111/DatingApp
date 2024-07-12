namespace DatingApp.Application.Helpers
{
    public class LikesParams : PaginationParams
    {
        public long UserId { get; set; }
        public string Predicate { get; set; }
    }
}
