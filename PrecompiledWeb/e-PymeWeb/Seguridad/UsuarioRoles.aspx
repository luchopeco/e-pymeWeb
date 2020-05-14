<%@ page title="" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="UsuarioRoles, App_Web_b1y0b0l5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h2 class=" main-header">
        Asignacion de roles
    </h2>
    <h3>
        Usuario:
        <asp:Label ID="lblUsuario" runat="server" Text=""></asp:Label>
    </h3>
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
            <div class=" row">
                <div class=" col-lg-6">
                    <p>
                        <asp:Button ID="btnSuperUsuario" runat="server" Text="" CssClass="btn btn-outline btn-primary btn-lg btn-block"
                            OnClick="btnSuperUsuario_Click" /></p>
                </div>
            </div>
            <div class="row ">
                <div class="col-lg-6">
                    <div class=" panel panel-default">
                        <div class=" panel panel-heading">
                            Roles
                        </div>
                        <div class=" panel panel-body">
                            <div class=" table table-responsive">
                                <asp:GridView ID="dgvRoles" runat="server" CssClass="table table-striped table-bordered table-hover"
                                    AutoGenerateColumns="False" EnableModelValidation="True" AllowPaging="True" HorizontalAlign="Center"
                                    DataKeyNames="IdRol" PageSize="100" OnRowCommand="dgvRoles_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Rol" />
                                        <asp:ButtonField Text="Asignar" ControlStyle-CssClass="btn btn-xs btn-info" CommandName="asignarRol">
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class=" panel panel-primary">
                        <div class=" panel panel-heading">
                            Roles Asignados
                        </div>
                        <div class=" panel panel-body">
                            <div class=" table table-responsive">
                                <asp:GridView ID="dgvRolesAsignados" runat="server" CssClass="table table-striped table-bordered table-hover"
                                    AutoGenerateColumns="False" EnableModelValidation="True" AllowPaging="True" HorizontalAlign="Center"
                                    DataKeyNames="IdRol" PageSize="100" OnRowCommand="dgvRolesAsignados_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Rol" />
                                        <asp:ButtonField Text="Eliminar" ControlStyle-CssClass="btn btn-xs btn-info" CommandName="eliminarRol">
                                            <ControlStyle CssClass="btn btn-xs btn-info" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Delete Record Modal Starts here-->
    <div id="deleteModal" class=" modal fade" tabindex="-1" role="dialog" aria-labelledby="delModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    ×</button>
            </div>
            <asp:UpdatePanel ID="upDel" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <asp:Label ID="lblPreguntaModal" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModal" runat="server" Text="Aceptar" CssClass="btn btn-success"
                            CausesValidation="False" OnClick="btnModal_Click" />
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
</asp:Content>
