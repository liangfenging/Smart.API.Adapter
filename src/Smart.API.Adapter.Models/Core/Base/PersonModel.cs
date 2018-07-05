
namespace Smart.API.Adapter.Models
{
    /// <summary>
    /// 人事资料
    /// </summary>
    public class PersonModel
    {
        /// <summary>
        /// 人员Id
        /// </summary>
        public string personId
        {
            get;
            set;
        }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string personNo
        {
            get;
            set;
        }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string personName
        {
            get;
            set;
        }

        /// <summary>
        /// 性别
        /// 0女 1男
        /// </summary>
        private int personGender
        {
            get;
            set;
        }

        /// <summary>
        /// 组织结构Id
        /// </summary>
        public string deptId
        {
            get;
            set;
        }

        /// <summary>
        /// 组织名称路径
        /// </summary>
        public string deptName
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

        /// <summary>
        /// 人员图片Base64String
        /// </summary>
        public string personPhoto
        {
            get;
            set;
        }
        /// <summary>
        /// 证件类型
        /// GENERIDENT
        /// IDENTITY 二代身份证
        /// 
        /// </summary>
        public string certificateType
        {
            get;
            set;
        }
        
        /// <summary>
        /// 证件号
        /// </summary>
        public string identityNo
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile
        {
            get;
            set;
        }

        public string tel1
        {
            get;
            set;
        }

        public string tel2
        {
            get;
            set;
        }

        public string email
        {
            get;
            set;
        }

        public string roomNo
        {
            get;
            set;
        }

        /// <summary>
        /// 房号
        /// </summary>
        public string address
        {
            get;
            set;
        }

        /// <summary>
        /// 住户类型
        /// 1职员
        /// 2业主
        /// 3租户
        /// 4商户
        /// 5临时人员
        /// 6户主
        /// 7其他
        /// </summary>
        public int tenementType
        {
            get;
            set;
        }
    }
}
