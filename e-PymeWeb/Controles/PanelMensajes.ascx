<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PanelMensajes.ascx.cs"
    Inherits="Controles_PanelMensajes" %>
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
