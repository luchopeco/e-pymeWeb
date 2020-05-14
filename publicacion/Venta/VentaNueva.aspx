<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="VentaNueva.aspx.cs" Inherits="Venta_VentaNueva" %>

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
    <div class="main-header">
        <h2>
            Nueva Venta
        </h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class=" col-lg-12">
                <asp:Panel ID="PanelError" Visible="true" runat="server">
                    <div class="alert alert-danger alert-dismissable">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                            ×</button>
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
                <asp:Panel ID="PanelMensaje" Visible="true" runat="server">
                    <div class="alert alert-success alert-dismissable">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                            ×</button>
                        <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>
            </div>
            <div class=" col-md-8">
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Linea de Venta
                        </h3>
                    </div>
                    <div class=" widget-content">
                        <legend>Busqueda de articulos</legend>
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
                        <hr class="inner-separator">
                        <legend>Articulos de la Venta</legend>
                        <div>
                            <asp:GridView ID="dgvArticulos" runat="server" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                                AutoGenerateColumns="False" OnRowCommand="dgvArticulos_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" />
                                    <asp:BoundField DataField="Subtotal" HeaderText="SubTotal" />
                                    <asp:ButtonField CommandName="deleteArticulo" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                        <ControlStyle CssClass="btn btn-xs btn-danger" />
                                    </asp:ButtonField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class=" col-md-4">
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Total
                        </h3>
                    </div>
                    <div class=" widget-content">
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox ID="txtTotal" CssClass=" form-control " runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Forma de pago
                        </h3>
                        <div class="btn-group widget-header-toolbar">
                            <asp:Button ID="btnNuevaFormaPago" runat="server" CssClass=" btn btn-xs btn-info"
                                Text="Nueva Forma Pago" CausesValidation="false" OnClick="btnNuevaFormaPago_Click" />
                        </div>
                    </div>
                    <div class=" widget-content">
                        <asp:GridView ID="dgvFormaPago" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                            runat="server" AutoGenerateColumns="False" OnRowCommand="dgvFormaPago_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="Forma Pago" DataField="Descripcion" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                <asp:ButtonField CommandName="deleteFP" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                    <ControlStyle CssClass="btn btn-xs btn-danger" />
                                </asp:ButtonField>
                            </Columns>
                        </asp:GridView>
                        <div>
                            Total<div class="input-group">
                                <span class="input-group-addon">$</span>
                                <asp:TextBox ID="txtTotalFP" Enabled="false" CssClass=" form-control " runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class=" widget">
                    <div class=" widget-header">
                        <h3>
                            <i class="fa fa-edit"></i>Comprobante
                        </h3>
                        <div class="btn-group widget-header-toolbar">
                            <asp:Button ID="btnAgregarComprobante" runat="server" CssClass=" btn btn-xs btn-info"
                                Text="Agregar Comprobante" CausesValidation="false" OnClick="btnAgregarComprobante_Click" />
                        </div>
                    </div>
                    <div class=" widget-content">
                        <asp:GridView ID="dgvComprobante" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                            runat="server" AutoGenerateColumns="False" OnRowCommand="dgvComprobante_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="Tipo" DataField="TipoComprobante" />
                                <asp:BoundField DataField="Numero" HeaderText="Numero" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                <asp:ButtonField CommandName="deleteComp" Text="&lt;i class='fa fa-times'&gt;&lt;/i&gt;">
                                    <ControlStyle CssClass="btn btn-xs btn-danger" />
                                </asp:ButtonField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class=" col-lg-12">
                <asp:TextBox ID="txtDescripcionVenta" runat="server" PlaceHolder="Ingrese una observacion si lo desea"
                    CssClass=" form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
            <p>
            </p>
            <div class=" col-lg-12">
                <asp:Button ID="btnGuardarVenta" Text="Agregar Venta" runat="server" CssClass="btn btn-block btn-success"
                    OnClick="btnGuardarVenta_Click" /></div>
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
                            Agregando Articulo</h3>
                    </div>
                    <div class=" modal-body">
                        <asp:HiddenField ID="hfClave" runat="server"></asp:HiddenField>
                        <div class=" col-lg-12">
                            Articulo
                            <asp:TextBox ID="txtArticuloModal" CssClass=" form-control" Enabled="false" runat="server"></asp:TextBox>
                            Precio
                            <asp:RequiredFieldValidator ControlToValidate="txtPrecioModal" ID="RequiredFieldValidator2"
                                runat="server" ErrorMessage="Obligatorio" ForeColor="Red" Display="Dynamic" ValidationGroup="cantidad"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ControlToValidate="txtPrecioModal" runat="server" ID="compareValidator2"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Ingrese un importe" ValidationGroup="cantidad"
                                Type="Double" Operator="DataTypeCheck"></asp:CompareValidator>
                            <div class="form-group input-group">
                                <span class="input-group-addon">$ </span>
                                <asp:TextBox ID="txtPrecioModal" CssClass=" form-control" runat="server"></asp:TextBox>
                            </div>
                            Cantidad
                            <asp:RequiredFieldValidator ControlToValidate="txtCantidadModal" ID="RequiredFieldValidator1"
                                runat="server" ErrorMessage="Ingrese una Cantidad" ForeColor="Red" Display="Dynamic"
                                ValidationGroup="cantidad"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ControlToValidate="txtCantidadModal" runat="server" ID="compareValidator"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="Ingrese una cantidad Valida"
                                ValidationGroup="cantidad" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                            <asp:TextBox ID="txtCantidadModal" CssClass=" form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="true" ValidationGroup="cantidad" OnClick="btnAgregar_Click" />
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
    <!--articulo Record Modal Ends here-->
    <!-- formaPago Modal Starts here-->
    <div id="formaPagoModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H3">
                            Agregar Forma de Pago</h3>
                    </div>
                    <div class=" modal-body">
                        <uc1:PanelMensajes ID="ucPanelMensajesFormaPago" runat="server" />
                        <div class=" row">
                            <div class=" col-lg-12">
                                Tipo Forma Pago<asp:DropDownList ID="cbxTipoFormaPago" CssClass=" form-control" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="cbxTipoFormaPago_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-12">
                                <asp:Panel ID="panelNotaCredito" runat="server">
                                    Numero Nota Credito
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Ingrese un monto valido"
                                        Operator="DataTypeCheck" ForeColor="Red" ValidationGroup="nc" ControlToValidate="txtNroNotaCredito"
                                        Type="Double"></asp:CompareValidator>
                                    <div class="form-group input-group">
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnResetarNotaCredito" CausesValidation="false" ToolTip="Limpiar nota credito"
                                                class="btn btn-warning" runat="server" OnClick="btnResetarNotaCredito_Click"><i class="fa fa-power-off"></i></asp:LinkButton>
                                        </span>
                                        <asp:TextBox ID="txtNroNotaCredito" CssClass=" form-control" runat="server"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnBuscarNroCredito" CausesValidation="true" ValidationGroup="nc"
                                                class="btn btn-success" ToolTip=" buscar " runat="server" OnClick="btnBuscarNroCredito_Click">
                                    <i class="fa fa-search">
</i></asp:LinkButton>
                                        </span>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class=" col-md-12">
                                Monto
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="fp"
                                    ErrorMessage="Ingrese un monto" ForeColor="Red" ControlToValidate="txtMontoFP"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator7" runat="server" ErrorMessage="Ingrese un monto valido"
                                    Operator="DataTypeCheck" ForeColor="Red" ValidationGroup="fp" ControlToValidate="txtMontoFP"
                                    Type="Double"></asp:CompareValidator>
                                <div class="input-group">
                                    <span class="input-group-addon">$</span>
                                    <asp:TextBox ID="txtMontoFP" CssClass=" form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarFormaPAgo" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="true" ValidationGroup="fp" OnClick="btnAgregarFormaPAgo_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- forma pago modal Modal Ends here-->
    <!-- confirmacion Modal Starts here-->
    <div id="confirmModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H2">
                    ¿Desea Agregar la venta?</h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class=" modal-body">
                        <div class=" row">
                            <div class=" col-lg-12">
                                <h4>
                                    Por un Total de:$
                                    <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label></h4>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarVentaFinal" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="false" OnClick="btnAgregarVentaFinal_Click" />
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
    <!-- confirmacion modal Modal Ends here-->
    <!-- comprobante Modal Starts here-->
    <div id="comprobanteModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H4">
                            Comprobante</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class=" row">
                            <div class="col-md-12">
                                Fecha
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaComprobanteModal"
                                    Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="comprobante"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator10" Operator="DataTypeCheck" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                    Type="Date" ControlToValidate="txtFechaComprobanteModal" Display="Dynamic" ValidationGroup="comprobante"></asp:CompareValidator>
                                <asp:TextBox ID="txtFechaComprobanteModal" runat="server" CssClass=" form-control datepicker"></asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                Tipo Comprobante
                                <asp:CompareValidator ID="CompareValidator9" Operator="NotEqual" ForeColor="" CssClass="validador"
                                    SetFocusOnError="True" runat="server" ErrorMessage="Seleccione tipo comprobante"
                                    ValueToCompare="0" ControlToValidate="cbxTipoComprobanteModal" Display="Dynamic"
                                    ValidationGroup="comprobante"></asp:CompareValidator>
                                <asp:DropDownList ID="cbxTipoComprobanteModal" runat="server" CssClass=" form-control">
                                    <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                                    <asp:ListItem>A</asp:ListItem>
                                    <asp:ListItem>B</asp:ListItem>
                                    <asp:ListItem>C</asp:ListItem>
                                    <asp:ListItem>X</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-12">
                                Numero Comprobante
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" ControlToValidate="txtNumeroComprobanteModal"
                                    Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="comprobante"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtNumeroComprobanteModal" runat="server" CssClass=" form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarComprobanteModal" runat="server" Text="Aceptar" CssClass="btn btn-success"
                            ValidationGroup="comprobante" CausesValidation="true" OnClick="btnAceptarComprobanteModal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!--comprobante Modal Ends here-->
</asp:Content>
