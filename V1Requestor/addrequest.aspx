<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AddRequest.aspx.cs" Inherits="V1Requestor._addrequest" %>
<asp:Content ID="addrequest" ContentPlaceHolderID="MainContent" runat="server">
    <form class="form-horizontal">

<div id="errsection" class="alert alert-warning" role="alert" visible="false" runat="server">
    <asp:Label ID="errlabel" runat="server" Text="Label"></asp:Label>
</div>

<fieldset class="form-group">
<div class="col-md-8">
<h1>Create Request</h1>
</div>
  <div class="col-md-4" align="right">
    <asp:button id="addrequestsubmit" name="singlebutton" class="btn btn-warning" OnClick="SubmitNewRequest" runat="server" Text="Save"></asp:button>&nbsp;&nbsp;
    <button id="addrequestcancel" name="singlebutton" class="btn btn-secondary"> Cancel </button>
  </div>
</fieldset>

<fieldset class="form-group">
  <label class="col-md-3 control-label" for="planninglevel">Planning Level</label>
  <div class="col-md-6">
    <asp:DropDownList id="planninglevel" name="planninglevel" class="form-control" 
        AutoPostBack="True" OnSelectedIndexChanged="PlanningLevelIndexChange" runat="server">

    </asp:DropDownList>
  </div>
</fieldset>

<fieldset class="form-group">
  <label class="col-md-3 control-label" for="servicetype">Service</label>
  <div class="col-md-6">
    <asp:DropDownList id="servicetype" name="servicetype" class="form-control" runat="server">

    </asp:DropDownList>
  </div>
</fieldset>

<fieldset class="form-group">
  <label class="col-md-3 control-label" for="requesttitle">Title</label>  
  <div class="col-md-6">
  <asp:TextBox id="requesttitle" name="requesttitle" type="text" class="form-control input-md" runat="server">
  </asp:TextBox>
  </div>
</fieldset>
    
<fieldset class="form-group">
  <label class="col-md-3 control-label" for="requestdescription">Description</label>
  <div class="col-md-6">                     
    <asp:TextBox class="form-control" TextMode="multiline" Rows="5" id="requestdescription" name="requestdescription" runat="server"></asp:TextBox>
  </div>
</fieldset>

</form>

</asp:Content>
