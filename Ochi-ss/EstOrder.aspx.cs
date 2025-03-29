using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Web.Configuration;
using AjaxControlToolkit;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.WebForms;
using WebGrease.Activities;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using System.Net.NetworkInformation;
using System.IO;
using System.Text;

namespace Ochi_ss
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public class EstimateDetail
        {
            public decimal UnitPrice { get; set; } // 単価
            public decimal SumPrice { get; set; }  // 金額
        }

        private List<EstimateDetail> GetEstimateDetails()
        {
            List<EstimateDetail> details = new List<EstimateDetail>();

            // ListView1 の各行をループ処理
            foreach (ListViewDataItem item in ListView1.Items)
            {
                // 単価ラベルを取得
                Label lblUnitPrice = item.FindControl("lblUnitPrice") as Label;
                // 金額ラベルを取得
                Label lblPrice = item.FindControl("lblPrice") as Label;

                if (lblUnitPrice != null && lblPrice != null)
                {
                    // ラベルの値を decimal に変換（必要なら例外処理も追加）
                    decimal unitPrice = 0;
                    decimal sumPrice = 0;

                    if (decimal.TryParse(lblUnitPrice.Text.Replace(",", ""), out unitPrice) &&
                        decimal.TryParse(lblPrice.Text.Replace(",", ""), out sumPrice))
                    {
                        // Detail オブジェクトを作成してリストに追加
                        details.Add(new EstimateDetail
                        {
                            UnitPrice = unitPrice,
                            SumPrice = sumPrice
                        });
                    }
                }
            }

            return details;
        }

        private string GetSessionValue(string key)
        {
            return Session[key]?.ToString() ?? string.Empty;
        }

        private void SetControlValueFromSession(string sessionKey, Control control)
        {
            string value = GetSessionValue(sessionKey);
            if (!string.IsNullOrEmpty(value))
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = value;
                }
                else if (control is Label label)
                {
                    label.Text = value;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Collections.Specialized.NameValueCollection postedValues = Request.Form;

            if (!IsPostBack)
            {
                GeneralCls gCls;
                gCls = new GeneralCls();

                // 初期化: 見積完了フラグをfalseに設定
                ViewState["IsEstimated"] = false;
                ViewState["IsEstimatedAll"] = false;

                // ボタンの初期状態を設定
                BtnAddRecord.Enabled = false; // デフォルトで非活性

                // DB接続文字列
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                // セッションデータをコントロールにセット
                SetControlValueFromSession("SessionID", SessionID);
                SetControlValueFromSession("CustomerCd", CstCode);
                SetControlValueFromSession("CustomerName", CustomerName);
                SetControlValueFromSession("UserName", CustomreCharge);
                SetControlValueFromSession("ChargeName", CustomreCharge);

                // デバッグラベル設定 (本番では削除)
                lblDubugText1.Text = "CustomerCd :" + GetSessionValue("CustomerCd");
                lblDubugText2.Text = "CustomerName :" + GetSessionValue("CustomerName");
                lblDubugText3.Text = "UserName :" + GetSessionValue("UserName");
                lblDubugText4.Text = "ChargeName :" + GetSessionValue("ChargeName");
                lblDubugText5.Text = "EstOrderNo :" + GetSessionValue("EstOrderNo");

                // 得意先コードの取得
                string customerCd = GetSessionValue("CustomerCd");

                // SessionIdの取得
                string currSessionID = Session.SessionID;

                // 編集モードの取得
                string editMode = "";
                if (Session["EditMode"] != null)
                {
                    editMode = GetSessionValue("EditMode");
                }
                else
                {
                    editMode = "New";
                }

                // 見積Noの取得
                string estOrderNo = "";
                if (Session["EstOrderNo"] != null)
                {
                    estOrderNo = GetSessionValue("EstOrderNo");
                }

                // 見積・入力日付の初期値
                InpDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                EstDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

                // 見積発注データの取得(見積No指定の場合)
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ASP_EstimateHeader_get"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // SQLパラメータ設定
                        List<SqlParameter> sqlParameters = new List<SqlParameter>();
                        sqlParameters.AddSqlParam(new SqlParameter("@SessionID", SqlDbType.VarChar), ParameterDirection.Input).Value = (string)Session.SessionID;
                        sqlParameters.AddSqlParam(new SqlParameter("@WOEstimateNo", SqlDbType.VarChar), ParameterDirection.Input).Value = estOrderNo;
                        cmd.Parameters.AddParams(sqlParameters);
                        cmd.Connection = con;
                        con.Open();

                        // ヘッダ項目の各値を取得
                        using (var sdr = cmd.ExecuteReader())
                        {
                            string estimateNo = "";
                            string orderNo = "";
                            string inpDate = "";
                            string estDate = "";
                            string cstCode = "";
                            string cstName = "";
                            string customreCharge = "";
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
                                    estimateNo = sdr["WO見積No"].ToString();
                                    orderNo = sdr["見積管理番号"].ToString();
                                    inpDate = sdr["登録日付"].ToString();
                                    estDate = sdr["見積日付"].ToString();
                                    cstCode = sdr["得意先コード"].ToString();
                                    cstName = sdr["得意先名"].ToString();
                                    customreCharge = sdr["得意先担当者名"].ToString();
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

                            // ヘッダ項目の各値を表示
                            switch (editMode)
                            {
                                case "Edit":
                                    hidEstOrderNo.Value = estOrderNo;
                                    EstOrderNo.Text = estimateNo;
                                    OrderNo.Text = orderNo;
                                    InpDate.Text = inpDate;
                                    EstDate.Text = estDate;
                                    break;

                                case "New":
                                case "Copy":
                                    hidEstOrderNo.Value = string.Empty;
                                    EstOrderNo.Text = string.Empty;
                                    OrderNo.Text = string.Empty;
                                    InpDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                                    EstDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

                                    // 見積Noの初期化（複写元のデータを呼出し後はEstOrderNoはクリア）
                                    Session["EstOrderNo"] = string.Empty;

                                    break;
                                default:
                                    // 予期しない値が来た場合のエラーハンドリング
                                    DebugLog("editMode: " + editMode);
                                    throw new ArgumentException($"無効な estOrderNo: {estOrderNo}");
                            }

                            // CstCode.Text = cstCode;
                            // CustomerName.Text = cstName;
                            // CustomreCharge.Text = customreCharge;
                            DistCompCd.Text = distCompCd;
                            DistCompName.Text = distCompName;
                            DistCompPost.Text = distCompPost;
                            DistCharge.Text = distCharge;
                            DistCompZipCd.Text = distCompZipCd;
                            DistCompAddress.Text = distCompAddress;
                            DistTel.Text = distTel;
                            DistFax.Text = distFax;
                            ShippingMethod.SelectedValue = shippingMethodId;

                            // 見積金額と納期のクリア
                            Nouki_S.Text = string.Empty;
                            Price_U.Text = string.Empty;
                            Price_S.Text = string.Empty;

                            // 直送先Section表示の制御
                            if (shippingMethodId == "1")
                            {
                                DivDistination.Visible = true;
                            }
                            else
                            {
                                DivDistination.Visible = false;
                            }
                        }

                    }

                    // 明細項目を取得
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("SessionID", currSessionID),
                        new SqlParameter("WOEstimateNo", estOrderNo),
                        new SqlParameter("EditMode", editMode)
                    };
                    var dt = gCls.GetDataTable(con, "usp_ASP_EstimateDetailWork_get", parameters);

                    ListView1.DataSource = dt;
                    ListView1.DataBind();
                    con.Close();
                }

                // ①素材リストのデータバインド
                MaterialList(customerCd);

                // ②仕上りリストのデータバインド
                FinishingMethodList(customerCd);

                // ③加工仕様リストのデータバインド
                CuttingMethodList(customerCd);

                // 合計金額を表示
                DisplayTotalAmount();

                // 全明細見積金額算出済みかどうかチェックし、見積書発行ボタンの活性・非活性を変更
                CheckIfAllEstimated();

                // OrderNoにFocus
                OrderNo.Focus();
            }
            // 見積書発行
            else if (IsPostBack && Request["action"] == "generatePDF")
            {
                string estimateNo = Request["estimateNo"]; // 見積Noを取得
                GenerateQuotationPDF(Session.SessionID, estimateNo, Response);
            }
            BtnUpdate.Attributes["onclick"] = "return confirm('登録します。よろしいですか？');";

        }

        private decimal CalculateTotalAmount(string controlName)
        {
            decimal total = 0;

            foreach (var item in ListView1.Items)
            {
                var label = item.FindControl(controlName) as Label;
                if (label != null && decimal.TryParse(label.Text, out var value))
                {
                    total += value;
                }
            }
            return total;
        }

        private void DisplayTotalAmount()
        {
            // 合計金額を計算する
            decimal totalAmount = CalculateTotalAmount("lblPrice");

            // LayoutTemplate内のLabelをFindControlで取得
            Label lblTotalAmount = (Label)ListView1.FindControl("lbl合計金額");

            if (lblTotalAmount != null)
            {
                // カンマ区切りで合計金額を表示
                lblTotalAmount.Text = totalAmount.ToString("N0");
            }
        }

        int estSummary = 0;

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)lvdi.DataItem;

                // 金額の合計
                if (drv.Row[25] != DBNull.Value)
                {
                    int val;
                    bool res = int.TryParse(drv.Row[25].ToString(), out val);
                    estSummary = estSummary + val;
                }
            }
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            Label label = (Label)ListView1.FindControl("lbl合計金額");
            if (label != null)
            {
                label.Text = String.Format("{0,8:C}", estSummary);
            }
        }

        private void MaterialList(string CostomerCd)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("CostomerCd", CostomerCd)
                };
                var dt = gCls.GetDataTable(con, "usp_ASP_MaterialList_get", parameters);

                // 表示項目と値のセット
                Material.DataTextField = "材料名";
                Material.DataValueField = "材料コード";

                // 先頭に空行を追加
                DataRow newRow = dt.NewRow();
                newRow["材料名"] = "";
                newRow["材料コード"] = "";
                dt.Rows.InsertAt(newRow, 0);

                // リストボックスに取得データを設定
                Material.DataSource = dt;
                Material.DataBind();
                con.Close();
            }
        }

        private void CuttingMethodList(string CostomerCd)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("CostomerCd", CostomerCd)
                };
                var dt = gCls.GetDataTable(con, "usp_ASP_CuttingMethod_get", parameters);

                // 表示項目と値のセット
                Kakou_T.DataTextField = "加工指示表示";
                Kakou_T.DataValueField = "加工指示コード";
                Kakou_A.DataTextField = "加工指示表示";
                Kakou_A.DataValueField = "加工指示コード";
                Kakou_B.DataTextField = "加工指示表示";
                Kakou_B.DataValueField = "加工指示コード";

                // 先頭に空行を追加
                DataRow newRow = dt.NewRow();
                newRow["加工指示表示"] = "";
                newRow["加工指示コード"] = DBNull.Value;
                dt.Rows.InsertAt(newRow, 0);

                // リストボックスに取得データを設定
                Kakou_T.DataSource = dt;
                Kakou_T.DataBind();
                Kakou_A.DataSource = dt;
                Kakou_A.DataBind();
                Kakou_B.DataSource = dt;
                Kakou_B.DataBind();
                con.Close();
            }
        }

        private void FinishingMethodList(string CostomerCd)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("CostomerCd", CostomerCd)
                };
                var dt = gCls.GetDataTable(con, "usp_ASP_FinishingMethod_get", parameters);

                // 表示項目と値のセット
                Kakou.DataTextField = "加工仕様";
                Kakou.DataValueField = "加工仕様コード";

                // 先頭に空行を追加
                DataRow newRow = dt.NewRow();
                newRow["加工仕様"] = "";
                newRow["加工仕様コード"] = DBNull.Value;
                dt.Rows.InsertAt(newRow, 0);

                // リストボックスに取得データを設定
                Kakou.DataSource = dt;
                Kakou.DataBind();
                con.Close();
            }
        }

        protected void BtnMainForm_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainForm.aspx");
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("EstOrderSearch.aspx");
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

            // 見積データの削除処理
            try
            {
                // 削除対象のデータのIDを取得
                string estOrderNo = (string)EstOrderNo.Text;

                // DB接続文字列取得
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                    sqlParams.AddSqlParam(new SqlParameter("@SessionID", SqlDbType.VarChar, 30), ParameterDirection.Input).Value = (string)Session.SessionID;
                    sqlParams.AddSqlParam(new SqlParameter("@WOEstimateNo", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = estOrderNo;
                    // レコード取得
                    var dt = gCls.GetDataTable(con, "usp_ASP_Estimate_del", sqlParams);

                    ListView1.DataSource = dt;
                    ListView1.DataBind();
                    con.Close();

                    // 入力項目のクリア
                    Material.SelectedValue = null;
                    Kakou.SelectedValue = null;
                    Kakou_T.SelectedValue = null;
                    Kakou_B.SelectedValue = null;
                    Kakou_A.SelectedValue = null;
                    Size_T.Text = string.Empty;
                    Size_A.Text = string.Empty;
                    Size_B.Text = string.Empty;
                    Kousa_T_U.Text = string.Empty;
                    Kousa_A_U.Text = string.Empty;
                    Kousa_B_U.Text = string.Empty;
                    Kousa_T_L.Text = string.Empty;
                    Kousa_A_L.Text = string.Empty;
                    Kousa_B_L.Text = string.Empty;
                    Mentori_4.Text = string.Empty;
                    Mentori_8.Text = string.Empty;
                    Suryou.Text = string.Empty;
                    CustomerNo.Text = string.Empty;
                    EndUserNo.Text = string.Empty;
                    Refer.Text = string.Empty;

                    // 見積金額と納期のクリア
                    Nouki_S.Text = string.Empty;
                    Price_U.Text = string.Empty;
                    Price_S.Text = string.Empty;
                    Nouki_S.Text = "";
                    Price_U.Text = "0";
                    Price_S.Text = "0";

                    // ヘッダ項目の各値をクリア
                    hidEstOrderNo.Value = "";
                    EstOrderNo.Text = string.Empty;
                    OrderNo.Text = string.Empty;
                    InpDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    EstDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    CustomreCharge.Text = string.Empty;
                    DistCompCd.Text = string.Empty;
                    DistCompName.Text = string.Empty;
                    DistCompPost.Text = string.Empty;
                    DistCharge.Text = string.Empty;
                    DistCompZipCd.Text = string.Empty;
                    DistCompAddress.Text = string.Empty;
                    DistTel.Text = string.Empty;
                    DistFax.Text = string.Empty;
                    ShippingMethod.SelectedValue = "1";

                    // 直送先Section表示の制御
                    DivDistination.Visible = true;

                    // 登録完了のメッセージ
                    lblDubugText1.Text = "登録完了しました。";
                }
            }
            catch (Exception ex)
            {
                // エラーメッセージを表示
                ClientScript.RegisterStartupScript(this.GetType(), "Error",
                    $"alert('エラーが発生しました: {ex.Message}');", true);
            }
        }

        protected void BtnNew_Click(object sender, EventArgs e)
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

            // 見積データの削除処理
            try
            {
                // DB接続文字列取得
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                    sqlParams.AddSqlParam(new SqlParameter("@SessionID", SqlDbType.VarChar, 30), ParameterDirection.Input).Value = (string)Session.SessionID;
                    // レコード取得
                    var dt = gCls.GetDataTable(con, "usp_ASP_EstimateNewRec_get", sqlParams);

                    ListView1.DataSource = dt;
                    ListView1.DataBind();
                    con.Close();

                    // 入力項目のクリア
                    Material.SelectedValue = null;
                    Kakou.SelectedValue = null;
                    Kakou_T.SelectedValue = string.Empty;
                    Kakou_B.SelectedValue = string.Empty;
                    Kakou_A.SelectedValue = string.Empty;
                    Size_T.Text = string.Empty;
                    Size_A.Text = string.Empty;
                    Size_B.Text = string.Empty;
                    Kousa_T_U.Text = string.Empty;
                    Kousa_A_U.Text = string.Empty;
                    Kousa_B_U.Text = string.Empty;
                    Kousa_T_L.Text = string.Empty;
                    Kousa_A_L.Text = string.Empty;
                    Kousa_B_L.Text = string.Empty;
                    Mentori_4.Text = string.Empty;
                    Mentori_8.Text = string.Empty;
                    Suryou.Text = string.Empty;
                    CustomerNo.Text = string.Empty;
                    EndUserNo.Text = string.Empty;
                    Refer.Text = string.Empty;

                    // 見積金額と納期のクリア
                    Nouki_S.Text = string.Empty;
                    Price_U.Text = string.Empty;
                    Price_S.Text = string.Empty;
                    Nouki_S.Text = "";
                    Price_U.Text = "0";
                    Price_S.Text = "0";

                    // 見積Noの初期化（複写元のデータを呼出し後はEstOrderNoはクリア）
                    Session["EstOrderNo"] = string.Empty;

                    // ヘッダ項目の各値をクリア
                    hidEstOrderNo.Value = string.Empty;
                    EstOrderNo.Text = string.Empty;
                    OrderNo.Text = string.Empty;
                    InpDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    EstDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    CustomreCharge.Text = string.Empty;
                    DistCompCd.Text = string.Empty;
                    DistCompName.Text = string.Empty;
                    DistCompPost.Text = string.Empty;
                    DistCharge.Text = string.Empty;
                    DistCompZipCd.Text = string.Empty;
                    DistCompAddress.Text = string.Empty;
                    DistTel.Text = string.Empty;
                    DistFax.Text = string.Empty;
                    ShippingMethod.SelectedValue = "1";

                    // 直送先Section表示の制御
                    DivDistination.Visible = true;

                    // セッションデータをコントロールにセット
                    SetControlValueFromSession("SessionID", SessionID);
                    SetControlValueFromSession("CustomerCd", CstCode);
                    SetControlValueFromSession("CustomerName", CustomerName);
                    SetControlValueFromSession("UserName", CustomreCharge);
                    SetControlValueFromSession("ChargeName", CustomreCharge);

                    // デバッグラベル設定 (本番では削除)
                    lblDubugText1.Text = "CustomerCd :" + GetSessionValue("CustomerCd");
                    lblDubugText2.Text = "CustomerName :" + GetSessionValue("CustomerName");
                    lblDubugText3.Text = "UserName :" + GetSessionValue("UserName");
                    lblDubugText4.Text = "ChargeName :" + GetSessionValue("ChargeName");
                    lblDubugText5.Text = "EstOrderNo :" + GetSessionValue("EstOrderNo");

                }
            }
            catch (Exception ex)
            {
                // エラーメッセージを表示
                ClientScript.RegisterStartupScript(this.GetType(), "Error",
                    $"alert('エラーが発生しました: {ex.Message}');", true);
            }
        }

        protected void BtnEstimate_Click(object sender, EventArgs e)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            bool res;
            string ShortestNouki = "";
            decimal UnitPrice = 0;
            decimal SumPrice = 0;

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

            // 入力検証
            bool isValid = gCls.ValidateControls(
                out List<string> errorMessages,
                (Material, "材質を選択してください。", value => !string.IsNullOrWhiteSpace(value)),
                (Kakou_T, "厚み面の加工内容を選択してください。", value => !string.IsNullOrWhiteSpace(value)),
                (Kakou_B, "巾面の加工内容を選択してください。", value => !string.IsNullOrWhiteSpace(value)),
                (Kakou_A, "長さ面の加工内容を選択してください。", value => !string.IsNullOrWhiteSpace(value)),
                (Size_T, "厚みの寸法は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Size_B, "巾面の寸法は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Size_A, "長さ面の寸法は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_T_U, "厚みの公差（上限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_B_U, "巾面の公差（上限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_A_U, "長さ面の公差（上限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_T_L, "厚みの公差（下限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_B_L, "巾面の公差（下限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_A_L, "長さ面の公差（下限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Suryou, "数量は必須項目です", value => !string.IsNullOrWhiteSpace(value))
            );

            // エラーがある場合
            if (!isValid)
            {
                // エラー処理
                lblErrorMessages.Text = string.Join("<br />", errorMessages);
                lblErrorMessages.ForeColor = System.Drawing.Color.Red;
                SetFocusToFirstError(new List<Control>
                {
                    Material,
                    Kakou_T,
                    Kakou_B,
                    Kakou_A,
                    Size_T,
                    Size_B,
                    Size_A,
                    Kousa_T_U,
                    Kousa_B_U,
                    Kousa_A_U,
                    Kousa_T_L,
                    Kousa_B_L,
                    Kousa_A_L,
                    Suryou
                });
                return;
            }

            // 入力が有効であれば処理を続行
            lblErrorMessages.Text = string.Empty; // エラーメッセージをクリア

            // 各入力値を取得
            int rowNo;
            res = int.TryParse(RowNo.Text, style, culture, out rowNo);
            string customerCd = (string)CstCode.Text;
            string customreCharge = (string)CustomreCharge.Text;
            string distCompCd = (string)DistCompCd.Text;
            string distCompName = (string)DistCompName.Text;
            string distCompPost = (string)DistCompPost.Text;
            string distCharge = (string)DistCharge.Text;
            string distCompZipCd = (string)DistCompZipCd.Text;
            string distCompAddress = (string)DistCompAddress.Text;
            string distTel = (string)DistTel.Text;
            string distFax = (string)DistFax.Text;
            string materialCd = (string)Material.SelectedValue;
            string materialName = (string)Material.SelectedItem.Text;
            string kakou = (string)Kakou.SelectedItem.Text;
            int kakouShiyouCd;
            res = int.TryParse(Kakou.SelectedValue, style, culture, out kakouShiyouCd);
            string kakouShiyou = (string)Kakou.SelectedItem.Text;
            string kakou_T = (string)Kakou_T.SelectedItem.Value;
            string kakou_B = (string)Kakou_B.SelectedItem.Value;
            string kakou_A = (string)Kakou_A.SelectedItem.Value;
            string kakouShiji_T = (string)Kakou_T.SelectedItem.Text;
            string kakouShiji_B = (string)Kakou_B.SelectedItem.Text;
            string kakouShiji_A = (string)Kakou_A.SelectedItem.Text;

            decimal.TryParse(Size_T.Text, style, culture, out decimal size_T);
            decimal.TryParse(Size_B.Text, style, culture, out decimal size_B);
            decimal.TryParse(Size_A.Text, style, culture, out decimal size_A);
            int.TryParse(Suryou.Text, out int suryou);

            decimal kousa_T_U;
            res = decimal.TryParse(Kousa_T_U.Text, style, culture, out kousa_T_U);
            decimal kousa_B_U;
            res = decimal.TryParse(Kousa_B_U.Text, style, culture, out kousa_B_U);
            decimal kousa_A_U;
            res = decimal.TryParse(Kousa_A_U.Text, style, culture, out kousa_A_U);
            decimal kousa_T_L;
            res = decimal.TryParse(Kousa_T_L.Text, style, culture, out kousa_T_L);
            decimal kousa_B_L;
            res = decimal.TryParse(Kousa_B_L.Text, style, culture, out kousa_B_L);
            decimal kousa_A_L;
            res = decimal.TryParse(Kousa_A_L.Text, style, culture, out kousa_A_L);

            string mentoriShiji = (string)MentoriShiji.SelectedItem.Value;
            decimal mentori_4;
            res = decimal.TryParse(Mentori_4.Text, style, culture, out mentori_4);
            decimal mentori_8;
            res = decimal.TryParse(Mentori_8.Text, style, culture, out mentori_8);

            string customerNo = (string)CustomerNo.Text;    // お客様注文番号
            string endUserNo = (string)EndUserNo.Text;      // 送り先様注文番号
            string refer = (string)Refer.Text;              // 備考

            // DB接続文字列取得
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            string estOrderNo = Session["EstOrderNo"] != null ? Convert.ToString(Session["EstOrderNo"]) : string.Empty;

            using (SqlConnection con = new SqlConnection(constr))
            {
                // ストアド名
                string storedProcedureName = "usp_ASP_EstimateAmountCalculation_get";

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // パラメータの追加
                    cmd.Parameters.AddWithValue("@SessionID", Session.SessionID);
                    cmd.Parameters.AddWithValue("@RowID", rowNo);
                    cmd.Parameters.AddWithValue("@WOEstimateNo", estOrderNo);
                    cmd.Parameters.AddWithValue("@ZairyouCd", materialCd);
                    cmd.Parameters.AddWithValue("@ZairyouName", materialName);
                    cmd.Parameters.AddWithValue("@KakouShiyouCd", kakouShiyouCd);
                    cmd.Parameters.AddWithValue("@KakouShiyou", kakouShiyou);
                    cmd.Parameters.AddWithValue("@Kakou_T", kakou_T);
                    cmd.Parameters.AddWithValue("@Kakou_A", kakou_A);
                    cmd.Parameters.AddWithValue("@Kakou_B", kakou_B);
                    cmd.Parameters.AddWithValue("@KakouShijiCd_T", kakouShiji_T);
                    cmd.Parameters.AddWithValue("@KakouShijiCd_A", kakouShiji_A);
                    cmd.Parameters.AddWithValue("@KakouShijiCd_B", kakouShiji_B);
                    cmd.Parameters.AddWithValue("@Size_T", size_T);
                    cmd.Parameters.AddWithValue("@Size_A", size_A);
                    cmd.Parameters.AddWithValue("@Size_B", size_B);
                    cmd.Parameters.AddWithValue("@Kousa_T_U", kousa_T_U);
                    cmd.Parameters.AddWithValue("@Kousa_A_U", kousa_A_U);
                    cmd.Parameters.AddWithValue("@Kousa_B_U", kousa_B_U);
                    cmd.Parameters.AddWithValue("@Kousa_T_L", kousa_T_L);
                    cmd.Parameters.AddWithValue("@Kousa_A_L", kousa_A_L);
                    cmd.Parameters.AddWithValue("@Kousa_B_L", kousa_B_L);
                    cmd.Parameters.AddWithValue("@MentoriShiji", mentoriShiji);
                    cmd.Parameters.AddWithValue("@Mentori_4", mentori_4);
                    cmd.Parameters.AddWithValue("@Mentori_8", mentori_8);
                    cmd.Parameters.AddWithValue("@Suryou", suryou);
                    cmd.Parameters.AddWithValue("@RequestPrice", 0);
                    cmd.Parameters.AddWithValue("@RequestNouki", "");
                    cmd.Parameters.AddWithValue("@CustomerNo", customerNo);
                    cmd.Parameters.AddWithValue("@EndUserNo", endUserNo);
                    cmd.Parameters.AddWithValue("@Refer", refer);
                    cmd.Parameters.AddWithValue("@TokuisakiCd", customerCd);
                    cmd.Parameters.AddWithValue("@EndUserCd", "");
                    cmd.Parameters.AddWithValue("@TyokusousakiCd", distCompCd);
                    cmd.Parameters.AddWithValue("@TyokusousakiName", distCompName);
                    cmd.Parameters.AddWithValue("@TyokusousakiZipCd", distCompZipCd);
                    cmd.Parameters.AddWithValue("@TyokusousakiAddr", distCompAddress);
                    cmd.Parameters.AddWithValue("@TyokusousakiPost", distCompPost);
                    cmd.Parameters.AddWithValue("@TyokusousakiCharge", distCharge);

                    // OUTPUT パラメータの追加
                    SqlParameter shortestNoukiParam = cmd.Parameters.Add("@ShortestNouki", SqlDbType.VarChar, 10);
                    shortestNoukiParam.Direction = ParameterDirection.Output;

                    SqlParameter unitPriceParam = cmd.Parameters.Add("@UnitPrice", SqlDbType.Money);
                    unitPriceParam.Direction = ParameterDirection.Output;

                    SqlParameter sumPriceParam = cmd.Parameters.Add("@SumPrice", SqlDbType.Money);
                    sumPriceParam.Direction = ParameterDirection.Output;

                    // 接続を開く
                    con.Open();

                    // cmd.Parameters をリスト化
                    var sqlParams = cmd.Parameters.Cast<SqlParameter>().ToList();

                    // デバッグクエリを生成
                    string debugQuery = GeneralCls.BuildDebugQuery(storedProcedureName, sqlParams);

                    // デバッグクエリを出力
                    DebugLog("Generated Debug Query:");
                    DebugLog(debugQuery);

                    try
                    {
                        // ストアドプロシージャの実行
                        cmd.ExecuteNonQuery();

                        // OUTPUT パラメータの値を取得
                        ShortestNouki = shortestNoukiParam.Value != DBNull.Value ? Convert.ToString(shortestNoukiParam.Value) : string.Empty;

                        if (string.IsNullOrWhiteSpace(ShortestNouki) || !DateTime.TryParse(ShortestNouki, out _))
                        {
                            // ShortestNouki がブランクまたは無効な場合
                            ShortestNouki = "納期回答待ち";
                        }
                        DebugLog("@ShortestNouki: " + ShortestNouki);


                        UnitPrice = unitPriceParam.Value != DBNull.Value ? Convert.ToDecimal(unitPriceParam.Value) : 0;
                        DebugLog("@UnitPrice: " + UnitPrice);

                        SumPrice = sumPriceParam.Value != DBNull.Value ? Convert.ToDecimal(sumPriceParam.Value) : 0;
                        DebugLog("@SumPrice: " + SumPrice);
                    }
                    catch (Exception ex)
                    {
                        // エラーログ出力
                        DebugLog("エラーが発生しました: " + ex.Message);
                        DebugLog("詳細: " + ex.StackTrace);
                    }
                }
            }

            // 見積金額と納期の表示
            Nouki_S.Text = ShortestNouki;
            Price_U.Text = UnitPrice > 0 ? UnitPrice.ToString("#,0") : string.Empty;
            Price_S.Text = SumPrice > 0 ? SumPrice.ToString("#,0") : string.Empty;

            // 見積完了フラグを設定（ViewState）
            ViewState["IsEstimated"] = true;

            // 追加ボタンを有効化しFocus
            BtnAddRecord.Enabled = true;
            BtnAddRecord.Focus();
        }
        // 各明細のUnitPriceとSumPriceが設定されているか確認
        protected void CheckIfAllEstimated()
        {
            // 例として、明細データをリストで管理していると仮定
            List<EstimateDetail> details = GetEstimateDetails(); // 明細データを取得するメソッド

            // フラグを初期化
            bool isAllEstimated = true;

            // 全明細を確認
            foreach (var detail in details)
            {
                // 各明細のUnitPriceとSumPriceが設定されているか確認
                if (detail.UnitPrice == 0 || detail.SumPrice == 0)
                {
                    isAllEstimated = false;
                    break;
                }
            }

            // ViewStateに保存
            ViewState["IsEstimatedAll"] = isAllEstimated;

            // 見積書発行ボタンの状態を更新
            BtnGenerateEstimateReport.Enabled = isAllEstimated;
        }

        protected void SetFocusToFirstError(IEnumerable<Control> controls)
        {
            foreach (var control in controls)
            {
                // コントロールの背景色がエラー色（LightCoral）の場合にフォーカスを移動
                if (control is WebControl webControl && webControl.BackColor == System.Drawing.Color.LightCoral)
                {
                    webControl.Focus();
                    return;
                }
            }
        }

        protected void BtnClearRecord_Click(object sender, EventArgs e)
        {
            // 入力項目のクリア
            RowNo.Text = null;
            Material.SelectedValue = null;
            Kakou.SelectedValue = null;
            Kakou_T.SelectedValue = null;
            Kakou_B.SelectedValue = null;
            Kakou_A.SelectedValue = null;
            Size_T.Text = string.Empty;
            Size_A.Text = string.Empty;
            Size_B.Text = string.Empty;
            Kousa_T_U.Text = string.Empty;
            Kousa_A_U.Text = string.Empty;
            Kousa_B_U.Text = string.Empty;
            Kousa_T_L.Text = string.Empty;
            Kousa_A_L.Text = string.Empty;
            Kousa_B_L.Text = string.Empty;
            Mentori_4.Text = string.Empty;
            Mentori_8.Text = string.Empty;
            Suryou.Text = string.Empty;
            CustomerNo.Text = string.Empty;
            EndUserNo.Text = string.Empty;
            Refer.Text = string.Empty;

            // 見積金額と納期のクリア
            Nouki_S.Text = string.Empty;
            Price_U.Text = string.Empty;
            Price_S.Text = string.Empty;

            // 入力・編集エリアのボタン表示変更
            BtnAddRecord.Text = "追 加";

            // 入力が有効であれば処理を続行
            lblErrorMessages.Text = string.Empty; // エラーメッセージをクリア

            // 全明細見積金額算出済みかどうかチェックし、見積書発行ボタンの活性・非活性を変更
            CheckIfAllEstimated();

            Material.Focus();
        }

        protected void BtnAddRecord_Click(object sender, EventArgs e)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            bool res;

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

            // バリデーションを定義
            bool isValid = gCls.ValidateControls(
                out List<string> errorMessages,
                (Material, "材質を選択してください。", value => value != ""),
                (Kakou_T, "厚み面の加工内容を選択してください。", value => value != ""),
                (Kakou_B, "巾面の加工内容を選択してください。", value => value != ""),
                (Kakou_A, "長さ面の加工内容を選択してください。", value => value != ""),
                (Size_T, "厚みの寸法は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Size_B, "巾面の寸法は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Size_A, "長さ面の寸法は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_T_U, "厚みの公差（上限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_B_U, "巾面の公差（上限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_A_U, "長さ面の公差（上限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_T_L, "厚みの公差（下限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_B_L, "巾面の公差（下限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Kousa_A_L, "長さ面の公差（下限）は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
                (Suryou, "数量は必須項目です", value => !string.IsNullOrWhiteSpace(value))
            );

            // エラーがある場合
            if (!isValid)
            {
                // エラーメッセージを表示
                lblErrorMessages.Text = string.Join("<br />", errorMessages);
                lblErrorMessages.ForeColor = System.Drawing.Color.Red;

                // 最初のエラー項目にフォーカス
                SetFocusToFirstError(new List<Control> {
                    Material,
                    Kakou_T,
                    Kakou_B,
                    Kakou_A,
                    Size_T,
                    Size_B,
                    Size_A,
                    Kousa_T_U,
                    Kousa_B_U,
                    Kousa_A_U,
                    Kousa_T_L,
                    Kousa_B_L,
                    Kousa_A_L,
                    Suryou
                });
                return;
            }

            // 入力が有効であれば処理を続行
            lblErrorMessages.Text = string.Empty; // エラーメッセージをクリア

            // 各入力値を取得
            string orderNo = (string)OrderNo.Text;
            string inpDate = (string)InpDate.Text;
            string estDate = (string)EstDate.Text;
            string cstCode = (string)CstCode.Text;
            string cstName = (string)CustomerName.Text;
            string customreCharge = (string)CustomreCharge.Text;
            string distCompCd = (string)DistCompCd.Text;
            string distCompName = (string)DistCompName.Text;
            string distCompPost = (string)DistCompPost.Text;
            string distCharge = (string)DistCharge.Text;
            string distCompZipCd = (string)DistCompZipCd.Text;
            string distCompAddress = (string)DistCompAddress.Text;
            string distTel = (string)DistTel.Text;
            string distFax = (string)DistFax.Text;
            int shippingMethodId;
            res = int.TryParse(ShippingMethod.SelectedValue, out shippingMethodId);

            // 各入力値を取得
            int rowNo;
            res = int.TryParse(RowNo.Text, style, culture, out rowNo);
            string materialCd = (string)Material.SelectedValue;
            string materialName = (string)Material.SelectedItem.Text;
            string kakou = (string)Kakou.SelectedValue;
            int kakouShiyouCd;
            res = int.TryParse(Kakou.SelectedItem.Value, style, culture, out kakouShiyouCd);
            string kakouShiyou = (string)Kakou.SelectedItem.Text;
            string kakou_T = (string)Kakou_T.SelectedItem.Value;
            string kakou_B = (string)Kakou_B.SelectedItem.Value;
            string kakou_A = (string)Kakou_A.SelectedItem.Value;
            string kakouShiji_T = (string)Kakou_T.SelectedItem.Text;
            string kakouShiji_B = (string)Kakou_B.SelectedItem.Text;
            string kakouShiji_A = (string)Kakou_A.SelectedItem.Text;

            decimal size_T = 0.00m;
            res = decimal.TryParse(Size_T.Text, style, culture, out size_T);
            decimal size_B;
            res = decimal.TryParse(Size_B.Text, style, culture, out size_B);
            decimal size_A;
            res = decimal.TryParse(Size_A.Text, style, culture, out size_A);

            decimal kousa_T_U;
            res = decimal.TryParse(Kousa_T_U.Text, style, culture, out kousa_T_U);
            decimal kousa_B_U;
            res = decimal.TryParse(Kousa_B_U.Text, style, culture, out kousa_B_U);
            decimal kousa_A_U;
            res = decimal.TryParse(Kousa_A_U.Text, style, culture, out kousa_A_U);
            decimal kousa_T_L;
            res = decimal.TryParse(Kousa_T_L.Text, style, culture, out kousa_T_L);
            decimal kousa_B_L;
            res = decimal.TryParse(Kousa_B_L.Text, style, culture, out kousa_B_L);
            decimal kousa_A_L;
            res = decimal.TryParse(Kousa_A_L.Text, style, culture, out kousa_A_L);

            decimal mentori_4;
            res = decimal.TryParse(Mentori_4.Text, style, culture, out mentori_4);
            decimal mentori_8;
            res = decimal.TryParse(Mentori_8.Text, style, culture, out mentori_8);

            int suryou;
            res = int.TryParse(Suryou.Text, out suryou);

            string customerNo = (string)CustomerNo.Text;
            string endUserNo = (string)EndUserNo.Text;
            string refer = (string)Refer.Text;
            string ShortestNouki = (string)Nouki_S.Text;

            decimal UnitPrice;
            if (decimal.TryParse(Price_U.Text, style, culture, out decimal unitPrice))
            {
                UnitPrice = unitPrice;
            }
            else { DebugLog("Price_Uの値をdecimal型に変換できませんでした。"); }

            decimal SumPrice;
            if (decimal.TryParse(Price_S.Text, style, culture, out decimal sumPrice))
            {
                SumPrice = sumPrice;
            }
            else { DebugLog("Price_Sの値をdecimal型に変換できませんでした。"); }

            // DB接続文字列取得
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            string estOrderNo = "";
            if (Session["EstOrderNo"] != null)
            {
                estOrderNo = Convert.ToString(Session["EstOrderNo"]);
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.AddSqlParam(new SqlParameter("@SessionID", SqlDbType.VarChar, 30), ParameterDirection.Input).Value = (string)Session.SessionID;
                sqlParams.AddSqlParam(new SqlParameter("@RowID", SqlDbType.Int), ParameterDirection.Input).Value = rowNo;
                sqlParams.AddSqlParam(new SqlParameter("@WOEstimateNo", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = estOrderNo;
                sqlParams.AddSqlParam(new SqlParameter("@ZairyouCd", SqlDbType.VarChar, 4), ParameterDirection.Input).Value = materialCd;
                sqlParams.AddSqlParam(new SqlParameter("@ZairyouName", SqlDbType.VarChar, 20), ParameterDirection.Input).Value = materialName;
                sqlParams.AddSqlParam(new SqlParameter("@KakouShiyouCd", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = kakouShiyouCd;
                sqlParams.AddSqlParam(new SqlParameter("@KakouShiyou", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = kakouShiyou;
                sqlParams.AddSqlParam(new SqlParameter("@Kakou_T", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = kakou_T;
                sqlParams.AddSqlParam(new SqlParameter("@Kakou_A", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = kakou_A;
                sqlParams.AddSqlParam(new SqlParameter("@Kakou_B", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = kakou_B;
                sqlParams.AddSqlParam(new SqlParameter("@KakouShijiCd_T", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = kakouShiji_T;
                sqlParams.AddSqlParam(new SqlParameter("@KakouShijiCd_A", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = kakouShiji_A;
                sqlParams.AddSqlParam(new SqlParameter("@KakouShijiCd_B", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = kakouShiji_B;
                sqlParams.AddSqlParam(new SqlParameter("@Size_T", SqlDbType.Decimal), ParameterDirection.Input).Value = size_T;
                sqlParams.AddSqlParam(new SqlParameter("@Size_A", SqlDbType.Decimal), ParameterDirection.Input).Value = size_A;
                sqlParams.AddSqlParam(new SqlParameter("@Size_B", SqlDbType.Decimal), ParameterDirection.Input).Value = size_B;
                sqlParams.AddSqlParam(new SqlParameter("@Kousa_T_U", SqlDbType.Decimal), ParameterDirection.Input).Value = kousa_T_U;
                sqlParams.AddSqlParam(new SqlParameter("@Kousa_A_U", SqlDbType.Decimal), ParameterDirection.Input).Value = kousa_A_U;
                sqlParams.AddSqlParam(new SqlParameter("@Kousa_B_U", SqlDbType.Decimal), ParameterDirection.Input).Value = kousa_B_U;
                sqlParams.AddSqlParam(new SqlParameter("@Kousa_T_L", SqlDbType.Decimal), ParameterDirection.Input).Value = kousa_T_L;
                sqlParams.AddSqlParam(new SqlParameter("@Kousa_A_L", SqlDbType.Decimal), ParameterDirection.Input).Value = kousa_A_L;
                sqlParams.AddSqlParam(new SqlParameter("@Kousa_B_L", SqlDbType.Decimal), ParameterDirection.Input).Value = kousa_B_L;
                sqlParams.AddSqlParam(new SqlParameter("@MentoriShiji", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = "";
                sqlParams.AddSqlParam(new SqlParameter("@Mentori_4", SqlDbType.Decimal), ParameterDirection.Input).Value = mentori_4;
                sqlParams.AddSqlParam(new SqlParameter("@Mentori_8", SqlDbType.Decimal), ParameterDirection.Input).Value = mentori_8;
                sqlParams.AddSqlParam(new SqlParameter("@Suryou", SqlDbType.Int), ParameterDirection.Input).Value = suryou;
                sqlParams.AddSqlParam(new SqlParameter("@RequestPrice", SqlDbType.Money), ParameterDirection.Input).Value = 0;
                sqlParams.AddSqlParam(new SqlParameter("@RequestNouki", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = "";
                sqlParams.AddSqlParam(new SqlParameter("@CustomerNo", SqlDbType.VarChar, 100), ParameterDirection.Input).Value = customerNo;
                sqlParams.AddSqlParam(new SqlParameter("@EndUserNo", SqlDbType.VarChar, 100), ParameterDirection.Input).Value = endUserNo;
                sqlParams.AddSqlParam(new SqlParameter("@Refer", SqlDbType.VarChar, 255), ParameterDirection.Input).Value = refer;
                sqlParams.AddSqlParam(new SqlParameter("@TokuisakiCd", SqlDbType.VarChar, 6), ParameterDirection.Input).Value = cstCode;
                sqlParams.AddSqlParam(new SqlParameter("@EndUserCd", SqlDbType.VarChar, 6), ParameterDirection.Input).Value = "";
                sqlParams.AddSqlParam(new SqlParameter("@TyokusousakiCd", SqlDbType.VarChar, 6), ParameterDirection.Input).Value = distCompCd;
                sqlParams.AddSqlParam(new SqlParameter("@TyokusousakiName", SqlDbType.VarChar, 80), ParameterDirection.Input).Value = distCompName;
                sqlParams.AddSqlParam(new SqlParameter("@TyokusousakiZipCd", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = distCompZipCd;
                sqlParams.AddSqlParam(new SqlParameter("@TyokusousakiAddr", SqlDbType.VarChar, 255), ParameterDirection.Input).Value = distCompAddress;
                sqlParams.AddSqlParam(new SqlParameter("@TyokusousakiPost", SqlDbType.VarChar, 50), ParameterDirection.Input).Value = distCompPost;
                sqlParams.AddSqlParam(new SqlParameter("@TyokusousakiCharge", SqlDbType.VarChar, 20), ParameterDirection.Input).Value = distCharge;
                sqlParams.AddSqlParam(new SqlParameter("@ShortestNouki", SqlDbType.VarChar, 10), ParameterDirection.Input).Value = ShortestNouki;
                sqlParams.AddSqlParam(new SqlParameter("@UnitPrice", SqlDbType.Money), ParameterDirection.Input).Value = unitPrice;
                sqlParams.AddSqlParam(new SqlParameter("@SumPrice", SqlDbType.Money), ParameterDirection.Input).Value = sumPrice;
                sqlParams.AddSqlParam(new SqlParameter("@SeqNo", SqlDbType.Int), ParameterDirection.Output).Value = 0;

                // ストアド名
                string storedProcedureName = "usp_ASP_EstimateDetailWork_ins";

                // デバッグクエリを生成
                string debugQuery = GeneralCls.BuildDebugQuery(storedProcedureName, sqlParams);

                // デバッグクエリを出力
                DebugLog("Generated Debug Query:");
                DebugLog(debugQuery);

                try
                {
                    // レコード取得
                    var dt = gCls.GetDataTable(con, storedProcedureName, sqlParams);

                    ListView1.DataSource = dt;
                    ListView1.DataBind();

                    // 合計金額を表示
                    DisplayTotalAmount();
                }
                catch (Exception ex)
                {
                    // エラーログ出力
                    DebugLog("エラーが発生しました: " + ex.Message);
                    DebugLog("詳細: " + ex.StackTrace);
                }
                con.Close();
            }

            // 入力項目のクリア
            RowNo.Text = null;
            Material.SelectedValue = null;
            Kakou.SelectedValue = null;
            Kakou_T.SelectedValue = null;
            Kakou_B.SelectedValue = null;
            Kakou_A.SelectedValue = null;
            Size_T.Text = string.Empty;
            Size_A.Text = string.Empty;
            Size_B.Text = string.Empty;
            Kousa_T_U.Text = string.Empty;
            Kousa_A_U.Text = string.Empty;
            Kousa_B_U.Text = string.Empty;
            Kousa_T_L.Text = string.Empty;
            Kousa_A_L.Text = string.Empty;
            Kousa_B_L.Text = string.Empty;
            Mentori_4.Text = string.Empty;
            Mentori_8.Text = string.Empty;
            Suryou.Text = string.Empty;
            CustomerNo.Text = string.Empty;
            EndUserNo.Text = string.Empty;
            Refer.Text = string.Empty;

            // 見積金額と納期のクリア
            Nouki_S.Text = string.Empty;
            Price_U.Text = string.Empty;
            Price_S.Text = string.Empty;

            // 入力・編集エリアのボタン表示変更
            BtnAddRecord.Text = "追 加";

            // 追加ボタンを再び無効化
            BtnAddRecord.Enabled = false;

            // 見積フラグをリセット
            ViewState["IsEstimated"] = false;

            // 全明細見積金額算出済みかどうかチェックし、見積書発行ボタンの活性・非活性を変更
            CheckIfAllEstimated();

            // 材質にFocus
            Material.Focus();
        }
        protected void BtnMentoriDetail_Click(object sender, EventArgs e)
        {
            // 初期化
            string sizeT = string.Empty;
            string sizeA = string.Empty;
            string sizeB = string.Empty;

            Button i = (Button)sender;
            ListViewDataItem item = (ListViewDataItem)i.Parent.Parent.Parent;
            int index = (int)item.DataItemIndex;

            // ListView の項目から Label を取得
            Label lblSizeT = ListView1.Items[index].FindControl("lblSize_T") as Label;
            Label lblSizeA = ListView1.Items[index].FindControl("lblSize_A") as Label;
            Label lblSizeB = ListView1.Items[index].FindControl("lblSize_B") as Label;

            if (lblSizeT != null) sizeT = lblSizeT.Text;
            if (lblSizeA != null) sizeA = lblSizeA.Text;
            if (lblSizeB != null) sizeB = lblSizeB.Text;

            // 寸法をSession変数に保存
            Session["sizeT"] = sizeT;
            Session["sizeA"] = sizeA;
            Session["sizeB"] = sizeB;

            // JavaScriptでポップアップを開くスクリプトを登録
            string script = "<script type='text/javascript'>window.open('Cube3DImage.aspx', 'Cube3DImage', " +
                            "'toolbar=no, location=no, status=no, menubar=no, scrollbars=yes, resizable=yes, width=970, height=870');</script>";

            ClientScript.RegisterStartupScript(this.GetType(), "OpenPopup", script);
        }

        protected bool IsChkSelectable(object nouki, object price, object unitPrice)
        {
            DateTime date;
            bool isDateValid = DateTime.TryParse(nouki?.ToString(), out date);

            decimal priceValue, unitPriceValue;
            bool isPriceValid = decimal.TryParse(price?.ToString(), out priceValue) && priceValue > 0;
            bool isUnitPriceValid = decimal.TryParse(unitPrice?.ToString(), out unitPriceValue) && unitPriceValue > 0;

            return isDateValid && isPriceValid && isUnitPriceValid;
        }


        protected void BtnEditList_Click(object sender, EventArgs e)
        {
            Button i = (Button)sender;
            ListViewDataItem item = (ListViewDataItem)i.Parent.Parent.Parent;
            // int index = item.Parent.Controls.IndexOf(item);
            int index = (int)item.DataItemIndex;

            // 材料コード
            HiddenField materialCd = ((HiddenField)ListView1.Items[index].FindControl("hidMaterialCd"));
            lblDubugText1.Text = materialCd.Value.ToString();

        }

        protected void BtnDeleteList_Click(object sender, EventArgs e)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            Button i = (Button)sender;
            ListViewDataItem item = (ListViewDataItem)i.Parent.Parent.Parent;
            int idx = (int)item.DataItemIndex;

            // 明細No
            Label rowNo = ((Label)ListView1.Items[idx].FindControl("lblSeqNo"));

            // DB接続文字列
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.AddSqlParam(new SqlParameter("@SessionID", SqlDbType.VarChar, 30), ParameterDirection.Input).Value = (string)Session.SessionID;
                sqlParams.AddSqlParam(new SqlParameter("@RowNo", SqlDbType.Int), ParameterDirection.Input).Value = (string)rowNo.Text;

                // レコード取得
                var dt = gCls.GetDataTable(con, "usp_ASP_EstimateDetailWork_del", sqlParams);

                ListView1.DataSource = dt;
                ListView1.DataBind();
                con.Close();

                // 合計金額を表示
                DisplayTotalAmount();
            }
        }

        protected void BtnStandKousa_Click(object sender, EventArgs e)
        {
            // DB接続文字列
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            string cstCode = (string)CstCode.Text;
            string kousa_T_U = string.Empty;
            string kousa_T_L = string.Empty;
            string kousa_A_U = string.Empty;
            string kousa_A_L = string.Empty;
            string kousa_B_U = string.Empty;
            string kousa_B_L = string.Empty;

            // 得意先別の標準公差取得
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ASP_ToleranceSetting_get"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // SQLパラメータ設定
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    sqlParameters.AddSqlParam(new SqlParameter("@TokuisakiCd", SqlDbType.VarChar), ParameterDirection.Input).Value = cstCode;
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
                                kousa_T_U = sdr["標準公差1T"].ToString();
                                kousa_T_L = sdr["標準公差2T"].ToString();
                                kousa_A_U = sdr["標準公差1A"].ToString();
                                kousa_A_L = sdr["標準公差2A"].ToString();
                                kousa_B_U = sdr["標準公差1B"].ToString();
                                kousa_B_L = sdr["標準公差2B"].ToString();
                            }
                        }
                    }

                    // 見積明細に標準公差の表示
                    Kousa_T_U.Text = FormatTolerance(kousa_T_U);
                    Kousa_T_L.Text = FormatTolerance(kousa_T_L);
                    Kousa_A_U.Text = FormatTolerance(kousa_A_U);
                    Kousa_A_L.Text = FormatTolerance(kousa_A_L);
                    Kousa_B_U.Text = FormatTolerance(kousa_B_U);
                    Kousa_B_L.Text = FormatTolerance(kousa_B_L);

                    BtnStandMentori.Focus();
                }
            }
        }

        protected void BtnStandMentori_Click(object sender, EventArgs e)
        {
            // DB接続文字列
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            string cstCode = (string)CstCode.Text;
            string mentori = string.Empty;
            string mentori_4 = string.Empty;
            string mentori_8 = string.Empty;

            // 得意先別の標準面取り量取得
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ASP_ChamferAmountSetting_get"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // SQLパラメータ設定
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    sqlParameters.AddSqlParam(new SqlParameter("@TokuisakiCd", SqlDbType.VarChar), ParameterDirection.Input).Value = cstCode;
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
                                mentori = sdr["面取指示区分"].ToString();
                                mentori_4 = sdr["標準面取4C"].ToString();
                                mentori_8 = sdr["標準面取8C"].ToString();
                            }
                        }
                    }

                    // 見積明細に標準面取り量の表示
                    // MentoriShiji.SelectedIndex = 1;
                    Mentori_4.Text = FormatChamfering(mentori_4);
                    Mentori_8.Text = FormatChamfering(mentori_8);

                    Suryou.Focus();
                }
            }
        }

        private DataTable UpdateEstimateData()
        {
            try
            {
                GeneralCls gCls = new GeneralCls();

                // 各種入力値を取得
                string estOrderNo = EstOrderNo.Text;
                string orderNo = OrderNo.Text;
                string inpDate = InpDate.Text;
                string estDate = EstDate.Text;
                string cstCode = CstCode.Text;
                string cstName = CustomerName.Text;
                string customreCharge = CustomreCharge.Text;
                string distCompCd = DistCompCd.Text;
                string distCompName = DistCompName.Text;
                string distCompPost = DistCompPost.Text;
                string distCharge = DistCharge.Text;
                string distCompZipCd = DistCompZipCd.Text;
                string distCompAddress = DistCompAddress.Text;
                string distTel = DistTel.Text;
                string distFax = DistFax.Text;
                int shippingMethodId;
                bool res = int.TryParse(ShippingMethod.SelectedValue, out shippingMethodId);

                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ASP_Estimate_upd", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // パラメータリストの作成
                        cmd.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.VarChar, 30) { Value = Session.SessionID });
                        cmd.Parameters.Add(new SqlParameter("@WOEstimateNo", SqlDbType.VarChar, 10) { Value = estOrderNo });
                        cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.VarChar, 20) { Value = orderNo });
                        cmd.Parameters.Add(new SqlParameter("@InpDate", SqlDbType.VarChar, 10) { Value = inpDate });
                        cmd.Parameters.Add(new SqlParameter("@EstDate", SqlDbType.VarChar, 10) { Value = estDate });
                        cmd.Parameters.Add(new SqlParameter("@CstCode", SqlDbType.VarChar, 6) { Value = cstCode });
                        cmd.Parameters.Add(new SqlParameter("@CstName", SqlDbType.VarChar, 50) { Value = cstName });
                        cmd.Parameters.Add(new SqlParameter("@CustomreCharge", SqlDbType.VarChar, 20) { Value = customreCharge });
                        cmd.Parameters.Add(new SqlParameter("@DistCompCd", SqlDbType.VarChar, 6) { Value = distCompCd });
                        cmd.Parameters.Add(new SqlParameter("@DistCompName", SqlDbType.VarChar, 80) { Value = distCompName });
                        cmd.Parameters.Add(new SqlParameter("@DistCompPost", SqlDbType.VarChar, 50) { Value = distCompPost });
                        cmd.Parameters.Add(new SqlParameter("@DistCharge", SqlDbType.VarChar, 20) { Value = distCharge });
                        cmd.Parameters.Add(new SqlParameter("@DistCompZipCd", SqlDbType.VarChar, 10) { Value = distCompZipCd });
                        cmd.Parameters.Add(new SqlParameter("@DistCompAddress", SqlDbType.VarChar, 255) { Value = distCompAddress });
                        cmd.Parameters.Add(new SqlParameter("@DistTel", SqlDbType.VarChar, 15) { Value = distTel });
                        cmd.Parameters.Add(new SqlParameter("@DistFax", SqlDbType.VarChar, 15) { Value = distFax });
                        cmd.Parameters.Add(new SqlParameter("@ShippingMethodId", SqlDbType.Int) { Value = shippingMethodId });

                        // OUTPUTパラメータ
                        SqlParameter outputParam = new SqlParameter("@NewWOEstimateNo", SqlDbType.VarChar, 10)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        // データベース接続を開く
                        con.Open();
                        cmd.ExecuteNonQuery();

                        // OUTPUTパラメータの取得
                        string generatedEstimateNo = outputParam.Value.ToString();
                        Session["EstOrderNo"] = generatedEstimateNo;
                        DebugLog($"Session[\"EstOrderNo\"]: {Session["EstOrderNo"]}");

                        // 結果を取得
                        return gCls.GetDataTable(con, "usp_ASP_Estimate_upd", cmd.Parameters.Cast<SqlParameter>().ToList());
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                DebugLog($"SQLエラーが発生しました: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                DebugLog($"予期しないエラーが発生しました: {ex.Message}");
            }

            return null; // エラー時は null を返す
        }


        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            GeneralCls gCls = new GeneralCls();

            // バリデーション
            bool isValid = gCls.ValidateControls(
                out List<string> errorMessages,
                (InpDate, "入力日付は必須項目です。", value => !string.IsNullOrWhiteSpace(value))
            );

            if (!isValid)
            {
                lblErrorMessages.Text = string.Join("<br />", errorMessages);
                lblErrorMessages.ForeColor = System.Drawing.Color.Red;
                SetFocusToFirstError(new List<Control> { InpDate });
                return;
            }

            lblErrorMessages.Text = string.Empty;

            // データ更新
            DataTable dt = UpdateEstimateData();
            ListView1.DataSource = dt;
            ListView1.DataBind();

            // フィールドのクリア処理
            Material.SelectedValue = null;
            Kakou.SelectedValue = null;
            Kakou_T.SelectedValue = null;
            Kakou_B.SelectedValue = null;
            Kakou_A.SelectedValue = null;
            Size_T.Text = string.Empty;
            Size_A.Text = string.Empty;
            Size_B.Text = string.Empty;
            Kousa_T_U.Text = string.Empty;
            Kousa_A_U.Text = string.Empty;
            Kousa_B_U.Text = string.Empty;
            Kousa_T_L.Text = string.Empty;
            Kousa_A_L.Text = string.Empty;
            Kousa_B_L.Text = string.Empty;
            Mentori_4.Text = string.Empty;
            Mentori_8.Text = string.Empty;
            Suryou.Text = string.Empty;
            CustomerNo.Text = string.Empty;
            EndUserNo.Text = string.Empty;
            Refer.Text = string.Empty;

            Nouki_S.Text = string.Empty;
            Price_U.Text = "0";
            Price_S.Text = "0";

            hidEstOrderNo.Value = "";
            EstOrderNo.Text = string.Empty;
            OrderNo.Text = string.Empty;
            InpDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            EstDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            CustomreCharge.Text = string.Empty;
            DistCompCd.Text = string.Empty;
            DistCompName.Text = string.Empty;
            DistCompPost.Text = string.Empty;
            DistCharge.Text = string.Empty;
            DistCompZipCd.Text = string.Empty;
            DistCompAddress.Text = string.Empty;
            DistTel.Text = string.Empty;
            DistFax.Text = string.Empty;
            ShippingMethod.SelectedValue = "1";

            DivDistination.Visible = true;
            lblDubugText1.Text = "登録完了しました。";
        }

        protected void BtnOrder_Click(object sender, EventArgs e)
        {
            // 先にデータ更新
            UpdateEstimateData();

            List<OrderDetail> selectedOrders = new List<OrderDetail>();

            // string estimateNo = EstOrderNo.Text;
            string estimateNo = GetSessionValue("EstOrderNo");
            string sessionId = Session.SessionID;

            foreach (ListViewItem item in ListView1.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("ChkSelect");
                Label lblSeqNo = (Label)item.FindControl("lblSeqNo");

                if (cb != null && cb.Checked && lblSeqNo != null)
                {
                    if (int.TryParse(lblSeqNo.Text, out int rowNo))
                    {
                        selectedOrders.Add(new OrderDetail(rowNo, estimateNo, sessionId));
                    }
                }
            }

            Debug.WriteLine("選択された注文:");
            foreach (var order in selectedOrders)
            {
                Debug.WriteLine($"RowNo: {order.RowNo}, EstimateNo: {order.EstimateNo}, SessionID: {order.SessionID}");
            }

            if (selectedOrders.Count > 0)
            {
                // セッションに保存
                Session["SelectedOrders"] = selectedOrders;
                Session["EstOrderNo"] = estimateNo;

                // OrderConfirm(注文内容確認画面)へ遷移
                Response.Redirect("OrderConfirm.aspx");
            }
        }

        protected void BtnGenerateEstimateReport_Click(object sender, EventArgs e)
        {
            string estimateNo = EstOrderNo.Text; // ユーザー入力から見積Noを取得
            string sessionID = (string)Session.SessionID;

            try
            {
                GenerateQuotationPDF(sessionID, estimateNo, Response);
            }
            catch (Exception ex)
            {
                // エラーハンドリング
                lblDubugText5.Text = "見積書の生成中にエラーが発生しました: " + ex.Message;
            }
        }

        public void GenerateQuotationPDF(string sessionID, string estimateNo, HttpResponse response)
        {
            DataSet ds = null; // データセットを初期化
            string reportPath = string.Empty; // レポートパスを初期化
            string saveFolder = HttpContext.Current.Server.MapPath("~/Reports/Quotation/");
            string fileName = $"Quotation_{estimateNo}.pdf";
            string savePath = Path.Combine(saveFolder, fileName);

            try
            {
                // データベースから見積データを取得
                ds = GetQuotationData();
                DebugLog("データ取得成功");

                // データチェック
                ValidateDataSet(ds);
                DebugLog("データチェック成功");

                // データセットのデバック
                DebugDataSet(ds);
            }
            catch (Exception ex)
            {
                DebugLog($"データ取得または検証でエラー発生: {ex.Message}");
                throw;
            }

            try
            {
                // RDLCレポートファイルのパスを取得
                reportPath = HttpContext.Current.Server.MapPath("~/Reports/Quotation.rdlc");

                if (!File.Exists(reportPath))
                {
                    throw new FileNotFoundException($"RDLCレポートファイルが見つかりません: {reportPath}");
                }
                DebugLog($"レポートパス: {reportPath}");
            }
            catch (Exception ex)
            {
                DebugLog($"レポートファイルエラー: {ex.Message}");
                throw;
            }

            try
            {
                // レポートの準備
                using (LocalReport report = new LocalReport())
                {
                    report.ReportPath = reportPath;

                    // RDLCにデータソースをバインド
                    report.DataSources.Clear();
                    report.DataSources.Add(new ReportDataSource("dsHeader", ds.Tables["Header"]));
                    DebugLog($"RDLCにデータソースをバインド: dsHeader");
                    report.DataSources.Add(new ReportDataSource("dsDetail", ds.Tables["Detail"]));
                    DebugLog($"RDLCにデータソースをバインド: dsDetail");

                    // デバッグ用データを準備してパラメータとして渡す
                    // string debugDataString = PrepareDebugData(ds);
                    // report.SetParameters(new[] { new ReportParameter("DebugData", debugDataString) });

                    // デバッグ　RDLCレポートパラメータ確認
                    LogReportParameters(report);

                    // レポートをPDFとしてレンダリング
                    DebugLog($"レポートをPDFとしてレンダリング: RenderReportToPDF　開始");
                    byte[] pdfBytes = RenderReportToPDF(report, out string mimeType);
                    DebugLog($"レポートをPDFとしてレンダリング: RenderReportToPDF　終了");

                    // PDF を保存
                    File.WriteAllBytes(savePath, pdfBytes);
                    DebugLog($"PDFを保存: {savePath}");

                    // DB に登録
                    SavePDFInfoToDB(estimateNo, fileName, "/Reports/Quotation/", sessionID);

                    // PDFをクライアントに送信
                    DebugLog($"PDFをクライアントに送信: SendPDFToClient　開始");
                    SendPDFToClient(response, pdfBytes, mimeType, estimateNo);
                    DebugLog($"PDFをクライアントに送信: SendPDFToClient　終了");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                SendErrorResponse(response, ex);
            }
        }

        private void DebugDataSet(DataSet ds)
        {
            foreach (DataTable table in ds.Tables)
            {
                DebugLog($"テーブル: {table.TableName}, 行数: {table.Rows.Count}");
                foreach (DataRow row in table.Rows)
                {
                    DebugLog(string.Join(", ", row.ItemArray));
                }
            }
        }

        // データセットの検証
        private void ValidateDataSet(DataSet ds)
        {
            if (ds == null || ds.Tables.Count < 1)
            {
                throw new Exception("見積データが取得できませんでした。");
            }

            if (!ds.Tables.Contains("Header") || !ds.Tables.Contains("Detail"))
            {
                throw new Exception("必要なデータテーブル (Header, Detail) が存在しません。");
            }

            DebugLog($"Header行数: {ds.Tables["Header"].Rows.Count}");
            DebugLog($"Detail行数: {ds.Tables["Detail"].Rows.Count}");
        }

        // デバッグデータの準備
        private string PrepareDebugData(DataSet ds)
        {
            // デバッグデータを構築（必要に応じてカスタマイズ）
            return $"Header行数: {ds.Tables["Header"].Rows.Count}{Environment.NewLine}" +
                   $"Detail行数: {ds.Tables["Detail"].Rows.Count}";
        }

        // レポートをPDFとしてレンダリング
        private byte[] RenderReportToPDF(LocalReport report, out string mimeType)
        {
            string deviceInfo = @"
                                <DeviceInfo>
                                    <OutputFormat>PDF</OutputFormat>
                                    <PageWidth>11.69in</PageWidth>
                                    <PageHeight>8.27in</PageHeight>
                                    <MarginTop>0.5in</MarginTop>
                                    <MarginLeft>0.5in</MarginLeft>
                                    <MarginRight>0.5in</MarginRight>
                                    <MarginBottom>0.5in</MarginBottom>
                                </DeviceInfo>";

            try
            {
                return report.Render(
                    "PDF",            // 出力形式
                    deviceInfo,       // デバイス情報
                    out mimeType,     // MIMEタイプ
                    out string _,     // エンコーディング（不要なので破棄）
                    out string _,     // 拡張子（不要なので破棄）
                    out string[] _,   // ストリームID（不要なので破棄）
                    out Microsoft.Reporting.WebForms.Warning[] warnings // 明示的に名前空間を指定
                );
            }
            catch (Exception ex)
            {
                // ログに詳細エラー情報を出力
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("PDF生成エラー:");
                sb.AppendLine($"メッセージ: {ex.Message}");
                sb.AppendLine($"スタックトレース: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    sb.AppendLine($"内部例外: {ex.InnerException.Message}");
                    sb.AppendLine($"内部スタックトレース: {ex.InnerException.StackTrace}");
                }

                // Windows イベントログまたはファイルに出力 (例: log.txt)
                System.IO.File.AppendAllText(@"E:\rdlc_error.log", sb.ToString());

                throw new Exception("PDF生成中にエラーが発生しました。詳細はログを確認してください。", ex);
            }
        }

        // パラメーターが不足していないかログ出力
        private void LogReportParameters(LocalReport report)
        {
            System.IO.File.AppendAllText(@"E:\rdlc_parameters.log", "現在設定されているパラメーター:\n");

            foreach (var param in report.GetParameters())
            {
                System.IO.File.AppendAllText(@"E:\rdlc_parameters.log", $"パラメーター名: {param.Name}, 値: {(param.Values.Count > 0 ? param.Values[0] : "なし")}\n");
            }
        }

        private void SavePDFInfoToDB(string estimateNo, string fileName, string path, string sessionID)
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            // 現在の日付・時刻を取得
            string printDate = DateTime.Now.ToString("yyyy/MM/dd");
            string printTime = DateTime.Now.ToString("HH:mm:ss");

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ASP_Report_Estimate_upd", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WOEstimateNo", estimateNo);
                        cmd.Parameters.AddWithValue("@PrintDate", printDate);
                        cmd.Parameters.AddWithValue("@PrintTime", printTime);
                        cmd.Parameters.AddWithValue("@FileName", fileName);
                        cmd.Parameters.AddWithValue("@Path", path);
                        cmd.Parameters.AddWithValue("@SID", sessionID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        DebugLog($"DB更新成功: {fileName} を {path} に保存");
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLog($"DB更新エラー: {ex.Message}");
                throw;
            }
        }

        // PDFをクライアントに送信
        private void SendPDFToClient(HttpResponse response, byte[] pdfBytes, string mimeType, string estimateNo)
        {
            try
            {
                response.ContentType = mimeType;
                response.AddHeader("Content-Disposition", $"attachment; filename=Quotation_{estimateNo}.pdf");
                response.BinaryWrite(pdfBytes);
                response.Flush();  // データをすべて送信
                response.SuppressContent = true;  // 残りの処理をスキップ
                HttpContext.Current.ApplicationInstance.CompleteRequest();  // スレッドを強制終了せずに処理を完了
            }
            catch (HttpException httpEx)
            {
                throw new HttpException("HTTPレスポンス送信中にエラーが発生しました。", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("PDF送信中にエラーが発生しました。", ex);
            }
        }

        // エラーログの記録
        private void LogError(Exception ex)
        {
            DebugLog($"エラー: {ex.Message}");
            DebugLog($"スタックトレース: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                DebugLog($"内部エラー: {ex.InnerException.Message}");
            }
        }

        // エラー応答の送信
        private void SendErrorResponse(HttpResponse response, Exception ex)
        {
            response.ContentType = "text/plain";
            response.StatusCode = 500;
            response.Write($"エラーが発生しました: {ex.Message}");
            response.End();
        }

        private DataSet GetQuotationData()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            // 各入力値を取得
            string estimateNo = (string)EstOrderNo.Text;
            string orderNo = (string)OrderNo.Text;
            string inpDate = (string)InpDate.Text;
            string estDate = (string)EstDate.Text;
            string cstCode = (string)CstCode.Text;
            string cstName = (string)CustomerName.Text;
            string customreCharge = (string)CustomreCharge.Text;
            string distCompCd = (string)DistCompCd.Text;
            string distCompName = (string)DistCompName.Text;
            string distCompPost = (string)DistCompPost.Text;
            string distCharge = (string)DistCharge.Text;
            string distCompZipCd = (string)DistCompZipCd.Text;
            string distCompAddress = (string)DistCompAddress.Text;
            string distTel = (string)DistTel.Text;
            string distFax = (string)DistFax.Text;
            int shippingMethodId;
            bool res = int.TryParse(ShippingMethod.SelectedValue, out shippingMethodId);

            string storedProcedureName = "usp_ASP_Report_Estimate_get";
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(storedProcedureName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SessionID", Session.SessionID);
                cmd.Parameters.AddWithValue("@WOEstimateNo", estimateNo);
                cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                cmd.Parameters.AddWithValue("@InpDate", inpDate);
                cmd.Parameters.AddWithValue("@EstDate", estDate);
                cmd.Parameters.AddWithValue("@CstCode", cstCode);
                cmd.Parameters.AddWithValue("@CstName", cstName);
                cmd.Parameters.AddWithValue("@CustomreCharge", customreCharge);
                cmd.Parameters.AddWithValue("@DistCompCd", distCompCd);
                cmd.Parameters.AddWithValue("@DistCompName", distCompName);
                cmd.Parameters.AddWithValue("@DistCompPost", distCompPost);
                cmd.Parameters.AddWithValue("@DistCharge", distCharge);
                cmd.Parameters.AddWithValue("@DistCompZipCd", distCompZipCd);
                cmd.Parameters.AddWithValue("@DistCompAddress", distCompAddress);
                cmd.Parameters.AddWithValue("@DistTel", distTel);
                cmd.Parameters.AddWithValue("@DistFax", distFax);
                cmd.Parameters.AddWithValue("@ShippingMethodId", shippingMethodId);

                // cmd.Parameters をリスト化
                var sqlParams = cmd.Parameters.Cast<SqlParameter>().ToList();

                // デバッグクエリを生成
                string debugQuery = GeneralCls.BuildDebugQuery(storedProcedureName, sqlParams);

                // デバッグクエリを出力
                DebugLog("Generated Debug Query:");
                DebugLog(debugQuery);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                // テーブル名を設定
                ds.Tables[0].TableName = "Detail";
                ds.Tables[1].TableName = "Header";

                return ds;
            }
        }

        protected void Material_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 「材質」変更時の処理
            HandleInputChange((Control)sender);
        }

        protected void Kakou_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kakou = (string)Kakou.SelectedValue;

            // debug
            lblDubugText2.Text = string.Format("{0:s}Selected (value={1:s})", kakou, kakou);

            if (kakou == "6F")
            {
                Kakou_T.SelectedValue = "2";    // 1:G / 2:W / 4:～
                Kakou_A.SelectedValue = "2";    // 1:G / 2:W / 4:～
                Kakou_B.SelectedValue = "2";    // 1:G / 2:W / 4:～

                Size_T.Focus();
            }
            else if (kakou == "黒皮")
            {
                Kakou_T.SelectedValue = "4";    // 1:G / 2:W / 4:～
                Kakou_A.SelectedValue = "4";    // 1:G / 2:W / 4:～
                Kakou_B.SelectedValue = "4";    // 1:G / 2:W / 4:～

                Size_T.Focus();
            }
            else
            {
                Kakou_T.SelectedValue = "2";    // 1:G / 2:W / 4:～
                Kakou_A.SelectedValue = "2";    // 1:G / 2:W / 4:～
                Kakou_B.SelectedValue = "2";    // 1:G / 2:W / 4:～

                Kakou_T.Focus();
            }

        }

        protected void Kakou_T_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 「T面加工」変更時の処理
            HandleInputChange((Control)sender);
        }

        protected void Kakou_A_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 「A面加工」変更時の処理
            HandleInputChange((Control)sender);
        }

        private void UpdateKakouList()
        {
            // T, A, Bの選択値を取得
            int shijiT = 0, shijiA = 0, shijiB = 0;

            if (!string.IsNullOrEmpty(Kakou_T.SelectedValue))
                int.TryParse(Kakou_T.SelectedValue, out shijiT);

            if (!string.IsNullOrEmpty(Kakou_A.SelectedValue))
                int.TryParse(Kakou_A.SelectedValue, out shijiA);

            if (!string.IsNullOrEmpty(Kakou_B.SelectedValue))
                int.TryParse(Kakou_B.SelectedValue, out shijiB);

            // DB接続設定
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("usp_ASP_KakouShiyouCd_get", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // パラメータ設定
                cmd.Parameters.AddWithValue("@ShijiT", shijiT);
                cmd.Parameters.AddWithValue("@ShijiA", shijiA);
                cmd.Parameters.AddWithValue("@ShijiB", shijiB);

                SqlParameter outputParam = new SqlParameter("@KakouShiyouCd", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                con.Open();
                cmd.ExecuteNonQuery();

                // ストアドプロシージャの結果を取得
                int kakouShiyouCd = 0;
                if (outputParam.Value != DBNull.Value)
                {
                    kakouShiyouCd = (int)outputParam.Value;
                }

                // Kakouリストに値を反映
                if (kakouShiyouCd > 0)
                {
                    Kakou.SelectedValue = kakouShiyouCd.ToString();
                }
                else
                {
                    Kakou.SelectedValue = ""; // 値が見つからない場合は空にする
                }

                con.Close();
            }
        }

        protected void Kakou_B_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 「B面加工」変更時の処理
            HandleInputChange((Control)sender);
        }

        protected void ShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 直送先Section表示の制御
            string shippingMethodId = (string)ShippingMethod.SelectedValue;

            if (shippingMethodId == "1")
            {
                DivDistination.Visible = true;
            }
            else
            {
                DivDistination.Visible = false;
            }
        }

        protected void MentoriShiji_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 面取り量入力項目の制御
            string mentoriShijiId = (string)MentoriShiji.SelectedValue;

            if (mentoriShijiId == "9")
            {
                Mentori_4.Enabled = true;
                Mentori_8.Enabled = true;
                Mentori_4.ReadOnly = false;
                Mentori_8.ReadOnly = false;
            }
            else
            {
                Mentori_4.Text = string.Empty;
                Mentori_8.Text = string.Empty;
                Mentori_4.Enabled = false;
                Mentori_8.Enabled = false;
                Mentori_4.ReadOnly = true;
                Mentori_8.ReadOnly = true;
            }
        }

        protected void BtnShukkasakiList_Click(object sender, EventArgs e)
        {
            string url = "ShukkasakiList.aspx";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenNewWindow", "window.open('" + url + "', null, height=200, wieth=200);", true);
        }

        protected void ListView1_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            GeneralCls gCls;
            gCls = new GeneralCls();

            if (String.Equals(e.CommandName, "EditEstimate"))
            {
                if (e.CommandArgument != null)
                {
                    // 行Noの取得
                    string rowNo = e.CommandArgument.ToString();

                    // DB接続文字列
                    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                    string material = "";
                    string kakou = "";
                    string kakou_T = "";
                    string kakou_A = "";
                    string kakou_B = "";
                    string size_T = "";
                    string size_A = "";
                    string size_B = "";

                    string kousa_T_U = "";
                    string kousa_T_L = "";
                    string kousa_A_U = "";
                    string kousa_A_L = "";
                    string kousa_B_U = "";
                    string kousa_B_L = "";

                    string mentori_4 = "";
                    string mentori_8 = "";

                    string suryou = "";

                    string customerNo = "";
                    string endUserNo = "";

                    string shortestNouki = "";
                    decimal unitPrice = 0.0m;
                    decimal sumPrice = 0.0m;

                    // 見積発注データの取得(見積No指定の場合)
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_ASP_EstimateOrderEdit_get"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // SQLパラメータ設定
                            List<SqlParameter> sqlParameters = new List<SqlParameter>();
                            sqlParameters.AddSqlParam(new SqlParameter("@SessionID", SqlDbType.VarChar), ParameterDirection.Input).Value = (string)Session.SessionID;
                            sqlParameters.AddSqlParam(new SqlParameter("@RowNo", SqlDbType.Int), ParameterDirection.Input).Value = Int32.Parse(rowNo);
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
                                        material = sdr["材料コード"].ToString();
                                        kakou = sdr["加工仕様"].ToString();
                                        kakou_T = sdr["加工指示コードT"].ToString();
                                        kakou_A = sdr["加工指示コードA"].ToString();
                                        kakou_B = sdr["加工指示コードB"].ToString();
                                        size_T = sdr["仕上りサイズT"].ToString();
                                        size_A = sdr["仕上りサイズA"].ToString();
                                        size_B = sdr["仕上りサイズB"].ToString();

                                        kousa_T_U = sdr["加工公差_UT"].ToString();
                                        kousa_T_L = sdr["加工公差_LT"].ToString();
                                        kousa_A_U = sdr["加工公差_UA"].ToString();
                                        kousa_A_L = sdr["加工公差_LA"].ToString();
                                        kousa_B_U = sdr["加工公差_UB"].ToString();
                                        kousa_B_L = sdr["加工公差_LB"].ToString();

                                        mentori_4 = sdr["面取り量_4C"].ToString();
                                        mentori_8 = sdr["面取り量_8C"].ToString();

                                        suryou = sdr["製品数量"].ToString();

                                        customerNo = sdr["客先注番"].ToString();
                                        endUserNo = sdr["送り先注番"].ToString();

                                        shortestNouki = sdr["最短納期"].ToString();

                                        // 数値として取得し、変換エラーを防ぐためにdecimal.TryParseを使用
                                        decimal.TryParse(sdr["見積単価"].ToString(), out unitPrice);
                                        decimal.TryParse(sdr["見積金額"].ToString(), out sumPrice);
                                    }
                                }
                            }

                            // 見積明細を呼び出し
                            RowNo.Text = rowNo;
                            Material.Text = material;
                            Kakou.Text = kakou;
                            Kakou_T.Text = kakou_T;
                            Kakou_A.Text = kakou_A;
                            Kakou_B.Text = kakou_B;
                            Size_T.Text = size_T;
                            Size_A.Text = size_A;
                            Size_B.Text = size_B;
                            Kousa_T_U.Text = FormatTolerance(kousa_T_U);
                            Kousa_T_L.Text = FormatTolerance(kousa_T_L);
                            Kousa_A_U.Text = FormatTolerance(kousa_A_U);
                            Kousa_A_L.Text = FormatTolerance(kousa_A_L);
                            Kousa_B_U.Text = FormatTolerance(kousa_B_U);
                            Kousa_B_L.Text = FormatTolerance(kousa_B_L);
                            Mentori_4.Text = FormatChamfering(mentori_4);
                            Mentori_8.Text = FormatChamfering(mentori_8);
                            Suryou.Text = suryou;
                            CustomerNo.Text = customerNo;
                            EndUserNo.Text = endUserNo;

                            // 見積金額と納期の表示
                            Nouki_S.Text = shortestNouki;
                            Price_U.Text = unitPrice > 0 ? unitPrice.ToString("#,0") : string.Empty;
                            Price_S.Text = sumPrice > 0 ? sumPrice.ToString("#,0") : string.Empty;

                            // 入力・編集エリアのボタン表示変更
                            BtnAddRecord.Text = "更 新";
                        }
                    }
                }
            }
            else if (String.Equals(e.CommandName, "DeleteEstimate"))
            {
                if (e.CommandArgument != null)
                {
                    // 行Noの取得
                    string rowNo = e.CommandArgument.ToString();

                    // 行Noの取得
                    string strResult = e.CommandArgument.ToString();

                    // DB接続文字列
                    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        List<SqlParameter> sqlParams = new List<SqlParameter>();
                        sqlParams.AddSqlParam(new SqlParameter("@SessionID", SqlDbType.VarChar, 30), ParameterDirection.Input).Value = (string)Session.SessionID;
                        sqlParams.AddSqlParam(new SqlParameter("@RowNo", SqlDbType.Int), ParameterDirection.Input).Value = rowNo;

                        // レコード取得
                        var dt = gCls.GetDataTable(con, "usp_ASP_EstimateDetailWork_del", sqlParams);

                        ListView1.DataSource = dt;
                        ListView1.DataBind();
                        con.Close();

                        // 合計金額を表示
                        DisplayTotalAmount();

                        // 入力・編集エリアのボタン表示変更
                        BtnAddRecord.Text = "追 加";
                    }
                }
            }
            else if (String.Equals(e.CommandName, "CopyEstimate"))
            {
                if (e.CommandArgument != null)
                {
                    // 行Noの取得
                    string rowNo = e.CommandArgument.ToString();

                    // DB接続文字列
                    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                    string material = "";
                    string kakou = "";
                    string kakou_T = "";
                    string kakou_A = "";
                    string kakou_B = "";
                    string size_T = "";
                    string size_A = "";
                    string size_B = "";

                    string kousa_T_U = "";
                    string kousa_T_L = "";
                    string kousa_A_U = "";
                    string kousa_A_L = "";
                    string kousa_B_U = "";
                    string kousa_B_L = "";

                    string mentori_4 = "";
                    string mentori_8 = "";

                    string suryou = "";

                    string customerNo = "";
                    string endUserNo = "";

                    // 見積発注データの取得(見積No指定の場合)
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_ASP_EstimateOrderEdit_get"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // SQLパラメータ設定
                            List<SqlParameter> sqlParameters = new List<SqlParameter>();
                            sqlParameters.AddSqlParam(new SqlParameter("@SessionID", SqlDbType.VarChar), ParameterDirection.Input).Value = (string)Session.SessionID;
                            sqlParameters.AddSqlParam(new SqlParameter("@RowNo", SqlDbType.Int), ParameterDirection.Input).Value = Int32.Parse(rowNo);
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
                                        material = sdr["材料コード"].ToString();
                                        kakou = sdr["加工仕様"].ToString();
                                        kakou_T = sdr["加工指示コードT"].ToString();
                                        kakou_A = sdr["加工指示コードA"].ToString();
                                        kakou_B = sdr["加工指示コードB"].ToString();
                                        size_T = sdr["仕上りサイズT"].ToString();
                                        size_A = sdr["仕上りサイズA"].ToString();
                                        size_B = sdr["仕上りサイズB"].ToString();

                                        kousa_T_U = sdr["加工公差_UT"].ToString();
                                        kousa_T_L = sdr["加工公差_LT"].ToString();
                                        kousa_A_U = sdr["加工公差_UA"].ToString();
                                        kousa_A_L = sdr["加工公差_LA"].ToString();
                                        kousa_B_U = sdr["加工公差_UB"].ToString();
                                        kousa_B_L = sdr["加工公差_LB"].ToString();

                                        mentori_4 = sdr["面取り量_4C"].ToString();
                                        mentori_8 = sdr["面取り量_8C"].ToString();

                                        suryou = sdr["製品数量"].ToString();

                                        customerNo = sdr["客先注番"].ToString();
                                        endUserNo = sdr["送り先注番"].ToString();

                                    }
                                }
                            }

                            // 見積明細を呼び出し
                            RowNo.Text = string.Empty;
                            Material.Text = material;
                            Kakou.Text = kakou;
                            Kakou_T.Text = kakou_T;
                            Kakou_A.Text = kakou_A;
                            Kakou_B.Text = kakou_B;
                            Size_T.Text = size_T;
                            Size_A.Text = size_A;
                            Size_B.Text = size_B;
                            Kousa_T_U.Text = FormatTolerance(kousa_T_U);
                            Kousa_T_L.Text = FormatTolerance(kousa_T_L);
                            Kousa_A_U.Text = FormatTolerance(kousa_A_U);
                            Kousa_A_L.Text = FormatTolerance(kousa_A_L);
                            Kousa_B_U.Text = FormatTolerance(kousa_B_U);
                            Kousa_B_L.Text = FormatTolerance(kousa_B_L);
                            Mentori_4.Text = FormatChamfering(mentori_4);
                            Mentori_8.Text = FormatChamfering(mentori_8);
                            Suryou.Text = suryou;
                            CustomerNo.Text = customerNo;
                            EndUserNo.Text = endUserNo;

                            // 見積金額と納期の表示
                            Nouki_S.Text = string.Empty;
                            Price_U.Text = string.Empty;
                            Price_S.Text = string.Empty;

                            // 入力・編集エリアのボタン表示変更
                            BtnAddRecord.Text = "追 加";
                        }
                    }
                }
            }
        }

        // 公差数値（符号付き小数第2位）
        private string FormatTolerance(string value)
        {
            if (decimal.TryParse(value, out decimal num))
            {
                return num.ToString("+0.00;-0.00;0.00"); // "+0.10", "-0.15", "0.00"
            }
            return value; // 数値変換できなかった場合は元の値を返す
        }

        // 面取り量数値（符号なし小数第2位）
        private string FormatChamfering(string value)
        {
            if (decimal.TryParse(value, out decimal num))
            {
                return num.ToString("0.00"); // "0.10", "1.25"
            }
            return value; // 数値変換できなかった場合は元の値を返す
        }

        protected void Input_Changed(object sender, EventArgs e)
        {
            /////////////////////////////////////////////////////
            ///　※　呼び出すたびにPostBackされるので
            ///　　　Client側のJavascriptに変更
            ///　　　現在未使用
            /////////////////////////////////////////////////////
            HandleInputChange((Control)sender);
        }

        private void HandleInputChange(Control control)
        {
            /////////////////////////////////////////////////////
            ///　※　呼び出すたびにPostBackされるので
            ///　　　Client側のJavascriptに変更
            ///　　　現在未使用
            /////////////////////////////////////////////////////
            // コントロール名を取得（必要なら処理をコントロールごとに分岐可能）
            string controlName = control.ID;

            // 必要に応じてコントロールごとの特別な処理を追加可能
            switch (controlName)
            {
                case "Material":
                    // 材料が変更された場合の特別な処理
                    break;

                case "Kakou_T":
                    // 加工指示Tが変更された場合の特別な処理
                    UpdateKakouList();
                    break;

                case "Kakou_B":
                    // 加工指示Bが変更された場合の特別な処理
                    UpdateKakouList();
                    break;

                case "Kakou_A":
                    // 加工指示Aが変更された場合の特別な処理
                    UpdateKakouList();
                    break;

                case "Suryou":
                    // 数量が変更された場合の特別な処理
                    break;

                case "Size_T":
                case "Size_A":
                case "Size_B":
                    // 寸法（T/A/B）が変更された場合の特別な処理
                    break;

                case "Kousa_T_U":
                case "Kousa_A_U":
                case "Kousa_B_U":
                case "Kousa_T_L":
                case "Kousa_A_L":
                case "Kousa_B_L":
                    // 公差（T/A/B）上限・下限　が変更された場合の特別な処理
                    break;

                case "Mentori_4":
                case "Mentori_8":
                    // 面取り量（4/8）が変更された場合の特別な処理
                    break;
            }

            // 追加・登録ボタンの非活性化
            BtnAddRecord.Enabled = false;
        }

        private void DebugLog(string message)
        {
            Debug.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss} - {message}");
        }
    }
}