<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.Edit.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="EstOrder.aspx.cs" Inherits="Ochi_ss.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Async = "true" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="main table-responsive" style="margin: 0 auto; text-align: center;">
  <form id="form1" runat="server" style="display: inline-block; text-align: left;">
        
<!-- メニューボタン -->
<table class="TopButtonArea" runat="server">
  <tbody>
    <!-- 1行目：タイトル部分 -->
    <tr>
        <td colspan="6">
            お見積もり入力
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
        <td>
            <asp:Button 
                id="BtnSearch" 
                Text=" 検 索 " 
                runat="server" 
                CssClass="btn btn-primary btn-sm" 
                OnClick="BtnSearch_Click" />
        </td>
        <td>
            <asp:Button 
                id="BtnNew" 
                Text=" 新 規 " 
                runat="server" 
                CssClass="btn btn-primary btn-sm" 
                OnClick="BtnNew_Click" />
        </td>
        <td>
            <asp:Button 
                id="BtnDelete" 
                Text=" 削 除 " 
                runat="server" 
                OnClientClick="return confirm('この見積データを削除してよろしいですか？');"
                CssClass="btn btn-primary btn-sm" 
                OnClick="BtnDelete_Click" />
        </td>
        <td>
            <asp:Button 
                id="BtnUpdate" 
                Text=" この見積りを保存 " 
                runat="server" 
                CssClass="btn btn-primary btn-sm" 
                OnClick="BtnUpdate_Click" />
        </td>
        <td>
            <asp:CheckBox ID="chkGenerateCustomerCopy" runat="server" Text="入力内容の控えを発行する" />
        </td>
        <td>
            <asp:Button 
                id="BtnOrder" 
                Text=" この見積りを注文 " 
                runat="server" 
                CssClass="btn btn-primary btn-sm" 
                OnClientClick="return validateSelection();" 
                OnClick="BtnOrder_Click" />
        </td>
        <td>
            <asp:Button 
                id="BtnGenerateEstimateReport" 
                Text="見積書発行" 
                runat="server" 
                Enabled="false" 
                CssClass="btn btn-primary btn-sm" 
                OnClientClick="return confirm('見積書を発行します。よろしいでしょうか？');" 
                OnClick="BtnGenerateEstimateReport_Click" />
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

    <div id="EditSection" class="">
            
      <table id="TableEditSection" class="single free" runat="server">
        <tbody>
          <tr>
            <td>
              <!-- 得意先情報 -->
              <table id="EstOrderHeader" class="single free" runat="server">
                <tbody>
                  <tr>
                    <td colspan="1" runat="server" class="single free tlbl">見積No</td>
                    <td colspan="2" runat="server" class="single free tbody nowrap">
                      <asp:TextBox ID="EstOrderNo" runat="server" Enabled="False" ReadOnly="True" TabIndex="1"></asp:TextBox>
                      <asp:HiddenField ID="hidEstOrderNo" runat="server" Value="" />
                    </td>

                    <td colspan="1" runat="server" class="single free tlbl">注文No</td>
                    <td colspan="2" runat="server" class="single free tbody">
                      <asp:TextBox ID="OrderNo" runat="server" TabIndex="2"></asp:TextBox>
                    </td>

                    <td colspan="1" runat="server" class="single free tlbl">入力日付</td>
                    <td colspan="2" runat="server" class="single free tbody nowrap">
                      <asp:TextBox ID="InpDate" runat="server" TabIndex="3"></asp:TextBox>
                      <act:CalendarExtender ID="InpDate_CalendarExtender" 
                          runat="server" 
                          BehaviorID="InpDate_CalendarExtender" 
                          TargetControlID="InpDate" 
                          DaysModeTitleFormat="yyyy年M月"
                          TodaysDateFormat="yyyy年M月d日"
                          Format="yyyy/MM/dd" />
                    </td>

                    <td colspan="1" runat="server" class="single free tlbl">見積日付</td>
                    <td colspan="2" runat="server" class="single free tbody nowrap">
                      <asp:TextBox ID="EstDate" runat="server" TabIndex="4"></asp:TextBox>
                      <act:CalendarExtender ID="EstDate_CalendarExtender" 
                          runat="server" 
                          BehaviorID="EstDate_CalendarExtender" 
                          TargetControlID="EstDate" 
                          DaysModeTitleFormat="yyyy年M月"
                          TodaysDateFormat="yyyy年M月d日"
                          Format="yyyy/MM/dd" />
                    </td>
                  </tr>
                  <tr>
                    <td colspan="1" runat="server" class="single free tlbl">お客様名</td>
                    <td colspan="1" runat="server" class="single free tbody">
                      <asp:TextBox ID="CstCode" runat="server" Enabled="False" ReadOnly="True" TabIndex="5"></asp:TextBox>
                    </td>
                    <td colspan="7" runat="server" class="single free tbody">
                      <asp:TextBox ID="CustomerName" runat="server" Enabled="False" ReadOnly="True" TabIndex="6" Width="400px"></asp:TextBox>
                    </td>
                    <td colspan="1" runat="server" class="single free tlbl">ご担当者</td>
                    <td colspan="2" runat="server" class="single free tbody nowrap">
                      <asp:TextBox ID="CustomreCharge" runat="server" TabIndex="7"></asp:TextBox>
                      <span class="inline-block">様</span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </td>
          </tr> 
          <tr>
            <td>
              <!-- 発送方法 -->
              <asp:Label ID="lblShippingMethod" runat="server" Text="発送方法 "></asp:Label>
              <asp:DropDownList ID="ShippingMethod" runat="server" OnSelectedIndexChanged="ShippingMethod_SelectedIndexChanged" AutoPostBack="True" TabIndex="8">
                <asp:ListItem Value="1">発送</asp:ListItem>
                <asp:ListItem Value="2">引取り</asp:ListItem>
                <asp:ListItem Value="3">配達</asp:ListItem>
              </asp:DropDownList>
              <asp:Label ID="lblShippingMethodMsg" runat="server" Text="リストよりお選びください "></asp:Label>
            </td>
          </tr> 
          <tr>
            <td>
              <!-- 出荷先情報 -->
              <div runat="server" id="DivDistination"> 
                <table id="EstOrderDistination" class="single free" runat="server">
                <tbody>
                  <tr>
                <td colspan="1" runat="server" class="single free tlbl">出荷先</td>
                <td colspan="1" runat="server" class="single free tbody">
                    <asp:TextBox ID="DistCompCd" runat="server" TabIndex="9" name="DistCompCd" style="display:inline-block;align-items:center;gap:5px;"></asp:TextBox>
                </td>
                <td colspan="1" runat="server" class="single free tbody">
                    <asp:Button ID="BtnShukkasakiList" runat="server" Text=" ..." OnClick="BtnShukkasakiList_Click" style="display:inline-block;align-items:center;gap:5px;" TabIndex="10"/>
                </td>
                <td colspan="10" runat="server" class="single free tbody">
                    <asp:TextBox ID="DistCompName" runat="server" TabIndex="11" Width="300px"></asp:TextBox>
                </td>
                </tr>
                  <tr>
                <td colspan="1" runat="server" class="single free tlbl">出荷先部署</td>
                <td colspan="4" runat="server" class="single free tbody">
                    <asp:TextBox ID="DistCompPost" runat="server" TabIndex="12"></asp:TextBox>
                </td>
                <td colspan="2" runat="server" class="single free tlbl">出荷先ご担当者</td>
                <td colspan="3" runat="server" class="single free tbody nowrap">
                    <asp:TextBox ID="DistCharge" runat="server" TabIndex="13"></asp:TextBox>
                    <span class="inline-block">様</span>
                </td>
                <td colspan="1" runat="server" class="single free tlbl">TEL</td>
                <td colspan="2" runat="server" class="single free tbody">
                    <asp:TextBox ID="DistTel" runat="server" TabIndex="14"></asp:TextBox>
                </td>
                </tr>
                  <tr>
                <td colspan="1" runat="server" class="single free tlbl">出荷先住所</td>
                <td colspan="1" runat="server" class="single free tbody">
                    <asp:TextBox ID="DistCompZipCd" runat="server" TabIndex="15" size="10" maxlength="8" onKeyUp="AjaxZip3.zip2addr(this,'','DistCompAddress','DistCompAddress');"></asp:TextBox>
                </td>
                <td colspan="8" runat="server">
                    <asp:TextBox ID="DistCompAddress" runat="server" TabIndex="16" Width="550px"></asp:TextBox>
                </td>
                <td colspan="1" runat="server" class="single free tlbl">FAX</td>
                <td colspan="2" runat="server" class="single free tbody">
                    <asp:TextBox ID="DistFax" runat="server" TabIndex="17"></asp:TextBox>
                </td>
                </tr> 
                  <tr>
                <td colspan="1" runat="server" class="single free tlbl">通信欄</td>
                <td colspan="12" runat="server" class="single free tbody">
                    <asp:TextBox ID="Contact" runat="server" TabIndex="18" size="100" maxlength="500"></asp:TextBox>
                </td>
                </tr>
              </tbody>
            </table>
              </div> 
            </td>
          </tr> 
        </tbody>
      </table>

      <table id="InputAreaTitle" class="title-table">
        <tr>
          <td class="title-table">見積明細編集</td>
        </tr>
      </table>

    <table id="EstOrderInput" runat="server" class="single free inputarea">
      <thead>
        <tr>
            <td colspan="1" rowspan="2" runat="server" class="single free tlbl col-no">No</td>
            <td colspan="1" rowspan="2" runat="server" class="single free tlbl col-zairyou">材料</td>
            <td colspan="3" rowspan="1" runat="server" class="single free tlbl">加工仕様</td>
            <td colspan="1" rowspan="2" runat="server" class="single free tlbl col-shiagari">仕上り</td>
            <td colspan="3" rowspan="1" runat="server" class="single free tlbl">寸法</td>
            <td colspan="4" rowspan="1" runat="server" class="single free tlbl">公差</td>
            <td colspan="3" rowspan="1" runat="server" class="single free tlbl">面取り
                <asp:DropDownList ID="MentoriShiji" runat="server" tabindex="33" name="MentoriShiji" Height="20px" AutoPostBack="True" OnSelectedIndexChanged="MentoriShiji_SelectedIndexChanged" Font-Size="Small">
                    <asp:ListItem Value="1">面取図参照</asp:ListItem>
                    <asp:ListItem Value="2">面取不可</asp:ListItem>
                    <asp:ListItem Selected="True" Value="9">---</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tlbl col-suryou">数量</td>
        </tr>
        <tr>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-kakoushiji">厚み</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-kakoushiji">巾</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-kakoushiji">長さ</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-size-t">厚み</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-size-b">巾</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-size-a">長さ</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-kousa-btn"></td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-kousa">厚み</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-kousa">巾</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-kousa">長さ</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-mentori-btn"></td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-mentori">4角</td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl col-mentori">8辺</td>
        </tr>
      </thead>
      <tbody>
        <tr>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-no">
                <asp:TextBox ID="RowNo" runat="server" name="RowNo" Width="15" Height="40px" Font-Size="Small" enabled="false"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-zairyou">
                <asp:DropDownList ID="Material" runat="server" tabindex="40" name="Material" Height="40px" Font-Size="Large" data-trigger-change="true"></asp:DropDownList>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-kakoushiji">
                <asp:DropDownList ID="Kakou_T" runat="server" tabindex="41" name="Kakou_T" Width="70" Height="40px" Font-Size="Large" data-trigger-change="true"></asp:DropDownList>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-kakoushiji">
                <asp:DropDownList ID="Kakou_B" runat="server" tabindex="42" name="Kakou_B" Width="70" Height="40px" Font-Size="Large" data-trigger-change="true"></asp:DropDownList>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-kakoushiji">
                <asp:DropDownList ID="Kakou_A" runat="server" tabindex="43" name="Kakou_A" Width="70" Height="40px" Font-Size="Large" data-trigger-change="true"></asp:DropDownList>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-shiyou">
                <asp:DropDownList ID="Kakou" runat="server" tabindex="44" name="Kakou" Width="80" Height="40px" Font-Size="Large" data-trigger-change="true"></asp:DropDownList>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-size-t">
                <asp:TextBox ID="Size_T" runat="server" tabindex="45" name="Size_T" Width="56px" Height="40px" CssClass="col-size" data-trigger-change="true"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-size-b">
                <asp:TextBox ID="Size_B" runat="server" tabindex="46" name="Size_B" Width="76px" Height="40px" CssClass="col-size" data-trigger-change="true"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-size-a">
                <asp:TextBox ID="Size_A" runat="server" tabindex="47" name="Size_A" Width="86px" Height="40px" CssClass="col-size" data-trigger-change="true"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-kousa-btn">
                <asp:Button ID="BtnStandKousa" runat="server" Text="標 準" name="btn_AddEstimateExp" Height="40px" tabindex="48" OnClick="BtnStandKousa_Click" Font-Size="Small" />
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody col-kousa">
                <asp:TextBox ID="Kousa_T_U" runat="server" tabindex="49" name="Kousa_T_U" Width="55px" Height="22px" CssClass="col-tolerance" data-trigger-change="true"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody col-kousa">
                <asp:TextBox ID="Kousa_B_U" runat="server" tabindex="51" name="Kousa_B_U" Width="55px" Height="22px" CssClass="col-tolerance" data-trigger-change="true"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody col-kousa">
                <asp:TextBox ID="Kousa_A_U" runat="server" tabindex="53" name="Kousa_A_U" Width="55px" Height="22px" CssClass="col-tolerance" data-trigger-change="true"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-mentori-btn">
                <asp:Button ID="BtnStandMentori" runat="server" Text="標 準" name="BtnStandMentori" Height="40px" tabindex="54" OnClick="BtnStandMentori_Click" Font-Size="Small" />
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody nowrap col-mentori">
                <asp:TextBox ID="Mentori_4" runat="server" tabindex="55" name="Mentori_4" Width="40px" Height="40px" CssClass="col-chamfering" data-trigger-change="true"></asp:TextBox>
                <span class="inline-block">C</span>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody nowrap col-mentori">
                <asp:TextBox ID="Mentori_8" runat="server" tabindex="56" name="Mentori_8" Width="40px" Height="40px" CssClass="col-chamfering" data-trigger-change="true"></asp:TextBox>
                <span class="inline-block">C</span>
            </td>
            <td colspan="1" rowspan="2" runat="server" class="single free tbody col-suryou">
                <asp:TextBox ID="Suryou" runat="server" tabindex="57" name="Suryou" Width="50px" Height="40px" CssClass="col-amount" data-trigger-change="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody col-kousa">
                <asp:TextBox ID="Kousa_T_L" runat="server" tabindex="50" name="Kousa_T_L" Width="55px" Height="22px" CssClass="col-tolerance" data-trigger-change="true"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody col-kousa">
                <asp:TextBox ID="Kousa_B_L" runat="server" tabindex="52" name="Kousa_B_L" Width="55px" Height="22px" CssClass="col-tolerance" data-trigger-change="true"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody col-kousa">
                <asp:TextBox ID="Kousa_A_L" runat="server" tabindex="54" name="Kousa_A_L" Width="55px" Height="22px" CssClass="col-tolerance" data-trigger-change="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" rowspan="1" runat="server" class="single free tlbl">お客様注文番号</td>
            <td colspan="2" rowspan="1" runat="server" class="single free tbody">
                <asp:TextBox ID="CustomerNo" runat="server" tabindex="58" name="CustomerNo" Width="120px" Height="22px" Font-Size="Large"></asp:TextBox>
            </td>
            <td colspan="2" rowspan="1" runat="server" class="single free tlbl">送り先様注文番号</td>
            <td colspan="2" rowspan="1" runat="server" class="single free tbody">
                <asp:TextBox ID="EndUserNo" runat="server" tabindex="59" name="EndUserNo" Width="120px" Height="22px" Font-Size="Large"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tlbl">備考</td>
            <td colspan="8" rowspan="1" runat="server" class="single free tbody">
                <asp:TextBox ID="Refer" runat="server" tabindex="60" name="Refer" Width="500px" Height="22px" Font-Size="Large"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" rowspan="1" runat="server" class="single free tlbl">最短納期</td>
            <td colspan="2" rowspan="1" runat="server" class="single free tbody nowrap">
                <asp:TextBox ID="Nouki_S" runat="server" Enabled="False" tabindex="0" name="Nouki_S" CssClass="date-cell" Font-Size="Large"></asp:TextBox>
            </td>
            <td colspan="2" rowspan="1" runat="server" class="single free tlbl">送料込みプレート単価</td>
            <td colspan="2" rowspan="1" runat="server" class="single free tbody">
                <asp:TextBox ID="Price_U" runat="server" Enabled="False" tabindex="0" name="UnitPrice" CssClass="total-cell" Font-Size="Large"></asp:TextBox>
            </td>
            <td colspan="2" rowspan="1" runat="server" class="single free tlbl">送料込みプレート金額</td>
            <td colspan="3" rowspan="1" runat="server" class="single free tbody">
                <asp:TextBox ID="Price_S" runat="server" Enabled="False" tabindex="0" name="SumPrice" CssClass="total-cell" Font-Size="Large"></asp:TextBox>
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody">
                <asp:Button 
                    ID="BtnEstimate" 
                    runat="server" 
                    Text="見 積" 
                    name="BtnEstimate" 
                    tabindex="61" 
                    Height="30px" 
                    OnClick="BtnEstimate_Click" 
                    Font-Size="Small" />
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody">
                <asp:Button 
                    ID="BtnClearRecord" 
                    runat="server" 
                    Text="クリア" 
                    name="BtnClearRecord" 
                    tabindex="0" 
                    Height="30px" 
                    OnClick="BtnClearRecord_Click" 
                    Font-Size="Small" />
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody">
                <asp:Button 
                    ID="BtnAddRecord" 
                    runat="server" 
                    Text="追 加" 
                    name="BtnAddRecord"
                    Enabled="false" 
                    tabindex="62" 
                    Height="30px" 
                    OnClick="BtnAddRecord_Click" 
                    Font-Size="Small" />
            </td>
            <td colspan="1" rowspan="1" runat="server" class="single free tbody">
            </td>
        </tr>
      </tbody>
    </table>
        
    <table id="ErrorMessageArea" class="single">
      <tr>
        <td class="multi free tbody"><asp:Label ID="lblErrorMessages" runat="server" Text=""></asp:Label></td>
      </tr>
    </table>
    </div>

    <div ID="RecordSection" class="table-area" tabindex="50">

    <table id="RegisteredDetailsAreaTitle" class="title-table">
      <tr>
        <td class="title-table">登録済み明細</td>
        <td class="title-table" width="250"></td>
        <td class="title-table"><asp:Label ID="Notice" runat="server" Text="このお見積もりの有効期間は本日 HH:MM までです。"></asp:Label></td>
      </tr>
    </table>
        <asp:ListView ID="ListView1" runat="server" 
            GroupPlaceholderID="groupPlaceHolder1" 
            ItemPlaceholderID="itemPlaceHolder1" 
            OnDataBound="ListView1_DataBound" 
            OnItemDataBound="ListView1_ItemDataBound"
            OnItemCommand="ListView1_OnItemCommand">

            <EmptyDataTemplate>
                <div class="header-area">
                  <table id="TblEmpty" class="multi">
                        <thead>
                          <tr>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-no">No</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-order">注文</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-zairyou">材料</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">厚み</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">幅</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">長さ</td>
                            <td colspan="3" rowspan="1" runat="server" class="multi free tlbl">面取り</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-suryou">数量</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-nouki">納期</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-order-no">ご注文番号</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">プレート金額</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-btn"></td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-btn"></td>
                          </tr>
                          <tr>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl">加工</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-kousa">公差</td>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl">加工</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-kousa">公差</td>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl">加工</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-kousa">公差</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-mentori">4角</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-mentori">8辺</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-mentori-detail">詳細</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-uprice">単価</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-price">金額</td>
                          </tr>
                          <tr>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl rcol-size-t">寸法</td>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl rcol-size-a">寸法</td>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl rcol-size-b">寸法</td>
                          </tr>
                        </thead>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr class="footer">
                          <td colspan ="16" runat="server" class="multi free tlbl"> </td>
                          <td colspan ="1" runat="server" class="multi free tlbl">合計金額</td>
                          <td colspan ="3" runat="server" class="multi free tlbl">
                            <asp:Label ID="lbl合計金額" runat="server" CssClass="number-cell" />
                          </td>
                        </tr>
                     </table>
                     <div><label><p class="error-text">データが存在しません。</p></label></div>
                            
                </div>
            </EmptyDataTemplate>

            <LayoutTemplate>
                <div class="header-area">
                  <table id="EstOrderList" class="multi">
                        <thead>
                          <tr>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-no">No</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-order">注文
                                <asp:Button ID="btnToggleCheck" runat="server" Text="すべて選択" OnClientClick="toggleCheckboxes(this); return false;" />
                            </td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-zairyou">材料</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">厚み</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">幅</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">長さ</td>
                            <td colspan="3" rowspan="1" runat="server" class="multi free tlbl">面取り</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-suryou">数量</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-nouki">納期</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-order-no">ご注文番号</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">プレート金額</td>
                            <td colspan="3" rowspan="3" runat="server" class="multi free tlbl rcol-btn"></td>
                          </tr>
                          <tr>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl">加工</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-kousa">公差</td>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl">加工</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-kousa">公差</td>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl">加工</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-kousa">公差</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-mentori">4角</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-mentori">8辺</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-mentori-detail">詳細</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-uprice">単価</td>
                            <td colspan="1" rowspan="2" runat="server" class="multi free tlbl rcol-price">金額</td>
                          </tr>
                          <tr>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl rcol-size-t">寸法</td>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl rcol-size-a">寸法</td>
                            <td colspan="1" rowspan="1" runat="server" class="multi free tlbl rcol-size-b">寸法</td>
                          </tr>
                        </thead>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                        <tr class="footer">
                          <td colspan ="16" runat="server" class="multi free tlbl"> </td>
                          <td colspan ="1" runat="server" class="multi free tlbl">合計金額</td>
                          <td colspan ="3" runat="server" class="multi free tlbl">
                            <asp:Label ID="lbl合計金額" runat="server" CssClass="number-cell" />
                          </td>
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
                  <tr runat="server">
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-no">
	                  <asp:Label ID="lblSeqNo" runat="server" Text='<%# (Eval("No")) %>'>No</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-order text-center">
                        <asp:CheckBox ID="ChkSelect" runat="server" 
                            Enabled='<%# IsChkSelectable(Eval("最短納期"), Eval("見積金額"), Eval("見積単価")) %>' 
                            Visible='<%# Eval("受注確定区分").ToString() != "1" %>' />

                        <asp:Image ID="imgOrderConfirmed" runat="server" ImageUrl="~/Images/order_confirm.png" 
                                CssClass="order-confirm-img"
                                Visible='<%# Eval("受注確定区分").ToString() == "1" %>' />

                        <asp:HiddenField ID="IsOrderConfirmed" runat="server" Value='<%# Eval("受注確定区分") %>' />
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-zairyou">
	                  <asp:Label ID="lblMaterialName" runat="server" Text='<%# (Eval("材料名")) %>'>材料</asp:Label>
                      <asp:HiddenField ID="hidMaterialCd" runat="server" Value='<%# (Eval("材料コード")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblKakoushiji_T" runat="server" Text='<%# (Eval("加工指示_T")) %>'>加工(T)</asp:Label>
                      <asp:HiddenField ID="hidKakoushijiCd_T" runat="server" Value='<%# (Eval("加工指示コードT")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblKakoukousa_UT" runat="server" Text='<%# (Eval("加工公差_UT")) %>' CssClass="tolerance-cell">公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="Label5" runat="server" Text='<%# (Eval("加工指示_A")) %>'>加工(A)</asp:Label>
                      <asp:HiddenField ID="hidKakoushijiCd_A" runat="server" Value='<%# (Eval("加工指示コードA")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblKakoukousa_UA" runat="server" Text='<%# (Eval("加工公差_UA")) %>' CssClass="tolerance-cell">公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="Label7" runat="server" Text='<%# (Eval("加工指示_B")) %>'>加工(B)</asp:Label>
                      <asp:HiddenField ID="hidKakoushijiCd_B" runat="server" Value='<%# (Eval("加工指示コードB")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblKakoukousa_UB" runat="server" Text='<%# (Eval("加工公差_UB")) %>' CssClass="tolerance-cell">公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblMentori_4C" runat="server" Text='<%# (Eval("面取り量_4C")) %>' CssClass="tolerance-cell">4角</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblMentori_8C" runat="server" Text='<%# (Eval("面取り量_8C")) %>' CssClass="tolerance-cell">8辺</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody">
                      <asp:Button ID="BtnMentoriDetail" runat="server" Text="詳細"
                            name="BtnMentoriDetail" 
                            tabindex="21" 
                            OnClick="BtnMentoriDetail_Click" />
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-suryou">
	                  <asp:Label ID="lblSuryou" runat="server" Text='<%# (Eval("製品数量")) %>' CssClass="number-cell">数量</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-nouki">
	                  <asp:Label ID="lblRequestNouki" runat="server" Text='<%# (Eval("希望納期")) %>'>最短納期</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-order-no">
	                  <asp:Label ID="lblOrderNo" runat="server" Text='<%# (Eval("客先注番")) %>'>注文番号</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-uprice">
	                  <asp:Label ID="lblUnitPrice" runat="server" Text='<%# (Eval("見積単価", "{0:N0}")) %>' CssClass="number-cell">単価</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-price">
	                  <asp:Label ID="lblPrice" runat="server" Text='<%# (Eval("見積金額", "{0:N0}")) %>' CssClass="number-cell">金額</asp:Label>
                    </td>

                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-btn">
                        <asp:LinkButton runat="server" 
                            ID="BtnEditList" 
                            CommandName="EditEstimate" 
                            CommandArgument='<%# Eval("No") %>'
                            tabindex="21" 
                            Text="編集"
                            CssClass='<%# Eval("受注確定区分").ToString() == "1" ? "button-style disabled-btn" : "button-style" %>'
                            Enabled='<%# Eval("受注確定区分").ToString() != "1" %>' />
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-btn">
                        <asp:LinkButton runat="server" 
                            ID="BtnCopyList" 
                            CommandName="CopyEstimate" 
                            CommandArgument='<%# Eval("No") %>'
                            tabindex="21" 
                            Text="複写"
                            CssClass='<%# Eval("受注確定区分").ToString() == "1" ? "button-style disabled-btn" : "button-style" %>'
                            Enabled='<%# Eval("受注確定区分").ToString() != "1" %>' />
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody rcol-btn">
                        <asp:LinkButton runat="server" 
                            ID="BtnDeleteList" 
                            CommandName="DeleteEstimate" 
                            CommandArgument='<%# Eval("No") %>'
                            tabindex="22" 
                            Text="削除"
                            CssClass='<%# Eval("受注確定区分").ToString() == "1" ? "delete-button button-style disabled-btn" : "delete-button button-style" %>'
                            Enabled='<%# Eval("受注確定区分").ToString() != "1" %>'
                            OnClientClick='<%# Eval("受注確定区分").ToString() == "1" ? "return false;" : "return confirm(\"この明細を削除します。よろしいですか？\");" %>' />
                    </td>

                  </tr>
                  <tr runat="server" class="multi free tbody">
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-size-t">
                      <asp:Label ID="lblSize_T" runat="server" Text='<%# (Eval("仕上りサイズT")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-kousa">
	                  <asp:Label ID="lblKakoukousa_LT" runat="server" Text='<%# (Eval("加工公差_LT")) %>' CssClass="tolerance-cell">公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-size-a">
	                  <asp:Label ID="lblSize_A" runat="server" Text='<%# (Eval("仕上りサイズA")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-kousa">
	                  <asp:Label ID="lblKakoukousa_LA" runat="server" Text='<%# (Eval("加工公差_LA")) %>' CssClass="tolerance-cell">公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-size-b">
	                  <asp:Label ID="lblSize_B" runat="server" Text='<%# (Eval("仕上りサイズB")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-kousa">
	                  <asp:Label ID="lblKakoukousa_LB" runat="server" Text='<%# (Eval("加工公差_LB")) %>' CssClass="tolerance-cell">公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblShortestNouki" runat="server" Text='<%# (Eval("最短納期")) %>'>最短納期</asp:Label>
                    </td>
                  </tr>
            </ItemTemplate>

        </asp:ListView>
        </div>

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