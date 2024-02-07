using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Success : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        String customer = Request.QueryString["customer"];
        String invoiceNum = Request.QueryString["invoice"];        
        String orderNum = Request.QueryString["order"];
        String emailAdd = Request.QueryString["email"];

        if (customer == null) {
            hSuccess.InnerText = "FAIL";
            hOrderLine.InnerText = "You came hear manually; no order was submitted.";
            hInvoiceSent.InnerText = "There is no invoice to send.";
        } else {
            hSuccess.InnerText = "Success";
                        
            hInvoiceSent.InnerText = "Invoice " + invoiceNum + " has been sent to " + emailAdd + " for order confirmation.";
            hOrderLine.InnerText = "Order number " + orderNum + " has been submitted successfully.";
        }
        
    }
        
    
    protected void homeButton_Click(object sender, EventArgs e) {
        Response.Redirect("Default.aspx");
    }
}