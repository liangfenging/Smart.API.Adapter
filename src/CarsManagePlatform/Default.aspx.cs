using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace CarsManagePlatform
{
    public partial class Login : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
               if(Session["UserName"] != null)
               {
                   Response.Redirect("~/CarsManage.aspx");
               }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SendService service = new SendService();
            string userName = this.txtUserName.Text.Trim();
            string password = service.GetMD5(txtPassword.Text.Trim());

            MySqlConnection con = new MySqlConnection(connectionString);
            string cmdString = "select * from tb_users where UserName='"+ userName +"' and Password='"+ password +"'";
            MySqlCommand cmd = new MySqlCommand(cmdString, con);

            try
            {
                con.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Session["UserName"] = dr.GetValue(1);
                    Response.Redirect("~/CarsManage.aspx");
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