namespace WebAPI.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int _pageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
    }
}