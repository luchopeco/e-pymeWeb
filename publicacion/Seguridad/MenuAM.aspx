<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="MenuAM.aspx.cs" Inherits="MenuAM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
                <div class="main-header">
                <h2>
                    <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label></h2>
                <em></em>
            </div>
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
            <asp:Panel ID="PanelModificacion" runat="server">
             <asp:Literal ID="LiteralMenu" runat="server"></asp:Literal>
            </asp:Panel>  
            <asp:Panel ID="PanelPrimerItem" runat="server">
                <asp:Button ID="btnAgregarItem" runat="server" Text="Agregar Primer Item" 
                    CssClass="btn btn-success" onclick="btnAgregarItem_Click" />
            </asp:Panel>         
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
