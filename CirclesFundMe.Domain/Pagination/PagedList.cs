﻿namespace CirclesFundMe.Domain.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellation)
        {
            int count = await source.CountAsync(cancellation);
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellation);
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
        public static PagedList<T> ToPagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
