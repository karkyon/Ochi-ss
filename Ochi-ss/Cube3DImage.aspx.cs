using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ochi_ss
{
    public partial class Cube3DImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["sizeT"] != null) dimT.Text = Session["sizeT"].ToString();
                if (Session["sizeA"] != null) dimA.Text = Session["sizeA"].ToString();
                if (Session["sizeB"] != null) dimB.Text = Session["sizeB"].ToString();
            }
        }
    }
}