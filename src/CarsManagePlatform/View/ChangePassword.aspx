<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="CarsManagePlatform.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
    </p>
    <h2>修改用户密码</h2>
    <p>&nbsp;</p>
    <p>
        原账号：&nbsp; 
        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
    &nbsp;
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName" ErrorMessage="原账号必须输入"></asp:RequiredFieldValidator>
    </p>
    <p>
        原密码：&nbsp; 
        <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password"></asp:TextBox>
    </p>
    <p>
        新密码：&nbsp; 
        <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password"></asp:TextBox>
    &nbsp;
    </p>
    <p>
        确认密码：<asp:TextBox ID="txtNewPwdAgain" runat="server" TextMode="Password"></asp:TextBox>
    &nbsp;
        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPwdAgain" ControlToValidate="txtNewPwd" ErrorMessage="两次输入的新密码不同"></asp:CompareValidator>
    </p>
    <p>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Label ID="lbTips" runat="server"></asp:Label>
    </p>
    <p>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;<asp:Button ID="btnChangePwd" runat="server" OnClick="btnChangePwd_Click" Text="修改" />
    </p>
</asp:Content>
