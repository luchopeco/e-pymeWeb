<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="CajaActual.aspx.cs" Inherits="Caja_CajaActual" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="main-header">
        <h2>
            <asp:Label runat="server" ID="lblFechaTitulo"></asp:Label>
        </h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            <div class="row">
                <div class="col-md-4">
                    Fondo Inicial
                    <div class="form-group input-group">
                        <span class="input-group-addon">$ </span>
                        <asp:TextBox Enabled="false" ID="txtFondoInicial" runat="server" CssClass=" form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    .
                    <asp:Button runat="server" ID="btnRetiroCaja" CssClass="btn btn-block btn-warning"
                        Text="Retiro De Caja" OnClick="btnRetiroCaja_Click" CausesValidation="false" /></div>
                <div class="col-md-4">
                    .
                    <asp:Button runat="server" ID="btnIngresoCaja" CssClass="btn btn-block btn-success"
                        Text="Ingreso De Caja" OnClick="btnIngresoCaja_Click" CausesValidation="false" />
                </div>
            </div>
            <div class="row">
                <div class=" col-md-6">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                Movimientos En Efectivo
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class=" table-responsive">
                                <asp:GridView ID="dgvMovimientosEfectivo" runat="server" CssClass="table  table-bordered table-striped table-hover no-footer"
                                    AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                        <asp:BoundField DataField="DescTipoMovimiento" HeaderText="Tipo Movimiento" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class=" col-md-6">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                Movimientos Sin Fondos
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class=" table-responsive">
                                <asp:GridView ID="dgvMovimientosSinFondos" runat="server" CssClass="table  table-bordered table-striped table-hover no-footer"
                                    AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                        <asp:BoundField DataField="DescTipoMovimiento" HeaderText="Tipo Movimiento" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    Total Teórico
                    <div class="form-group input-group">
                        <span class="input-group-addon">$ </span>
                        <asp:TextBox ID="txtTotalFondos" runat="server" Enabled="false" CssClass=" form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    Fondo Final
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFondoFinal"
                        Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="caja"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" Operator="DataTypeCheck" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un importe"
                        Type="Double" ControlToValidate="txtFondoFinal" Display="Dynamic" ValidationGroup="caja"></asp:CompareValidator>
                    <div class="form-group input-group">
                        <span class="input-group-addon">$ </span>
                        <asp:TextBox ID="txtFondoFinal" runat="server" CssClass=" form-control"></asp:TextBox>
                        <span class="input-group-addon">
                            <asp:LinkButton runat="server" ID="btnActualizarDiferencia" CssClass=" btn btn-success btn-xs"
                                OnClick="btnActualizarDiferencia_Click"><i class="fa fa-refresh"></i></asp:LinkButton>
                        </span>
                    </div>
                </div>
                <div class="col-md-4">
                    Diferencia
                    <div class="form-group input-group">
                        <span class="input-group-addon">$ </span>
                        <asp:TextBox ID="txtDiferencia" runat="server" Enabled="false" CssClass=" form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-6">
                    <asp:Button runat="server" ID="btnCerrarCaja" CssClass="btn btn-block btn-danger"
                        Text="Cerrar Caja" OnClick="btnCerrarCaja_Click" />
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
    <!-- Nuevo Cliente Modal Starts here-->
    <div id="retiroCajaModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                            Nuevo Retiro de Caja</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class="col-md-12">
                                Descripcion
                                <asp:TextBox runat="server" ID="txtDescripcionRetiroCajaModal" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                Monto
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" ControlToValidate="txtMotoRetiroModal"
                                    Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="retiro"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator3" Operator="DataTypeCheck" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un importe"
                                    Type="Double" ControlToValidate="txtMotoRetiroModal" Display="Dynamic" ValidationGroup="retiro"></asp:CompareValidator>
                                <div class="form-group input-group">
                                    <span class="input-group-addon">$ </span>
                                    <asp:TextBox ID="txtMotoRetiroModal" runat="server" Text="0" CssClass=" form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarRetiroCajaModal" CausesValidation="true" ValidationGroup="retiro"
                            runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnAceptarRetiroCajaModal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->
    <!-- Nuevo Cliente Modal Starts here-->
    <div id="ingresoCajaModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H1">
                            Nuevo Ingreso de Caja</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class="col-md-12">
                                Descripcion
                                <asp:TextBox runat="server" ID="txtDescripcionIngresoModal" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                Monto
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" ControlToValidate="txtMontoIngresoModal"
                                    Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="ingreso"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator4" Operator="DataTypeCheck" ForeColor=""
                                    CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un importe"
                                    Type="Double" ControlToValidate="txtMontoIngresoModal" Display="Dynamic" ValidationGroup="ingreso"></asp:CompareValidator>
                                <div class="form-group input-group">
                                    <span class="input-group-addon">$ </span>
                                    <asp:TextBox ID="txtMontoIngresoModal" runat="server" Text="0" CssClass=" form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarIngresoCajaModal" CausesValidation="true" ValidationGroup="ingreso"
                            runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnAceptarIngresoCajaModal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->
    <!-- Nuevo Cliente Modal Starts here-->
    <div id="confirmacionModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                    </div>
                    <div class=" modal-body ">
                        ¿Desea Cerrar la caja?
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarModalFinal" CausesValidation="true" ValidationGroup="ingreso"
                            runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnAceptarModalFinal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->
</asp:Content>
