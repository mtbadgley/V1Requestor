<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="V1Requestor._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="requestlisting" visible="false" runat="server">
        <div id="errsection" class="alert alert-warning" role="alert" visible="false" runat="server">
            <asp:Label ID="errlabel" runat="server" Text="Label"></asp:Label>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h1>VersionOne Requests</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <p><asp:CheckBox ID="includeClosed" runat="server" AutoPostBack="true"/> Show Closed Items </p>
            </div>
            <div class="col-sm-6" align="right">
                <p><a class="btn btn-primary" href="addrequest.aspx" target="_blank">Add Request</a></p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="table-responsive">
                    <asp:GridView ID="assetsview" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered table-hover pagination-ys" AllowPaging="true" PageSize="20" OnPageIndexChanging="AssetsviewPageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" HtmlEncode="false" ItemStyle-Width="90px" />
                            <asp:BoundField DataField="Title" HeaderText="Title" ReadOnly="true" HtmlEncode="false" ItemStyle-Width="560px" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="true" HtmlEncode="false" ItemStyle-Width="75px" />
                            <asp:BoundField DataField="GroupTeam" HeaderText="Group/Team" ReadOnly="true" HtmlEncode="false"  ItemStyle-Width="125px" />
                            <asp:BoundField DataField="ServiceRequested" HeaderText="Service Requested" HtmlEncode="false" ReadOnly="true" ItemStyle-Width="125px" />
                            <asp:BoundField DataField="LastUpdated" HeaderText="Last Updated" ReadOnly="true" HtmlEncode="false"  ItemStyle-Width="75px" />
                            <asp:BoundField DataField="RelatedWorkitems" HeaderText="Related Workitems" ReadOnly="true" HtmlEncode="false" ItemStyle-Width="125px" />
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
