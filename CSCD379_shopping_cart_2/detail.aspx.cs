using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class Detail : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

        if (Page.IsPostBack == false) {

            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataReader drItem = null;

            String SKU = Request.QueryString["item"];


            //start strings for inner HTML placement
            String hImages = "";
            String hTitle = "";
            String hCost = "";
            String hDescription = "";
            String hOnHand = "";

            //strings for populating data from the database
            String dInstImage = "";
            String dItemImage = "";
            String dMfrImage = "";
            String dDescription = "";
            String dOhQty = "";
            String dTitle = "";
            String dUnitPrice = "";
            String dWeight = "";            

            try {

                cn = new SqlConnection();
                cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
                cn.Open();

                cmd = new SqlCommand("SELECT * FROM Inventory WHERE SKU = @SKU", cn);
                cmd.Parameters.AddWithValue("@SKU", SKU);
                drItem = cmd.ExecuteReader();

                while (drItem.Read()) {
                    dTitle = drItem["Title"].ToString();
                    dInstImage = drItem["InstImage"].ToString();
                    dItemImage = drItem["SmallImage"].ToString();
                    dMfrImage = drItem["MfrImage"].ToString();
                    dDescription = drItem["Description"].ToString();
                    dOhQty = drItem["OnHand"].ToString();
                    dUnitPrice = drItem["UnitPrice"].ToString();
                    dWeight = drItem["Weight"].ToString();
                }

                String.Format("{0:C2}", dUnitPrice);

                hImages += "<img class='card-img-top img-fluid' src='img/items/" + dItemImage + "' alt=''>\n"
                    + "<img class='card-img-top img-fluid' src='img/instructors/" + dInstImage + "' alt=''>\n"
                    + "<img class='card-img-top img-fluid' src='img/mfrs/" + dMfrImage + "' alt=''>\n"
                    ;

                hTitle = dTitle;
                hCost = "$" + dUnitPrice;
                hDescription = dDescription;
                hOnHand = dOhQty;

                QtyList.Items.Add("1");
                QtyList.Items.Add("2");
                QtyList.Items.Add("3");
                QtyList.Items.Add("4");
                QtyList.Items.Add("5");

            } catch (Exception err) {
                lblStatus.Text = err.Message;
            } finally {
                if (cn != null) {
                    cn.Close();
                }
            }
            //place inner HTML setting here
            detail_pics.InnerHtml = hImages;
            title.InnerHtml = hTitle;
            cost.InnerHtml = hCost;
            long_description.InnerHtml = hDescription;
            instock.InnerHtml = hOnHand;

            wWeight.InnerHtml = dWeight;
            wSKU.InnerHtml = SKU;

            wWeight.Visible = false;
            wSKU.Visible = false;
        } //end if not postback

    }

    protected void AddToCart_Click(object sender, EventArgs e) {
        //TODO send data to database to make a new shopping cart, then redirect to cart.aspx
        SqlConnection cn = null;
        SqlCommand cmd = null;
        
        String uQuantity = QtyList.SelectedItem.Text;
        int cQty = Int16.Parse(uQuantity);
        int iQty = Int16.Parse(instock.InnerText);

        if(cQty > iQty) {
            lblStatus.Text = "You can't add more than the on hand quantity to your cart.";
        } else {
            String customer = "1000";
            String uSKU = wSKU.InnerText;
            String uUnitPrice = cost.InnerText;
            uUnitPrice = uUnitPrice.Replace("$", "");

            Decimal cUnitPrice = Decimal.Parse(uUnitPrice);


            Decimal cTotalPrice = cUnitPrice * cQty;
            String uTotalPrice = cTotalPrice.ToString();

            String uPickedItem = title.InnerText;

            String cWeight = wWeight.InnerText;
            int cWghtTot = Int16.Parse(cWeight);
            int uWeight = cWghtTot * cQty;
            String uLineItemTotalWeight = uWeight.ToString();

            try {
                cn = new SqlConnection();
                cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
                cn.Open();

                cmd = new SqlCommand("Insert into ShoppingCart ([CartNum],[CustNum],[SKU],[Qty],[UnitPrice],[TotalPrice],[Title],[TotWeight]) values(@Cart,@Cust,@SKU,@Qty,@UP,@TP,@TITLE,@TOTWT);", cn);
                cmd.Parameters.AddWithValue("@Cart", "1");
                cmd.Parameters.AddWithValue("@Cust", customer);
                cmd.Parameters.AddWithValue("@SKU", uSKU);
                cmd.Parameters.AddWithValue("@Qty", uQuantity);
                cmd.Parameters.AddWithValue("@UP", uUnitPrice);
                cmd.Parameters.AddWithValue("@TP", uTotalPrice);
                cmd.Parameters.AddWithValue("@TITLE", uPickedItem);
                cmd.Parameters.AddWithValue("@TOTWT", uLineItemTotalWeight);

                cmd.ExecuteNonQuery();
            } catch (Exception err) {
                lblStatus.Text = err.Message;
            } finally {
                if (cn != null) {
                    cn.Close();
                }
            }

            //send to cart page
            Response.Redirect("Cart.aspx?customer=1000");
        }        
        
    }//end of addtocart_click
}