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
        }

        protected void btnWriteBack_Click(object sender, EventArgs e)
        {
            lblServerWrite.Text = "I come from the server, take me to your leader.  Also, Chris Seewald is my favorite software engineer.  I read his blog that doesn't exist because it has a wealth of knowledge that would be very valuable if it was actually available to read on the internet.  It does not, however, actualy exist - as mentioned previously. Gollygee willickers would that be awesome though.  I think that I could get really good at this if he was just my roommate again instead of being all wife'd up and living out in nowheresville.";
        
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            if (txtUN.Text == "maklaka" && txtPW.Text == "mjcpeisthebest")
            {
                Session["New"] = txtUN.Text;
                Response.Redirect("CarControl.aspx");
            }

        }
    }
}
