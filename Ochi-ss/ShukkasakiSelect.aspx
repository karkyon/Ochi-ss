<%@ Page Title="" Language="C#" MasterPageFile="~/Ochi-ss.Master.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ShukkasakiSelect.aspx.cs" Inherits="Ochi_ss.ShukkasakiSelect" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main">
        <form id="form1" runat="server">
            
        <h4><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/4cube.gif" />出荷先一覧</h4>
        
            <!--検索条件-->
            <asp:Label id="lblCondlDestCompName" runat="server" Text="出荷先名"></asp:Label>
            <asp:TextBox id="CondlDestCompName" runat="server" TextMode="Search"></asp:TextBox>
            <asp:Label id="lblCondlDestCompArea" runat="server" Text="住所"></asp:Label>
            <asp:TextBox id="CondlDestCompArea" runat="server" TextMode="Search"></asp:TextBox>
            <asp:Button ID="Search" runat="server" Text="検索" OnClick="Search_Click" />

            <asp:Label ID="lblDubugText1" runat="server" Text="Label"></asp:Label>

            <asp:ListView ID="DestCompList" runat="server" 
                OnItemUpdated="DestCompList_ItemUpdated"
                OnPagePropertiesChanged="DestCompNameList_PagePropertiesChanged"
                OnItemCommand="DestCompList_OnItemCommand">
                
            <EmptyDataTemplate>
                <table data-mini= "true" class="table table-bordered table-sm table-hover">
                    <thead class="thead-dark">
                      <tr id="row1" runat="server" class="header">              
                        <th id="Th1" runat="server">出荷先コード</th>
                        <th id="Th2" runat="server">出荷先名</th>            
                        <th id="Th3" runat="server">住所</th>
                        <th id="Th4" runat="server">出荷先部署名</th>
                        <th id="Th5" runat="server">出荷先担当者名</th>
                        <th id="Th6" runat="server">出荷先電話番号</th>
                        <th id="Th7" runat="server">出荷先FAX番号</th>
                        <th id="header1" runat="server">選択</th>
                      </tr>
                      <tr runat="server" id="itemPlaceholder" />
                    </thead>
                </table>
                     <div><label><p class="error-text">データが存在しません。</p></label></div>
            </EmptyDataTemplate>
              <LayoutTemplate>
                    
                <table data-mini= "true" class="table table-bordered table-sm table-hover">
                    <thead class="thead-dark">
                      <tr id="row1" runat="server" class="header">              
                        <th id="Th1" runat="server">出荷先コード</th>
                        <th id="Th2" runat="server">出荷先名</th>            
                        <th id="Th3" runat="server">住所</th>
                        <th id="Th4" runat="server">出荷先部署名</th>
                        <th id="Th5" runat="server">出荷先担当者名</th>
                        <th id="Th6" runat="server">出荷先電話番号</th>
                        <th id="Th7" runat="server">出荷先FAX番号</th>
                        <th id="header1" runat="server">選択</th>
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
                  <tr id="row2" runat="server">            
                    <td runat="server">
                      <asp:Label ID="lblDestCd" name="lblDestCd" runat="Server" Text='<%#Eval("出荷先コード") %>'>出荷先コード</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDestName" runat="Server" Text='<%#Eval("出荷先名") %>'>出荷先名</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDestCompAddr" runat="Server" Text='<%#Eval("住所") %>'>住所</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDestSectionName" runat="Server" Text='<%#Eval("出荷先部署名") %>'>出荷先部署名</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDestChargeName" runat="Server" Text='<%#Eval("出荷先担当者名") %>'>出荷先担当者名</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDestTel" runat="Server" Text='<%#Eval("出荷先電話番号") %>'>出荷先電話番号</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:Label ID="lblDestFax" runat="Server" Text='<%#Eval("出荷先FAX番号") %>'>出荷先FAX番号</asp:Label>
                    </td>
                    <td runat="server">
                      <asp:LinkButton runat="server" 
                          ID="btnSelect" 
                          CommandName="SelectShukkasaki" 
                          CommandArgument='<%#Eval("出荷先コード") %>'
                          Text="選択" />&nbsp;
                    </td>
                  </tr>
                </ItemTemplate>

                <EditItemTemplate>
                  <tr style="background-color: #ADD8E6">            
                    <td>
                      <asp:TextBox ID="txtDestCd"  runat="Server" Text='<%#Eval("出荷先コード") %>' MaxLength="50">出荷先コード</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestName"  runat="Server" Text='<%#Eval("出荷先名") %>' MaxLength="50">出荷先名</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestSectionName"  runat="Server" Text='<%#Eval("出荷先部署名") %>' MaxLength="50">出荷先部署名</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestChargeName"  runat="Server" Text='<%#Eval("出荷先担当者名") %>' MaxLength="50">出荷先担当者名</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestCompZipaddr"  runat="Server" Text='<%#Eval("出荷先郵便番号") %>' MaxLength="50">出荷先郵便番号</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestCompAddr1"  runat="Server" Text='<%#Eval("住所1") %>' MaxLength="50">住所1</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestCompAddr2"  runat="Server" Text='<%#Eval("住所2") %>' MaxLength="50">住所2</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtDestTel"  runat="Server" Text='<%#Eval("出荷先電話番号") %>' MaxLength="50">出荷先電話番号</asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="txtlDestFax"  runat="Server" Text='<%#Eval("出荷先FAX番号") %>' MaxLength="50">出荷先FAX番号</asp:TextBox>
                    </td>
                    <td>
                      <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />&nbsp;
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
