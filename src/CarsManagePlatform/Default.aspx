<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CarsManagePlatform.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="page">
                <div class="header">
                    <div class="title2">
                        <h1>欢迎使用车辆管理平台</h1>
                    </div>
                </div>
            </div>
        </div>
        <p>
            &nbsp;
        </p>
        <p>
            &nbsp;
        </p>
        <p>
            &nbsp;
        </p>
        <h2 align="center">&nbsp;
            用户登录</h2>
        <table align="center" border="1" width="400px">
            <tr>
                <td align="center">
                    <p />
                    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Width="300px" BackColor="#99CCFF" BorderColor="#3399FF" BorderStyle="Groove">
                        <p />
                        &nbsp;<p />
                        <asp:Label ID="Label1" runat="server" Text="账号："></asp:Label>
                        &nbsp;
            <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                        &nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" BorderColor="Red" ControlToValidate="txtUserName" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <asp:Label ID="Label2" runat="server" Text="密码："></asp:Label>
                        &nbsp;
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        &nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" BorderColor="Red" ControlToValidate="txtPassword" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <asp:Label ID="lbTips" runat="server"></asp:Label>
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="登  陆" />
                        <br />
                    </asp:Panel>
                    <p />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
