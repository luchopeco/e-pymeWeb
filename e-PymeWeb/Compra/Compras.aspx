<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Compras.aspx.cs" Inherits="Compra_Compras" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('.datepicker').datepicker({
                dateFormat: "dd/mm/yy",
                firstDay: 1,
                dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                dayNamesShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
                monthNames:
            ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio",
            "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                monthNamesShort:
            ["Ene", "Feb", "Mar", "Abr", "May", "Jun",
            "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"]
            });
        });
    </script>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                $('.datepicker').datepicker({
                    dateFormat: "dd/mm/yy",
                    firstDay: 1,
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    dayNamesShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
                    monthNames:
            ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio",
            "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    monthNamesShort:
            ["Ene", "Feb", "Mar", "Abr", "May", "Jun",
            "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"]
                });
            }
        }  
    </script>
    <div class="main-header">
        <h2>
            Compras
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
                    <asp:Button ID="btnNuevaCompra" CssClass="btn btn-primary btn-block" runat="server"
                        Text="Nueva Compra" OnClick="btnNuevaCompra_Click" />
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
                    .<asp:Button ID="btnBuscarCompras" CssClass=" btn btn-success btn-block" runat="server"
                        Text="Buscar" OnClick="btnBuscarCompras_Click" />
                </div>
            </div>
            <hr />
            <div class=" row">
                <div class=" col-lg-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Compras</h3>
                            <div class="btn-group widget-header-toolbar">
                                <asp:LinkButton ID="btnExoprtarAExcel" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                    OnClick="btnExoprtarAExcel_Click"><h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                            </div>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class=" col-md-12">
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvCompras" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                                            runat="server" AutoGenerateColumns="False" DataKeyNames="Idcompra" OnRowCommand="dgvCompras_RowCommand"
                                            OnRowCreated="dgvCompras_RowCreated">
                                            <Columns>
                                                <asp:BoundField DataField="Fecha" DataFormatString=" {0:d}" HeaderText="Fecha" />
                                                <asp:BoundField DataField="DescTipoCompra" HeaderText="Tipo Compra" />
                                                <asp:BoundField DataField="DescProveedor" HeaderText="Proveedor" />
                                                <asp:BoundField DataField="Total" HeaderText="Total" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                                <asp:BoundField DataField="DescComprobate" HeaderText="Comprobante" />
                                                <asp:BoundField DataField="DescSucursal" HeaderText="Sucursal" />
                                                <asp:BoundField DataField="DescUsuario" HeaderText="Usuario" />
                                                <asp:ButtonField CommandName="detCompra" Text="&lt;i class=&quot;fa fa-eye&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-info" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="editar" Text="&lt;i class=&quot;fa fa-pencil-square-o&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-warning" />
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
                            <asp:Label ID="lblTotalCompras" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
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
                            Detalle Compra</h3>
                    </div>
                    <div class=" modal-body">
                        <div class=" row">
                            <div class=" col-md-12">
                                <div class=" panel panel-default">
                                    <div class=" panel-heading">
                                        DetalleCompra</div>
                                    <div class=" panel-body">
                                        <div class=" table-responsive">
                                            <asp:DetailsView runat="server" ID="dvDetalleCompra" CssClass=" table  table-bordered table-striped table-hover no-footer"
                                                AutoGenerateRows="false">
                                                <Fields>
                                                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" />
                                                    <asp:BoundField HeaderText="Tipo Compra" DataField="DescTipoCompra" />
                                                    <asp:BoundField HeaderText="Proveedor" DataField="DescProveedor" />
                                                    <asp:BoundField HeaderText="Total" DataField="Total" />
                                                </Fields>
                                            </asp:DetailsView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=" row">
                            <div class=" panel panel-default">
                                <div class=" panel-heading">
                                    Linea De Compra</div>
                                <div class=" panel-body">
                                    <asp:HiddenField ID="hfIdCompra" runat="server" />
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvArticulos" runat="server" CssClass="table  table-bordered table-striped table-hover no-footer"
                                            AutoGenerateColumns="False" DataKeyNames="Idarticulo" OnRowCreated="dgvArticulos_RowCreated"
                                            OnRowCommand="dgvArticulos_RowCommand" OnRowDataBound="dgvArticulos_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                <asp:BoundField DataField="CostoUnitario" HeaderText="Costo Unitario" />
                                                <asp:BoundField DataField="Subtotal" HeaderText="SubTotal" />
                                                <asp:CheckBoxField DataField="DadoBaja" HeaderText="Anulado">
                                                    <ControlStyle CssClass="form-control" />
                                                </asp:CheckBoxField>
                                                <asp:ButtonField CommandName="bajaLinea" Text="&lt;i class=&quot;fa fa-eraser&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-danger" />
                                                </asp:ButtonField>
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
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
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
                        <asp:Button ID="btnAceptarModalConfirmacion" runat="server" Text="Aceptar" CssClass="btn btn-success"
                            OnClick="btnAceptarModalConfirmacion_OnClick" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->
    <!-- eliminarModal Starts here-->
    <div id="eliminarModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class=" col-md-12">
                                <asp:HiddenField ID="hfIdCompraAeliminar" runat="server" />
                                <h3>
                                    ¿Desea Eliminar la compra? NO la podra recuperar</h3>
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
