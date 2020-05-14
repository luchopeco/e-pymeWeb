<%@ page title="" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="ABMGenerico, App_Web_jl2n3mjl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
            <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label>
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
            <div class="row">
                <div class=" col-lg-6">
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-primary btn-lg btn-block"
                        OnClick="btnNuevo_Click" />
                </div>
            </div>
            <p>
            </p>
            <div class=" panel panel-default">
                <div class=" panel-heading">
                    <asp:Label ID="lblPanel" runat="server" Text=""></asp:Label>
                    <div class="pull-right">
                        <div class="btn-group">
                            <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                LEER <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu pull-right" role="menu">
                                <%--                                <li><a>Hay problema con la Validaciones del lado del cliente.</a></li>
                                <li><a>Por Ahora las validaciones del lado del servidor tiran</a></li>
                                <li><a>errores Genericos</a></li>--%>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class=" panel-body">
                    <div class=" table table-responsive">
                        <asp:GridView ID="dgv" runat="server" CssClass="table table-striped table-bordered table-hover dataTable no-footer"
                            OnRowEditing="dgv_RowEditing" OnRowDeleting="dgv_RowDeleting" OnRowCreated="dgv_RowCreated"
                            PageSize="1000">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" EditText="<i class='fa fa-pencil-square-o'></i>"
                                    InsertText="Agregar" UpdateText="Aceptar" CausesValidation="False" DeleteText="<i class='fa fa-times'></i>">
                                    <ControlStyle CssClass="btn btn-xs btn-info" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Delete Record Modal Starts here-->
    <div id="deleteModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="delModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="delModalLabel">
                    Eliminar</h3>
            </div>
            <asp:UpdatePanel ID="upDel" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        ¿Desea eliminar
                        <asp:Label ID="lblEliminar" runat="server" Text=""></asp:Label>?
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnEliminarMenu" runat="server" Text="Eliminar" CssClass="btn btn-success"
                            CausesValidation="False" OnClick="btnEliminarMenu_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnEliminarPagina" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Delete Record Modal Ends here -->
    <!-- Add Record Modal Starts here-->
    <div id="addModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="addModalLabel">
                    Agregar</h3>
            </div>
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class=" col-lg-12">
                        <div id="divTabla" class="table table-responsive" runat="server">
                            <asp:Table ID="tablaAgregar" CssClass="table table-bordered table-hover table-responsive"
                                runat="server">
                            </asp:Table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgrear" runat="server" Text="Agregar" CssClass="btn btn-success"
                            CausesValidation="False" OnClick="btnAgrear_Click" />
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
    <!-- edit Record Modal Starts here-->
    <div id="editModal" class=" modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H1">
                    Modificar</h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hfClave" runat="server"></asp:HiddenField>
                    <div class=" col-lg-12">
                        <div id="div2" class="table table-responsive" runat="server">
                            <asp:Table ID="tablaModificar" CssClass="table table-bordered table-hover table-responsive"
                                runat="server">
                            </asp:Table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-success"
                            CausesValidation="False" OnClick="btnModificar_Click" />
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
</asp:Content>
