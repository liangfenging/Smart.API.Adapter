using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using Smart.API.Adapter.Biz;
using Smart.API.Adapter.Models;

namespace CarsManagePlatform
{
    public partial class SearchCar : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRoomID.Text))
            {
                lbResult.Text = "请输入房间号！";
                return;
            }

            string roomID = this.txtRoomID.Text;
            int i = Bind(roomID);

            if (i == 0)
            {
                lbResult.Text = "没有记录！";
            }
        }

        private int Bind(string roomID)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter("Select * from tb_cars where RoomID='" + roomID + "' order by ID desc", con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            this.GridView1.DataSource = ds.Tables[0];
            this.GridView1.DataBind();
            con.Close();

            int num = ds.Tables[0].Rows.Count;
            return num;
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            Bind(txtRoomID.Text.Trim());
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = GridView1.DataKeys[e.RowIndex].Value.ToString();
            string roomID = ((TextBox)GridView1.Rows[e.RowIndex].Cells[3].Controls[0]).Text.ToString().Trim();
            string carPlateNumber = ((TextBox)GridView1.Rows[e.RowIndex].Cells[4].Controls[0]).Text.ToString().Trim();
            string starTime = ((TextBox)GridView1.Rows[e.RowIndex].Cells[5].Controls[0]).Text.ToString().Trim();
            string endTime = ((TextBox)GridView1.Rows[e.RowIndex].Cells[6].Controls[0]).Text.ToString().Trim();
            string remark = ((TextBox)GridView1.Rows[e.RowIndex].Cells[7].Controls[0]).Text.ToString().Trim();

            //向JieLink+平台发送
            BlackWhiteListModel blackWhiteCar = new BlackWhiteListModel();
            blackWhiteCar.PlateNumber = carPlateNumber;
            blackWhiteCar.BlackWhiteType = 3;
            blackWhiteCar.StartDate = starTime;
            blackWhiteCar.EndDate = endTime;
            blackWhiteCar.Reason = "";
            blackWhiteCar.Remark = remark;

            JielinkApi jieLinApi = new JielinkApi();
            APIResultBase<BlackWhiteListModel> result = jieLinApi.AddBackWhiteList(blackWhiteCar);

            if (result.code == "0")
            {
                string strUpdate = "update tb_cars set RoomID='" + roomID + "',CarPlateNumber='" + carPlateNumber + "',StarTime='" + starTime + "',EndTime='" + endTime + "',Remark='" + remark + "' where ID=" + id;

                //连接数据库，执行更新语句
                MySqlConnection con = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand(strUpdate, con);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    this.lbResult.Text = "更新记录成功！";
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    con.Close();
                }

            }
            else
            {
                this.lbResult.Text = "更新失败！";
            }
            GridView1.EditIndex = -1;
            Bind(txtRoomID.Text.Trim());
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            Bind(txtRoomID.Text.Trim());
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = GridView1.DataKeys[e.RowIndex].Value.ToString();
            string strDel = "delete from tb_cars where ID=" + id;

            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(strDel, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            this.lbResult.Text = "删除记录成功！";
            Bind(txtRoomID.Text.Trim());
        }
    }
}
