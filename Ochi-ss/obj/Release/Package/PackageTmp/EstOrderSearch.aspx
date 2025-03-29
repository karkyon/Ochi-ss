<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.Search.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="EstOrderSearch.aspx.cs" Inherits="Ochi_ss.EstOrderSearch" %>
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
                お見積り履歴検索
              </td>
            </tr>
            <!-- 2行目：ボタン部分 -->
            <tr>
                <td>
                    <asp:Button 
                        id="BtnMainForm" 
                        Text="メインメニュー" 
                        runat="server" 
                        CssClass="btn btn-primary btn-sm" 
                        OnClick="BtnMainForm_Click" />
                </td>
            </tr>
          </tbody>
        </table>
        
        <!-- Debug用 -->
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>

        <!-- SessionID情報 -->
        <div id="SessionInfo">
            <h6><asp:Label ID="Label1" runat="server" Text=""></asp:Label></h6>
        </div>
    </div>
      
    <!-- 検索条件 -->
    <table class="SearchConditionArea" runat="server">
      <tbody>
        <tr>
          <td><asp:Label id="lblCondInputDate" runat="server" Text="入力日付"></asp:Label></td>
          <td><asp:TextBox id="CondInputDate_s" runat="server" TextMode="Date"></asp:TextBox></td>
          <td>～</td>
          <td><asp:TextBox id="CondInputDate_e" runat="server" TextMode="Date"></asp:TextBox></td>
          <td><asp:Label id="lblCondlDestCompName" runat="server" Text="出荷先名"></asp:Label></td>
          <td><asp:TextBox id="CondlDestCompName" runat="server" TextMode="Search"></asp:TextBox></td>
          <td width="120"></td>
          <td><asp:Button ID="BtnSearch" runat="server" Text="検索" OnClick="BtnSearch_Click" CssClass="button-style" /></td>
          <td><asp:Button ID="BtnClearCondition" runat="server" Text="クリア" OnClick="BtnClearCondition_Click" CssClass="button-style" /></td>
        </tr>
      </tbody>
    </table>

    <asp:ListView ID="EstOrderList" runat="server" 
        OnItemUpdated="EstOrderList_ItemUpdated"
        OnPagePropertiesChanged="EstOrderList_PagePropertiesChanged">
  
        <EmptyDataTemplate>
            <div class="header-area">
                <table id="TblEmpty" class="multi">
                <thead>
                    <tr>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-no">見積No</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-date">見積日付</td>            
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-order-no">お客様注文No</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-dest">出荷先名</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-addr">出荷先住所</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-sum">見積合計額</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-count">明細件数</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-status">状況</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-edit-btn">編集</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-edit-btn">コピー</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-edit-btn">見積書</td>
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
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-no">見積No</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-date">見積日付</td>            
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-order-no">お客様注文No</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-dest">出荷先名</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-deliver-addr">出荷先住所</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-sum">見積合計額</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-count">明細件数</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-estimate-status">状況</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-edit-btn">編集</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-edit-btn">コピー</td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tlbl col-edit-btn">見積書</td>
                  </tr>
                  <tr runat="server" id="itemPlaceholder" />
                </thead>
              </table>

              <asp:DataPager runat="server" ID="deptsDataPager" PageSize="10">
                <Fields>
                <asp:NextPreviousPagerField 
                    ShowFirstPageButton="True" 
                    ShowLastPageButton="True"
                    FirstPageText="|&lt;&lt; "
                    LastPageText=" &gt;&gt;|"
                    NextPageText=" &gt; " 
                    PreviousPageText=" &lt; " />
                </Fields>
              </asp:DataPager>
          </div>
        </LayoutTemplate>

        <ItemTemplate>
          <tr runat="server">
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblEstNo" runat="Server" Text='<%#Eval("WO見積No") %>'>見積No</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblEstDate" runat="Server" Text='<%#Eval("見積日付") %>'>見積日付</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblCstOrderNo" runat="Server" Text='<%#Eval("お客様注文No") %>'>お客様注文No</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestCompName" runat="Server" Text='<%#Eval("出荷先名") %>'>出荷先名</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDestCompAddr" runat="Server" Text='<%#Eval("出荷先住所") %>'>出荷先住所</asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblTotalEstAmount" runat="Server"
                    Style="text-align: right; display: block;"
                    Text='<%# String.Format("{0:C}", Eval("見積合計額")) %>'>見積合計額

                </asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblDetailCount" runat="Server" 
                    Style="text-align: right; display: block;"
                    Text='<%#Eval("明細件数") %>'>明細件数
                </asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Label ID="lblStatus" runat="server" 
                    Text='<%# FormatStatus(Eval("状況"), Eval("回答状況")) %>'>
                </asp:Label>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Button runat="server" 
                 ID ="BtnLoadEditEstOrder" 
                 OnCommand ="BtnLoadEditEstOrder_Click" 
                 CommandArgument ='<%#Eval("WO見積No") %>'
                 Text ="呼出し" 
                 CommandName ="BtnLoadEditEstOrder"
                 CssClass="button-style" />
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:Button runat="server" 
                 ID ="Button1" 
                 OnCommand ="BtnLoadCopyEstOrder_Click" 
                 CommandArgument ='<%#Eval("WO見積No") %>'
                 Text ="内容コピー" 
                 CommandName ="BtnLoadCopyEstOrder"
                 CssClass="button-style" />
            </td>
            <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
                <asp:PlaceHolder ID="phPDFDownload" runat="server">
                    <asp:HyperLink ID="lnkDownloadPDF" runat="server" 
                        NavigateUrl='<%# string.IsNullOrEmpty(Eval("フルパス") as string) ? "" : Eval("フルパス", "~/ReportDownload.aspx?file={0}") %>' 
                        Text="ダウンロード" 
                        Target="_blank"
                        CssClass="button-style"
                        Visible='<%# !string.IsNullOrEmpty(Eval("フルパス") as string) %>' />
        
                    <asp:Button ID="btnGenerateEstimation" runat="server" 
                        Text="見積書発行" 
                        CssClass="button-style"
                        OnClick="btnGenerateEstimation_Click"
                        Visible='<%# string.IsNullOrEmpty(Eval("フルパス") as string) %>' />
                </asp:PlaceHolder>
            </td>
          </tr>
        </ItemTemplate>

    </asp:ListView>
        
    <asp:SqlDataSource ID="dsOchiEstOrder" runat="server"></asp:SqlDataSource>
    <br />
    <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>

  </form>
</div>
</asp:Content>