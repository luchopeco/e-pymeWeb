<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="GestionArtciulosSucursal.aspx.cs" Inherits="Articulo_GestionArtciulosSucursal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ClientArticuloSelected(source, eventArgs) {
            //alert(" Key : " + eventArgs.get_text() + "  Value :  " + eventArgs.get_value());
            document.getElementById('<%=hfIdArticuloDescripcion.ClientID %>').value = eventArgs.get_value();
        }
        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=hfIdSucursal.ClientID %>").value);
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-header">
                <h2>
                    Gestion Articulos
                </h2>
                <asp:Label ID="lblSucursal" runat="server"></asp:Label>
            </div>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class="row">
                <div class=" col-md-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                Busqueda de Articulos
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class=" row">
                                <div class=" col-sm-6">
                                    <div class="input-group ">
                                        <asp:TextBox ID="txtArticuloCodigo" PlaceHolder="Busqueda Codigo" class="form-control"
                                            runat="server"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnBuscarArtXCodigo" class="btn btn-default" runat="server" OnClick="btnBuscarArtXCodigo_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div class=" col-md-6">
                                    <div class="input-group ">
                                        <asp:TextBox ID="txtDescArticulo" onkeyup="SetContextKey()" PlaceHolder="Busqueda por nombre"
                                            class="form-control" runat="server"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnBuscarArtXDesc" class="btn btn-default" runat="server" OnClick="btnBuscarArtXDesc_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="3" ServiceMethod="BuscarArticulosVenta"
                                        UseContextKey="true" ServicePath="~/Autocomplete.asmx" TargetControlID="txtDescArticulo"
                                        OnClientItemSelected="ClientArticuloSelected">
                                    </asp:AutoCompleteExtender>
                                    <asp:HiddenField ID="hfIdArticuloDescripcion" runat="server" />
                                    <asp:HiddenField ID="hfIdSucursal" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div style="position: fixed; top: 2%; left: 50%; z-index: 1051;">
                        <button class="btn btn-default btn-lg">
                            <i class="fa fa-spinner fa-spin"></i>Cargando....
                        </button>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- articulo Record Modal Starts here-->
    <div id="articuloModal" class=" modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H1">
                            Modificar Articulo</h3>
                    </div>
                    <div class=" modal-body">
                        <div class=" row">
                            <asp:HiddenField ID="hfClave" runat="server"></asp:HiddenField>
                            <div class=" col-lg-12">
                                Articulo
                                <asp:TextBox ID="txtArticuloModal" CssClass=" form-control" Enabled="false" runat="server"></asp:TextBox>
                                Ultimo Costo
                                <div class="form-group input-group">
                                    <span class="input-group-addon">$ </span>
                                    <asp:TextBox ID="txtCostoUltimo" Enabled="false" CssClass=" form-control" runat="server"></asp:TextBox>
                                </div>
                                Stock
                                <asp:TextBox ID="txtStockModal" Enabled="false" CssClass=" form-control" runat="server"></asp:TextBox>
                                Precio
                                <asp:RequiredFieldValidator ControlToValidate="txtPrecioModal" ID="RequiredFieldValidator2"
                                    runat="server" ErrorMessage="Obligatorio" ForeColor="Red" Display="Dynamic" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ControlToValidate="txtPrecioModal" runat="server" ID="compareValidator1"
                                    Display="Dynamic" ForeColor="Red" ErrorMessage="Ingrese una importe" ValidationGroup="a"
                                    Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                                <div class="form-group input-group">
                                    <span class="input-group-addon">$ </span>
                                    <asp:TextBox ID="txtPrecioModal" CssClass=" form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModificarArticulo" runat="server" Text="Modificar" CssClass="btn btn-success"
                            CausesValidation="true" ValidationGroup="a" OnClick="btnModificarArticulo_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--articulo Record Modal -->
</asp:Content>
