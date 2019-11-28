using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Model
{
    public class PagingModel
    {
        public PagingModel()
        {
            this.PageNo = 1;
            this.PageSize = 10;
        }

        //[Ignore]
        public int PageNo { get; set; }

        //[Ignore]
        public int PageSize { get; set; }
    }

    public class PagingResponseModel<T> : PagingModel where T : class
    {
        public List<T> List { get; set; }

        public int Total { get; set; }

        public int TotalPage
        {
            get
            {
                if (PageSize == 0)
                {
                    return 0;
                }
                else
                {
                    return Total % PageSize == 0 ? Total / PageSize : Total / PageSize + 1;
                }
            }
        }
    }
}
