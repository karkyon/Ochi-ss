using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.AxHost;
using System.Diagnostics;
using System.Web.Configuration;

namespace Ochi_ss
{
    public partial class OrderThanx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // セッション値の取得
                string sessionID = Session.SessionID;
                string OrderNo = GetSessionValue("OrderNo");
                string customerCd = GetSessionValue("CustomerCd");

                txbOrderNo.Text = OrderNo;
                txbOrderDate.Text = DateTime.Now.ToString("yyyy/MM/dd"); ;
            }
        }

        private string GetSessionValue(string key)
        {
            return Session[key]?.ToString() ?? string.Empty;
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            // 見積Noクリア
            Session["EditMode"] = string.Empty;
            Session["EstOrderNo"] = string.Empty;
            Response.Redirect("EstOrderSearch.aspx");
        }

        protected void BtnReturnEdit_Click(object sender, EventArgs e)
        {
            // 見積Noクリア
            Session["EditMode"] = "Edit";
            Session["EstOrderNo"] = string.Empty;
            Response.Redirect("EstOrder.aspx");
        }

        protected void BtnMainForm_Click(object sender, EventArgs e)
        {
            // 見積Noクリア
            Session["EditMode"] = string.Empty;
            Session["EstOrderNo"] = string.Empty;
            Response.Redirect("MainForm.aspx");
        }
        
        protected void BtnGenerateOrderReport_Click(object sender, EventArgs e)
        {
            // セッション値の取得
            string sessionID = Session.SessionID;
            string OrderNo = GetSessionValue("OrderNo");
            string customerCd = GetSessionValue("CustomerCd");
            // レポート出力
            // GenerateOrderReport(OrderNo, customerCd);
        }

        private void DebugLog(string message)
        {
            Debug.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss} - {message}");
        }
    }
}
