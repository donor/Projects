using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using System.Net.Mail;
using System.Web.Profile;


namespace Rezerwacje.Account
{
    public partial class ActivateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (String.IsNullOrEmpty(Request.Params["Id"]))
            {
               Response.Redirect("http://rezerwacje.dyndns.org/rezerwacje/ErrorPage.aspx");
            }
            else
            {
             
                try
                {
                    Guid userId = new Guid(Request.Params["Id"]);
                    MembershipUser user = Membership.GetUser(userId);
                    
                    user.IsApproved = true;
                 
                    Membership.UpdateUser(user);
                   
                    
                    Response.Redirect("http://rezerwacje.dyndns.org/rezerwacje/Welcome.aspx", true);
                }
                catch (Exception exp)
                {                    
                    Response.Redirect("http://rezerwacje.dyndns.org/rezerwacje/ErrorPage.aspx");
                    throw new Exception("Błąd: Nie można dodać imprezy do Koszyka - " + exp.Message.ToString(), exp);
                }
                finally
                {
                  Response.Redirect("http://rezerwacje.dyndns.org/rezerwacje/Welcome.aspx");
                }
            }
            Response.Redirect("http://rezerwacje.dyndns.org/Default/Default.aspx", true);
        }

    }
}