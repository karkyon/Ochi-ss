<%@ Page Title="" Language="C#" MasterPageFile="~/Ochi-ss.Edit.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="EstOrder.aspx.cs" Inherits="Ochi_ss.WebForm1" %>
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
        <asp:Button id="BtnUpdate" Text=" 買い物カゴに保存 " runat="server" CssClass="btn btn-primary" OnClick="BtnUpdate_Click" />
        <asp:Button id="BtnOrder" Text=" この見積りを注文 " runat="server" CssClass="btn btn-primary" OnClick="BtnOrder_Click" />
                          <asp:TextBox ID="Information" runat="server" Enabled="False" ReadOnly="True" TabIndex="0" ForeColor="Red" Width="457px"></asp:TextBox>
        <h4><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/4cube.gif" />見積・発注入力</h4>
        
        <div ID="EditSection" class="fa-edit">

            <!-- 得意先情報 -->
            <table id="EstOrderHeader" runat="server" class="table table-bordered table-sm table-responsive">
                <thead class="thead-dark">
                    <tr class="even">
                      <th colspan="1" runat="server"><label>見積No</label></th>
                      <td colspan="2" runat="server">
                        <asp:TextBox ID="EstOrderNo" runat="server" Font-Size="Small" Enabled="False" ReadOnly="True" TabIndex="1"></asp:TextBox>
                        <asp:HiddenField ID="hidEstOrderNo" runat="server" Value="" />
                      </td>

                      <th colspan="1" runat="server"><label>注文No</label></th>
                      <td colspan="2" runat="server">
                        <asp:TextBox ID="OrderNo" runat="server" Font-Size="Small" TabIndex="2"></asp:TextBox>
                      </td>

                      <th colspan="1" runat="server"><label>入力日付</label></th>
                      <td colspan="2" runat="server">
                        <asp:TextBox ID="InpDate" runat="server" Font-Size="Small" TabIndex="3"></asp:TextBox>
                      </td>

                      <th colspan="1" runat="server"><label>見積日付</label></th>
                      <td colspan="2" runat="server">
                        <asp:TextBox ID="EstDate" runat="server" Font-Size="Small" TabIndex="4"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <th colspan="1" runat="server"><label>お客様名</label></th>
                      <td colspan="1" runat="server">
                          <asp:TextBox ID="CstCode" runat="server" Font-Size="Small" Enabled="False" ReadOnly="True" TabIndex="0"></asp:TextBox>
                        </td>
                      <td colspan="7" runat="server">
                        <asp:TextBox ID="CustomerName" runat="server" Enabled="False" ReadOnly="True" TabIndex="0" Width="400px"></asp:TextBox>
                      </td>
                      <th colspan="1" runat="server"><label>ご担当者</label></th>
                      <td colspan="1" runat="server">
                        <asp:TextBox ID="CustomreCharge" runat="server" Font-Size="Small" TabIndex="5"></asp:TextBox>
                      </td>
                      <th colspan="1" runat="server"><label>様</label></th>
                    </tr> 
                </thead>
            </table>
            <!-- 発送方法 -->
            <asp:Label ID="lblShippingMethod" runat="server" Text="発送方法 "></asp:Label>
            <asp:DropDownList ID="ShippingMethod" runat="server" Font-Size="Small" OnSelectedIndexChanged="ShippingMethod_SelectedIndexChanged" AutoPostBack="True" TabIndex="6">
                <asp:ListItem Value="1">発送</asp:ListItem>
                <asp:ListItem Value="2">引取り</asp:ListItem>
                <asp:ListItem Value="3">配達</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblShippingMethodMsg" runat="server" Font-Size="Small" Text="リストよりお選びください "></asp:Label>
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

            <h6>見積明細編集《新規》</h6>
            <asp:HiddenField ID="RowNo" runat="server" Value="" />

            <table id="EstOrderInput" runat="server" class="table table-bordered table-sm table-responsive" style="border:1px white solid; padding:1px;">
                <thead class="thead-dark">
                    <tr class="even">
                        <th colspan="1" rowspan="2" runat="server" style="border:1px black solid; padding:1px;"><label>①素材</label></th>
                        <th colspan="1" rowspan="2" runat="server" style="border:1px black solid; padding:1px;"><label>②仕上り</label></th>
                        <th colspan="3" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>③加工仕様</label></th>
                        <th colspan="3" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>④寸法</label></th>
                        <th colspan="4" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>⑤公差</label></th>
                        <th colspan="3" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>⑥面取り</label></th>
                        <th colspan="1" rowspan="2" runat="server" style="border:1px black solid; padding:1px;"><label>⑦数量</label></th>
                        <th colspan="1" rowspan="2" runat="server" style="border:1px black solid; padding:1px;"><label>⑧お客様注文番号<p>⑨送り先様注文番号</p></label></th>
                        <th colspan="1" rowspan="2" runat="server" style="border:1px black solid; padding:1px;"><label>見積追加</label></th>
                    </tr>
                    <tr>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>厚み</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>巾</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>長さ</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>厚み</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>巾</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>長さ</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label></label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>厚み</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>巾</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>長さ</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>
                          <asp:DropDownList ID="MentoriShiji" runat="server" tabindex="33" name="MentoriShiji" Height="20px" AutoPostBack="True" OnSelectedIndexChanged="MentoriShiji_SelectedIndexChanged" Font-Size="Small">
                              <asp:ListItem Value="1">面取図参照</asp:ListItem>
                              <asp:ListItem Value="2">面取不可</asp:ListItem>
                              <asp:ListItem Selected="True" Value="9">---</asp:ListItem>
                            </asp:DropDownList></label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>4角</label></th>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>8辺</label></th>
                    </tr>
                        
                    <tr>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:DropDownList ID="Material" runat="server" tabindex="16" name="Material" OnSelectedIndexChanged="Material_SelectedIndexChanged" Height="40px" Font-Size="Large"></asp:DropDownList>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:DropDownList ID="Kakou" runat="server" tabindex="17" name="Kakou" AutoPostBack="True" OnSelectedIndexChanged="Kakou_SelectedIndexChanged" Height="40px" Font-Size="Large"></asp:DropDownList>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:DropDownList ID="Kakou_T" runat="server" tabindex="18" name="Kakou_T" AutoPostBack="True" OnSelectedIndexChanged="Kakou_T_SelectedIndexChanged" Height="40px" Font-Size="Large"></asp:DropDownList>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:DropDownList ID="Kakou_B" runat="server" tabindex="19" name="Kakou_B" AutoPostBack="True" OnSelectedIndexChanged="Kakou_B_SelectedIndexChanged" Height="40px" Font-Size="Large"></asp:DropDownList>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:DropDownList ID="Kakou_A" runat="server" tabindex="20" name="Kakou_A" AutoPostBack="True" OnSelectedIndexChanged="Kakou_A_SelectedIndexChanged" Height="40px" Font-Size="Large"></asp:DropDownList>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Size_T" runat="server" tabindex="21" name="Size_T" Width="56px" Height="40px" Font-Size="Small"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Size_B" runat="server" tabindex="22" name="Size_B" Width="76px" Height="40px" Font-Size="Small"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Size_A" runat="server" tabindex="23" name="Size_A" Width="86px" Height="40px" Font-Size="Small"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:Button ID="BtnStandKousa" runat="server" Text="標 準" name="btn_AddEstimateExp" Height="40px" tabindex="24" OnClick="BtnStandKousa_Click" Font-Size="Small" />
                        </td>
                        <td colspan="1" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Kousa_T_U" runat="server" tabindex="25" name="Kousa_T_U" Width="55px" Height="22px" Font-Size="Small"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Kousa_B_U" runat="server" tabindex="27" name="Kousa_B_U" Width="55px" Height="22px" Font-Size="Small"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Kousa_A_U" runat="server" tabindex="29" name="Kousa_A_U" Width="55px" Height="22px" Font-Size="Small"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:Button ID="BtnStandMentori" runat="server" Text="標 準" name="BtnStandMentori" Height="40px" tabindex="31" OnClick="BtnStandMentori_Click" Font-Size="Small" />
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Mentori_4" runat="server" tabindex="33" name="Mentori_4" Width="40px" Height="40px" Font-Size="Small"></asp:TextBox> C
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Mentori_8" runat="server" tabindex="34" name="Mentori_8" Width="40px" Height="40px" Font-Size="Small"></asp:TextBox> C
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Suryou" runat="server" tabindex="35" name="Suryou" Width="50px" Height="40px" Font-Size="Medium"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="CustomerNo" runat="server" tabindex="36" name="CustomerNo" Width="120px" Height="22px" Font-Size="Medium"></asp:TextBox><p>
                          <asp:TextBox ID="EndUserNo" runat="server" tabindex="37" name="CustomerNo" Width="120px" Height="22px" Font-Size="Medium"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:Button ID="BtnEstimate" runat="server" Text="見 積" name="BtnEstimate" tabindex="38" Height="40px" OnClick="BtnEstimate_Click" Font-Size="Small" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Kousa_T_L" runat="server" tabindex="26" name="Kousa_T_L" Width="55px" Height="22px" Font-Size="Small"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Kousa_B_L" runat="server" tabindex="28" name="Kousa_B_L" Width="55px" Height="22px" Font-Size="Small"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Kousa_A_L" runat="server" tabindex="30" name="Kousa_A_L" Width="55px" Height="22px" Font-Size="Small"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Msg" runat="server" Enabled="False" ReadOnly="True" TabIndex="0" ForeColor="Red" Width="457px"></asp:TextBox>
                          <asp:HiddenField ID="EstStat" runat="server" Value="" />
                        </td>
                        <th colspan="1" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>最短納期</label></th>
                        <td colspan="2" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Nouki_S" runat="server" tabindex="0" name="Nouki_S" Width="88px" Height="22px" Font-Size="Medium" BackColor="#CCFFFF" ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                        </td>
                        <th colspan="2" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>送料込みプレート単価</label></th>
                        <td colspan="2" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Price_U" runat="server" tabindex="0" name="UnitPrice" Width="90px" Height="22px" Font-Size="Medium" BackColor="#CCFFFF" ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                        </td>
                        <th colspan="2" rowspan="1" runat="server" style="border:1px black solid; padding:1px;"><label>送料込みプレート金額</label></th>
                        <td colspan="1" rowspan="1" style="border:1px black solid; padding:1px;">
                          <asp:TextBox ID="Price_S" runat="server" tabindex="0" name="SumPrice" Width="120px" Height="22px" Font-Size="Medium" BackColor="#CCFFFF" ForeColor="Blue" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td colspan="1" rowspan="2" style="border:1px black solid; padding:1px;">
                          <asp:Button ID="BtnAddRecord" runat="server" Text="追 加" name="BtnAddRecord" tabindex="39" Height="20px" OnClick="BtnAddRecord_Click" Font-Size="Small" />
                        </td>
                    </tr>
                </thead>
            </table>

        </div>

        <div ID="RecordSection" class="table-area" tabindex="50">
        <script src="Scripts/jquery-1.10.2.min.js"></script>
        <script type="text/javascript">
            $(function () {
                $('#chkAll').on('change', function () {
                    var rowCnt = $(".ChkSelect").length;
                    for (var i = 0; i < rowCnt; i++) {
                        $(".ChkSelect")[i].childNodes[0].checked = this.checked;
                    }
                });
            });
        </script>
        <h6>登録済み明細</h6>
        <asp:ListView ID="ListView1" runat="server" 
            GroupPlaceholderID="groupPlaceHolder1" 
            ItemPlaceholderID="itemPlaceHolder1" 
            OnDataBound="ListView1_DataBound" 
            OnItemDataBound="ListView1_ItemDataBound"
            OnItemCommand="ListView1_OnItemCommand" DataKeyNames="No">

            <EmptyDataTemplate>
                <div class="header-area">
                    <table ID="EstOrderRegList" class="EstOrderRegList table table-bordered table-sm table-responsive">
                        <thead class="thead-dark">
                          <tr>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>No</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>Chk</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>材料</label></th>
                            <th colspan="2" rowspan="1" runat="server" width=""><label>厚み</label></th>
                            <th colspan="2" rowspan="1" runat="server" width=""><label>幅</label></th>
                            <th colspan="2" rowspan="1" runat="server" width=""><label>長さ</label></th>
                            <th colspan="3" rowspan="1" runat="server" width=""><label>面取り</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>数量</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>納期</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>ご注文番号</label></th>
                            <th colspan="2" rowspan="1" runat="server" width=""><label>プレート金額</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>ご希望価格</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label></label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label></label></th>
                          </tr>
                          <tr>
                            <th colspan="1" rowspan="1" runat="server" width="">加工</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">公差</label></th>
                            <th colspan="1" rowspan="1" runat="server" width="">加工</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">公差</label></th>
                            <th colspan="1" rowspan="1" runat="server" width="">加工</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">公差</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">4角</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">8辺</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">詳細</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">単価</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">金額</label></th>
                          </tr>
                          <tr>
                            <th colspan="1" rowspan="1" runat="server" width="">寸法</label></th>
                            <th colspan="1" rowspan="1" runat="server" width="">寸法</label></th>
                            <th colspan="1" rowspan="1" runat="server" width="">寸法</label></th>
                          </tr>
                        </thead>
                        <tr class="footer">
                          <td colspan ="18"> </td>
                          <td>合計金額</td>
                          <td colspan ="2" style="text-align: right;"><asp:Label ID="lbl合計金額" runat="server" Font-Size="Small" /></td>
                        </tr>
                     </table>
                     <div><label><p class="error-text">データが存在しません。</p></label></div>
                            
                </div>
            </EmptyDataTemplate>

            <LayoutTemplate>
                <div class="header-area">
                    <table ID="EstOrderRegList" class="EstOrderRegList table table-bordered table-sm table-responsive">
                        <thead class="thead-dark">
                          <tr>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>No</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>Chk</label><p>
                                <input id="chkAll" class="checkbox" type="checkbox" /></p>
                            </th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>材料</label></th>
                            <th colspan="2" rowspan="1" runat="server" width=""><label>厚み</label></th>
                            <th colspan="2" rowspan="1" runat="server" width=""><label>幅</label></th>
                            <th colspan="2" rowspan="1" runat="server" width=""><label>長さ</label></th>
                            <th colspan="3" rowspan="1" runat="server" width=""><label>面取り</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>数量</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>納期</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>ご注文番号</label></th>
                            <th colspan="2" rowspan="1" runat="server" width=""><label>プレート金額</label></th>
                            <th colspan="1" rowspan="3" runat="server" width=""><label>ご希望価格</label></th>
                            <th colspan="1" rowspan="2" runat="server" width=""><label></label></th>
                            <th colspan="1" rowspan="2" runat="server" width=""><label></label></th>
                          </tr>
                          <tr>
                            <th colspan="1" rowspan="1" runat="server" width="">加工</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">公差</label></th>
                            <th colspan="1" rowspan="1" runat="server" width="">加工</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">公差</label></th>
                            <th colspan="1" rowspan="1" runat="server" width="">加工</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">公差</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">4角</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">8辺</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">詳細</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">単価</label></th>
                            <th colspan="1" rowspan="2" runat="server" width="">金額</label></th>
                          </tr>
                          <tr>
                            <th colspan="1" rowspan="1" runat="server" width="">寸法</label></th>
                            <th colspan="1" rowspan="1" runat="server" width="">寸法</label></th>
                            <th colspan="1" rowspan="1" runat="server" width="">寸法</label></th>
                            <th colspan="2" rowspan="1" runat="server" width="">お問合せNo</label></th>
                          </tr>

                        </thead>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr class="footer">
                          <td colspan ="18"> </td>
                          <td>合計金額</td>
                          <td colspan ="2" style="text-align: right;"><asp:Label ID="lbl合計金額" runat="server" Font-Size="Small" /></td>
                        </tr>
                    </table>
                </div>
            </LayoutTemplate>

            <GroupTemplate>
                <div class="data-area">
                    <tr>
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                    </tr>
                </div>
            </GroupTemplate>

            <ItemTemplate>
                <tbody>
                  <tr runat="server">
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblSeqNo" runat="server" Font-Size="Medium" Text='<%# (Eval("No")) %>'>No</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server">
	                    <asp:CheckBox ID="ChkSelect" 
                                    runat="server" 
                          CommandName="EditEstimate" 
                          CommandArgument='<%# (Eval("No")) %>'
                                    Text="Label"
                                    Font-Size="Medium"
                                    TextAlign="Left"
                                    Checked="False"
                                    OnClientClick="ChkSelect_Check" />
                    </td>
                    <td colspan="1" rowspan="2" runat="server">
	                  <asp:Label ID="lblMaterialName" runat="server" Font-Size="Medium" Text='<%# (Eval("材料名")) %>'>材料</asp:Label>
                      <asp:HiddenField ID="hidMaterialCd" runat="server" Value='<%# (Eval("材料コード")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblKakoushiji_T" runat="server" Font-Size="Medium" Text='<%# (Eval("加工指示_T")) %>'>加工(T)</asp:Label>
                      <asp:HiddenField ID="hidKakoushijiCd_T" runat="server" Value='<%# (Eval("加工指示コードT")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblKakoukousa_UT" runat="server" Font-Size="Small" Text='<%# (Eval("加工公差_UT")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="Label5" runat="server" Text='<%# (Eval("加工指示_A")) %>'>加工(A)</asp:Label>
                      <asp:HiddenField ID="hidKakoushijiCd_A" runat="server" Value='<%# (Eval("加工指示コードA")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblKakoukousa_UA" runat="server" Font-Size="Small" Text='<%# (Eval("加工公差_UA")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="Label7" runat="server" Font-Size="Medium" Text='<%# (Eval("加工指示_B")) %>'>加工(B)</asp:Label>
                      <asp:HiddenField ID="hidKakoushijiCd_B" runat="server" Value='<%# (Eval("加工指示コードB")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblKakoukousa_UB" runat="server" Font-Size="Small" Text='<%# (Eval("加工公差_UB")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server">
	                  <asp:Label ID="lblMentori_4C" runat="server" Font-Size="Small" Text='<%# (Eval("面取り量_4C")) %>'>4角</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server">
	                  <asp:Label ID="lblMentori_8C" runat="server" Font-Size="Small" Text='<%# (Eval("面取り量_8C")) %>'>8辺</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server">
                      <asp:LinkButton runat="server" 
                          ID="BtnMentoriDetail" 
                          CommandName="MentoriDetail" 
                          CommandArgument='<%# (Eval("No")) %>'
                          tabindex="21" 
                          Font-Size="Medium" 
                          Text="詳細" />&nbsp;
                    </td>
                    <td colspan="1" rowspan="2" runat="server">
	                  <asp:Label ID="lblSuryou" runat="server" Font-Size="Medium" Text='<%# (Eval("製品数量")) %>'>数量</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblRequestNouki" runat="server" Font-Size="Medium" Text='<%# (Eval("希望納期")) %>'>希望納期</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server">
	                  <asp:Label ID="lblOrderNo" runat="server" Font-Size="Medium" Text='<%# (Eval("客先注番")) %>'>注文番号</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblUnitPrice" runat="server" Font-Size="Medium" Text='<%# (Eval("見積単価", "{0:c}")) %>'>単価</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblPrice" runat="server" Font-Size="Medium" Text='<%# (Eval("見積金額", "{0:c}")) %>'>金額</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblRequestPrice" runat="server" Font-Size="Medium" Text='<%# (Eval("希望価格", "{0:c}")) %>'>金額</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
                      <asp:LinkButton runat="server" 
                          ID="BtnEditList" 
                          CommandName="EditEstimate" 
                          CommandArgument='<%# (Eval("No")) %>'
                          tabindex="21" 
                          Font-Size="Medium" 
                          Text="編集" />&nbsp;
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
                      <asp:LinkButton runat="server" 
                          ID="BtnDeleteList" 
                          CommandName="DeleteEstimate" 
                          CommandArgument='<%# (Eval("No")) %>'
                          tabindex="22" 
                          Font-Size="Medium" 
                          Text="削除" />&nbsp;
                    </td>
                  </tr>
                  <tr runat="server">
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblOrderStat" runat="server" Font-Size="Medium" Text='<%# (Eval("進捗区分")) %>'>No</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
                      <asp:Label ID="lblSize_T" runat="server" Font-Size="Small" Text='<%# (Eval("仕上りサイズT")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblKakoukousa_LT" runat="server" Font-Size="Small" Text='<%# (Eval("加工公差_LT")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblSize_A" runat="server" Font-Size="Small" Text='<%# (Eval("仕上りサイズA")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblKakoukousa_LA" runat="server" Font-Size="Small" Text='<%# (Eval("加工公差_LA")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblSize_B" runat="server" Font-Size="Small" Text='<%# (Eval("仕上りサイズB")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblKakoukousa_LB" runat="server" Font-Size="Small" Text='<%# (Eval("加工公差_LB")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server">
	                  <asp:Label ID="lblShortestNouki" runat="server" Font-Size="Medium" Text='<%# (Eval("最短納期")) %>'>最短納期</asp:Label>
                    </td>
                    <td colspan="3" rowspan="1" runat="server">
                        <label></label>
                    </td>
                    <td colspan="2" rowspan="1" runat="server">
	                  <asp:Label ID="lblContactNo" runat="server" Font-Size="Small" Text='<%# (Eval("問合せNo")) %>'>お問合せNo</asp:Label>
                    </td>
                  </tr>
                </tbody>
            </ItemTemplate>

        </asp:ListView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
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