<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.List.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ShukkasakiList.aspx.cs" Inherits="Ochi_ss.ShukkasakiList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="main table-responsive" style="margin: 0 auto; text-align: center;">
  <form id="form1" runat="server" style="display: inline-block; text-align: left;">
            
    <div class="middle">
        <!-- メニューボタン -->
        <table class="TopButtonArea" runat="server">
          <tbody>
            <!-- 1行目：タイトル部分 -->
            <tr>
              <td colspan="6">
                出荷先一覧
              </td>
            </tr>
          </tbody>
        </table>

        <!-- Debug用 -->
        <asp:Label ID="lblDubugText1" runat="server" Text=""></asp:Label>

        <!-- SessionID情報 -->
        <div id="SessionInfo">
            <h6><asp:Label ID="SessionID" runat="server" Text=""></asp:Label></h6>
        </div>
    </div>
      
    <!-- 検索条件 -->
    <table class="SearchConditionArea" runat="server">
      <tbody>
        <tr>
          <td><asp:Label id="lblCondlDestCompName" runat="server" Text="出荷先名"></asp:Label></td>
          <td><asp:TextBox id="CondlDestCompName" runat="server" TextMode="Search"></asp:TextBox></td>
          <td><asp:Label id="lblCondlDestCompArea" runat="server" Text="住所"></asp:Label></td>
          <td><asp:TextBox id="CondlDestCompArea" runat="server" TextMode="Search"></asp:TextBox></td>
          <td width="120"></td>
          <td><asp:Button ID="BtnSearch" runat="server" Text="検索" OnClick="BtnSearch_Click" CssClass="button-style" /></td>
          <td><asp:Button ID="BtnClearCondition" runat="server" Text="クリア" OnClick="BtnClearCondition_Click" CssClass="button-style" /></td>
        </tr>
      </tbody>
    </table>

    <asp:ListView ID="DestCompList" runat="server" 
        OnItemUpdated="DestCompList_ItemUpdated"
        OnPagePropertiesChanged="DestCompNameList_PagePropertiesChanged"
        OnItemCommand="DestCompList_OnItemCommand">
                
        <EmptyDataTemplate>
            <div class="header-area">
              <table id="TblEmpty" class="multi">
                <thead>
                  <tr>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-dest-cd">出荷先コード</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-dest">出荷先名</td>            
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-addr">住所</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-dep">出荷先部署名</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-pic">出荷先担当者名</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-tel">出荷先電話番号</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-fax">出荷先FAX番号</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-edit-btn">編集</td>
                  </tr>
                  <tr runat="server" id="itemPlaceholder" />
                </thead>
              </table>
            <div><label><p class="error-text">データが存在しません。</p></label></div>
        </EmptyDataTemplate>
        
        <LayoutTemplate>
            <div class="header-area">
              <table id="EstOrderList" class="multi">
                <thead>
                  <tr>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-dest-cd">出荷先コード</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-dest">出荷先名</td>            
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-addr">住所</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-dep">出荷先部署名</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-pic">出荷先担当者名</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-tel">出荷先電話番号</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-fax">出荷先FAX番号</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-edit-btn">編集</td>
                  </tr>
                  <tr runat="server" id="itemPlaceholder" />
                </thead>
            </table>

            <asp:DataPager runat="server" ID="deptsDataPager" PageSize="10">
              <Fields>
                <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowLastPageButton="True"
                FirstPageText="|&lt;&lt; " LastPageText=" &gt;&gt;|"
                NextPageText=" &gt; " PreviousPageText=" &lt; " />
              </Fields>
            </asp:DataPager>

        </LayoutTemplate>
        
        <ItemTemplate>
          <tr runat="server">
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestCd" name="lblDestCd" runat="Server" Text='<%#Eval("出荷先コード") %>'>出荷先コード</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestName" runat="Server" Text='<%#Eval("出荷先名") %>'>出荷先名</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestCompAddr" runat="Server" Text='<%#Eval("住所") %>'>住所</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestSectionName" runat="Server" Text='<%#Eval("出荷先部署名") %>'>出荷先部署名</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestChargeName" runat="Server" Text='<%#Eval("出荷先担当者名") %>'>出荷先担当者名</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestTel" runat="Server" Text='<%#Eval("出荷先電話番号") %>'>出荷先電話番号</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestFax" runat="Server" Text='<%#Eval("出荷先FAX番号") %>'>出荷先FAX番号</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
              <asp:LinkButton runat="server" 
                 ID ="btnSelect" 
                 CommandName ="SelectShukkasaki" 
                 CommandArgument ='<%#Eval("出荷先コード") %>'
                 Text ="選択" 
                 CssClass="button-style" />
            </td>
          </tr>
        </ItemTemplate>

        <EditItemTemplate>
          <tr runat="server">
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtDestCd"  runat="Server" Text='<%#Eval("出荷先コード") %>' MaxLength="50">出荷先コード</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtDestName"  runat="Server" Text='<%#Eval("出荷先名") %>' MaxLength="50">出荷先名</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtDestSectionName"  runat="Server" Text='<%#Eval("出荷先部署名") %>' MaxLength="50">出荷先部署名</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtDestChargeName"  runat="Server" Text='<%#Eval("出荷先担当者名") %>' MaxLength="50">出荷先担当者名</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtDestCompZipaddr"  runat="Server" Text='<%#Eval("出荷先郵便番号") %>' MaxLength="50">出荷先郵便番号</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtDestCompAddr1"  runat="Server" Text='<%#Eval("住所1") %>' MaxLength="50">住所1</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtDestCompAddr2"  runat="Server" Text='<%#Eval("住所2") %>' MaxLength="50">住所2</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtDestTel"  runat="Server" Text='<%#Eval("出荷先電話番号") %>' MaxLength="50">出荷先電話番号</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:TextBox ID="txtlDestFax"  runat="Server" Text='<%#Eval("出荷先FAX番号") %>' MaxLength="50">出荷先FAX番号</asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
              <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />
              <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
          </tr>
        </EditItemTemplate>

    </asp:ListView>
        
    <asp:SqlDataSource ID="dsShukkasaki" runat="server"></asp:SqlDataSource>
        
    <br />
    <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>

  </form>
</div>

</asp:Content>
