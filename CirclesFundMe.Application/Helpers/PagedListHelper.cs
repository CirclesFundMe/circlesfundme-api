namespace CirclesFundMe.Application.Helpers
{
    public class PagedListHelper<T>
    {
        public static PagedList<T> MapToList<U>(PagedList<U> inputList, IMapper mapper)
        {
            List<T> data = [];

            inputList.ForEach(input => { data.Add(mapper.Map<T>(input)); });

            return new(data, inputList.TotalCount, inputList.CurrentPage, inputList.PageSize);
        }

        public static MetaData GetPaginationInfo(PagedList<T> result)
        {
            return new MetaData()
            {
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages,
                HasNext = result.HasNext,
                HasPrevious = result.HasPrevious
            };
        }
        public record MetaData
        {
            public int TotalCount { get; set; }
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public bool HasNext { get; set; }
            public bool HasPrevious { get; set; }
        }
    }
}
