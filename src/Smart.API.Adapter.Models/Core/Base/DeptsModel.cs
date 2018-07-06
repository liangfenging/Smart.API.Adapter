using System.Collections.Generic;
namespace Smart.API.Adapter.Models
{
    public class DeptsModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public string deptId
        {
            get;
            set;
        }

        /// <summary>
        /// 组织编号
        /// </summary>
        public int deptNo
        {
            get;
            set;
        }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string deptName
        {
            get;
            set;
        }

        /// <summary>
        /// 父节点
        /// </summary>

        public string parentId
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>

        public string remark
        {
            get;
            set;
        }
    }


    public class requestDeptModel : PagesBase
    {
        public string parentId
        {
            get;
            set;
        }
    }

    public class responseDeptModel : PagesBase
    {
        public List<DeptsModel> depts
        {
            get;
            set;
        }
    }
}
