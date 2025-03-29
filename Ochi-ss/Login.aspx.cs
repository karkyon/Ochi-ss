using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using Ochi_ss;

namespace Ochi_ss
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        protected void ValidateUser(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            int userId = 0;
            string userName = username;
            string csutomerCd = "";
            string csutomerName = "";
            string chargeName = "";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ASP_Validate_User_get"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // SQLパラメータ設定
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    sqlParameters.AddSqlParam(new SqlParameter("@Username", SqlDbType.VarChar), ParameterDirection.Input).Value = username;
                    sqlParameters.AddSqlParam(new SqlParameter("@Password", SqlDbType.VarChar), ParameterDirection.Input).Value = password;
                    sqlParameters.AddSqlParam(new SqlParameter("@CustomerCd", SqlDbType.VarChar, 6), ParameterDirection.Output).Value = "";
                    sqlParameters.AddSqlParam(new SqlParameter("@CustomerName", SqlDbType.VarChar, 100), ParameterDirection.Output).Value = "";
                    sqlParameters.AddSqlParam(new SqlParameter("@ChargeName", SqlDbType.VarChar, 100), ParameterDirection.Output).Value = "";
                    cmd.Parameters.AddParams(sqlParameters);
                    cmd.Connection = con;
                    con.Open();

                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                    csutomerCd = Convert.ToString(cmd.Parameters["@CustomerCd"].Value);
                    csutomerName = Convert.ToString(cmd.Parameters["@CustomerName"].Value);
                    chargeName = Convert.ToString(cmd.Parameters["@ChargeName"].Value);

                    con.Close();
                }
                switch (userId)
                {
                    case -1:
                        dvMessage.Visible = true;
                        lblMessage.Text = "　ユーザー名またはパスワードが無効です。";
                        break;
                    case -2:
                        dvMessage.Visible = true;
                        lblMessage.Text = "　アカウントはアクティブ化されていません。";
                        break;
                    default:
                        if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                        {
                            FormsAuthentication.SetAuthCookie(username, chkRememberMe.Checked);
                            Session["CustomerCd"] = csutomerCd;
                            Session["CustomerName"] = csutomerName;
                            Session["UserName"] = username;
                            Session["ChargeName"] = chargeName;

                            Response.Redirect(Request.QueryString["ReturnUrl"]);
                        }
                        else
                        {
                            Session["CustomerCd"] = csutomerCd;
                            Session["CustomerName"] = csutomerName;
                            Session["UserName"] = username;
                            Session["ChargeName"] = chargeName;

                            FormsAuthentication.RedirectFromLoginPage(username, chkRememberMe.Checked);
                        }

                        break;
                }
            }
        }
    }
}