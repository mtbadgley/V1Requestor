<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SubmitEmail.aspx.cs" Inherits="V1Requestor.SubmitEmail" %>
<asp:Content ID="SubmitEmail" ContentPlaceHolderID="MainContent" runat="server">
    <form class="form-horizontal">
        <fieldset class="form-group">
            <div class="col-md-12">
                <h1>Who's making the request?</h1>
            </div>
        </fieldset>
        <fieldset class="form-group">
            <label class="col-md-2 control-label" for="emailaddress">Email Address</label>
            <div class="col-md-4">
                <asp:TextBox runat="server" class="form-control input-md" ID="emailaddress" name="emailaddress"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <asp:Button runat="server" ID="submitemail" class="btn btn-warning" Text="OK" OnClick="SubmitEmailAddress"/>
            </div>
        </fieldset>        
    </form>
</asp:Content>
