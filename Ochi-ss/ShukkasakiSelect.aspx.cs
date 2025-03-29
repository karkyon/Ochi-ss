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
    public partial class ShukkasakiSelect : System.Web.UI.Page
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

        protected void Search_Click(object sender, EventArgs e)
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
                                        distCompCd = sdr["出荷先コード"].ToString();
                                        distCompName = sdr["出荷先名"].ToString();
                                        distCompPost = sdr["出荷先部署名"].ToString();
                                        distCharge = sdr["出荷先担当者名"].ToString();
                                        distCompZipCd = sdr["出荷先郵便番号"].ToString();
                                        distCompAddress = sdr["出荷先住所１"].ToString();
                                        distTel = sdr["出荷先電話番号"].ToString();
                                        distFax = sdr["出荷先FAX番号"].ToString();
                                    }
                                }
                            }
                        }
                    }

                    string scriptStr;
                    scriptStr = "";
                    // Debug
                    scriptStr += "alert('出荷先コード :" + distCompCd + "');";
                    scriptStr += "alert('出荷先名: " + distCompName + "');";
                    scriptStr += "alert('出荷先部署名: " + distCompPost + "');";
                    scriptStr += "alert('出荷先担当者名: " + distCharge + "');";
                    scriptStr += "alert('出荷先郵便番号: " + distCompZipCd + "');";
                    scriptStr += "alert('出荷先住所１: " + distCompAddress + "');";
                    scriptStr += "alert('出荷先電話番号: " + distTel + "');";
                    scriptStr += "alert('出荷先FAX番号: " + distFax + "');";
                    // 出荷先情報の呼び出し
                    scriptStr = "";
                    scriptStr += "window.opener.form1.ContentPlaceHolder1_DistCompCd.value = '" + distCompCd + "';";
                    scriptStr += "window.opener.form1.ContentPlaceHolder1_DistCompName.value = '" + distCompName + "';";
                    scriptStr += "window.opener.form1.ContentPlaceHolder1_DistCompPost.value = '" + distCompPost + "';";
                    scriptStr += "window.opener.form1.ContentPlaceHolder1_DistCharge.value = '" + distCharge + "';";
                    scriptStr += "window.opener.form1.ContentPlaceHolder1_DistCompZipCd.value = '" + distCompZipCd + "';";
                    scriptStr += "window.opener.form1.ContentPlaceHolder1_DistCompAddress.value = '" + distCompAddress + "';";
                    scriptStr += "window.opener.form1.ContentPlaceHolder1_DistTel.value = '" + distTel + "';";
                    scriptStr += "window.opener.form1.ContentPlaceHolder1_DistFax.value = '" + distFax + "';";
                    scriptStr += "window.close();";

                    Type cstype = this.GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    cs.RegisterStartupScript(cstype, "key", scriptStr, true);
                }
            }
        }
    }
}