<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssetDetails.aspx.cs" Inherits="V1Requestor.AssetDetails" %>
<asp:Content ID="AssetDetailsContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="mustlogin" class="jumbotron" visible="false" runat="server">
        <h1>Welcome to the Aflac VerisonOne Requestor!</h1>
        <p>In order to submit 
            and review requests, you must first Register and Login. Once registered, a listing of 
            requests will be displayed.
        </p>
        <p><a class="btn btn-primary btn-lg" href="Account/Register">Register</a>
            <a class="btn btn-primary btn-lg" href="Account/Login">Login</a></p>
    </div>
    <div id="assetdetails" class="" visible="true" runat="server">
        <div id="errsection" class="alert alert-warning" role="alert" visible="false" runat="server">
            <asp:Label ID="errlabel" runat="server" Text="Label"></asp:Label>
        </div>
        <div class="row">
            <div class="col-md-9">
                <h1>
                    <asp:Label ID="assetId" runat="server" Text=""></asp:Label> - 
                    <asp:Label ID="assetTitle" runat="server" Text=""></asp:Label>
                </h1>
            </div>
            <div class="col-sm-3" align="right">
                <button class="btn btn-secondary" onclick="window.close();"> Close </button>
            </div>
        </div>
        <div id="pwdetails" class="" visible="false" runat="server">
            <div class="row">
                <dl class="dl-horizontal">
                    <dt class="col-sm-3">Planning Level:</dt>
                    <dd class="col-sm-7"><asp:Label ID="pwPlanningLevel" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Team:</dt>
                    <dd class="col-sm-7"><asp:Label ID="pwTeamName" runat="server" Text=""></asp:Label></dd>                
                    <dt class="col-sm-3">Status:</dt>
                    <dd class="col-sm-7"><asp:Label ID="pwStatusName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Sprint:</dt>
                    <dd class="col-sm-7"><asp:Label ID="pwSprintName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Service:</dt>
                    <dd class="col-sm-7"><asp:Label ID="pwServiceName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Portfolio Item:</dt>
                    <dd class="col-sm-7"><asp:Label ID="pwEpicName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Description:</dt>
                    <dd class="col-sm-7"><asp:Label ID="pwDescription" runat="server" Text=""></asp:Label></dd>
                </dl>
            </div>
        </div>
        <div id="epdetails" class="" visible="false" runat="server">
            <div class="row">
                <dl class="dl-horizontal">
                    <dt class="col-sm-3">Planning Level:</dt>
                    <dd class="col-sm-7"><asp:Label ID="epPlanningLevel" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Team:</dt>
                    <dd class="col-sm-7"><asp:Label ID="epTeamName" runat="server" Text=""></asp:Label></dd>                
                    <dt class="col-sm-3">Status:</dt>
                    <dd class="col-sm-7"><asp:Label ID="epStatusName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Service:</dt>
                    <dd class="col-sm-7"><asp:Label ID="epServiceName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Portfolio Item:</dt>
                    <dd class="col-sm-7"><asp:Label ID="epParentName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Description:</dt>
                    <dd class="col-sm-7"><asp:Label ID="epDescription" runat="server" Text=""></asp:Label></dd>
                </dl>
            </div>
        </div>
        <div id="rqdetails" class="" visible="false" runat="server">
            <div class="row">
                <dl class="dl-horizontal">
                    <dt class="col-sm-3">Planning Level:</dt>
                    <dd class="col-sm-7"><asp:Label ID="rqPlanningLevel" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Status:</dt>
                    <dd class="col-sm-7"><asp:Label ID="rqStatusName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Service:</dt>
                    <dd class="col-sm-7"><asp:Label ID="rqServiceName" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Description:</dt>
                    <dd class="col-sm-7"><asp:Label ID="rqDescription" runat="server" Text=""></asp:Label></dd>
                    <dt class="col-sm-3">Owner:</dt>
                    <dd class="col-sm-7"><asp:Label ID="rqOwnerName" runat="server" Text=""></asp:Label></dd>
                </dl>
            </div>
        </div>
    </div>
</asp:Content>
