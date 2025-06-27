namespace CirclesFundMe.Domain.Pagination.QueryParams
{
    public abstract record BaseParam
    {
        private readonly int _maxPageSize = 100;
        private int _pageSize = 25;

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; } = 1;

        [JsonProperty("pageSize")]
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > _maxPageSize) ? _maxPageSize : value; }
        }

        [JsonProperty("searchKey")]
        public string? SearchKey { get; set; }
    }
}
