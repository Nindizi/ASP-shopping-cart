<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="detail.aspx.cs" Inherits="Detail" %>

<asp:Content ID="DetailContent" ContentPlaceHolderID="BodyContentPlaceholder" Runat="Server">
    <div class="row">
        <div class="col-lg-4 col-md-6 mb-4">        
            <div id="detail_pics" class="card h-100" runat="server">
                <!-- replace images -->
            </div>
        </div>
        <div class="col-lg-6 col-md-6 mb-4">
            <div class="card h-100">
                <div class="card-body">
                    <!-- replace title -->
                    <h4 id="title" class="card-title" runat="server"></h4>
                    <!-- replace cost -->
                    <h5 id="cost" runat="server"></h5>
                    <div class="card-body">
                        <!-- replace long description -->
                        <p id="long_description" class="card-text" runat="server"></p>
                        <!-- replace in stock qty -->
                        <p>Qty on hand: <span id="instock" runat="server"></span></p>
                        <p>Order quantity:</p>
                        <asp:DropDownList ID="QtyList" runat="server"></asp:DropDownList>                      
                        <asp:Button ID="AddToCart" CssClass="nav_btn" runat="server" OnClick="AddToCart_Click" Text="Add To Cart" /> <!-- PostBackUrl="cart.aspx" -->
                        <p></p>
                        <div id="btnDiv" class="card-footer">
                            <small class="text-muted">★ ★ ★ ★ ☆</small>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="wWeight" runat="server"></div>
    <div id="wSKU" runat="server"></div>
<asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
</asp:Content>

