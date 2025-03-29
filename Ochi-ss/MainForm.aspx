<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.Main.Master" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="Ochi_ss.MainForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<div class="main">
  <form id="form1" runat="server" style="display: inline-block; text-align: left;">
    <!-- ユーザー情報 -->
    <div id="CustomerInfo">
        <h6><asp:Label ID="LoginUserInfo" runat="server" Text="ログインユーザー: ○○"></asp:Label></h6>
        <h6><asp:Label ID="SessionID" runat="server" Text="セッションID: ####"></asp:Label></h6>
    </div>

    <!-- メインメニュー -->
    <div class="menu-container">
        <asp:Button id="EstimateOrder" Text="お見積り・ご注文" runat="server" CssClass="btn btn-primary btn-lg" OnClick="EstimateOrder_Click"/>
        <asp:Button id="EstimateOrderHist" Text="お見積り履歴" runat="server" CssClass="btn btn-primary btn-lg" OnClick="EstimateOrderHist_Click"/>
        <asp:Button id="OrderHist" Text="ご注文履歴" runat="server" CssClass="btn btn-primary btn-lg" OnClick="OrderHist_Click"/>
        <asp:Button id="DeliveryDest" Text="納入先管理" runat="server" CssClass="btn btn-primary btn-lg" OnClick="DeliveryDest_Click"/>
        <asp:Button id="DataOutput" Text="納品書発行・データ出力" runat="server" CssClass="btn btn-primary btn-lg" OnClick="DataOutput_Click"/>
    </div>
      
    <!-- メッセージ欄 -->
    <div class="message-container">
        <asp:GridView ID="gvAnnouncements" runat="server" AutoGenerateColumns="False" CssClass="table table-striped"
            EmptyDataText="お知らせはありません">
            <Columns>
                <asp:BoundField DataField="Date" HeaderText="日付" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="Message" HeaderText="お知らせ内容" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- ログアウトボタン -->
    <div class="exit">
        <asp:LoginStatus id="LoginStatus1" runat="server" CssClass="btn btn-secondary btn-md" />
    </div>
  </form>
</div>

</asp:Content>