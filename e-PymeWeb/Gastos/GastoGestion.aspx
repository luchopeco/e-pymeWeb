<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="GastoGestion.aspx.cs" Inherits="Gastos_GastoGestion" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="../js/datepickerPropio.js" type="text/javascript"></script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-header">
                <h2>
                    Gestion Gastos
                </h2>
            </div>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class="row">
                <div class=" col-md-12">
                    <asp:Button ID="btnNuevoGasto" runat="server" CssClass=" btn btn-block btn-primary"
                        Text="Nuevo Gasto" CausesValidation="false" OnClick="btnNuevoGasto_Click" />
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-md-5">
                    Fecha Desde
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaDesde"
                        Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="fecha"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator3" Operator="DataTypeCheck" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                        Type="Date" ControlToValidate="txtFechaDesde" Display="Dynamic" ValidationGroup="fecha"></asp:CompareValidator>
                    <asp:TextBox ID="txtFechaDesde" runat="server" CssClass=" form-control datepicker"></asp:TextBox>
                </div>
                <div class="col-md-5">
                    Fecha Hasta
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaHasta"
                        Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="fecha"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator4" Operator="DataTypeCheck" ForeColor=""
                        CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                        Type="Date" ControlToValidate="txtFechaHasta" Display="Dynamic" ValidationGroup="fecha"></asp:CompareValidator>
                    <asp:TextBox ID="txtFechaHasta" runat="server" CssClass=" form-control datepicker"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    .
                    <asp:LinkButton ID="btnBuscarGastos" runat="server" CssClass="btn btn-block btn-success"
                        ValidationGroup="fecha" CausesValidation="true" OnClick="btnBuscarGastos_Click"><i class="fa fa-search"></i></asp:LinkButton>
                </div>
            </div>
            <hr />
            <div class=" row">
                <div class="col-md-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Gastos
                            </h3>
                            <div class="btn-group widget-header-toolbar">
                                <asp:LinkButton ID="btnExoprtarAExcel" runat="server" ToolTip="Exportar a Planilla de Calculo"
                                    OnClick="btnExoprtarAExcel_Click"><h3><i class="fa fa-file-excel-o"></i></h3></asp:LinkButton>
                            </div>
                        </div>
                        <div class=" widget-content">
                            <div class=" table-responsive">
                                <asp:GridView ID="dgvGastos" CssClass=" table table-bordered table-condensed table-hover"
                                    AutoGenerateColumns="false" runat="server" DataKeyNames="IdGasto" OnRowCommand="dgvGastos_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="DescTipoGasto" HeaderText="Tipo Gasto" />
                                        <asp:BoundField DataField="DescFormaPago" HeaderText="Forma Pago" />
                                        <asp:BoundField DataField="Monto" HeaderText="Monto" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        <asp:BoundField DataField="DescSucursal" HeaderText="Sucursal" />
                                        <asp:CheckBoxField DataField="Anulado" HeaderText="Anulado" />
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
                        <div class=" widget-footer">
                            <asp:Label ID="lblTotalGastos" runat="server"></asp:Label>
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
    <!-- gastoModal Starts here-->
    <div id="gastoModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                            <asp:Label ID="lblTituloModalGasto" runat="server"></asp:Label></h3>
                    </div>
                    <div class=" modal-body ">
                        <div class=" widget">
                            <div class=" widget-header">
                                <h3>
                                    <i class="fa fa-edit"></i>
                                    <asp:Label ID="lblPanelModalGasto" runat="server"></asp:Label>
                                </h3>
                            </div>
                            <div class=" widget-content">
                                <div class="row">
                                    <div class="col-md-12">
                                        Fecha
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor=""
                                            CssClass="validador" SetFocusOnError="True" ControlToValidate="txtFechaModal"
                                            Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="gasto"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" Operator="DataTypeCheck" ForeColor=""
                                            CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese una fecha"
                                            Type="Date" ControlToValidate="txtFechaModal" Display="Dynamic" ValidationGroup="gasto"></asp:CompareValidator>
                                        <asp:TextBox ID="txtFechaModal" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                    </div>
                                    <div class="col-md-12">
                                        Tipo Gasto
                                        <asp:DropDownList ID="cbxTipoGasto" runat="server" CssClass=" form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class=" col-md-12">
                                        Forma Pago
                                        <asp:DropDownList ID="cbxFormaPago" runat="server" CssClass=" form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-12">
                                        Monto
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor=""
                                            CssClass="validador" SetFocusOnError="True" ControlToValidate="txtMontoModal"
                                            Display="Dynamic" ErrorMessage="Obligatorio " ValidationGroup="gasto"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator2" Operator="DataTypeCheck" ForeColor=""
                                            CssClass="validador" SetFocusOnError="True" runat="server" ErrorMessage="Ingrese un monto"
                                            Type="Double" ControlToValidate="txtMontoModal" Display="Dynamic" ValidationGroup="gasto"></asp:CompareValidator>
                                        <asp:TextBox ID="txtMontoModal" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-12">
                                        Descripcion
                                        <asp:TextBox ID="txtObservaionGastoModal" runat="server" CssClass="form-control"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarModalGasto" runat="server" Text="" CssClass="btn btn-success"
                            CausesValidation="true" ValidationGroup="gasto" OnClick="btnAceptarModalGasto_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--gastoModal Modal Ends here-->
    <!-- confirmacionModal Starts here-->
    <div id="confirmacionModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H1">
                            Anular Gasto</h3>
                    </div>
                    <div class=" modal-body ">
                        <asp:HiddenField runat="server" ID="hfidGastoConfirmacionModal" />
                        <div class="row">
                            <div class="col-md-12">
                                ¿Desea anular el gasto?
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarConfirmar" runat="server" Text="Aceptar" CssClass="btn btn-success"
                            OnClick="btnAceptarConfirmar_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--confirmacionModalEnds here-->
</asp:Content>
