namespace DatingApp.Infrastructure.Persistence.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int itemPerPage, int totalItems, int tOTALPageS)
        {
            CurrentPage = currentPage;
            ItemPerPage = itemPerPage;
            TotalItems = totalItems;
            this.tOTALPageS = tOTALPageS;
        }

        public int CurrentPage { get; set; }
        public int ItemPerPage { get; set; }
        public int TotalItems { get; set; }
        public int tOTALPageS { get; set; }
    }
}
