using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;

namespace V1Requestor
{
    public partial class SubmitEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params.Count == 0)
            {
                GetEmailFromCookie();
            }
        }

        private void GetEmailFromCookie()
        {
            HttpCookie emailCookie = Request.Cookies["RequestorEmailCookie"];

            if (emailCookie == null)
            {
                Helpers.EmailAddress = String.Empty;
            }
            else
            {
                Helpers.EmailAddress = emailCookie.Values["emailaddress"];
                Response.Redirect("Default.aspx");
            }

        }

        protected void SubmitEmailAddress(object sender, EventArgs e)
        {
            string emailAddress = emailaddress.Text.ToString();
            if (!string.IsNullOrEmpty(emailAddress))
            {
                //create a cookie
                HttpCookie emailCookie = new HttpCookie("RequestorEmailCookie");
                emailCookie.Values.Add("emailaddress", emailaddress.Text.ToString());
                emailCookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(emailCookie);
                Response.Redirect("Default.aspx");
            }
        }
    }
}