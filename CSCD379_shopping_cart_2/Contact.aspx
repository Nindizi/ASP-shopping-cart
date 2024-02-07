<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content ID="ContactBodyContent" ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <div class="row">
        <div class="col-lg-4 col-md-6 mb-4">
            <div id="text_fields" class="card h-100" runat="server">
                <div class="card-body">
                    <h4 class="card-title">Contact Us</h4>
                    <p>&nbsp;</p>
                    <p class="card-text" runat="server"><a href="tel:XXX-XXX-XXXX" rel="nofollow">(XXX) XXX-XXXX</a></p>
                    <p>&nbsp;</p>
                    <p class="card-text" runat="server"><a href="mailto:XXXX@pnwgamer.com" rel="nofollow">XXXX@pnwgamer.com</a></p>
                    <p>&nbsp;</p>
                    <p class="card-text" runat="server">
                        <asp:button runat="server" CssClass="nav_btn" text="Send" onclick="SendEmail_Click" />
                    </p>
                    <p>&nbsp;</p>
                    <div id="btnDiv" class="card-footer">
                        <p id="lblStatus" runat="server"></p>
                    </div>
                    
                </div>
            </div>
        </div>
        <div class="col-lg-6 col-md-6 mb-4">
            <div class="card h-100">
                <div class="card-body">
                    <div class="card-body">
                        <h4 class="card-title">Your Contact Information</h4>
                        <p class="card-text">Your Name (required)</p>
                        <input type="text" name="mailname" id="sendername" runat="server" />
                        <p class="card-text">Your Email (required)</p>
                        <input type="text" name="address" id="senderemail" runat="server" />
                        <p class="card-text">Subject</p>
                        <input type="text" name="mailsubject" id="sendersubject" runat="server" />
                        <p class="card-text">Your Message</p>
                        <input type="text" name="mailbody" id="senderbody" runat="server" />
                        <p class="card-text">&nbsp;</p>
                        <div class="card-footer">
                            <p id="lblError" runat="server"></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

