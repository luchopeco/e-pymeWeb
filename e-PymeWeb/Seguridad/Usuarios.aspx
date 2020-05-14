<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Usuarios.aspx.cs" Inherits="Seguridad_Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                $('.datepicker').datepicker({
                    dateFormat: "dd-mm-yy",
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
    <div class=" main-header">
        <h2>
            Gestion de usuarios</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelError" Visible="false" runat="server">
                <div class="alert alert-danger alert-dismissable">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                        ×</button>
                    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="PanelMensaje" Visible="false" runat="server">
                <div class="alert alert-success alert-dismissable">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                        ×</button>
                    <asp:Label ID="LabelMensaje" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
            <div class="row">
                <div class=" col-lg-6">
                    <p>
                        <asp:Button ID="btnNuevoUsuario" runat="server" Text="Nuevo Usuario" CssClass="btn btn-outline btn-primary btn-lg btn-block"
                            OnClick="btnNuevoUsuario_Click" CausesValidation="False" />
                    </p>
                </div>
            </div>
            <div class=" row">
                <div class="col-md-12">
                    <div class=" table-responsive">
                        <asp:GridView ID="dgvUsuario" CssClass="table table-striped table-bordered table-hover"
                            runat="server" AutoGenerateColumns="False" OnRowCommand="dgvUsuario_RowCommand"
                            DataKeyNames="Idusuario" PageSize="100" EnableModelValidation="True">
                            <Columns>
                                <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre Usuario" />
                                <asp:BoundField DataField="NombreApellido" HeaderText="Nombre" />
                                <asp:BoundField DataField="FechaBaja" HeaderText="Fecha Baja" />
                                <asp:CheckBoxField DataField="EsSuperUsuario" HeaderText="Es Super Usuario" />
                                <asp:ButtonField Text="<span class='ui-icon ui-icon-pencil'>" CommandName="editRecord"
                                    CausesValidation="false" />
                                <asp:ButtonField Text="<span class='ui-icon ui-icon-trash'>" CommandName="deleteRecord"
                                    CausesValidation="false" />
                                <asp:ButtonField HeaderText="Roles" Text="<span class='ui-icon ui-icon-document'>"
                                    CommandName="roles" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Add Record Modal Starts here-->
    <div id="addModal" class=" modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="addModalLabel">
                    Agregar Usuario</h3>
            </div>
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>
                                    Nombre Usuario
                                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Ingrese un nombre de usuario"
                                        ControlToValidate="txtNombreUsuario" ForeColor="Red" ValidationGroup="agregar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nombre y Apellido
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Ingrese un nombre de usuario"
                                        ControlToValidate="txtNombreUsuario" ForeColor="Red" ValidationGroup="agregar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Clave
                                    <asp:TextBox ID="txtClave" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Ingrese una clave"
                                        ControlToValidate="txtClave" ForeColor="Red" ValidationGroup="agregar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Reingrese Clave
                                    <asp:TextBox ID="txtClaveReingresada" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                    <div>
                                        <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtClaveReingresada"
                                            runat="server" ForeColor="Red" ErrorMessage="Las claves no Coinciden" ControlToCompare="txtClave"
                                            ValidationGroup="agregar"></asp:CompareValidator></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Baja
                                    <asp:TextBox ID="txtFechaBaja" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarUsuario" runat="server" Text="Agregar" CssClass="btn btn-success"
                            OnClick="btnAgregarUsuario_Click" CausesValidation="true" ValidationGroup="agregar" />
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
    <!-- Edit Record Modal Starts here-->
    <div id="editModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H1">
                    Modificar Usuario</h3>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>
                                    Nombre Usuario
                                    <asp:TextBox ID="txtNombreUsuarioModificar" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nombre y Apellido
                                    <asp:TextBox ID="txtNombreModificar" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Clave
                                    <asp:TextBox ID="txtClaveModificar" placeholder="Si la clave es vacia no se modifica"
                                        runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Reingrese Clave
                                    <asp:TextBox ID="txtClaveReingresadaModificar" runat="server" CssClass="form-control"
                                        TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Baja
                                    <asp:TextBox ID="txtFechaBajaModificar" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                    <asp:HiddenField ID="hfId" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-success"
                            CausesValidation="true" ValidationGroup="modificar" OnClick="btnModificar_Click" />
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
    <!--Edit Record Modal Ends here-->
</asp:Content>
