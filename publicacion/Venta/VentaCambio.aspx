<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="VentaCambio.aspx.cs" Inherits="Venta_VentaCambio" %>

<%@ Register Src="../Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-header">
                <h2>
                    Realizar Cambio
                </h2>
                venta
            </div>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class=" row">
                <div class="col-md-12">
                    <div class=" widget">
                        <div class=" widget-header">
                            <h3>
                                <i class="fa fa-edit"></i>Cambio
                            </h3>
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="multiselect dropdown-toggle btn btn-sm" data-toggle="dropdown"
                                        title="Bar">
                                        <i class="fa fa-question-circle"></i><b class="caret"></b>
                                    </button>
                                    <ul class="multiselect-container dropdown-menu pull-right">
                                        <li>Un cambio se realizan entre articulos del mismo grupo. Un ejemplo de cambio serian
                                            articulos con talle, se pueden realizar cambios entre mismos grupos de articulos
                                            de diferente talle. Este cambio no genera cargo y se manteniene el costo del articulo
                                            anterior</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class=" widget-content">
                            <div class="row">
                                <div class=" col-md-12">
                                    <div class=" widget">
                                        <div class=" widget-header">
                                            <h3>
                                                <i class="fa fa-edit"></i>Referido a la venta
                                            </h3>
                                        </div>
                                        <div class=" widget-content">
                                            <div class="row">
                                                <div class=" col-md-12">
                                                    <div class=" table-responsive">
                                                        <asp:GridView ID="dgvVEnta" AutoGenerateColumns="false" CssClass=" table table-bordered table-condensed table-hover"
                                                            runat="server">
                                                            <Columns>
                                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                                <asp:BoundField DataField="Total" HeaderText="Total" />
                                                                <asp:BoundField DataField="NumeroComprobante" HeaderText="Comprobante" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
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
                                                <i class="fa fa-edit"></i>Articulos a cambiar
                                            </h3>
                                        </div>
                                        <div class=" widget-content">
                                            <div class=" table-responsive">
                                                <asp:GridView ID="dgvArticulosACambiar" AutoGenerateColumns="False" CssClass=" table table-bordered table-condensed table-hover"
                                                    runat="server" DataKeyNames="Idarticulo" OnRowCommand="dgvArticulosACambiar_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="DescArticulo" HeaderText="Articulo" />
                                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" />
                                                        <asp:CheckBoxField DataField="DadoBaja" HeaderText="Cambiado" />
                                                        <asp:CheckBoxField DataField="Devuelto" HeaderText="Devuelto" />
                                                        <asp:ButtonField CommandName="cambiar" Text="&gt;&gt;">
                                                            <ControlStyle CssClass="btn btn-success btn-xs" />
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
    <!-- articuloCambioModal Starts here-->
    <div id="articuloCambioModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                            Cambio de articulo</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class="col-md-12">
                                <div class=" widget">
                                    <div class=" widget-header">
                                        <h3>
                                            <i class="fa fa-edit"></i>Articulos
                                        </h3>
                                        <div class="pull-right">
                                            <div class="btn-group">
                                                <button type="button" class="multiselect dropdown-toggle btn btn-sm" data-toggle="dropdown"
                                                    title="Bar">
                                                    <i class="fa fa-question-circle"></i><b class="caret"></b>
                                                </button>
                                                <ul class="multiselect-container dropdown-menu pull-right">
                                                    <li>Solo por esos articulos puede cambiar el articulo seleccionado</li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <div class=" widget-content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                Agrupacion de Articulos
                                                <asp:TextBox ID="txtAgrupacionArticuloModal" runat="server" CssClass=" form-control"
                                                    Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                Articulo a CAmbiar
                                                <asp:TextBox ID="txtArticuloACambiar" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hfIdArticuloACambiar" runat="server" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                Nuevo Articulo
                                                <asp:DropDownList ID="cbxArticulosModal" runat="server" CssClass=" form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                Cantidad
                                                <asp:TextBox ID="txtCantidad" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAceptarModalCambio" runat="server" Text="Aceptar" CssClass="btn btn-success"
                            OnClick="btnAceptarModalCambio_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--articuloCambioModal Ends here-->
</asp:Content>
