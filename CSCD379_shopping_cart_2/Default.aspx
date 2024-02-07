<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="DefaultBodyContent" ContentPlaceHolderID="BodyContentPlaceholder" Runat="Server">
    <div id="DynamicBody" class="row" runat="server">

                        <div class="col-lg-4 col-md-6 mb-4">
                            <div  class="card h-100" >
                                <a id="item1Pic" runat="server" href="detail.aspx?item=1001"></a>
                                <div class="card-body">
                                    <h4 id="item1Title" class="card-title" runat="server"></h4>
                                    <h5 id="item1Price" runat="server"></h5>
                                    <p id="item1Desc" class="card-text" runat="server"></p>
                                </div>
                                <div class="card-footer">
                                    <small class="text-muted">&#9733; &#9733; &#9733; &#9733; &#9734;</small>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card h-100">
                                <a id="item2Pic" runat="server" href="detail.aspx?item=1002"></a>
                                <div class="card-body">
                                    <h4 id="item2Title" runat="server" class="card-title"></h4>
                                    <h5 id="item2Price" runat="server"></h5>
                                    <p id="item2Desc" class="card-text" runat="server"></p>
                                </div>
                                <div class="card-footer">
                                    <small class="text-muted">&#9733; &#9733; &#9733; &#9733; &#9734;</small>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card h-100">
                                <a id="item3Pic" runat="server" href="detail.aspx?item=1003"></a>
                                <div class="card-body">
                                    <h4 id="item3Title" class="card-title" runat="server"></h4>
                                    <h5 id="item3Price" runat="server"></h5>
                                    <p id="item3Desc" class="card-text" runat="server"></p>
                                </div>
                                <div class="card-footer">
                                    <small class="text-muted">&#9733; &#9733; &#9733; &#9733; &#9734;</small>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card h-100">
                                <a id="item4Pic" runat="server" href="detail.aspx?item=1004"></a>
                                <div class="card-body">
                                    <h4 id="item4Title" class="card-title" runat="server"></h4>
                                    <h5 id="item4Price" runat="server"></h5>
                                    <p id="item4Desc" class="card-text" runat="server"></p>
                                </div>
                                <div class="card-footer">
                                    <small class="text-muted">&#9733; &#9733; &#9733; &#9733; &#9734;</small>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card h-100">
                                <a id="item5Pic" runat="server" href="detail.aspx?item=1005"></a>
                                <div class="card-body">
                                    <h4 id="item5Title" class="card-title" runat="server"></h4>
                                    <h5 id="item5Price" runat="server"></h5>
                                    <p id="item5Desc" class="card-text" runat="server"></p>
                                </div>
                                <div class="card-footer">
                                    <small class="text-muted">&#9733; &#9733; &#9733; &#9733; &#9734;</small>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card h-100">
                                <a id="item6Pic" runat="server" href="detail.aspx?item=1006"></a>
                                <div class="card-body">
                                    <h4 id="item6Title" class="card-title" runat="server"></h4>
                                    <h5 id="item6Price" runat="server"></h5>
                                    <p id="item6Desc" class="card-text" runat="server"></p>
                                </div>
                                <div class="card-footer">
                                    <small class="text-muted">&#9733; &#9733; &#9733; &#9733; &#9734;</small>
                                </div>
                            </div>
                        </div>

                    </div>
                    <!-- /.row -->
    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
</asp:Content>

