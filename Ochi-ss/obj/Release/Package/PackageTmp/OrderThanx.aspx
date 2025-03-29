<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.Edit.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="OrderThanx.aspx.cs" Inherits="Ochi_ss.OrderThanx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Async = "true" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="main table-responsive" style="margin: 0 auto; text-align: center;">
  <form id="form1" runat="server" style="display: inline-block; text-align: left;">

    <!-- メニューボタン -->
    <table class="TopButtonArea" runat="server">
      <tbody>
        <tr>
            <td>
                <asp:Button 
                    id="BtnMainForm" 
                    Text="メインメニュー" 
                    runat="server" 
                    CssClass="btn btn-primary btn-sm" 
                    OnClick="BtnMainForm_Click" />
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
      </tbody>
    </table>

    <asp:ScriptManager ID="ScriptManager"
        runat="server"
        EnableScriptGlobalization="True">
    </asp:ScriptManager>
        
    <!-- SessionID情報 -->
    <div id="SessionInfo">
        <h6><asp:Label ID="SessionID" runat="server" Text=""></asp:Label></h6>
    </div>

    <table id="OrderInfomationSection" class="single free" runat="server">
          <tbody>
            <tr>
              <td colspan="4" runat="server" class="single free tlbl">ご注文ありがとうございます</td>
            </tr>
            <tr>
              <td colspan="1" runat="server" class="single free tlbl">ご注文番号: </td>
              <td colspan="1" runat="server" class="single free tbody nowrap">
                <asp:TextBox ID="txbOrderNo" runat="server" Enabled="False" ReadOnly="True" TabIndex="1"></asp:TextBox>
              </td>

              <td colspan="1" runat="server" class="single free tlbl">ご注文日: </td>
              <td colspan="1" runat="server" class="single free tbody">
                <asp:TextBox ID="txbOrderDate" runat="server" Enabled="False" ReadOnly="True" TabIndex="2"></asp:TextBox>
              </td>
            </tr>
          </tbody>
        </table>


    <!--DEBUG START-->
    <table id="MessageArea" class="single">
      <tr>
        <td class="multi free tbody"><asp:Label ID="lblDubugText1" runat="server" Text=""></asp:Label> </td>
        <td class="multi free tbody"><asp:Label ID="lblDubugText2" runat="server" Text=""></asp:Label></td>
        <td class="multi free tbody"><asp:Label ID="lblDubugText3" runat="server" Text=""></asp:Label></td>
        <td class="multi free tbody"><asp:Label ID="lblDubugText4" runat="server" Text=""></asp:Label></td>
        <td class="multi free tbody"><asp:Label ID="lblDubugText5" runat="server" Text=""></asp:Label></td>
      </tr>
    </table>
    <!--DEBUG END-->

  </form>
</div>

</asp:Content>