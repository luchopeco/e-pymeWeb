<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="Ventas.aspx.cs" Inherits="Venta_Ventas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">    
    <script src="../js/datepickerPropio.js"></script>
    <div class="main-header">
        <h2>
            Ventas
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
            <div class=" row">
                <div class=" col-lg-12">
                    <asp:Button ID="btnNuevaVenta" CssClass="btn btn-primary btn-block" runat="server"
                        Text="Nueva Venta" OnClick="btnNuevaVenta_Click" />
                </div>
            </div>
            <hr />
            <div class="row">
                <div class=" col-md-4">
                    Fecha Desde<div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                        <asp:TextBox ID="txtFechaDesde" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class=" col-md-4">
                    Fecha Hasta<div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                        <asp:TextBox ID="txtFechaHasta" CssClass=" form-control datepicker" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class=" col-md-4">
                    .<asp:Button ID="btnBuscarVentas" CssClass=" btn btn-success btn-block" runat="server"
                        Text="Buscar" OnClick="btnBuscarVentas_Click" />
                </div>
            </div>
            <hr />
            <div class=" row">
                <div class=" col-lg-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Ventas
                            </h3>
                            <div class="btn-group widget-header-toolbar">
                                <asp:LinkButton ID="btnExoprtarAExcel" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                    OnClick="btnExoprtarAExcel_Click"><h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                            </div>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvVentas" CssClass="table table-striped table-sorting table-bordered table-hover dataTable no-footer"
                                            runat="server" AutoGenerateColumns="False" DataKeyNames="Idventa" OnRowCommand="dgvVentas_RowCommand"
                                            OnRowDataBound="dgvVentas_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="Fecha" DataFormatString=" {0:d}" HeaderText="Fecha" />
                                                <asp:BoundField DataField="Total" HeaderText="Total" />
                                                <asp:BoundField DataField="DescripcionArticulos" HeaderText="Articulos" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                                <asp:BoundField DataField="DescUsuario" HeaderText="Usuario" />
                                                <asp:ButtonField CommandName="detVenta" Text="&lt;i class=&quot;fa fa-eye&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-info" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="editarDescripcion" Text="&lt;i class=&quot;fa fa-pencil-square-o&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-primary" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="cambio" Text="&lt;i class=&quot;fa fa-retweet&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-success" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="editarFormaPago" Text="&lt;i clas
                                                    s=&quot;fa fa-money&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-info" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="notaCredito" Text="&lt;i class=&quot;fa fa-clipboard&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-warning" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="cambios" Text="&lt;i class=&quot;fa fa-clock-o&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-primary" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="eliminar" Text="&lt;i class=&quot;fa fa-times&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-danger" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=" widget-footer">
                            <h4>
                                <asp:Label ID="lblTotal" runat="server"></asp:Label></h4>
                        </div>
                    </div>
                </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="100">
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
    <!-- Add Record Modal Starts here-->
    <div id="detModal" class=" modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                            Detalle Venta</h3>
                    </div>
                    <div class=" modal-body">
                        <div class="row">
                            <div class=" col-md-12">
                                <div class=" panel panel-default">
                                    <div class=" panel-heading">
                                        Detalle Venta</div>
                                    <div class=" panel-body">
                                        <div class=" table-responsive">
                                            <asp:DetailsView ID="dvDetalleVenta" runat="server" CssClass="table table-bordered table-condensed table-hover"
                                                AutoGenerateRows="false">
                                                <Fields>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                                </Fields>
                                            </asp:DetailsView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class=" row">
                            <div class=" panel panel-default">
                                <div class=" panel-heading">
                                    Linea De Venta</div>
                                <div class=" panel-body">
                                    <asp:HiddenField ID="hfIdVenta" runat="server" />
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvArticulos" runat="server" CssClass="table  table-bordered table-striped table-hover no-footer"
                                            AutoGenerateColumns="False" DataKeyNames="Idarticulo" OnRowDataBound="dgvArticulos_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio" />
                                                <asp:BoundField DataField="Subtotal" HeaderText="SubTotal" />
                                                <asp:CheckBoxField DataField="Devuelto" HeaderText="Devuelto">
                                                    <ControlStyle CssClass="form-control" />
                                                </asp:CheckBoxField>
                                                <asp:CheckBoxField DataField="DadoBaja" HeaderText="Anulado">
                                                    <ControlStyle CssClass="form-control" />
                                                </asp:CheckBoxField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=" row">
                            <div class=" panel panel-default">
                                <div class=" panel-heading">
                                    Formas de Pago</div>
                                <div class=" panel-body">
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvFormaPago" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                                            runat="server" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField HeaderText="Forma Pago" DataField="Descripcion" />
                                                <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Detalle Modal Ends here-->
    <!--Add Record Modal Ends here-->
    <!-- confirmacion Modal Starts here-->
    <div id="confirmacionModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAddConfirmacion" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                    </div>
                    <div class=" modal-body ">
                        <asp:HiddenField ID="hfIdarticulo" runat="server" />
                        <asp:Label ID="lblModal" runat="server" Text="Label"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarModalConfirmacion" runat="server" Text="Aceptar" CssClass="btn btn-success" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->
    <!--historiaCambiosModal Starts here-->
    <div id="historiaCambiosModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H1">
                            Historial de Cambios</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class=" row">
                            <div class=" col-md-12">
                                <div class=" widget">
                                    <div class=" widget-header">
                                        <h3>
                                            <i class="fa fa-edit"></i>Cambios Realizados
                                        </h3>
                                    </div>
                                    <div class=" widget-content">
                                        <div class=" table-responsive">
                                            <asp:GridView ID="dgvHistorialCambio" runat="server" AutoGenerateColumns="false"
                                                CssClass=" table table-bordered table-condensed table-hover table">
                                                <Columns>
                                                    <asp:BoundField DataField="DescArticuloAnterior" HeaderText="Articulo Cambiado" />
                                                    <asp:BoundField DataField="DescArticulo" HeaderText="Articulo Actual" />
                                                    <asp:BoundField DataField="FechaCambio" HeaderText="Fecha Cambio" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--historiaCambiosModal Ends here-->
    <!-- modificarDescripcionModal Starts here-->
    <div id="modificarDescripcionModal" class="modal fade" tabindex="-1" role="dialog"
        aria-labelledby="addModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H2">
                            Modificar Descripcion Venta</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class=" col-md-12 ">
                                <asp:HiddenField ID="hfIdVentaModificarDescripcion" runat="server" />
                                <asp:TextBox ID="txtDescripcionModificarModal" runat="server" CssClass=" form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarModificarDescModal" runat="server" Text="Modificar" CssClass="btn btn-success"
                            OnClick="btnAceptarModificarDescModal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--modificarDescripcionModal Ends here-->
    <!-- eliminarModal Starts here-->
    <div id="eliminarModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class=" col-md-12">
                                <asp:HiddenField ID="hfIdVentaAeliminar" runat="server" />
                                <h3>
                                    ¿Desea Eliminar la Venta? NO la podra recuperar</h3>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarEliminar" runat="server" Text="Aceptar" CssClass="btn btn-success"
                            OnClick="btnAceptarEliminar_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--eliminarModal Ends here-->
</asp:Content>
