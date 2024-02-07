using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

        if (Page.IsPostBack == false) {

            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataReader drItem = null;

            String dPic = "";
            String dTitle = "";
            String dPrice = "";
            String dDesc = "";

            try {

                cn = new SqlConnection();
                cn.ConnectionString = WebConfigurationManager.ConnectionStrings["SalesMARS"].ConnectionString;
                cn.Open();

                //start item 1 setup
                cmd = new SqlCommand("SELECT UnitPrice,Description,SmallImage,Title FROM Inventory WHERE SKU = @SKU", cn);
                cmd.Parameters.AddWithValue("@SKU", 1001);
                drItem = cmd.ExecuteReader();

                while (drItem.Read()) {
                    dPic = drItem["SmallImage"].ToString();
                    dTitle = drItem["Title"].ToString();
                    dPrice = drItem["UnitPrice"].ToString();
                    dDesc = drItem["Description"].ToString();
                }
                String.Format("{0:C2}", dPrice);


                item1Pic.InnerHtml = "<img class='card-img-top' src='img/items/" + dPic + "' alt='item 1' />";
                item1Title.InnerHtml = "<a href='detail.aspx?item=1001'>" + dTitle + "</a>";
                item1Price.InnerHtml = "$" + dPrice;
                item1Desc.InnerHtml = dDesc;
                //finish item 1 setup

                //start item 2 setup
                cmd = new SqlCommand("SELECT UnitPrice,Description,SmallImage,Title FROM Inventory WHERE SKU = @SKU", cn);
                cmd.Parameters.AddWithValue("@SKU", 1002);
                drItem = cmd.ExecuteReader();

                while (drItem.Read()) {
                    dPic = drItem["SmallImage"].ToString();
                    dTitle = drItem["Title"].ToString();
                    dPrice = drItem["UnitPrice"].ToString();
                    dDesc = drItem["Description"].ToString();
                }
                String.Format("{0:C2}", dPrice);


                item2Pic.InnerHtml = "<img class='card-img-top' src='img/items/" + dPic + "' alt='item 1' />";
                item2Title.InnerHtml = "<a href='detail.aspx?item=1002'>" + dTitle + "</a>";
                item2Price.InnerHtml = "$" + dPrice;
                item2Desc.InnerHtml = dDesc;
                //finish item 2 setup

                //start item 3 setup
                cmd = new SqlCommand("SELECT UnitPrice,Description,SmallImage,Title FROM Inventory WHERE SKU = @SKU", cn);
                cmd.Parameters.AddWithValue("@SKU", 1003);
                drItem = cmd.ExecuteReader();

                while (drItem.Read()) {
                    dPic = drItem["SmallImage"].ToString();
                    dTitle = drItem["Title"].ToString();
                    dPrice = drItem["UnitPrice"].ToString();
                    dDesc = drItem["Description"].ToString();
                }
                String.Format("{0:C2}", dPrice);


                item3Pic.InnerHtml = "<img class='card-img-top' src='img/items/" + dPic + "' alt='item 1' />";
                item3Title.InnerHtml = "<a href='detail.aspx?item=1003'>" + dTitle + "</a>";
                item3Price.InnerHtml = "$" + dPrice;
                item3Desc.InnerHtml = dDesc;
                //finish item 3 setup

                //start item 4 setup
                cmd = new SqlCommand("SELECT UnitPrice,Description,SmallImage,Title FROM Inventory WHERE SKU = @SKU", cn);
                cmd.Parameters.AddWithValue("@SKU", 1004);
                drItem = cmd.ExecuteReader();

                while (drItem.Read()) {
                    dPic = drItem["SmallImage"].ToString();
                    dTitle = drItem["Title"].ToString();
                    dPrice = drItem["UnitPrice"].ToString();
                    dDesc = drItem["Description"].ToString();
                }
                String.Format("{0:C2}", dPrice);


                item4Pic.InnerHtml = "<img class='card-img-top' src='img/items/" + dPic + "' alt='item 1' />";
                item4Title.InnerHtml = "<a href='detail.aspx?item=1004'>" + dTitle + "</a>";
                item4Price.InnerHtml = "$" + dPrice;
                item4Desc.InnerHtml = dDesc;
                //finish item 4 setup

                //start item 5 setup
                cmd = new SqlCommand("SELECT UnitPrice,Description,SmallImage,Title FROM Inventory WHERE SKU = @SKU", cn);
                cmd.Parameters.AddWithValue("@SKU", 1005);
                drItem = cmd.ExecuteReader();

                while (drItem.Read()) {
                    dPic = drItem["SmallImage"].ToString();
                    dTitle = drItem["Title"].ToString();
                    dPrice = drItem["UnitPrice"].ToString();
                    dDesc = drItem["Description"].ToString();
                }
                String.Format("{0:C2}", dPrice);


                item5Pic.InnerHtml = "<img class='card-img-top' src='img/items/" + dPic + "' alt='item 1' />";
                item5Title.InnerHtml = "<a href='detail.aspx?item=1005'>" + dTitle + "</a>";
                item5Price.InnerHtml = "$" + dPrice;
                item5Desc.InnerHtml = dDesc;
                //finish item 5 setup

                //start item 6 setup
                cmd = new SqlCommand("SELECT UnitPrice,Description,SmallImage,Title FROM Inventory WHERE SKU = @SKU", cn);
                cmd.Parameters.AddWithValue("@SKU", 1006);
                drItem = cmd.ExecuteReader();

                while (drItem.Read()) {
                    dPic = drItem["SmallImage"].ToString();
                    dTitle = drItem["Title"].ToString();
                    dPrice = drItem["UnitPrice"].ToString();
                    dDesc = drItem["Description"].ToString();
                }
                String.Format("{0:C2}", dPrice);


                item6Pic.InnerHtml = "<img class='card-img-top' src='img/items/" + dPic + "' alt='item 1' />";
                item6Title.InnerHtml = "<a href='detail.aspx?item=1006'>" + dTitle + "</a>";
                item6Price.InnerHtml = "$" + dPrice;
                item6Desc.InnerHtml = dDesc;
                //finish item 6 setup

            } catch (Exception err) {
                lblStatus.Text = err.Message;
            } finally {
                if (cn != null) {
                    cn.Close();
                }
            }

        }

    }
}