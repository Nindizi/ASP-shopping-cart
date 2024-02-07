using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Net.Mail;
using System.Data.SqlTypes;
using System.Data;

public partial class Checkout : System.Web.UI.Page {

    private const Decimal SHIPPING_COST = 0.43M;
    protected void Page_Load(object sender, EventArgs e) {

        if(IsPostBack == false) {
            
            String customer = Request.QueryString["customer"];
            hCustomer.InnerText = customer;
            String addressText = "";

            String[] customerData = getCustomerInfo(customer);

            addressText = "<h4>" + customerData[0] + "</h4>\n" + "<p>" + customerData[1] + "</p>\n" + "<h4>Email And Address:</h4>\n";
                        
            //make html to render inside field
            String addressInnerText = addressText;

            //fill in text to field
            address.InnerHtml = addressInnerText;
            
            //post shopping cart
            Table cart = getCart(customer);
            WorkingCart.Controls.Add(cart);

            //store customer number for later use in this code behind
            hCustomer.Visible = false;
            hCustomer.InnerText = customer;
            
        } //if not postback

    }
    
    private Table getCart(String customerID) {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataReader drItem = null;

        Table cartTable = new Table();
        TableRow cartLabelRow = new TableRow();
        TableRow subTotalRow = new TableRow();
        TableRow shippinglRow = new TableRow();
        TableRow cartTotalRow = new TableRow();
        TableCell[] cartCells = new TableCell[4];        

        String lineWeight = "";
        Decimal totalWeight = 0.00m;
        Decimal totalShip = 0.00m;
        Decimal totalCost = 0.00m;

        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();

            //String[] itemStuff = getCartInfo(customerID);

            //set up table column labels
            cartCells[0] = new TableCell { Text = "Item   " };
            cartCells[1] = new TableCell { Text = "   U/P " };
            cartCells[1].HorizontalAlign = HorizontalAlign.Center;
            cartCells[2] = new TableCell { Text = " Qty " };
            cartCells[2].HorizontalAlign = HorizontalAlign.Center;
            cartCells[3] = new TableCell { Text = " T/P    " };
            cartCells[3].HorizontalAlign = HorizontalAlign.Center;
            
            cartLabelRow.Cells.AddRange(cartCells);
            cartTable.Rows.Add(cartLabelRow);

            //set up to read from database
            cmd = new SqlCommand("SELECT * FROM ShoppingCart WHERE CustNum = @CUST", cn);
            cmd.Parameters.AddWithValue("@CUST", customerID);
            drItem = cmd.ExecuteReader();

            //read shopping cart table and put into table cells
            while (drItem.Read()) {
                TableRow tempRow = new TableRow();
                
                cartCells[0] = new TableCell { Text = drItem["Title"].ToString() };
                cartCells[0].Width = 175;
                cartCells[1] = new TableCell { Text = "$" + drItem["UnitPrice"].ToString() };
                cartCells[1].HorizontalAlign = HorizontalAlign.Center;
                cartCells[2] = new TableCell { Text = drItem["Qty"].ToString() };
                cartCells[2].HorizontalAlign = HorizontalAlign.Center;
                cartCells[3] = new TableCell { Text = "$" + drItem["TotalPrice"].ToString() };
                cartCells[3].HorizontalAlign = HorizontalAlign.Center;
                lineWeight = drItem["TotWeight"].ToString();
                totalCost += Decimal.Parse(drItem["TotalPrice"].ToString());
                totalWeight = Decimal.Parse(lineWeight);
                totalShip += Decimal.Multiply(totalWeight, SHIPPING_COST);
                //add cells to row, then add row to table
                tempRow.Cells.AddRange(cartCells);
                cartTable.Rows.Add(tempRow);
            }

            //cart subtotal
            cartCells[0] = new TableCell { Text = "" };
            cartCells[1] = new TableCell { Text = "" };
            cartCells[2] = new TableCell { Text = "total" };
            cartCells[2].HorizontalAlign = HorizontalAlign.Center;
            cartCells[3] = new TableCell { Text = "$" + totalCost };
            cartCells[3].HorizontalAlign = HorizontalAlign.Center;

            subTotalRow.Cells.AddRange(cartCells);
            cartTable.Rows.Add(subTotalRow);

            //shipping costs
            cartCells[0] = new TableCell { Text = "" };
            cartCells[1] = new TableCell { Text = "" };
            cartCells[2] = new TableCell { Text = "Shipping" };
            cartCells[2].HorizontalAlign = HorizontalAlign.Center;
            cartCells[3] = new TableCell { Text = "$" + totalShip };
            cartCells[3].HorizontalAlign = HorizontalAlign.Center;

            shippinglRow.Cells.AddRange(cartCells);
            cartTable.Rows.Add(shippinglRow);
            total.InnerText = totalCost.ToString();
            totalCost += totalShip;

            //cart grand total
            cartCells[0] = new TableCell { Text = "" };
            cartCells[1] = new TableCell { Text = "" };
            cartCells[2] = new TableCell { Text = "Total" };
            cartCells[2].HorizontalAlign = HorizontalAlign.Center;
            cartCells[3] = new TableCell { Text = "$" + totalCost };
            cartCells[3].HorizontalAlign = HorizontalAlign.Center;

            cartTotalRow.Cells.AddRange(cartCells);
            cartTable.Rows.Add(cartTotalRow);

        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            if (cn != null) { //if SQL connection is not null, close
                cn.Close();
            }
        }
        shipping.InnerText = totalShip.ToString();
        grandtotal.InnerText = totalCost.ToString();
        return cartTable;
    }

    private void clearCart() { //used as part of order finalize proess
        SqlConnection cn = null;
        SqlCommand cmd = null;
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
    }

    private String[,] PopulateInvoiceArray(String customerID) {
        int walker = 0;
        SqlDateTime date = DateTime.Now;
        String[] customerData = getCustomerInfo(customerID);
        //customer data row
        //  0      1     
        //name | phone | 

        String[] newAddress = newShip();
        //newAddress row
        //  0      1       2      3      4
        //Email | Street | Zip | City | State

        String[,] cartInfo = getCartInfo(customerID);
        //cart info rows
        //     0        1     2     3     4      5       6      
        // line Item | SKU | qty | U/P | T/P | Title | Weight

        String orderNum = getOrderNumber(customerID);

        String[,] returnMe = new string[cartInfo.GetLength(0), 25];
        //returnMe row
        //    0          1       2       3         4          5         6        7         8         9        10        11        12      13     14    15   16   17    18     19     20       21         22         23         24
        // cust name | email | phone | billStr | billApt | billCit | billSt | billZip | shipStr | shipApt | shipCit | shipSt | shipZip | line | SKU | qty | up | tp | title | wgt | ord# | totalwgt | shipping | grandToT | ord Date

        while(walker<cartInfo.GetLength(0)) {
            returnMe[walker, 0] = customerData[0];         //0 customer name
            returnMe[walker, 1] = newAddress[0];           //1 email from textbox
            returnMe[walker, 2] = customerData[1];         //2 customer phone 
            returnMe[walker, 3] = newAddress[1];           //3 billing street
            returnMe[walker, 4] = "X";                     //4 billing apt
            returnMe[walker, 5] = newAddress[3];           //5 billing city
            returnMe[walker, 6] = newAddress[4];           //6 billing state
            returnMe[walker, 7] = newAddress[2];           //7 billing zip
            returnMe[walker, 8] = newAddress[1];           //8 shipping street
            returnMe[walker, 9] = "X";                     //9 shipping apt
            returnMe[walker, 10] = newAddress[3];          //10 shipping city
            returnMe[walker, 11] = newAddress[4];          //11 shipping state
            returnMe[walker, 12] = newAddress[2];          //12 shipping zip          
            returnMe[walker, 13] = cartInfo[walker, 0];    //13 line item
            returnMe[walker, 14] = cartInfo[walker, 1];    //14 SKU
            returnMe[walker, 15] = cartInfo[walker, 2];    //15 qty
            returnMe[walker, 16] = cartInfo[walker, 3];    //16 unit price
            returnMe[walker, 17] = cartInfo[walker, 4];    //17 total price
            returnMe[walker, 18] = cartInfo[walker, 5];    //18 item title
            returnMe[walker, 19] = cartInfo[walker, 6];    //19 line weight
            returnMe[walker, 20] = orderNum;               //20 order number
            returnMe[walker, 21] = total.InnerText;        //21 total before shipping
            returnMe[walker, 22] = shipping.InnerText;     //22 shipping cost
            returnMe[walker, 23] = grandtotal.InnerText;   //23 total order cost
            returnMe[walker, 24] = date.ToString();   //24 order date
            walker++;
        }

        //actually populate the invoice
        pushInvoice(returnMe, customerID); 
                
        return returnMe;
    }

    private void pushInvoice(String[,] data, String customerID) {
        // data row
        //    0          1       2       3         4          5         6        7         8         9        10        11        12   
        // cust name | email | phone | billStr | billApt | billCit | billSt | billZip | shipStr | shipApt | shipCit | shipSt | shipZip | 

        //    13     14    15   16   17    18     19     20     21        22         23        24
        //   line | SKU | qty | up | tp | title | wgt | ord# | total | shipping | grandToT | ordDate        
        SqlConnection cn = null;
        SqlCommand cmd = null;
        int walker = 0;
        SqlDateTime date = DateTime.Now;

        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();
            //                                         1        2           3       4       5          6       7         8       9         10       11       12       13     14  15   16      17       18        19       20       21   
            String invCom = "INSERT INTO Invoice (InvoiceNum,OrderDate,CustName,BillStreet,BillApt,BillCity,BillState,BillZip,ShipStreet,ShipApt,ShipCity,ShipState,ShipZip,SKU,Title,Qty,UnitPrice,TotalPrice,Shipping,GrandTot,LineItem)" +
                " values(@ORD,@DATE,@CUST,@BSRT,@BAPT,@BCITY,@BST,@BZIP,@SSRT,@SAPT,@SCITY,@SST,@SZIP,@SKU,@TIT,@QTY,@UP,@TP,@SHIP,@GTOT,@LINE);";
            //             1    2    3     4      5     6      7    8     9     10    11    12    13   14   15   16   17  18   19   20     21
            cmd = new SqlCommand(invCom, cn);

            cmd.Parameters.AddWithValue("@ORD", data[0,20]);            //1
            cmd.Parameters.AddWithValue("@DATE", date);                 //2
            cmd.Parameters.AddWithValue("@CUST", data[0, 0]);           //3
            cmd.Parameters.AddWithValue("@BSRT", data[0, 3]);           //4
            cmd.Parameters.AddWithValue("@BAPT", data[0, 4]);           //5
            cmd.Parameters.AddWithValue("@BCITY", data[0, 5]);          //6
            cmd.Parameters.AddWithValue("@BST", data[0, 6]);            //7
            cmd.Parameters.AddWithValue("@BZIP", data[0, 7]);           //8
            cmd.Parameters.AddWithValue("@SSRT", data[0, 8]);           //9
            cmd.Parameters.AddWithValue("@SAPT", data[0, 9]);           //10
            cmd.Parameters.AddWithValue("@SCITY", data[0, 10]);         //11
            cmd.Parameters.AddWithValue("@SST", data[0, 11]);           //12
            cmd.Parameters.AddWithValue("@SZIP", data[0, 12]);          //13

            SqlParameter skuID = new SqlParameter("@SKU", "");          //14
            cmd.Parameters.Add(skuID);
            SqlParameter titID = new SqlParameter("@TIT", "");          //15
            cmd.Parameters.Add(titID);
            SqlParameter qtyID = new SqlParameter("@QTY", "");          //16
            cmd.Parameters.Add(qtyID);
            SqlParameter upID = new SqlParameter("@UP", "");            //17
            cmd.Parameters.Add(upID);
            SqlParameter tpID = new SqlParameter("@TP", "");            //18
            cmd.Parameters.Add(tpID);

            cmd.Parameters.AddWithValue("@SHIP", data[0, 22]);          //19
            cmd.Parameters.AddWithValue("@GTOT", data[0, 23]);          //20

            SqlParameter lineID = new SqlParameter("@LINE", "");        //21
            cmd.Parameters.Add(lineID);

            while (walker < data.GetLength(0)) {

                lineID.Value = data[walker, 13];                       //21
                skuID.Value = data[walker, 14];                        //14
                qtyID.Value = data[walker, 15];                        //16
                upID.Value = data[walker, 16];                         //17
                tpID.Value = data[walker, 17];                         //18
                titID.Value = data[walker, 18];                        //15
                cmd.ExecuteNonQuery();
                walker++;
            }

        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            if (cn != null) {
                cn.Close();
            }
        }

    }

    private String[] newShip() {
        String[] returnMe = new string[5];
        int temp = 0;
        //if textboxes are populated and sorta valid
        
            returnMe[0] = Email.Text;
            returnMe[1] = newStreet.Text;
            returnMe[2] = newZip.Text;
            returnMe[3] = newCity.Text;
            returnMe[4] = newState.Text.ToUpper();
        
        return returnMe;
    }

    private Boolean addressFieldsPopulated() {
        if (Email.Text.Length == 0)
            return false;
        if (newStreet.Text.Length == 0)
            return false;
        if (newCity.Text.Length == 0)
            return false;
        if (newState.Text.Length == 0)
            return false;
        if (newZip.Text.Length == 0)
            return false;
        if (!Email.Text.Contains("@"))
            return false;
        if (!newStreet.Text.Contains(" "))
            return false;
        int x = 0;
        if (!int.TryParse(newZip.Text, out x))
            return false;
        if (newState.Text.Length > 2)
            return false;
        return true;
    }

    private String getOrderNumber(String customerID) {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataAdapter adapter = null;
        DataSet ds;
        DataTable dt;
        DataRow dr;
        int orderNumber = 0;
        String returnMe = "";

        try {
            string connectionString = WebConfigurationManager.ConnectionStrings["SalesNOMARS"].ConnectionString;
            cn = new SqlConnection();
            cn.ConnectionString = connectionString;

            cmd = new SqlCommand();
            cmd.Connection = cn;

            adapter = new SqlDataAdapter(cmd);

            cmd.Connection = cn;
            cn.Open();
            cmd.CommandText = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "BEGIN TRANSACTION;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT OrderNum FROM OrderNumber WHERE CustNum=@CUST;";
            cmd.Parameters.AddWithValue("@CUST", customerID);

            adapter = new SqlDataAdapter(cmd);
            // Fill the DataSet.
            ds = new DataSet();
            adapter.Fill(ds, "Next Order");
            dt = ds.Tables["Next Order"];
            dr = dt.Rows[0];

            orderNumber = Int32.Parse(dr["OrderNum"].ToString());
            returnMe = orderNumber.ToString();
            adapter = null;

            orderNumber++;

            cmd.CommandText = "UPDATE OrderNumber SET OrderNum = " + orderNumber + "WHERE CustNum='1000';";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "COMMIT TRANSACTION;";
            cmd.ExecuteNonQuery();

            cmd = null;
        } catch (Exception err) {
            lblError.Text = err.Message;
        } finally {
            if (cn != null) { //if SQL connection is not null, close
                cn.Close();
                cn = null;
            }
        }      

        return returnMe;
    }

    private String[] getCustomerInfo(String customer) {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataReader drCust = null;        
        String[] returnMe = new String[2];

        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();

            cmd = new SqlCommand("SELECT * FROM Customers WHERE CustNum = @CUST", cn);
            cmd.Parameters.AddWithValue("@CUST", customer);
            drCust = cmd.ExecuteReader();

            while(drCust.Read()) {
                returnMe[0] = drCust["First"].ToString() + " " + drCust["Last"].ToString();
                returnMe[1] = drCust["Phone"].ToString();
                
            }            

        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            if (cn != null) {
                cn.Close();
            }
                
        }

        return returnMe;
    }

    private String[,] getCartInfo(String customerID) {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataReader drItem = null;

        String rowCount = "";

        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();

            cmd = new SqlCommand("SELECT COUNT (*) AS 'Number of Rows' FROM ShoppingCart WHERE CustNum = @CUST", cn);
            cmd.Parameters.AddWithValue("@CUST", customerID);
            drItem = cmd.ExecuteReader();

            while(drItem.Read()) {
                rowCount = drItem["Number of Rows"].ToString();
            }

        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            if (cn != null) { //if SQL connection is not null, close
                cn.Close();
            }
        }

        int rows = Int32.Parse(rowCount);

        String[,] returnMe = new string[rows,7];

        int rowCounter = 0;

        try {
            cn = new SqlConnection();
            cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
            cn.Open();

            cmd = new SqlCommand("SELECT * FROM ShoppingCart WHERE CustNum = @CUST", cn);
            cmd.Parameters.AddWithValue("@CUST", customerID);
            drItem = cmd.ExecuteReader();

            while(drItem.Read()) {
                returnMe[rowCounter,0] = drItem["LineItem"].ToString();
                returnMe[rowCounter,1] = drItem["SKU"].ToString();
                returnMe[rowCounter,2] = drItem["Qty"].ToString();
                returnMe[rowCounter,3] = drItem["UnitPrice"].ToString();
                returnMe[rowCounter,4] = drItem["TotalPrice"].ToString();
                returnMe[rowCounter,5] = drItem["Title"].ToString();
                returnMe[rowCounter,6] = drItem["TotWeight"].ToString();
                rowCounter ++;
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

    private String[] sendMail(String[,] orderData) {
        //returnMe row
        //    0          1       2       3         4          5         6        7         8         9        10        11        12      13     14    15   16   17    18     19     20       21         22         23
        // cust name | email | phone | billStr | billApt | billCit | billSt | billZip | shipStr | shipApt | shipCit | shipSt | shipZip | line | SKU | qty | up | tp | title | wgt | ord# | totalwgt | shipping | grandToT | 
        int walker = 0;

        String emailAddress = orderData[0,1];
        String emailSubject = "About your order number " + orderData[0,20] + " from Diesel's Mountain.";
        String shipAddress = orderData[0, 8] + "\n" + "Apt: " + orderData[0, 9] + "\n" + orderData[0, 10] + ", " + orderData[0, 11] + " " + orderData[0, 12] + "\n";
        String billAddress = orderData[0, 3] + "\n" + "Apt: " + orderData[0, 4] + "\n" + orderData[0, 5] + ", " + orderData[0, 6] + " " + orderData[0, 7] + "\n";
        String orderedItems = "SKU    Title                                Qty    U/P     T/P\n";
        String emailBody = "Order number: " + orderData[0,20] + "\n\n";
        emailBody += "Order Date: " + orderData[0,24] +"\n\n" + "Billing Address:\n" + billAddress + "\nShipping address:\n" + shipAddress;

        while (walker<orderData.GetLength(0)) {
            String temp = orderData[walker, 14] + " " + orderData[walker, 18] + " " + orderData[walker, 15] + " $" +orderData[walker, 16] + " $" + orderData[walker, 17] + "\n";
            orderedItems += temp;
            walker++;
        }

        emailBody += "\nOrder:\n" + orderedItems + "\n\n";
        emailBody += "Shipping:   $" + orderData[0, 22] + "\n";
        emailBody += "Total Cost: $" + orderData[0, 23] + "\n\n";
        emailBody += "If you did not place this order please contact: support@pnwgamer.com";
        
        String[] returnMe = new String[4];
        
        MailMessage emailToSend = new MailMessage("sales@pnwgamer.com", emailAddress);

        emailToSend.Subject = emailSubject;
        emailToSend.Body = emailBody;

        SmtpClient client = new SmtpClient();
        client.Credentials = new System.Net.NetworkCredential("sales@pnwgamer.com","Q45dk5m4ks1!");
        client.Host = "mail.pnwgamer.com";

        try {
            client.Send(emailToSend);
        } catch (Exception err) {
            lblStatus.Text = err.Message;
        } finally {
            client = null;
        }
        returnMe[0] = orderData[0, 20]; //invoice num
        returnMe[1] = orderData[0, 20]; //order num
        returnMe[2] = emailAddress;     //email address
        returnMe[3] = orderData[0, 0];  //customer name
        return returnMe;
    }
    
    protected void BackToCart_Click(object sender, EventArgs e) {
        String shoppingCustomer = hCustomer.InnerText;
        Server.Transfer("Cart.aspx?customer="+ shoppingCustomer);
    }

    protected void FinalizeOrder_Click(object sender, EventArgs e) {
        if(addressFieldsPopulated()) {
            String servedCustomer = hCustomer.InnerText;
            String[,] array = PopulateInvoiceArray(servedCustomer);
            string[] viewstateStuff = sendMail(array);
            String orderNumber = getOrderNumber(servedCustomer);
            clearCart();
            Response.Redirect("Success.aspx?customer=" + servedCustomer + "&invoice=" + viewstateStuff[0] + "&order=" + viewstateStuff[0] + "&email=" + viewstateStuff[2]);
        } else {
            lblError.Text = "Address fields are malformed.";
        }
        
    }
}