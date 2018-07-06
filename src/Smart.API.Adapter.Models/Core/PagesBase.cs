
namespace Smart.API.Adapter.Models
{
    public class PagesBase
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int pageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int totalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount
        {
            get;
            set;
        }
    }
}
