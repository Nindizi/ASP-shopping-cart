using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Contact : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }

    protected void SendEmail_Click(object sender, EventArgs e) {
        String mName = sendername.Value;
        String mEmail = senderemail.Value;
        String mSubject = sendersubject.Value;
        String mBody = senderbody.Value;

        if(mEmail.Contains("@")) {

            //send the email using the text boxes

        } else {
            lblStatus.InnerText = "Bad email address.";
        }
    }
}