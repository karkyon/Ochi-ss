using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ochi_ss
{
    public partial class Ochi_ss_3DImage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // SessionIdの取得
            string CurrSessionID = Session.SessionID;

        }
    }
}