<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="MovimientosArticulos.aspx.cs" Inherits="Articulo_MovimientosArticulos"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<%@ Register Src="../Controles/CbxSucursales.ascx" TagName="CbxSucursales" TagPrefix="uc2" %>
<%@ Register Src="../Controles/CbxTipoMovimientoArticulo.ascx" TagName="CbxTipoMovimientoArticulo"
    TagPrefix="uc3" %>
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
        function ClientArticuloStockSelected(source, eventArgs) {
            //alert(" Key : " + eventArgs.get_text() + "  Value :  " + eventArgs.get_value());
            document.getElementById('<%=hfIdArticuloStock.ClientID %>').value = eventArgs.get_value();
        }
        function SetContextKeyStock() {
            $find('<%=AutoCompleteExtender2.ClientID%>').set_contextKey($get("<%=hfIdSucursal.ClientID %>").value);
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-header">
                <h2>
                    Movimientos De Articulos
                </h2>
                Entre Sucursales - Ajustes De Stock
            </div>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class=" row">
                <div class="col-md-6">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                Movimientos Entre Sucursal
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class=" col-md-12">
                                    Desde
                                    <uc2:CbxSucursales ID="ucCbxSucursalesDesde" runat="server" />
                                </div>
                                <div class=" col-md-12">
                                    Hasta
                                    <uc2:CbxSucursales ID="ucCbxSucursalHasta" runat="server" />
                                </div>
                                <div class=" col-md-12">
                                    Articulo
                                    <asp:TextBox ID="txtDescArticulo" onkeyup="SetContextKey()" PlaceHolder="Busqueda por nombre"
                                        class="form-control" runat="server"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="3" ServiceMethod="BuscarArticulosVenta"
                                        UseContextKey="true" ServicePath="~/Autocomplete.asmx" TargetControlID="txtDescArticulo"
                                        OnClientItemSelected="ClientArticuloSelected">
                                    </asp:AutoCompleteExtender>
                                    <asp:HiddenField ID="hfIdArticuloDescripcion" runat="server" />
                                    <asp:HiddenField ID="hfIdSucursal" runat="server" />
                                </div>
                                <div class=" col-md-12">
                                    Cantidad
                                    <asp:RequiredFieldValidator ControlToValidate="txtCantidadSucursal" ID="RequiredFieldValidator1"
                                        runat="server" ErrorMessage="Ingrese una Cantidad" ForeColor="Red" Display="Dynamic"
                                        ValidationGroup="sucu"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ControlToValidate="txtCantidadSucursal" runat="server" ID="compareValidator"
                                        Display="Dynamic" ForeColor="Red" ErrorMessage="Ingrese una cantidad Valida"
                                        ValidationGroup="sucu" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                    <asp:TextBox ID="txtCantidadSucursal" CssClass=" form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class=" col-md-12">
                                    Observaciones
                                    <asp:TextBox ID="txtObservacionesSucursal" runat="server" CssClass=" form-control"
                                        TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class=" col-md-12">
                                    <asp:Button ID="btnAgregarMovimientoSucursal" runat="server" CausesValidation="true"
                                        ValidationGroup="sucu" Text="Agregar" CssClass=" btn btn-block btn-success" OnClick="btnAgregarMovimientoSucursal_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                Ajustes de Stock
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class=" col-md-12">
                                    <uc2:CbxSucursales ID="ucCbxSucursalesStock" runat="server" />
                                </div>
                                <div class=" col-md-12">
                                    <uc3:CbxTipoMovimientoArticulo ID="ucCbxTipoMovimientoArticulo" runat="server" />
                                </div>
                                <div class=" col-md-12">
                                    Articulo
                                    <asp:TextBox ID="txtDescArtciuloStock" onkeyup="SetContextKeyStock()" PlaceHolder="Busqueda por nombre"
                                        class="form-control" runat="server"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="3" ServiceMethod="BuscarArticulosVenta"
                                        UseContextKey="true" ServicePath="~/Autocomplete.asmx" TargetControlID="txtDescArtciuloStock"
                                        OnClientItemSelected="ClientArticuloStockSelected">
                                    </asp:AutoCompleteExtender>
                                    <asp:HiddenField ID="hfIdArticuloStock" runat="server" />
                                </div>
                                <div class=" col-md-12">
                                    Cantidad
                                    <asp:RequiredFieldValidator ControlToValidate="txtCantidadStock" ID="RequiredFieldValidator2"
                                        runat="server" ErrorMessage="Ingrese una Cantidad" ForeColor="Red" Display="Dynamic"
                                        ValidationGroup="stock"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ControlToValidate="txtCantidadStock" runat="server" ID="compareValidator1"
                                        Display="Dynamic" ForeColor="Red" ErrorMessage="Ingrese una cantidad Valida"
                                        ValidationGroup="stock" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                    <asp:TextBox ID="txtCantidadStock" CssClass=" form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class=" col-md-12">
                                    Observaciones
                                    <asp:TextBox ID="txtObservacionesStock" runat="server" CssClass=" form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class=" col-md-12">
                                    <asp:Button ID="btnAgregarMovimiento" runat="server" CausesValidation="true" ValidationGroup="stock"
                                        Text="Agregar" CssClass=" btn btn-block btn-success" OnClick="btnAgregarMovimiento_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                Movmientos de Articulos
                            </h3>
                            <div class="btn-group widget-header-toolbar">
                                <asp:LinkButton ID="btnExoprtarAExcel" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                    OnClick="btnExoprtarAExcel_Click"><h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                            </div>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class=" col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor=""
                                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaDesde"
                                        Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="m"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator2" Operator="DataTypeCheck" ForeColor=""
                                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                        Type="Date" ControlToValidate="txtFechaDesde" Display="Dynamic" ValidationGroup="m"></asp:CompareValidator>
                                    Fecha Desde<div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtFechaDesde" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class=" col-md-4">
                                    Fecha Hasta
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor=""
                                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaHasta"
                                        Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="m"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator3" Operator="DataTypeCheck" ForeColor=""
                                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                        Type="Date" ControlToValidate="txtFechaHasta" Display="Dynamic" ValidationGroup="m"></asp:CompareValidator>
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="txtFechaHasta" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class=" col-md-4">
                                    .<asp:Button ID="btnBuscarMovimientos" CssClass=" btn btn-success btn-block" runat="server"
                                        Text="Buscar" OnClick="btnBuscarMovimientos_Click" ValidationGroup="m" CausesValidation="true" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class=" col-md-12">
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvMovimientos" AutoGenerateColumns="false" CssClass=" table table-bordered table-condensed table-hover"
                                            runat="server">
                                            <Columns>
                                                <asp:BoundField HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField HeaderText="Sucursal Desde" DataField="DescSucursalDesde" />
                                                <asp:BoundField HeaderText="Sucursal Hasta" DataField="DescSucursalHasta" />
                                                <asp:BoundField HeaderText="Tipo Movimiento" DataField="DescTipoMovimiento" />
                                                <asp:BoundField HeaderText="Articulo" DataField="DescArticulo" />
                                                <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" />
                                                <asp:BoundField HeaderText="Observacion" DataField="Observacion" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExoprtarAExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <!-- confirmacion Modal Starts here-->
    <div id="confirmacionModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                        </h3>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class=" col-md-12">
                                <asp:Label ID="lblModal" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarModal" runat="server" Text="Agregar" CssClass="btn btn-success"
                            OnClick="btnAceptarModal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->
</asp:Content>
