using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace CarsManagePlatform
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(Session["UserName"] == null)
                {
                    this.lbTips.Text = "用户已经登出！";
                }
            }
        }

        protected void btnChangePwd_Click(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                this.lbTips.Text = "用户已经登出！";
                return;
            }

            SendService service = new SendService();

            string userName = this.txtUserName.Text.Trim();
            string oldPassword = service.GetMD5(txtOldPwd.Text.Trim());
            //string newPwd = this.txtNewPwd.Text.Trim();
            string newPwdAgain = service.GetMD5(txtNewPwdAgain.Text.Trim());

            MySqlConnection con = new MySqlConnection(connectionString);
            string cmdString = "select * from tb_users where UserName='" + userName + "' and Password='" + oldPassword + "'";
            MySqlCommand cmd = new MySqlCommand(cmdString, con);

            try
            {
                con.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    dr.Close();
                    //更新数据库中密码
                    string updateString = "update tb_users set Password='"+ newPwdAgain +"' where UserName='"+ userName +"'";
                    cmd.CommandText = updateString;
                    cmd.ExecuteNonQuery();
                    this.lbTips.Text = "密码更新成功！";
                }
                else
                {
                    this.lbTips.Text = "账号或密码错误！";
                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
    }
}