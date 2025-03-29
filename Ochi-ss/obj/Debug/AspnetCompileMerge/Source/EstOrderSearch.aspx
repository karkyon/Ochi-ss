<%@ Page Title="" Language="C#" MasterPageFile="~/Ochi-ss.Edit.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="EstOrderSearch.aspx.cs" Inherits="Ochi_ss.EstOrderSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main">
        <form id="form1" runat="server">
            
        <asp:Button id="BtnMainForm" Text="メインメニュー" runat="server" CssClass="btn btn-primary" OnClick="BtnMainForm_Click" />
        <h4><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/4cube.gif" />見積・発注検索</h4>
        
            <!--検索条件-->
            <asp:Label id="lblCondInputDate" runat="server" Text="入力日付"></asp:Label>
            <asp:TextBox id="CondInputDate_s" runat="server" TextMode="Date"></asp:TextBox>～
            <asp:TextBox id="CondInputDate_e" runat="server" TextMode="Date"></asp:TextBox>

            <asp:Label id="lblCondlDestCompName" runat="server" Text="出荷先名"></asp:Label>
            <asp:TextBox id="CondlDestCompName" runat="server" TextMode="Search"></asp:TextBox>
            <asp:Button ID="Search" runat="server" Text="検索" OnClick="Search_Click" />

            <asp:ListView ID="EstOrderList" runat="server" 
                OnItemUpdated="EstOrderList_ItemUpdated"
                OnPagePropertiesChanged="EstOrderList_PagePropertiesChanged">

                
            <EmptyDataTemplate>
                <table id = "EstOrderList" data-mini= "true" class="EstOrderList table table-bordered table-sm table-hover">
                    <thead class="thead-dark">
                      <tr id="row1" runat="server" class="header">              
                        <th id="Th1" runat="server">見積No</th>
                        <th id="Th2" runat="server">見積日付</th>            
                        <th id="Th3" runat="server">お客様注文No</th>
                        <th id="Th4" runat="server">出荷先名</th>
                        <th id="Th5" runat="server">出荷先住所</th>
                        <th id="Th6" runat="server">見積合計額</th>
                        <th id="Th7" runat="server">明細件数</th>
                        <th id="header1" runat="server">編集</th>
                      </tr>
                      <tr runat="server" id="itemPlaceholder" />
                    </thead>
                </table>
                     <div><label><p class="error-text">データが存在しません。</p></label></div>
            </EmptyDataTemplate>

                <LayoutTemplate>
                    
                <table id = "EstOrderList" data-mini= "true" class="EstOrderList table table-bordered table-sm table-hover">
                    <thead class="thead-dark">
                      <tr id="row1" runat="server" class="header">              
                        <th id="Th1" runat="server">見積No</th>
                        <th id="Th2" runat="server">見積日付</th>            
                        <th id="Th3" runat="server">お客様注文No</th>
                        <th id="Th4" runat="server">出荷先名</th>
                        <th id="Th5" runat="server">出荷先住所</th>
                        <th id="Th6" runat="server">見積合計額</th>
                        <th id="Th7" runat="server">明細件数</th>
                        <th id="header1" runat="server">編集</th>
                      </tr>
                      <tr runat="server" id="itemPlaceholder" />
                    </thead>
                </table>

                  <asp:DataPager runat="server" ID="deptsDataPager" PageSize="15">
                    <Fields>
                      <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowLastPageButton="True"
                        FirstPageText="|&lt;&lt; " LastPageText=" &gt;&gt;|"
                        NextPageText=" &gt; " PreviousPageText=" &lt; " />
                    </Fields>
                  </asp:DataPager>

                </LayoutTemplate>

                <ItemTemplate>
                  <tr id="row2" runat="server">            
                    <td runat="server">
                      <asp:Label ID="lblEstNo" runat="Server" Text='<%#Eval("見積No") %>'>見積No</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblEstDate" runat="Server" Text='<%#Eval("見積日付") %>'>見積日付</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblCstOrderNo" runat="Server" Text='<%#Eval("お客様注文No") %>'>お客様注文No</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDestCompName" runat="Server" Text='<%#Eval("出荷先名") %>'>出荷先名</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDestCompAddr" runat="Server" Text='<%#Eval("出荷先住所") %>'>出荷先住所</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblTotalEstAmount" runat="Server" Text='<%#Eval("見積合計額") %>'>見積合計額</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDetailCount" runat="Server" Text='<%#Eval("明細件数") %>'>明細件数</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Button runat="server" 
                          ID="BtnLoadEditEstOrder" 
                          OnCommand="BtnLoadEditEstOrder_Click" 
                          CommandArgument='<%#Eval("見積No") %>'
                          Text="呼出し" 
                          name="BtnLoadEditEstOrder" />
                    </td>
                  </tr>
                </ItemTemplate>

                <EditItemTemplate>
                  <tr style="background-color: #ADD8E6">            
                    <td>
                      <asp:TextBox ID="txtEstNo"  runat="Server" Text='<%#Eval("見積No") %>' MaxLength="50">見積No</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtEstDate"  runat="Server" Text='<%#Eval("見積日付") %>' MaxLength="50">見積日付</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtCstOrderNo"  runat="Server" Text='<%#Eval("お客様注文No") %>' MaxLength="50">お客様注文No</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestCompName"  runat="Server" Text='<%#Eval("出荷先名") %>' MaxLength="50">出荷先名</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestCompAddr"  runat="Server" Text='<%#Eval("出荷先住所") %>' MaxLength="50">出荷先住所</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtTotalEstAmount"  runat="Server" Text='<%#Eval("見積合計額") %>' MaxLength="50">見積合計額</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtlDetailCount"  runat="Server" Text='<%#Eval("明細件数") %>' MaxLength="50">明細件数</asp:TextBox>
                    </td>
                    <td>
                      <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />&nbsp;
                      <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                    </td>
                  </tr>
                </EditItemTemplate>

          </asp:ListView>
        
            <asp:SqlDataSource ID="dsOchiEstOrder" runat="server"></asp:SqlDataSource>
        
        <br />
        <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>

    </form>

    </div>

</asp:Content>