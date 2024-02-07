<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Cart" %>

<asp:Content ID="CartContent" ContentPlaceHolderID="BodyContentPlaceholder" Runat="Server">
    <asp:Button ID="keepShopping" CssClass="nav_btn" runat="server" Text="Continue Shopping" PostBackURL="./Default.aspx"/>
    <p>&nbsp;</p>
    <div class="row">
        <article class="col cart">
            <asp:PlaceHolder ID="cartTable" runat="server"></asp:PlaceHolder><br />
            <h1><asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></h1>
        </article>
    </div>
    <p id="nope" runat="server">&nbsp;</p>
    <asp:Button ID="clearCartButton" CssClass="nav_btn" runat="server" OnClick="Clear_Click" text="Clear Cart" />
    <span id="textBoxLabelText" runat="server">Select line item to delete:</span>
    <asp:TextBox ID="LineItemTextBox" runat="server" Width="30px"></asp:TextBox>
    <asp:Button ID="deleteLineItemButton" CssClass="nav_btn" runat="server" Text="Delete" OnClick="DeleteItem_Click" />
    <asp:Button ID="checkOut" runat="server" CssClass="nav_btn" OnClick="CheckOut_Click" Text="Check Out" />    
    <asp:Button ID="goHome" runat="server" CssClass="nav_btn" OnClick="GoHome_Click" Text="Go Home" />
    <div id="customerID" runat="server" hidden="hidden"></div>
</asp:Content>

