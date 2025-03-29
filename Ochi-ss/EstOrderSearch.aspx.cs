using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Configuration;

namespace Ochi_ss
{
    public partial class EstOrderSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // テータバインド処理
                BindData();
            }
        }

        protected void BindData()
        {
            try
            {
                GeneralCls gCls = new GeneralCls();

                // 数値、通貨スタイルの設定
                NumberStyles style;
                CultureInfo culture;

                style = NumberStyles.Number                 // 先頭空白、最後空白、先頭符号、後続符号、小数点、3桁区切りカンマ
                      | NumberStyles.AllowCurrencySymbol    // 通貨記号
                      | NumberStyles.AllowDecimalPoint      // 小数点
                      | NumberStyles.AllowLeadingWhite      // 先頭空白
                      | NumberStyles.AllowTrailingWhite     // 後続空白
                      | NumberStyles.AllowThousands;        // 3桁区切りカンマ
                culture = CultureInfo.CreateSpecificCulture("ja-JP");

                // DB接続文字列
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                // 各入力値を取得
                string custCode = "";
                if (Session["CustomerCd"] != null)
                {
                    custCode = (string)Session["CustomerCd"];
                }
                string estDate_s = CondInputDate_s.Text;
                string estDate_e = CondInputDate_e.Text;
                string estNo_s = "";
                string estNo_e = "";
                string destCompany = CondlDestCompName.Text;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                    sqlParams.AddSqlParam(new SqlParameter("@CustomerCd", SqlDbType.VarChar, 6), ParameterDirection.Input).Value = custCode;
                    sqlParams.AddSqlParam(new SqlParameter("@EstDate_s", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = estDate_s;
                    sqlParams.AddSqlParam(new SqlParameter("@EstDate_e", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = estDate_e;
                    sqlParams.AddSqlParam(new SqlParameter("@EstNo_s", SqlDbType.VarChar, 8), ParameterDirection.Input).Value = estNo_s;
                    sqlParams.AddSqlParam(new SqlParameter("@EstNo_e", SqlDbType.VarChar, 8), ParameterDirection.Input).Value = estNo_e;
                    sqlParams.AddSqlParam(new SqlParameter("@DestCompany", SqlDbType.VarChar, 50), ParameterDirection.Input).Value = destCompany;

                    // レコード取得
                    var dt = gCls.GetDataTable(con, "usp_ASP_EstimateSearchLiteList_get", sqlParams);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        lblErrorMessage.Text = "検索結果がありません。";
                        lblErrorMessage.Visible = true;
                        EstOrderList.DataSource = null;
                    }
                    else
                    {
                        lblErrorMessage.Visible = false;
                        EstOrderList.DataSource = dt;
                    }
                    EstOrderList.DataBind();
                    con.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                // SQLエラー処理
                lblErrorMessage.Text = "データベースエラーが発生しました。管理者に連絡してください。";
                lblErrorMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine("SQLエラー: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // 一般的なエラー処理
                lblErrorMessage.Text = "エラーが発生しました。管理者に連絡してください。";
                lblErrorMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine("エラー: " + ex.Message);
            }
        }

        protected void EstOrderList_PagePropertiesChanged(object sender, EventArgs e)
        {
            BindData();  // ページ変更時にデータを再取得

            //Set the text to empty when navigating to a different page
            lblResult.Text = "";
        }

        protected void btnGenerateEstimation_Click(object sender, EventArgs e)
        {
            // 見積書を発行する処理をここに記述
            Response.Write("<script>alert('見積書を発行しました！');</script>");
        }

        protected void EstOrderList_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            lblResult.Text = e.AffectedRows.ToString() + " row(s) successfully updated";
        }

        protected void BtnLoadEditEstOrder_Click(object sender, EventArgs e)
        {
            Button i = (Button)sender;
            ListViewDataItem item = (ListViewDataItem)i.Parent.Parent.Parent;
            // int index = item.Parent.Controls.IndexOf(item);
            int index = (int)item.DataItemIndex;

            // 見積No
            Label estNo = ((Label)EstOrderList.Items[index].FindControl("lblEstNo"));
            Session["EstOrderNo"] = estNo.Text.ToString();
            Session["EditMode"] = "Edit";
            Response.Redirect("EstOrder.aspx");
        }

        protected void BtnLoadCopyEstOrder_Click(object sender, EventArgs e)
        {
            Button i = (Button)sender;
            ListViewDataItem item = (ListViewDataItem)i.Parent.Parent.Parent;
            // int index = item.Parent.Controls.IndexOf(item);
            int index = (int)item.DataItemIndex;

            // 見積No
            Label estNo = ((Label)EstOrderList.Items[index].FindControl("lblEstNo"));
            Session["EstOrderNo"] = estNo.Text.ToString();
            Session["EditMode"] = "Copy";
            Response.Redirect("EstOrder.aspx");
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected string FormatStatus(object 状況, object 回答状況)
        {
            string status = 状況 != null ? 状況.ToString().Replace("\r\n", "<br />") : "";
            string response = (回答状況 != null && !string.IsNullOrWhiteSpace(回答状況.ToString()))
                ? $"<b style='color:red;'>{回答状況.ToString().Replace("\r\n", "<br />")}</b>"
                : "";

            return status + response;
        }

        protected void BtnClearCondition_Click(object sender, EventArgs e)
        {
            // 入力項目のクリア
            CondInputDate_s.Text = "";
            CondInputDate_e.Text = "";
            CondlDestCompName.Text = "";
        }

        protected void BtnMainForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainForm.aspx");
        }
    }
}