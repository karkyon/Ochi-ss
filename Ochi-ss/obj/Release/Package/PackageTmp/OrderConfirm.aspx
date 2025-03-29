<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.Edit.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="OrderConfirm.aspx.cs" Inherits="Ochi_ss.OrderConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Async = "true" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="main table-responsive" style="margin: 0 auto; text-align: center;">
  <form id="form1" runat="server" style="display: inline-block; text-align: left;">

    <!-- メニューボタン -->
    <table class="TopButtonArea" runat="server">
      <tbody>
        <tr>
          <td colspan="6">
            ご注文内容の確認
          </td>
          <td>
            <asp:Button 
                id="BtnOrder" 
                Text=" ご注文確定 " 
                runat="server" 
                CssClass="btn btn-primary btn-sm" 
                OnClick="BtnOrder_Click" 
                OnClientClick="return confirm('ご注文を確定します。よろしいですか？');"/>
          </td>
          <td>
            <asp:Button 
                id="BtnReturnEdit" 
                Text=" 入力画面へ戻る " 
                runat="server" 
                CssClass="btn btn-primary btn-sm" 
                OnClick="BtnReturnEdit_Click" 
                OnClientClick="return confirm('見積入力画面へ戻ります。よろしいですか？');"/>
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
                      <asp:TextBox ID="OrderNo" runat="server" Enabled="False" ReadOnly="True" TabIndex="2"></asp:TextBox>
                    </td>

                    <td colspan="1" runat="server" class="single free tlbl">入力日付</td>
                    <td colspan="2" runat="server" class="single free tbody nowrap">
                      <asp:TextBox ID="InpDate" runat="server" Enabled="False" ReadOnly="True" TabIndex="3"></asp:TextBox>
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
                      <asp:TextBox ID="EstDate" runat="server" Enabled="False" ReadOnly="True" TabIndex="4"></asp:TextBox>
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
                      <asp:TextBox ID="CustomreCharge" runat="server" Enabled="False" ReadOnly="True" TabIndex="7"></asp:TextBox>
                      <span class="inline-block">様</span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </td>
          </tr> 
          <tr>
            <td colspan="12">
              <!-- 発送方法 -->
              <asp:Label ID="lblShippingMethod" runat="server" Text="発送方法 "></asp:Label>
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
                      <asp:TextBox ID="DistCompCd" runat="server" Enabled="False" ReadOnly="True" TabIndex="9" name="DistCompCd" style="display:inline-block;align-items:center;gap:5px;"></asp:TextBox>
                    </td>
                    <td colspan="8" runat="server" class="single free tbody">
                      <asp:TextBox ID="DistCompName" runat="server" Enabled="False" ReadOnly="True" TabIndex="11" Width="300px"></asp:TextBox>
                    </td>
                  </tr>
                  <tr>
                    <td colspan="1" runat="server" class="single free tlbl">出荷先部署</td>
                    <td colspan="2" runat="server" class="single free tbody">
                      <asp:TextBox ID="DistCompPost" runat="server" Enabled="False" ReadOnly="True" TabIndex="12"></asp:TextBox>
                    </td>
                    <td colspan="1" runat="server" class="single free tlbl">出荷先ご担当者</td>
                    <td colspan="3" runat="server" class="single free tbody nowrap">
                      <asp:TextBox ID="DistCharge" runat="server" Enabled="False" ReadOnly="True" TabIndex="13"></asp:TextBox>
                      <span class="inline-block">様</span>
                    </td>
                    <td colspan="1" runat="server" class="single free tlbl">TEL</td>
                    <td colspan="2" runat="server" class="single free tbody">
                      <asp:TextBox ID="DistTel" runat="server" Enabled="False" ReadOnly="True" TabIndex="14"></asp:TextBox>
                    </td>
                  </tr>
                  <tr>
                    <td colspan="1" runat="server" class="single free tlbl">出荷先住所</td>
                    <td colspan="1" runat="server" class="single free tbody">
                      <asp:TextBox ID="DistCompZipCd" runat="server" Enabled="False" ReadOnly="True" TabIndex="15" size="10" maxlength="8" onKeyUp="AjaxZip3.zip2addr(this,'','DistCompAddress','DistCompAddress');"></asp:TextBox>
                    </td>
                    <td colspan="5" runat="server">
                      <asp:TextBox ID="DistCompAddress" runat="server" Enabled="False" ReadOnly="True" TabIndex="16" Width="550px"></asp:TextBox>
                    </td>
                    <td colspan="1" runat="server" class="single free tlbl">FAX</td>
                    <td colspan="2" runat="server" class="single free tbody">
                      <asp:TextBox ID="DistFax" runat="server" Enabled="False" ReadOnly="True" TabIndex="17"></asp:TextBox>
                    </td>
                  </tr> 
                  <tr>
                    <td colspan="1" runat="server" class="single free tlbl">通信欄</td>
                    <td colspan="12" runat="server" class="single free tbody">
                      <asp:TextBox ID="Contact" runat="server" Enabled="False" ReadOnly="True" TabIndex="18" size="100" maxlength="500"></asp:TextBox>
                    </td>
                </tr>
              </tbody>
            </table>
              </div> 
            </td>
          </tr> 
        </tbody>
      </table>
        
    <table id="RegisteredDetailsAreaTitle" class="title-table">
      <tr>
        <td class="title-table">明細</td>
        <td class="title-table" width="250"></td>
        <td class="title-table"><asp:Label ID="Notice" runat="server" Text="このお見積もりの有効期間は本日 HH:MM までです。"></asp:Label></td>
      </tr>
    </table>
        <asp:ListView ID="ListView1" runat="server" 
            GroupPlaceholderID="groupPlaceHolder1" 
            ItemPlaceholderID="itemPlaceHolder1" >

            <EmptyDataTemplate>
                <div class="header-area">
                  <table id="TblEmpty" class="multi">
                        <thead>
                          <tr>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-no">No</td>
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
                          <td colspan ="2" runat="server" class="multi free tlbl">
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
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-zairyou">材料</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">厚み</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">幅</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">長さ</td>
                            <td colspan="3" rowspan="1" runat="server" class="multi free tlbl">面取り</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-suryou">数量</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-nouki">納期</td>
                            <td colspan="1" rowspan="3" runat="server" class="multi free tlbl rcol-order-no">ご注文番号</td>
                            <td colspan="2" rowspan="1" runat="server" class="multi free tlbl">プレート金額</td>
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
                          <td colspan ="14" runat="server" class="multi free tlbl"> </td>
                          <td colspan ="1" runat="server" class="multi free tlbl">合計金額</td>
                          <td colspan ="2" runat="server" class="multi free tlbl">
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
	                  <asp:Label ID="lblSeqNo" runat="server" Text='<%# (Eval("WO明細No")) %>'>No</asp:Label>
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
	                  <asp:Label ID="lblKakoukousa_UT" runat="server" Text='<%# (Eval("加工公差_UT")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="Label5" runat="server" Text='<%# (Eval("加工指示_A")) %>'>加工(A)</asp:Label>
                      <asp:HiddenField ID="hidKakoushijiCd_A" runat="server" Value='<%# (Eval("加工指示コードA")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblKakoukousa_UA" runat="server" Text='<%# (Eval("加工公差_UA")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="Label7" runat="server" Text='<%# (Eval("加工指示_B")) %>'>加工(B)</asp:Label>
                      <asp:HiddenField ID="hidKakoushijiCd_B" runat="server" Value='<%# (Eval("加工指示コードB")) %>' />
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblKakoukousa_UB" runat="server" Text='<%# (Eval("加工公差_UB")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblMentori_4C" runat="server" Text='<%# (Eval("面取り量_4C")) %>'>4角</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody">
	                  <asp:Label ID="lblMentori_8C" runat="server" Text='<%# (Eval("面取り量_8C")) %>'>8辺</asp:Label>
                    </td>
                    <td colspan="1" rowspan="2" runat="server" class="multi free tbody">
                      <asp:Button ID="BtnMentoriDetail" runat="server" Text="詳細" name="BtnMentoriDetail" tabindex="21" />
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
                  </tr>
                  <tr runat="server" class="multi free tbody">
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-size-t">
                      <asp:Label ID="lblSize_T" runat="server" Text='<%# (Eval("仕上りサイズT")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-kousa">
	                  <asp:Label ID="lblKakoukousa_LT" runat="server" Text='<%# (Eval("加工公差_LT")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-size-a">
	                  <asp:Label ID="lblSize_A" runat="server" Text='<%# (Eval("仕上りサイズA")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-kousa">
	                  <asp:Label ID="lblKakoukousa_LA" runat="server" Text='<%# (Eval("加工公差_LA")) %>'>公差</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-size-b">
	                  <asp:Label ID="lblSize_B" runat="server" Text='<%# (Eval("仕上りサイズB")) %>'>寸法</asp:Label>
                    </td>
                    <td colspan="1" rowspan="1" runat="server" class="multi free tbody rcol-kousa">
	                  <asp:Label ID="lblKakoukousa_LB" runat="server" Text='<%# (Eval("加工公差_LB")) %>'>公差</asp:Label>
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