<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <div class="row">
        <div class="col-lg-4 col-md-6 mb-4">
            <div class="card h-100">
                <div class="card-body">
                    <!-- address is filled by codebehind -->
                    <div id="address" runat="server"> 
                    </div>
                    <div>
                        Email: <asp:Textbox runat="server" ID="Email" Width="100%"></asp:Textbox><br />
                        Street <asp:Textbox runat="server" ID="newStreet" Width="100%"></asp:Textbox><br />
                        Zip    <asp:Textbox runat="server" ID="newZip" Width="100%"></asp:Textbox><br/>
                        City   <asp:Textbox runat="server" ID="newCity" Width="100%"></asp:Textbox><br />
                        State  <asp:Textbox runat="server" ID="newState" Width="100%"></asp:Textbox><br />                        
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-6 col-md-6 mb-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2>Your Cart:</h2>
                    <asp:placeholder id="WorkingCart" runat="server"></asp:placeholder>
                    <p id="test" runat="server"></p>
                    <p>
                        <asp:button id="BackToCart" cssclass="nav_btn" text="Back To Cart" onclick="BackToCart_Click" runat="server" />
                    </p>
                    <p>
                        <asp:button id="Finalize" cssclass="nav_btn" text="Make Purchase" onclick="FinalizeOrder_Click" runat="server" />
                    </p>
                </div>
            </div>
        </div>
    </div>
    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <div id="hCustomer" runat="server" hidden="hidden"></div>
    <div id="hError" runat="server" hidden="hidden"></div>
    <div id="total" runat="server" hidden="hidden"></div>
    <div id="shipping" runat="server" hidden="hidden"></div>
    <div id="grandtotal" runat="server" hidden="hidden"></div>
</asp:Content>

