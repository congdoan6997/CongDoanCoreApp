using System.Collections.Generic;

namespace CongDoanCoreApp.Utilities.Dtos
{
    public class PageResult<T> : PagedResultBase where T : class
    {
        public PageResult()
        {
            Results = new List<T>();
        }

        public IList<T> Results { get; set; }
    }
}