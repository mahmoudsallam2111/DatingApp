using BuildingBlocks.Helpers;

namespace DatingApp.Application.Helpers
{
    public class UserParams : PaginationParams
    {
        public string? CurrentUser { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = DatingAppConsts.MinAge;
        public int MaxAge { get; set; } = DatingAppConsts.MaxAge;
        public string OrderBy { get; set; } = "LastActive";
    }
}
