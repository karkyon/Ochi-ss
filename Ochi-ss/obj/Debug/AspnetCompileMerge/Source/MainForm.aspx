<%@ Page Title="" Language="C#" MasterPageFile="~/Ochi-ss.Master" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="Ochi_ss.MainForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="main">
    <div class="container">
        <div class="middle">

            <form id="form1" runat="server">
            <div id="CustomerInfo">
                <h6><asp:Label ID="LoginUserInfo" runat="server" Text=""></asp:Label></h6>
            </div>
            <div>
                <h4> メインメニュー</h4>
                <div class="col">
                <asp:Button id="EstimateOrder" Text="お見積り・ご発注" runat="server" CssClass="btn btn-primary btn-lg" width="50%" OnClick="EstimateOrder_Click"/>
                <br />
                <br />
                <asp:Button id="EstimateOrderHist" Text="お見積り・ご発注履歴" runat="server" CssClass="btn btn-primary btn-lg" width="50%" OnClick="EstimateOrderHist_Click"/>
                <br />
                <br />
                <asp:Button id="DeliveryDest" Text="納入先管理" runat="server" CssClass="btn btn-primary btn-lg" width="50%" OnClick="DeliveryDest_Click"/>
                <br />
                <br />
                <asp:Button id="DataOutput" Text="納品書発行・データ出力" runat="server" CssClass="btn btn-primary btn-lg" width="50%" OnClick="DataOutput_Click"/>
                <br />
                <br />
                <asp:Button id="UserList" Text="ユーザマスタ管理" runat="server" CssClass="btn btn-primary btn-lg" width="50%" OnClick="UserList_Click"/>
                <br />
                <br />
                </div>
                <br />
                <br />
                <asp:LoginStatus ID="LoginStatus1" runat="server" CssClass="btn btn-warning" />
            </div>
            </form>

        </div>
    </div>
</div>

</asp:Content>