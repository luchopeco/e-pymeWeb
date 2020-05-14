<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="GestionClientes.aspx.cs" Inherits="Clientes_GestionClientes" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ClientClienteSelected(source, eventArgs) {
            //alert(" Key : " + eventArgs.get_text() + "  Value :  " + eventArgs.get_value());
            document.getElementById('<%=hfIdCliente.ClientID %>').value = eventArgs.get_value();
        }         
    </script>
    <div class="main-header">
        <h2>
            <asp:Label ID="lblTituloPagina" runat="server"></asp:Label>
        </h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class=" col-md-6">
                        <div class="input-group ">
                            <asp:TextBox ID="txtNombreCliente" PlaceHolder="Busqueda por nombre y apellido"
                                class="form-control" runat="server"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnBuscarCliente" class="btn btn-default" 
                                runat="server" onclick="btnBuscarCliente_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                            </span>
                        </div>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                            DelimiterCharacters="" Enabled="True" MinimumPrefixLength="4" ServiceMethod="BuscarClientes"
                            UseContextKey="true" ServicePath="~/Autocomplete.asmx" TargetControlID="txtNombreCliente"
                            OnClientItemSelected="ClientClienteSelected">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfIdCliente" runat="server" />                        
                    </div>
                </div>
            </div>
            <div class="row">
                <div class=" col-md-12">
                    <asp:Button ID="btnNuevoCliente" runat="server" CssClass=" btn btn-block btn-primary"
                        Text="Nuevo Cliente" CausesValidation="false" OnClick="btnNuevoCliente_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="clienteModal" class="modal fade " tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                            <asp:Label ID="lblTituloModalCliente" runat="server"></asp:Label></h3>
                    </div>
                    <div class=" modal-body ">
                        <div class=" widget">
                            <div class=" widget-header">
                                <h3>
                                    <i class="fa fa-edit"></i>
                                    <asp:Label ID="lblPanelModalCliente" runat="server"></asp:Label>
                                </h3>
                            </div>
                            <div class=" widget-content">
                                <div class="row">
                                    <div class="col-md-12">
                                        Nombre y Apellido
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor=""
                                            CssClass="validador" SetFocusOnError="True" ControlToValidate="txtNombreModal"
                                            Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="cliente"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtNombreModal" runat="server" CssClass="form-control "></asp:TextBox>
                                    </div>
                                    <div class="col-md-12">
                                        Documento (DNI/CUIT)
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor=""
                                            CssClass="validador" SetFocusOnError="True" ControlToValidate="txtDocumentoModal"
                                            Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="cliente"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtDocumentoModal" runat="server" CssClass="form-control "></asp:TextBox>
                                    </div>
                                    <div class=" col-md-12">
                                        Direccion
                                        <asp:TextBox ID="txtDireccionModal" runat="server" CssClass="form-control "></asp:TextBox>
                                    </div>
                                    <div class="col-md-12">
                                        Telefono
                                        <asp:TextBox ID="txtTelefonoModal" runat="server" CssClass="form-control "></asp:TextBox>
                                    </div>
                                    <div class="col-md-12">
                                        Mail
                                        <asp:TextBox ID="txtMailModal" runat="server" CssClass="form-control "></asp:TextBox>
                                    </div>
                                    <div class="col-md-12">
                                        Observaciones
                                        <asp:TextBox ID="txtObservacionesModal" runat="server" CssClass="form-control " TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarModalCliente" runat="server" Text="" CssClass="btn btn-success"
                            CausesValidation="true" ValidationGroup="cliente" OnClick="btnAceptarModalCliente_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
