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
    public partial class Shukkasaki : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Collections.Specialized.NameValueCollection postedValues = Request.Form;

            if (!IsPostBack)
            { 
                GeneralCls gCls;
                gCls = new GeneralCls();

                // DB接続文字列
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                // ログインユーザ情報(顧客コード、出荷先コード、顧客名、担当者名)表示
                string customerCd = "";
                string shukkasakiCd = "";
                string customerName = "";
                string userName = "";

                if (Session["CustomerCd"] != null)
                {
                    customerCd = (string)Session["CustomerCd"];
                    CstCode.Text = customerCd;

                    // dubug
                    lblDubugText1.Text = customerCd;
                }
                if (Session["shukkasakiCd"] != null)
                {
                    shukkasakiCd = (string)Session["ShukkasakiCd"];
                    DistCompCd.Text = shukkasakiCd;

                    // dubug
                    lblDubugText1.Text = customerCd;
                }
                if (Session["CustomerName"] != null)
                {
                    customerName = (string)Session["CustomerName"];
                    CustomerName.Text = customerName;

                    // dubug
                    lblDubugText2.Text = customerName;
                }

                // SessionIdの取得
                string currSessionID = Session.SessionID;

                // 出荷先データの取得(出荷先cd指定の場合)
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ASP_WebDistCompanyInfo_get"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // SQLパラメータ設定
                        List<SqlParameter> sqlParameters = new List<SqlParameter>();
                        sqlParameters.AddSqlParam(new SqlParameter("@CustomerCd", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = customerCd;
                        sqlParameters.AddSqlParam(new SqlParameter("@DistCompCd", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = shukkasakiCd;
                        cmd.Parameters.AddParams(sqlParameters);
                        cmd.Connection = con;
                        con.Open();

                        // ヘッダ項目の各値を取得
                        using (var sdr = cmd.ExecuteReader())
                        {
                            string distCompCd = "";
                            string distCompName = "";
                            string distCompPost = "";
                            string distCharge = "";
                            string distCompZipCd = "";
                            string distCompAddress = "";
                            string distTel = "";
                            string distFax = "";
                            string shippingMethodId = "1";

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
                                    shippingMethodId = sdr["配送区分"].ToString();
                                }
                            }

                            if (distCompCd != "")
                            {
                                // ヘッダ項目の各値を表示
                                DistCompCd.Text = distCompCd;
                                DistCompName.Text = distCompName;
                                DistCompPost.Text = distCompPost;
                                DistCharge.Text = distCharge;
                                DistCompZipCd.Text = distCompZipCd;
                                DistCompAddress.Text = distCompAddress;
                                DistTel.Text = distTel;
                                DistFax.Text = distFax;
                            }
                            else
                            {
                                // ヘッダ項目の各値をクリア
                                DistCompCd.Text = "";
                                DistCompName.Text = "";
                                DistCompPost.Text = "";
                                DistCharge.Text = "";
                                DistCompZipCd.Text = "";
                                DistCompAddress.Text = "";
                                DistTel.Text = "";
                                DistFax.Text = "";
                            }

                        }

                    }

                    con.Close();
                }

                // DistCompNameにFocus
                DistCompName.Focus();
            }
            BtnUpdate.Attributes["onclick"] = "return confirm('入力された出荷先情報を登録します。よろしいですか？');";
            BtnDelete.Attributes["onclick"] = "return confirm('この出荷先情報を削除します。よろしいですか？');";
        }

        protected void BtnMainForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainForm.aspx");
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShukkasakiList.aspx");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
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


            // 各入力値を取得
            string customerCd = (string)CstCode.Text;
            string shukkasakiCd = (string)DistCompCd.Text;

            // DB接続文字列取得
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.AddSqlParam(new SqlParameter("@CustomerCd", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = customerCd;
                sqlParams.AddSqlParam(new SqlParameter("@DistCompCd", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = shukkasakiCd;
                sqlParams.AddSqlParam(new SqlParameter("@result", SqlDbType.Int), ParameterDirection.Output).Value = 0;

                var cmd = new SqlCommand("usp_ASP_WebDistCompanyInfo_del", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddParams(sqlParams);

                var result = new DataTable();
                var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(result);

                int res = 0;
                bool ex = int.TryParse(cmd.Parameters["@result"].Value.ToString(), out res);

                con.Close();

                if (res == 1) { 

                    // 入力項目のクリア
                    DistCompCd.Text = "";
                    DistCompName.Text = "";
                    DistCompPost.Text = "";
                    DistCharge.Text = "";
                    DistCompZipCd.Text = "";
                    DistCompAddress.Text = "";
                    DistTel.Text = "";
                    DistFax.Text = "";

                    // 削除完了のメッセージ
                    lblDubugText1.Text = "削除完了しました。";
                    Information.Text = "削除完了しました。 ";
                }
                else
                {
                    // 削除失敗のメッセージ
                    lblDubugText1.Text = "何らかの原因で、削除できませんでした。";
                    Information.Text = "何らかの原因で、削除できませんでした。";
                }
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
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

            // 各入力値を取得
            string cstCode = (string)CstCode.Text;
            string cstName = (string)CustomerName.Text;
            string distCompCd = (string)DistCompCd.Text;
            string distCompName = (string)DistCompName.Text;
            string distCompPost = (string)DistCompPost.Text;
            string distCharge = (string)DistCharge.Text;
            string distCompZipCd = (string)DistCompZipCd.Text;
            string distCompAddress = (string)DistCompAddress.Text;
            string distTel = (string)DistTel.Text;
            string distFax = (string)DistFax.Text;

            // DB接続文字列取得
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.AddSqlParam(new SqlParameter("@CstCode", SqlDbType.VarChar, 6), ParameterDirection.Input).Value = cstCode;
                sqlParams.AddSqlParam(new SqlParameter("@CstName", SqlDbType.VarChar, 50), ParameterDirection.Input).Value = cstName;
                sqlParams.AddSqlParam(new SqlParameter("@DistCompCd", SqlDbType.VarChar, 6), ParameterDirection.Input).Value = distCompCd;
                sqlParams.AddSqlParam(new SqlParameter("@DistCompName", SqlDbType.VarChar, 80), ParameterDirection.Input).Value = distCompName;
                sqlParams.AddSqlParam(new SqlParameter("@DistCompPost", SqlDbType.VarChar, 50), ParameterDirection.Input).Value = distCompPost;
                sqlParams.AddSqlParam(new SqlParameter("@DistCharge", SqlDbType.VarChar, 20), ParameterDirection.Input).Value = distCharge;
                sqlParams.AddSqlParam(new SqlParameter("@DistCompZipCd", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = distCompZipCd;
                sqlParams.AddSqlParam(new SqlParameter("@DistCompAddress", SqlDbType.VarChar, 255), ParameterDirection.Input).Value = distCompAddress;
                sqlParams.AddSqlParam(new SqlParameter("@DistTel", SqlDbType.VarChar, 15), ParameterDirection.Input).Value = distTel;
                sqlParams.AddSqlParam(new SqlParameter("@DistFax", SqlDbType.VarChar, 15), ParameterDirection.Input).Value = distFax;
                sqlParams.AddSqlParam(new SqlParameter("@NewEstimateNo", SqlDbType.VarChar, 10), ParameterDirection.Output).Value = "";

                // レコード取得
                var dt = gCls.GetDataTable(con, "usp_ASP_WebDistCompanyInfo_upd", sqlParams);

                con.Close();

                // 入力項目のクリア
                DistCompCd.Text = "";
                DistCompName.Text = "";
                DistCompPost.Text = "";
                DistCharge.Text = "";
                DistCompZipCd.Text = "";
                DistCompAddress.Text = "";
                DistTel.Text = "";
                DistFax.Text = "";

                // 登録完了のメッセージ
                lblDubugText1.Text = "登録完了しました。";
                Information.Text = "登録完了しました。";
            }
        }

        protected void BtnShukkasakiList_Click(object sender, EventArgs e)
        {
            string url = "ShukkasakiSelect.aspx";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenNewWindow", "window.open('" + url + "', null, height=200, wieth=200);", true);
        }
    }
}