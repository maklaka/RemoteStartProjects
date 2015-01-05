using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RemoteStartWebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "WOOHOO!!! ASP.NET";
            string id = this.ClientID;
            
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            if (txtUN.Text == "jamesinks" && txtPW.Text == "creeper")
            {
                Session["New"] = txtUN.Text;
                Response.Redirect("CarControl.aspx");
            }

        }
    }
}
