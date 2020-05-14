<%@ page title="" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="RolesGestion, App_Web_b1y0b0l5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="main-header">
        <h2>
            Gestion de Roles</h2>
        <em>Desde aqui se gestiones los roles</em>
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
            <div class="col-lg-6">
                <p>
                    <asp:Button ID="btnNuevoRol" runat="server" Text="Nuevo Rol" CssClass="btn btn-primary btn-lg btn-block"
                        CausesValidation="False" OnClick="btnNuevoRol_Click" /></p>
            </div>
            <div class="col-lg-12">
                <div class="table-responsive">
                </div>
            </div>
            <asp:GridView ID="dgvRoles" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                AllowPaging="True" DataKeyNames="IdRol" CssClass="table table-bordered table-hover tablesorter"
                EnableModelValidation="True" OnRowCommand="dgvRoles_RowCommand" PageSize="100">
                <Columns>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                    <asp:ButtonField CommandName="editRol" ControlStyle-CssClass="btn btn-xs btn-info"
                        ButtonType="Button" Text="Editar"></asp:ButtonField>
                    <asp:ButtonField Text="Editar Paginas" ControlStyle-CssClass="btn btn-xs btn-info"
                        CommandName="editPaginas"></asp:ButtonField>
                    <asp:ButtonField CommandName="deleteRecord" ControlStyle-CssClass="btn btn-xs btn-info"
                        ButtonType="Button" Text="Eliminar">
                        <ControlStyle CssClass="btn btn-xs btn-info" />
                    </asp:ButtonField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Add Record Modal Starts here-->
    <div id="addModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="addModalLabel">
                    Agregar Rol</h3>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar una descripcion"
                        ControlToValidate="txtDescripcion"></asp:RequiredFieldValidator></div>
            </div>
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>
                                    Descripcion
                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarRol" runat="server" Text="Agregar" CssClass="btn btn-success"
                            OnClick="btnAgregarRol_Click" />
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
    <!-- Delete Record Modal Starts here-->
    <div id="deleteModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="delModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="delModalLabel">
                    Eliminar Rol</h3>
            </div>
            <asp:UpdatePanel ID="upDel" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        ¿Desea eliminar el Rol:
                        <asp:Label ID="lblRolEliminar" runat="server" Text=""></asp:Label>?
                        <asp:HiddenField ID="hfCode" runat="server" />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnEliminarRol" runat="server" Text="Eliminar" CssClass="btn btn-success"
                            CausesValidation="False" OnClick="btnEliminarRol_Click" />
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
    <!-- Edit Record Modal Starts here-->
    <div id="editModal" class=" modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
                <h3 id="H1">
                    Editar Rol</h3>
                <div>
                    <asp:RequiredFieldValidator ValidationGroup="modif" ID="RequiredFieldValidator1"
                        runat="server" ErrorMessage="Debe indicar una descripcion" ControlToValidate="txtDescripcionModificar"></asp:RequiredFieldValidator></div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <table class="table table-bordered table-hover">
                            <tr>
                                <td>
                                    Descripcion
                                    <asp:TextBox ID="txtDescripcionModificar" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModificarRol" runat="server" Text="Modificar" CssClass="btn btn-success"
                            OnClick="btnModificarRol_Click" ValidationGroup="modif" />
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
