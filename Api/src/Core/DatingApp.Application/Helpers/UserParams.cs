using BuildingBlocks.Helpers;

namespace DatingApp.Application.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = DatingAppConsts.MaxPageSize;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? CurrentUser { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = DatingAppConsts.MinAge;
        public int MaxAge { get; set; } = DatingAppConsts.MaxAge;
        public string OrderBy { get; set; } = "LastActive";
    }
}
