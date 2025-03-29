using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;
using System.Configuration;

namespace Ochi_ss
{
    public partial class MainForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();

            }

            // ログインユーザ情報(顧客コード、顧客名、担当者名)表示
            if ((string)Session["UserName"] != "")
            {
                string msg = string.Format("[{0:s}] {1:s}　{2:s} 様", (string)Session["CustomerCd"], (string)Session["CustomerName"], (string)Session["ChargeName"]);
                LoginUserInfo.Text = msg;
            }else { LoginUserInfo.Text = ""; }

            // セッション情報(セッションID)表示　###　開発時のみ　###
            // SessionIdの取得
            string currSessionID = Session.SessionID;

            if (currSessionID != "")
            {
                SessionID.Text = currSessionID;
            }else { SessionID.Text = ""; }

            // 見積Noの初期化
            Session["EstOrderNo"] = string.Empty;


            // お知らせの表示
            LoadAnnouncements();
        }

        private void LoadAnnouncements()
        {
            // 仮のデータ（本来はDBから取得）
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Message", typeof(string));

            dt.Rows.Add(DateTime.Now, "新しいお知らせがあります！");
            dt.Rows.Add(DateTime.Now.AddDays(-1), "メンテナンスのお知らせ");

            gvAnnouncements.DataSource = dt;
            gvAnnouncements.DataBind();
        }

        protected void EstimateOrder_Click(object sender, EventArgs e)
        {
            // 見積画面の編集モードは「新規」
            Session["EditMode"] = "New";

            // 見積Noの初期化
            Session["EstOrderNo"] = string.Empty;

            Response.Redirect("EstOrder.aspx");
        }

        protected void EstimateOrderHist_Click(object sender, EventArgs e)
        {
            Response.Redirect("EstOrderSearch.aspx");
        }

        protected void OrderHist_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderSearch.aspx");
        }

        protected void DeliveryDest_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShukkasakiList.aspx");
        }

        protected void DataOutput_Click(object sender, EventArgs e)
        {
            Response.Redirect("EstOrder.aspx");
        }

        protected void Cube3DImage_Click(object sender, EventArgs e)
        {
            Response.Redirect("Cube3DImage.aspx");
        }
    }
}