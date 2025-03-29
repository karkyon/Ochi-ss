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
    public partial class OrderConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SelectedOrders"] != null)
                {
                    List<OrderDetail> selectedOrders = (List<OrderDetail>)Session["SelectedOrders"];
                    BindOrderDetails(selectedOrders);
                }

                // セッション値の取得
                string sessionID = Session.SessionID;
                string estOrderNo = GetSessionValue("EstOrderNo");
                string customerCd = GetSessionValue("CustomerCd");

                // DB接続
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ASP_EstimateHeader_get", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SessionID", sessionID);
                        cmd.Parameters.AddWithValue("@WOEstimateNo", estOrderNo);
                        con.Open();

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                while (sdr.Read())
                                {
                                    EstOrderNo.Text = sdr["WO見積No"].ToString();
                                    OrderNo.Text = sdr["見積管理番号"].ToString();
                                    InpDate.Text = sdr["登録日付"].ToString();
                                    EstDate.Text = sdr["見積日付"].ToString();
                                    CstCode.Text = sdr["得意先コード"].ToString();
                                    CustomerName.Text = sdr["得意先名"].ToString();
                                    CustomreCharge.Text = sdr["得意先担当者名"].ToString();
                                    DistCompCd.Text = sdr["出荷先コード"].ToString();
                                    DistCompName.Text = sdr["出荷先名"].ToString();
                                    DistCompPost.Text = sdr["出荷先部署名"].ToString();
                                    DistCharge.Text = sdr["出荷先担当者名"].ToString();
                                    DistCompZipCd.Text = sdr["出荷先郵便番号"].ToString();
                                    DistCompAddress.Text = sdr["出荷先住所１"].ToString();
                                    DistTel.Text = sdr["出荷先電話番号"].ToString();
                                    DistFax.Text = sdr["出荷先FAX番号"].ToString();
                                    // ShippingMethod.SelectedValue = sdr["配送区分"].ToString();
                                }
                            }
                        }
                    }
                }

                // 合計金額を表示
                DisplayTotalAmount();

            }
        }

        private void BindOrderDetails(List<OrderDetail> orders)
        {
            DataTable dt = ExecuteStoredProcedure(orders);
            ListView1.DataSource = dt;
            ListView1.DataBind();
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

        private DataTable ExecuteStoredProcedure(List<OrderDetail> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return new DataTable(); // 空の DataTable を返す
            }

            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("usp_ASP_EstimateDetailWork_OrderList_get", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // テーブル型パラメータを作成
                DataTable orderTable = new DataTable();
                orderTable.Columns.Add("RowNo", typeof(int));
                orderTable.Columns.Add("SessionID", typeof(string));
                orderTable.Columns.Add("EstimateNo", typeof(string));

                foreach (var order in orders)
                {
                    orderTable.Rows.Add(order.RowNo, order.SessionID, order.EstimateNo);
                }

                SqlParameter param = cmd.Parameters.AddWithValue("@OrderDetails", orderTable);
                param.SqlDbType = SqlDbType.Structured;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }


        private string GetSessionValue(string key)
        {
            return Session[key]?.ToString() ?? string.Empty;
        }

        protected void BtnReturnEdit_Click(object sender, EventArgs e)
        {
            // 見積No
            string estOrderNo = GetSessionValue("EstOrderNo");
            Session["EditMode"] = "Edit";
            Response.Redirect("EstOrder.aspx");
        }

        protected void BtnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // 発注処理を実行（データベースへの登録など）
                ProcessOrder();

                // サンクスページに遷移
                Response.Redirect("OrderThanx.aspx", false);
            }
            catch (Exception ex)
            {
                // 予期しない値が来た場合のエラーハンドリング
                DebugLog("エラー発生: " + ex.Message);

                // エラーメッセージを表示
                lblDubugText1.Text = "発注処理中にエラーが発生しました。";
            }
        }

        protected void ProcessOrder()
        {
            try
            {
                // セッションデータの取得
                List<OrderDetail> orders = new List<OrderDetail>();
                if (Session["SelectedOrders"] is List<OrderDetail> sessionOrders)
                {
                    orders = sessionOrders;
                }

                string sessionID = Session.SessionID;
                string estOrderNo = GetSessionValue("EstOrderNo") ?? string.Empty;

                // データがない場合は処理をスキップ
                if (orders.Count == 0)
                {
                    DebugLog("注文データが空のため、DB処理をスキップしました。");
                    return;
                }

                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ASP_OrderComplete_upd", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // DataTable の作成
                        DataTable orderTable = new DataTable();
                        orderTable.Columns.Add("RowNo", typeof(int));
                        orderTable.Columns.Add("SessionID", typeof(string));
                        orderTable.Columns.Add("EstimateNo", typeof(string));

                        foreach (var order in orders)
                        {
                            orderTable.Rows.Add(
                                order.RowNo,
                                order.SessionID ?? string.Empty,
                                order.EstimateNo ?? string.Empty
                            );
                        }

                        // パラメータ設定
                        SqlParameter param = cmd.Parameters.AddWithValue("@OrderDetails", orderTable);
                        param.SqlDbType = SqlDbType.Structured;

                        cmd.Parameters.Add("@SessionID", SqlDbType.NVarChar, 30).Value = sessionID;
                        cmd.Parameters.Add("@WOEstimateNo", SqlDbType.NVarChar, 10).Value = estOrderNo;

                        // OUTPUTパラメータの設定
                        SqlParameter outputParam = new SqlParameter("@WOOrderNo", SqlDbType.NVarChar, 10)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        // データベース処理
                        con.Open();
                        cmd.ExecuteNonQuery();

                        // OUTPUTパラメータの取得
                        string generatedOrderNo = outputParam.Value.ToString();
                        DebugLog($"生成された注文番号: {generatedOrderNo}");
                        Session["OrderNo"] = generatedOrderNo;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // SQL関連のエラー処理
                DebugLog($"SQLエラーが発生しました: {sqlEx.Message}");
            }
            catch (NullReferenceException nullEx)
            {
                // null 参照例外の処理（セッションデータの欠落など）
                DebugLog($"Null参照エラー: {nullEx.Message}");
            }
            catch (Exception ex)
            {
                // その他のエラー処理
                DebugLog($"予期しないエラーが発生しました: {ex.Message}");
            }
        }

        private void DebugLog(string message)
        {
            Debug.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss} - {message}");
        }
    }
}
