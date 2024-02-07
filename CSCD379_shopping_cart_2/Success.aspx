<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="Success.aspx.cs" Inherits="Success" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceholder" Runat="Server">
    <div class="row">
        <article  class="col cart h-50">            
            <h1 id="hSuccess" runat="server"></h1>
            <p id="hOrderLine" runat="server"></p>
            <p id="hInvoiceSent" runat="server"></p>
            <asp:Button ID="homeButton" CssClass="nav_btn" runat="server" Text="HOME" OnClick="homeButton_Click" />
        </article>
    </div>
    <asp:Label id="lblStatus" runat="server" text=""></asp:Label>
</asp:Content>

