using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class Cart : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

        if(Page.IsPostBack == false) {
            goHome.Visible = false;
            String customer = Request.QueryString["customer"];
            customerID.InnerText = customer;
            if (customer == null) {
                lblStatus.Text = "There is no shopping cart yet.";
                goHome.Visible = true; //go home button visibility
                keepShopping.Visible = false; //keep shopping button visibility
                checkOut.Visible = false; //check out button visibility
                clearCartButton.Visible = false;//clear cart button visibility
                deleteLineItemButton.Visible = false;
                LineItemTextBox.Visible = false;
                textBoxLabelText.Visible = false;
            } else {

                if (isCartEmpty(customer)) {
                    lblStatus.Text = "Your shopping cart is EMPTY";
                    goHome.Visible = true; //go home button visibility
                    keepShopping.Visible = false; //keep shopping button visibility
                    checkOut.Visible = false; //check out button visibility
                    clearCartButton.Visible = false;//clear cart button visibility
                    deleteLineItemButton.Visible = false;
                    LineItemTextBox.Visible = false;
                    textBoxLabelText.Visible = false;
                } else {
                    Table cart = getCart(customer);
                    cartTable.Controls.Add(cart);
                }
            } 
        } else { //if postback == true
            //Server.Transfer("Cart.aspx");
        }
    }

    protected void Clear_Click(object sender, EventArgs e) {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        String customer = customerID.InnerText;
        String transferString = "Cart.aspx?customer=" + customer;
        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();
            cmd = new SqlCommand("TRUNCATE TABLE ShoppingCart;", cn);
            cmd.ExecuteNonQuery();
        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            if (cn != null) { //if SQL connection is not null, close
                cn.Close();
            }
        }
        Server.Transfer(transferString);
    }

    protected void CheckOut_Click(object sender, EventArgs e) {
        String redirectString = "Checkout.aspx?customer=" + customerID.InnerText;
        Response.Redirect(redirectString);
    }    

    protected void GoHome_Click(object sender, EventArgs e) {
        Response.Redirect("Default.aspx");
    }

    private Table getCart(String customerID) {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataReader drItem = null;
        Table cartTable = new Table();
        TableRow cartLabelRow = new TableRow();
        TableCell[] cartCells = new TableCell[7];

        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();

            //set up table column labels
            cartCells[0] = new TableCell { Text = "Line Item" };
            cartCells[0].HorizontalAlign = HorizontalAlign.Center;
            cartCells[1] = new TableCell { Text = "Item" };
            cartCells[2] = new TableCell { Text = "U/P" };
            cartCells[2].HorizontalAlign = HorizontalAlign.Center;
            cartCells[3] = new TableCell { Text = "Qty" };
            cartCells[3].HorizontalAlign = HorizontalAlign.Center;
            cartCells[4] = new TableCell { Text = "T/P" };
            cartCells[4].HorizontalAlign = HorizontalAlign.Center;
            cartCells[5] = new TableCell { Text = "Weight" };
            cartCells[5].HorizontalAlign = HorizontalAlign.Center;
            cartCells[6] = new TableCell { Text = "" };            

            cartLabelRow.Cells.AddRange(cartCells);
            cartTable.Rows.Add(cartLabelRow);

            //set up to read from database
            cmd = new SqlCommand("SELECT * FROM ShoppingCart WHERE CustNum = @CUST", cn);
            cmd.Parameters.AddWithValue("@CUST", customerID);
            drItem = cmd.ExecuteReader();

            //read shopping cart table and put into table cells
            while (drItem.Read()) {
                TableRow tempRow = new TableRow();
                cartCells[0] = new TableCell { Text = "<span id='" + drItem["LineItem"].ToString() + "' runat='server'>" + drItem["LineItem"].ToString() + "</span>" };
                cartCells[0].HorizontalAlign = HorizontalAlign.Center;
                cartCells[1] = new TableCell { Text = drItem["Title"].ToString() };
                cartCells[1].Width = 175;
                cartCells[2] = new TableCell { Text = "$" + drItem["UnitPrice"].ToString() };
                cartCells[2].HorizontalAlign = HorizontalAlign.Center;
                cartCells[3] = new TableCell { Text = drItem["Qty"].ToString() };
                cartCells[3].HorizontalAlign = HorizontalAlign.Center;
                cartCells[4] = new TableCell { Text = "$" + drItem["TotalPrice"].ToString() };
                cartCells[4].HorizontalAlign = HorizontalAlign.Center;
                cartCells[5] = new TableCell { Text = drItem["TotWeight"].ToString() };
                cartCells[5].HorizontalAlign = HorizontalAlign.Center;

                //add modify button
                cartCells[6] = new TableCell { Text = "<input type='button' class='nav_btn' name='mod' value='CHANGE' runat='server'>" };
                //add cells to row, then add row to table
                tempRow.Cells.AddRange(cartCells);
                cartTable.Rows.Add(tempRow);
            }

        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            if (cn != null) { //if SQL connection is not null, close
                cn.Close();
            }
        }

        return cartTable;
    }

    private Boolean isCartEmpty(String customerID) {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataReader drItem = null;
        Boolean returnMe = true;
        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();
            cmd = new SqlCommand("SELECT * FROM ShoppingCart;", cn);
            drItem = cmd.ExecuteReader();
            while (drItem.Read()) {
                returnMe = false;
            }

        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            if (cn != null) { //if SQL connection is not null, close
                cn.Close();
            }
        }
        return returnMe;
    }

    protected void DeleteItem_Click(object sender, EventArgs e) {
        String lineItem = LineItemTextBox.Text;
        String transferString = "Cart.aspx?customer=" + customerID.InnerText;
        if (!LineItemExists(lineItem)) {
            nope.InnerHtml = "That line item doesn't exist";
        } else {
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try {
                cn = new SqlConnection();
                cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
                cn.Open();
                cmd = new SqlCommand("DELETE FROM ShoppingCart WHERE LineItem=@LINE", cn);
                cmd.Parameters.AddWithValue("@LINE", lineItem);
                cmd.ExecuteNonQuery();
            } catch (Exception err) {
                lblStatus.Text = err.Message;
            } finally {
                if (cn != null) { //if SQL connection is not null, close
                    cn.Close();
                }
            }
            Server.Transfer(transferString);
        }
    }

    private Boolean LineItemExists(String lineItem) {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataReader drItem = null;
        Boolean returnMe = true;
        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();
            cmd = new SqlCommand("SELECT * FROM ShoppingCart where LineItem='@LINE'", cn);
            cmd.Parameters.AddWithValue("@LINE", lineItem);

            drItem = cmd.ExecuteReader();
            while (drItem.Read()) {
                returnMe = true;
            }

        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            if (cn != null) { //if SQL connection is not null, close
                cn.Close();
            }
        }

        return returnMe;
    }
}