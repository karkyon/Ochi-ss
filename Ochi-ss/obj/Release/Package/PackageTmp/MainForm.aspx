<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.Master" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="Ochi_ss.MainForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="main table-responsive" style="margin: 0 auto; text-align: center;">
        <form id="form1" runat="server" style="display: inline-block; text-align: left;">
            <div id="CustomerInfo">
                <h6><asp:Label ID="LoginUserInfo" runat="server" Text=""></asp:Label></h6>
                <h6><asp:Label ID="SessionID" runat="server" Text=""></asp:Label></h6>
            </div>
            <div>
                <h4> メインメニュー</h4>

                <div class="manu">
                    <asp:Button id="EstimateOrder" Text="お見積り・ご注文" runat="server" CssClass="btn btn-primary btn-lg" width="" OnClick="EstimateOrder_Click"/>
                    <asp:Button id="EstimateOrderHist" Text="お見積り履歴" runat="server" CssClass="btn btn-primary btn-lg" width="" OnClick="EstimateOrderHist_Click"/>
                    <asp:Button id="OrderHist" Text="ご注文履歴" runat="server" CssClass="btn btn-primary btn-lg" width="" OnClick="OrderHist_Click"/>
                    <asp:Button id="DeliveryDest" Text="納入先管理" runat="server" CssClass="btn btn-primary btn-lg" width="" OnClick="DeliveryDest_Click"/>
                    <asp:Button id="DataOutput" Text="納品書発行・データ出力" runat="server" CssClass="btn btn-primary btn-lg" width="" OnClick="DataOutput_Click"/>
                </div>

                
                <div id="BulletinBoardSection" style="margin-top: 20px; text-align: left;">
                    <h4>取引先連絡用掲示板</h4>
                    <asp:TextBox ID="txtSenderName" runat="server" CssClass="input-small" Placeholder="お名前を入力してください"></asp:TextBox>
                    <asp:TextBox ID="txtMessage" runat="server" CssClass="input-small" TextMode="MultiLine" Rows="3" Placeholder="メッセージを入力してください"></asp:TextBox>
                    <asp:Button ID="btnPostMessage" runat="server" CssClass="btn btn-success btn-sm" Text="投稿" OnClick="btnPostMessage_Click" />

                    <div id="MessageList" style="margin-top: 30px;">
                        <asp:GridView ID="gvMessages" runat="server" AutoGenerateColumns="False" CssClass="custom-grid table-striped" OnRowCommand="gvMessages_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="SenderName" HeaderText="送信者" />
                                <asp:BoundField DataField="MessageText" HeaderText="メッセージ" />
                                <asp:BoundField DataField="PostedDate" HeaderText="投稿日時" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
        
                                <asp:TemplateField HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:Button ID="btnReply" runat="server" Text="返信" CommandName="Reply" CommandArgument='<%# Eval("MessageID") %>' CssClass="btn btn-sm btn-info" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="返信あり">
                                    <ItemTemplate>
                                        <asp:Image ID="imgHasReplies" runat="server" ImageUrl='<%# (bool)Eval("HasReplies") ? "~/images/reply_icon.png" : "~/images/no_reply_icon.png" %>' ToolTip='<%# (bool)Eval("HasReplies") ? "返信あり" : "返信なし" %>' Width="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <!-- モーダルウィンドウ -->
                <div class="modal fade" id="replyModal" tabindex="-1" role="dialog" aria-labelledby="replyModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="replyModalLabel">メッセージに返信</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <asp:HiddenField ID="hfMessageID" runat="server" />
                                <asp:TextBox ID="txtReplySenderName" runat="server" CssClass="form-control" Placeholder="お名前を入力してください" />
                                <br />
                                <asp:TextBox ID="txtReplyText" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" Placeholder="返信内容を入力してください" />
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnPostReply" runat="server" CssClass="btn btn-success" Text="返信を投稿" OnClick="btnPostReply_Click" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">閉じる</button>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="exit">
                    <asp:LoginStatus id="LoginStatus1" runat="server" CssClass="btn btn-secondary btn-md" />
                </div>
            </div>
        </form>
    </div>

</asp:Content>