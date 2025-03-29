<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ochi_ss.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<div class="login-container">

    <form id="form1" runat="server">
        <fieldset class="clearfix">
            <label for="txtUsername">ユーザー名</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="ユーザ名を入力" required></asp:TextBox>
            
            <label for="txtPassword">パスワード</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="パスワードを入力" required></asp:TextBox>
            
            <div class="checkbox">
                <asp:CheckBox ID="chkRememberMe" Text="ユーザ情報を記憶する" runat="server" />
            </div>
            
            <asp:Button ID="btnLogin" runat="server" Text="ログイン" OnClick="ValidateUser" CssClass="btn btn-primary" />
            
            <div id="dvMessage" runat="server" visible="false" class="alert alert-danger">
                <strong>エラー</strong>
                <asp:Label ID="lblMessage" runat="server" />
            </div>
        </fieldset>
    </form>

</div>


</asp:Content>