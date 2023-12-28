using Google.Apis.Util;
using static Google.Apis.Requests.BatchRequest;

namespace FladeUp_API.Models
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalPages, int totalRecords)
        {
            this.TotalPages = totalPages;
            this.TotalRecords = totalRecords;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
