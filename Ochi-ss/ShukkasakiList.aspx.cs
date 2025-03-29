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

namespace Ochi_ss
{
    public partial class ShukkasakiList : System.Web.UI.Page
    {
        public event EventHandler<System.Web.UI.WebControls.ListViewCommandEventArgs> ItemCommand;

        protected void Page_Load(object sender, EventArgs e)
        {

            GeneralCls gCls;
            gCls = new GeneralCls();

            // 数値、通貨スタイルの設定
            NumberStyles style;
            CultureInfo culture;

            style = NumberStyles.Number                 // 先頭空白、最後空白、先頭符号、後続符号、小数点、3桁区切りカンマ(Numberでこれらに対応)
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
            string destCompanyName = (string)CondlDestCompName.Text;
            string destCompanyArea = (string)CondlDestCompArea.Text;

            using (SqlConnection con = new SqlConnection(constr))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.AddSqlParam(new SqlParameter("@TokuisakiCd", SqlDbType.VarChar, 6), ParameterDirection.Input).Value = custCode;
                sqlParams.AddSqlParam(new SqlParameter("@TyokuName", SqlDbType.VarChar, 50), ParameterDirection.Input).Value = destCompanyName;
                sqlParams.AddSqlParam(new SqlParameter("@TyokuArea", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = destCompanyArea;

                // レコード取得
                var dt = gCls.GetDataTable(con, "usp_ASP_ShukkasakiList_get", sqlParams);

                DestCompList.DataSource = dt;
                DestCompList.DataBind();
                con.Close();
            }
        }

        protected void DestCompList_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            lblResult.Text = e.AffectedRows.ToString() + " row(s) successfully updated";
        }

        protected void DestCompNameList_PagePropertiesChanged(object sender, EventArgs e)
        {
            //Set the text to empty when navigating to a different page
            lblResult.Text = "";
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            // 数値、通貨スタイルの設定
            NumberStyles style;
            CultureInfo culture;

            style = NumberStyles.Number                 // 先頭空白、最後空白、先頭符号、後続符号、小数点、3桁区切りカンマ(Numberでこれらに対応)
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
            string destCompanyName = (string)CondlDestCompName.Text;
            string destCompanyArea = (string)CondlDestCompArea.Text;

            using (SqlConnection con = new SqlConnection(constr))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.AddSqlParam(new SqlParameter("@TokuisakiCd", SqlDbType.VarChar, 6), ParameterDirection.Input).Value = custCode;
                sqlParams.AddSqlParam(new SqlParameter("@TyokuName", SqlDbType.VarChar, 50), ParameterDirection.Input).Value = destCompanyName;
                sqlParams.AddSqlParam(new SqlParameter("@TyokuArea", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = destCompanyArea;

                // レコード取得
                var dt = gCls.GetDataTable(con, "usp_ASP_ShukkasakiList_get", sqlParams);

                DestCompList.DataSource = dt;
                DestCompList.DataBind();
                con.Close();
            }
        }

        protected void BtnClearCondition_Click(object sender, EventArgs e)
        {
            // 入力項目のクリア
            CondlDestCompName.Text = "";
            CondlDestCompArea.Text = "";
        }
        protected void BtnClose_Click(object sender, EventArgs e)
        {
            // 親フォームURLを取得
            string referrerUrl = Request.UrlReferrer?.ToString();

            // 親フォームがEstOrder.aspxの場合
            if (!string.IsNullOrEmpty(referrerUrl) && referrerUrl.Contains("EstOrder.aspx"))
            {
                Response.Redirect("EstOrder.aspx");
            }
            // 親フォームがMainForm.aspxの場合（何もしない場合でもコメントで明記）
            else if (!string.IsNullOrEmpty(referrerUrl) && referrerUrl.Contains("MainForm.aspx"))
            {
                Response.Redirect("MainForm.aspx");
            }
            // 親フォームがMainForm.aspxの場合（何もしない場合でもコメントで明記）
            else
            {
                Response.Redirect("MainForm.aspx");
            }

        }

        protected void DestCompList_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            if (String.Equals(e.CommandName, "SelectShukkasaki"))
            {
                if (e.CommandArgument != null)
                {
                    // 出荷先Cdの取得
                    string distCompCd = e.CommandArgument.ToString();

                    // DB接続文字列
                    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                    string customerCd = "";
                    string distCompName = "";
                    string distCompPost = "";
                    string distCharge = "";
                    string distCompZipCd = "";
                    string distCompAddress = "";
                    string distTel = "";
                    string distFax = "";

                    if (Session["CustomerCd"] != null)
                    {
                        customerCd = (string)Session["CustomerCd"];

                        // dubug
                        lblDubugText1.Text = customerCd;
                    }

                    // 見積発注データの取得(見積No指定の場合)
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_ASP_DistCompanyInfo_get"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // SQLパラメータ設定
                            List<SqlParameter> sqlParameters = new List<SqlParameter>();
                            sqlParameters.AddSqlParam(new SqlParameter("@CustomerCd", SqlDbType.VarChar), ParameterDirection.Input).Value = customerCd;
                            sqlParameters.AddSqlParam(new SqlParameter("@DistCompCd", SqlDbType.VarChar), ParameterDirection.Input).Value = distCompCd;
                            cmd.Parameters.AddParams(sqlParameters);
                            cmd.Connection = con;
                            con.Open();

                            // ヘッダ項目の各値を取得
                            using (var sdr = cmd.ExecuteReader())
                            {
                                if (sdr.HasRows)
                                {
                                    while (sdr.Read())
                                    {
                                        distCompCd = sdr["出荷先コード"] != DBNull.Value ? sdr["出荷先コード"].ToString() : string.Empty;
                                        distCompName = sdr["出荷先名"] != DBNull.Value ? sdr["出荷先名"].ToString() : string.Empty;
                                        distCompPost = sdr["出荷先部署名"] != DBNull.Value ? sdr["出荷先部署名"].ToString() : string.Empty;
                                        distCharge = sdr["出荷先担当者名"] != DBNull.Value ? sdr["出荷先担当者名"].ToString() : string.Empty;
                                        distCompZipCd = sdr["出荷先郵便番号"] != DBNull.Value ? sdr["出荷先郵便番号"].ToString() : string.Empty;
                                        distCompAddress = sdr["出荷先住所１"] != DBNull.Value ? sdr["出荷先住所１"].ToString() : string.Empty;
                                        distTel = sdr["出荷先電話番号"] != DBNull.Value ? sdr["出荷先電話番号"].ToString() : string.Empty;
                                        distFax = sdr["出荷先FAX番号"] != DBNull.Value ? sdr["出荷先FAX番号"].ToString() : string.Empty;
                                    }
                                }
                            }
                        }
                    }

                    string scriptStr = @"
    if (window.opener) {{
        var parentForm = window.opener.document.forms[0]; // 親フォームを取得
        if (parentForm) {{
            parentForm['ctl00$ContentPlaceHolder1$DistCompCd'].value = '{0}';
            parentForm['ctl00$ContentPlaceHolder1$DistCompName'].value = '{1}';
            parentForm['ctl00$ContentPlaceHolder1$DistCompPost'].value = '{2}';
            parentForm['ctl00$ContentPlaceHolder1$DistCharge'].value = '{3}';
            parentForm['ctl00$ContentPlaceHolder1$DistCompZipCd'].value = '{4}';
            parentForm['ctl00$ContentPlaceHolder1$DistCompAddress'].value = '{5}';
            parentForm['ctl00$ContentPlaceHolder1$DistTel'].value = '{6}';
            parentForm['ctl00$ContentPlaceHolder1$DistFax'].value = '{7}';
        }}
        window.close(); // ポップアップを閉じる
    }} else {{
        alert('親フォームが見つかりませんでした');
    }}
";

                    // コントロールに渡す値をエスケープして埋め込む
                    scriptStr = string.Format(scriptStr,
                        distCompCd?.Replace("'", "\\'") ?? "",       // 出荷先コード（シングルクォートのエスケープ処理）
                        distCompName?.Replace("'", "\\'") ?? "",     // 出荷先名
                        distCompPost?.Replace("'", "\\'") ?? "",     // 出荷先部署名
                        distCharge?.Replace("'", "\\'") ?? "",       // 出荷先担当者名
                        distCompZipCd?.Replace("'", "\\'") ?? "",    // 出荷先郵便番号
                        distCompAddress?.Replace("'", "\\'") ?? "",  // 出荷先住所
                        distTel?.Replace("'", "\\'") ?? "",          // 出荷先電話番号
                        distFax?.Replace("'", "\\'") ?? ""           // 出荷先FAX番号
                    );

                    // JavaScriptの登録
                    Type cstype = this.GetType();
                    ClientScriptManager cs = Page.ClientScript;
                    cs.RegisterStartupScript(cstype, "SetDistCompValues", scriptStr, true);


                }
            }
        }
    }
}