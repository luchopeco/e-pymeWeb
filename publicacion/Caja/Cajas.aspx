<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Cajas.aspx.cs" Inherits="Caja_Cajas" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
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
            Cajas
        </h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
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
                    .<asp:Button ID="btnBuscarCajas" CssClass=" btn btn-success btn-block" runat="server"
                        Text="Buscar" OnClick="btnBuscarCajas_Click" />
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                Cajas Historicas
                            </h3>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class=" table-responsive">
                                        <asp:GridView ID="dgvCajas" CssClass=" table table-bordered table-condensed table-hover"
                                            AutoGenerateColumns="False" runat="server" DataKeyNames="Idcaja" OnRowCommand="dgvCajas_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                <asp:BoundField DataField="FechaCierre" HeaderText="Cerrada" />
                                                <asp:BoundField DataField="DescUsuario" HeaderText="Usuario" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion " />
                                                <asp:BoundField DataField="FondoInicial" HeaderText="Fondo Inicial" />
                                                <asp:BoundField DataField="FondoFinal" HeaderText="FondoFinal" />
                                                <asp:BoundField DataField="TotalMovimientos" HeaderText="Total" />
                                                <asp:ButtonField CommandName="reabrir" 
                                                    Text="&lt;i class=&quot;fa fa-key&quot;&gt;&lt;/i&gt;">
                                                <ControlStyle CssClass="btn btn-xs btn-primary" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="detalle" Text="&lt;i class=&quot;fa fa-bars&quot;&gt;&lt;/i&gt;">
                                                    <ControlStyle CssClass="btn btn-xs btn-info" />
                                                </asp:ButtonField>
                                                <asp:ButtonField CommandName="cerrar" Text="Cerrar">
                                                    <ControlStyle CssClass="btn btn-xs btn-danger" />
                                                </asp:ButtonField>
                                                
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
    </asp:UpdatePanel>
    <!-- Nuevo Cliente Modal Starts here-->
    <div id="detalleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                            Detalle Caja</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class="col-md-12">
                                <div class=" widget">
                                    <div class=" widget-header">
                                        <h3>
                                            Datos Generales
                                        </h3>
                                    </div>
                                    <div class=" widget-content">
                                        <div class="table-responsive">
                                            <asp:DetailsView ID="dvDatosGeneralesModal" AutoGenerateRows="false" runat="server"
                                                CssClass=" table table-bordered table-hover table-condensed">
                                                <Fields>
                                                    <asp:BoundField HeaderText="Usuario" DataField="DescUsuario" />
                                                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" />
                                                    <asp:BoundField HeaderText="Cierre" DataField="FechaCierre" />
                                                    <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" />
                                                    <asp:BoundField HeaderText="FondoInicial" DataField="FondoInicial" />
                                                    <asp:BoundField HeaderText="FondoFinal" DataField="FondoFinal" />
                                                    <asp:BoundField HeaderText="Diferencia" DataField="TotalMovimientos" />
                                                </Fields>
                                            </asp:DetailsView>
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
                        </div>
                        <div class="row">
                            <div class="col-md-12">
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
                    </div>
                    <div class="modal-footer">
                     
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->

        <!-- Nuevo Cliente Modal Starts here-->
    <div id="reabirModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                    </div>
                    <div class=" modal-body ">
                        ¿Desea Reabrir la caja?
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnReabrirFinal" CausesValidation="true" ValidationGroup="ingreso"
                            runat="server" Text="Aceptar" CssClass="btn btn-success" 
                            onclick="btnReabrirFinal_Click"  />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Nuevo Cliente Modal Ends here-->
</asp:Content>
