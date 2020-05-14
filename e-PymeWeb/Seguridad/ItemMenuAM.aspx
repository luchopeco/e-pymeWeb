<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ItemMenuAM.aspx.cs" Inherits="ItemMenuAM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1 class="page-header">
        <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>
    </h1>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelError" Visible="false" runat="server">
                <div class="alert alert-danger alert-dismissable">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                        ×</button>
                    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="PanelMensaje" Visible="false" runat="server">
                <div class="alert alert-success alert-dismissable">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                        ×</button>
                    <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>  
            <div class="col-lg-12">    
                <asp:Panel ID="PanelAM" runat="server">                      
            <div>                 
            Etiqueta
            <asp:TextBox ID="txtEtiqueta" runat="server" CssClass="form-control"></asp:TextBox></div>  
            <div>
            Pagina
                <asp:DropDownList ID="cbxPaginas" runat="server" CssClass="form-control">
                </asp:DropDownList></div>
            <div><asp:CheckBox ID="chbxEsDivision" runat="server" Text="Es Division" CssClass="form-control"/></div>      
            <div> 
                <asp:Button ID="btnAgregar" runat="server" Text="Aceptar" 
                    CssClass="btn btn-success" onclick="btnAgregar_Click" />
            </div>
               </asp:Panel> 
                <asp:Panel ID="PanelEliminacion" runat="server">
                <p class="text-warning">Al eliminar el item menu, sus hijos(si posee), pasaran a ser hijos del nivel inmediatamente superior.</p>
                    <p class=" text-danger"><asp:Label ID="lblMensajeElimnacion" runat="server" Text="Label"></asp:Label></p>
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar Item" 
                        CssClass="btn btn-danger" onclick="btnEliminar_Click" />
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

