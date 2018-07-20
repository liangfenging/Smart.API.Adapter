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
    public partial class _Default : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if(!IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlDataAdapter adapter = new MySqlDataAdapter("Select * from tb_cars order by ID desc limit 6", con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            this.GridView1.DataSource = ds.Tables[0];
            this.GridView1.DataBind();
            con.Close();
        }

        protected void btnAddCar_Click(object sender, EventArgs e)
        {
            string strRoomID = txtRoomID.Text;
            string strPlateNumber = txtPlateNumber.Text;
            string strStarTime = Request["starTime"].ToString();
            string strEndTime = Request["endTime"].ToString();
            string strRemark = txtRemark.Text;

            if(string.IsNullOrEmpty(strStarTime))
            {
                this.lbTips.Text = "请填入住时间！";
                return;
            }
            if(string.IsNullOrEmpty(strEndTime))
            {
                this.lbTips.Text = "请填写结束时间！";
                return;
            }

            //向JieLink+平台发送
            BlackWhiteListModel blackWhiteCar = new BlackWhiteListModel();
            blackWhiteCar.PlateNumber = strPlateNumber;
            blackWhiteCar.BlackWhiteType = 3;
            blackWhiteCar.StartDate = strStarTime;
            blackWhiteCar.EndDate = strEndTime;
            blackWhiteCar.Reason = "";
            blackWhiteCar.Remark = strRemark;

            JielinkApi jieLinApi = new JielinkApi();
            APIResultBase<BlackWhiteListModel> result = jieLinApi.AddBackWhiteList(blackWhiteCar);
            if (result.code == "0")
            {
                //下发成功，则插入记录到数据库
                MySqlConnection con = new MySqlConnection(connectionString);
                string strCmd = "insert into tb_cars(RoomID,CarPlateNumber,StarTime,EndTime,Remark) values('" + strRoomID + "','" + strPlateNumber + "','" + strStarTime + "','" + strEndTime + "','" + strRemark + "');";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(strCmd, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Bind(); //重新绑定数据，就可以查看刚插入的记录了

                    //清空Panel中TextBox控件内容
                    foreach(Control c in Panel1.Controls)
                    {
                        if(c is TextBox)
                        {
                            (c as TextBox).Text = "";
                        }
                    }
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
                lbTips.Text = "添加记录失败，因为没有能下发车辆信息到平台！";
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            Bind();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            Bind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = GridView1.DataKeys[e.RowIndex].Value.ToString();
            string strDel = "delete from tb_cars where ID=" + id;

            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(strDel,con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            Bind();
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
                this.lbTips.Text = "更新失败！";
            }
            GridView1.EditIndex = -1;
            Bind();
        }
    }
}
