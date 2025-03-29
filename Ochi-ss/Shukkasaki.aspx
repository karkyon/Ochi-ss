<%@ Page Title="" Language="C#" MasterPageFile="~/Ochi-ss.Edit.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Shukkasaki.aspx.cs" Inherits="Ochi_ss.Shukkasaki" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Async = "true" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 90px;
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main">

    <form id="form1" runat="server">
        
        <asp:Button id="BtnMainForm" Text="メインメニュー" runat="server" CssClass="btn btn-primary" OnClick="BtnMainForm_Click" />
        <asp:Button id="BtnSearch" Text=" 検 索 " runat="server" CssClass="btn btn-primary" OnClick="BtnSearch_Click" />
        <asp:Button id="BtnDelete" Text=" 削 除 " runat="server" CssClass="btn btn-primary" OnClick="BtnDelete_Click" />
        <asp:Button id="BtnUpdate" Text=" 保 存 " runat="server" CssClass="btn btn-primary" OnClick="BtnUpdate_Click" />
                          <asp:TextBox ID="Information" runat="server" Enabled="False" ReadOnly="True" TabIndex="0" ForeColor="Red" Width="457px"></asp:TextBox>
        <h4><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/4cube.gif" />出荷先編集</h4>
        
        <div ID="EditSection" class="fa-edit">
            
            <!-- 得意先情報 -->
            <table id="EstOrderHeader" runat="server" class="table table-bordered table-sm table-responsive">
                <thead class="thead-dark">
                    <tr>
                      <th colspan="1" runat="server"><label>お客様名</label></th>
                      <td colspan="1" runat="server">
                          <asp:TextBox ID="CstCode" runat="server" Font-Size="Small" Enabled="False" ReadOnly="True" TabIndex="0"></asp:TextBox>
                        </td>
                      <td colspan="1" runat="server">
                        <asp:TextBox ID="CustomerName" runat="server" Enabled="False" ReadOnly="True" TabIndex="0" Width="400px"></asp:TextBox>
                      </td>
                    </tr> 
                </thead>
            </table>
            <!-- 出荷先情報 -->
            <div runat="server" id="DivDistination">
                
            <table id="EstOrderDistination" runat="server" class="table table-bordered table-sm table-responsive">
                <thead class="thead-dark">
                    <tr>
                      <th colspan="1" runat="server"><label>出荷先</label></th>
                      <td colspan="1" runat="server">
                        <asp:TextBox ID="DistCompCd" runat="server" Font-Size="Small" TabIndex="7" name="DistCompCd"></asp:TextBox>
                          <asp:Button ID="BtnShukkasakiList" runat="server" Font-Size="Small" Text=" > " OnClick="BtnShukkasakiList_Click" />
                      </td>
                      <td colspan="10" runat="server">
                        <asp:TextBox ID="DistCompName" runat="server" Font-Size="Small" TabIndex="8" Width="300px"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <th colspan="1" runat="server"><label>出荷先部署</label></th>
                      <td colspan="4" runat="server">
                        <asp:TextBox ID="DistCompPost" runat="server" Font-Size="Small" TabIndex="9"></asp:TextBox>
                      </td>
                      <th colspan="2" runat="server"><label>出荷先ご担当者</label></th>
                      <td colspan="1" runat="server">
                        <asp:TextBox ID="DistCharge" runat="server" Font-Size="Small" TabIndex="10"></asp:TextBox>
                      </td>
                      <th colspan="1" runat="server"><label>様</label></th>
                      <th colspan="1" runat="server"><label>TEL</label></th>
                      <td colspan="2" runat="server">
                        <asp:TextBox ID="DistTel" runat="server" Font-Size="Small" TabIndex="11"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <th colspan="1" runat="server"><label>出荷先住所</label></th>
                      <td colspan="1" runat="server">
                        <asp:TextBox ID="DistCompZipCd" runat="server" Font-Size="Small" TabIndex="12" size="10" maxlength="8" onKeyUp="AjaxZip3.zip2addr(this,'','DistCompAddress','DistCompAddress');"></asp:TextBox>
                      </td>
                      <td colspan="7" runat="server">
                        <asp:TextBox ID="DistCompAddress" runat="server" Font-Size="Small" TabIndex="13" Width="550px"></asp:TextBox>
                      </td>
                      <th colspan="1" runat="server"><label>FAX</label></th>
                      <td colspan="2" runat="server">
                        <asp:TextBox ID="DistFax" runat="server" Font-Size="Small" TabIndex="14"></asp:TextBox>
                      </td>
                    </tr> 
                    <tr>
                      <th colspan="1" runat="server"><label>通信欄</label></th>
                      <td colspan="11" runat="server">
                        <asp:TextBox ID="Contact" runat="server" Font-Size="Small" TabIndex="15" size="100" maxlength="500"></asp:TextBox>
                      </td>
                    </tr> 
                </thead>
            </table>
            </div> 

        <!--DEBUG START-->

        <asp:Label ID="lblDubugText1" runat="server" Text="Label"></asp:Label> | 
        <asp:Label ID="lblDubugText2" runat="server" Text="Label"></asp:Label> |
        <asp:Label ID="lblDubugText3" runat="server" Text="Label"></asp:Label> |
        <asp:Label ID="lblDubugText4" runat="server" Text="Label"></asp:Label> |
        <asp:Label ID="lblDubugText5" runat="server" Text="Label"></asp:Label>

        <!--DEBUG END-->

    </form>

</div>

</asp:Content>