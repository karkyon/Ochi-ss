<%@ Page Title="" Language="C#" MasterPageFile="~/Ochi-ss.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ochi_ss.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="main">
    <div class="container">
        <div class="middle">

            <div id="login">

                <form id="form1" runat="server">

                <fieldset class="clearfix">

                    <label for="txtUsername">ユーザー名</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter Username" required></asp:TextBox>
                    <br />

                    <label for="txtPassword">パスワード</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter Password" required></asp:TextBox>
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

                <div class="clearfix"></div>

                </form>

                <div class="clearfix"></div>

            </div> <!-- end login -->

        </div>
    </div>
</div>
</asp:Content>