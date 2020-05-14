<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="main-header">
        <h2>
            e-Pyme. Bienvenidos
        </h2>
        Sistema de Gestion de Wipahala Sistema
        <hr />
    </div>
    <div class="row">
        <div class="col-md-12">
            <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
        </div>
    </div>
</asp:Content>
