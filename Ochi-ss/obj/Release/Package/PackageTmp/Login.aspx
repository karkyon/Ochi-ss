<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ochi_ss.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="main table-responsive" style="margin: 0 auto; text-align: center;">
        <form id="form1" runat="server" style="display: inline-block; text-align: left;">
            <fieldset class="clearfix">
                <label for="txtUsername">ユーザー名</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="ユーザ名を入力してください。" value="j.arima" required></asp:TextBox>
                <br />

                <label for="txtPassword">パスワード</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="パスワードを入力してください。" value="333" required></asp:TextBox>
                <br />
                    
                <div class="checkbox">
                    <asp:CheckBox ID="chkRememberMe" Text="ユーザ情報を記憶しておく" runat="server" />
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="ログイン" OnClick="ValidateUser" Class="btn btn-primary" />
                    
                <div id="dvMessage" runat="server" visible="false" class="alert alert-danger">
                    <strong>エラー</strong>
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
            </fieldset>
        </form>
    </div> <!-- end login -->

</asp:Content>