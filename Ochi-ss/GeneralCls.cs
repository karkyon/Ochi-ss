using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.IO;

namespace Ochi_ss
{
    // 注文詳細クラス
    [Serializable] // セッションで使用するためにシリアライズ
    public class OrderDetail
    {
        public int RowNo { get; set; }
        public string EstimateNo { get; set; }
        public string SessionID { get; set; }

        public OrderDetail(int rowNo, string estimateNo, string sessionId)
        {
            RowNo = rowNo;
            EstimateNo = estimateNo;
            SessionID = sessionId;
        }
    }

    public class GeneralCls
    {
        public DataSet GetDataSet(SqlConnection conn, string storedProcName, string tableName, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand(storedProcName, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddRange(parameters);

            var result = new DataSet();
            var dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(result, tableName);

            return result;
        }

        public DataTable GetDataTable(SqlConnection conn, string storedProcName, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand(storedProcName, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddRange(parameters);

            var result = new DataTable();
            var dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(result);

            return result;
        }

        public DataTable GetDataTable(SqlConnection conn, string storedProcName, List<SqlParameter> parameters)
        {
            var cmd = new SqlCommand(storedProcName, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddParams(parameters);

            var result = new DataTable();
            var dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(result);

            return result;
        }

        public static string BuildDebugQuery(string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {
            var debugQuery = new System.Text.StringBuilder();
            debugQuery.AppendLine($"EXEC @return_value = [{storedProcedureName}]");

            foreach (var param in parameters)
            {
                string paramName = param.ParameterName;
                object paramValue = param.Value ?? "NULL";
                string formattedValue = param.SqlDbType == SqlDbType.VarChar ||
                                        param.SqlDbType == SqlDbType.NVarChar ||
                                        param.SqlDbType == SqlDbType.Char ||
                                        param.SqlDbType == SqlDbType.NChar ||
                                        param.SqlDbType == SqlDbType.Text ||
                                        param.SqlDbType == SqlDbType.NText
                    ? $"N'{paramValue}'"
                    : paramValue.ToString();

                // Null値を正しくフォーマット
                if (paramValue == DBNull.Value || paramValue == "NULL")
                {
                    formattedValue = "NULL";
                }

                debugQuery.AppendLine($"\t{paramName} = {formattedValue},");
            }

            // 末尾のカンマを削除してクエリを完成
            return debugQuery.ToString().TrimEnd(',', '\n', '\r') + ";";
        }

        /// <summary>
        /// テキストボックスやリスト、チェックボックスの入力状態をチェックするValidation関数
        /// 使用例
        /// bool isValid = ValidateControls(
        ///     out List<string> errorMessages,
        ///     (txtCustomerName, "得意先名は必須項目です。", value => !string.IsNullOrWhiteSpace(value)),
        ///     (ddlCategory, "カテゴリを選択してください。", value => value != "0"),
        ///     (chkAgree, "利用規約に同意してください。", value => bool.Parse(value))
        /// );
        /// </summary>
        //
        public bool ValidateControls(out List<string> errorMessages, params (Control control, string errorMessage, Func<string, bool> validationFunc)[] validations)
        {
            errorMessages = new List<string>();
            bool isValid = true;

            foreach (var validation in validations)
            {
                var control = validation.control;
                var errorMessage = validation.errorMessage;
                var validationFunc = validation.validationFunc;

                if (control is TextBox textBox)
                {
                    string inputValue = textBox.Text;
                    if (validationFunc != null && !validationFunc(inputValue))
                    {
                        errorMessages.Add(errorMessage);
                        textBox.BackColor = System.Drawing.Color.LightCoral; // エラー時は赤色
                        isValid = false;
                    }
                    else
                    {
                        textBox.BackColor = System.Drawing.Color.White; // 正常時は白色
                    }
                }
                else if (control is DropDownList dropDownList)
                {
                    if (validationFunc != null && !validationFunc(dropDownList.SelectedValue))
                    {
                        errorMessages.Add(errorMessage);
                        dropDownList.BackColor = System.Drawing.Color.LightCoral;
                        isValid = false;
                    }
                    else
                    {
                        dropDownList.BackColor = System.Drawing.Color.White;
                    }
                }
                else if (control is CheckBox checkBox)
                {
                    if (validationFunc != null && !validationFunc(checkBox.Checked.ToString()))
                    {
                        errorMessages.Add(errorMessage);
                        checkBox.BackColor = System.Drawing.Color.LightCoral;
                        isValid = false;
                    }
                    else
                    {
                        checkBox.BackColor = System.Drawing.Color.Transparent;
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// ValidateControlsでエラーがある最初のコントロールにフォーカスを当てる
        /// 使用例
        /// SetFocusToFirstError(new List<Control> { txtCustomerName, ddlCategory, chkAgree });
        /// </summary>
        public void SetFocusToFirstError(IEnumerable<Control> controls)
        {
            foreach (var control in controls)
            {
                if (control is WebControl webControl && webControl.BackColor == System.Drawing.Color.LightCoral)
                {
                    webControl.Focus();
                    return;
                }
            }
        }
    }

    public class DebugHelper
    {
        /// <summary>
        /// データセットの内容をDebug.WriteLineに出力します。
        /// </summary>
        /// <param name="dataSet">出力するデータセット</param>
        public static void PrintDataSetToDebug(DataSet dataSet)
        {
            if (dataSet == null)
            {
                Debug.WriteLine("データセットがnullです。");
                return;
            }

            Debug.WriteLine("=== データセットの内容 ===");
            foreach (DataTable table in dataSet.Tables)
            {
                Debug.WriteLine($"テーブル名: {table.TableName}");
                Debug.WriteLine("列情報:");
                foreach (DataColumn column in table.Columns)
                {
                    Debug.Write($"{column.ColumnName}\t");
                }
                Debug.WriteLine(""); // 列ヘッダーの改行

                Debug.WriteLine("データ:");
                foreach (DataRow row in table.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        Debug.Write($"{item}\t");
                    }
                    Debug.WriteLine(""); // 行データの改行
                }
                Debug.WriteLine(""); // テーブル間の区切り
            }
            Debug.WriteLine("=========================");
        }


        /// <summary>
        /// データセットの内容をCSV形式でローカルファイルに保存します。
        /// </summary>
        /// <param name="dataSet">保存するデータセット</param>
        /// <param name="directoryPath">ファイルを保存するディレクトリのパス</param>
        /// <param name="filePrefix">出力ファイルのプレフィックス（任意）</param>
        public static void SaveDataSetToLocalFile(DataSet dataSet, string directoryPath, string filePrefix = "DebugOutput")
        {
            if (dataSet == null)
            {
                throw new ArgumentNullException(nameof(dataSet), "データセットがnullです。");
            }

            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new ArgumentException("ディレクトリパスが無効です。", nameof(directoryPath));
            }

            // ディレクトリが存在しない場合は作成
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (DataTable table in dataSet.Tables)
            {
                // ファイル名を生成
                string fileName = $"{filePrefix}_{table.TableName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string filePath = Path.Combine(directoryPath, fileName);

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // 列ヘッダーを書き出し
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        writer.Write(table.Columns[i].ColumnName);
                        if (i < table.Columns.Count - 1) writer.Write(","); // カンマ区切り
                    }
                    writer.WriteLine();

                    // データ行を書き出し
                    foreach (DataRow row in table.Rows)
                    {
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            writer.Write(row[i]?.ToString()?.Replace(",", " ")); // カンマをスペースに置換
                            if (i < table.Columns.Count - 1) writer.Write(","); // カンマ区切り
                        }
                        writer.WriteLine();
                    }
                }

                Console.WriteLine($"データテーブル '{table.TableName}' がファイル '{filePath}' に保存されました。");
            }
        }

        /// <summary>
        /// デバッグ用データを準備し、辞書形式で出力
        /// </summary>
        /// <param name="dataSet">確認対象のデータセット</param>
        /// <returns>デバッグ用辞書</returns>
        public static Dictionary<string, string> PrepareDebugData(DataSet dataSet)
        {
            var debugData = new Dictionary<string, string>();

            if (dataSet == null || dataSet.Tables.Count == 0)
            {
                debugData["Error"] = "データセットが空です。";
                return debugData;
            }

            foreach (DataTable table in dataSet.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        string key = $"{table.TableName}.{column.ColumnName}";
                        string value = row[column]?.ToString() ?? "NULL";
                        debugData[key] = value;
                    }
                }
            }

            return debugData;
        }
    }

    public static class SqlParameterExtension
    {
        /// <summary>
        /// ListにSqlParameterを追加し、追加したSqlParameterを返す
        /// 使用例
        /// List<SqlParameter> sqlParameters  = new List<SqlParameter>();
        /// sqlParameters.AddSqlParam(new SqlParameter("@type1", SqlDbType.VarChar)).Value = "S";
        /// </summary>
        public static SqlParameter AddSqlParam(this List<SqlParameter> list, SqlParameter parameter, ParameterDirection paramDirection)
        {
            parameter.Direction = paramDirection;
            list.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// ListにSqlParameterを追加し、追加したSqlParameterを返す
        /// paramScale       桁数
        /// paramPrecision   精度
        /// 使用例
        /// List<SqlParameter> sqlParameters  = new List<SqlParameter>();
        /// sqlParameters.AddSqlParam(new SqlParameter("@type1", SqlDbType.VarChar)).Value = "S";
        /// </summary>
        public static SqlParameter AddSqlParam(this List<SqlParameter> list, SqlParameter parameter, ParameterDirection paramDirection, byte paramScale, byte paramPrecision)
        {
            parameter.Scale = paramScale;
            parameter.Precision = paramPrecision;
            parameter.Direction = paramDirection;
            list.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// SqlParameterCollectionにList<SqlParameter>を追加
        /// </summary>
        public static void AddParams(this SqlParameterCollection collection, List<SqlParameter> list)
        {
            foreach (SqlParameter parameter in list)
            {
                collection.Add(parameter);
            }
        }
    }
}