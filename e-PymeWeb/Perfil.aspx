<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Perfil.aspx.cs" Inherits="Perfil" %>

<%@ Register Src="Controles/PanelMensajes.ascx" TagName="PanelMensajes" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class=" col-lg-12">
                <h1 class=" page-header">
                    Perfil de Usuario
                </h1>
            </div>
            <div class=" col-lg-12">
                <uc1:PanelMensajes ID="ucPanelMensajes" runat="server" />
            </div>
            <div class=" col-lg-12">
                <div class=" col-md-3">
                </div>
                <div class=" col-md-6">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                <asp:Label ID="lblNombre" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-3 " align="center">
                                     <asp:Literal ID="literalImagen" runat="server"></asp:Literal>
                                </div>
                                <div class=" col-lg-9 ">
                                    <table class="table">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    Nombre Usuario:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNombreUsuario" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Fecha alta:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFechaalta" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <asp:LinkButton ID="btnModificarClave" data-original-title="Modificar Clave" ToolTip="Modificar Clave"
                                CssClass="btn btn-sm btn-warning" runat="server" OnClick="btnModificarClave_Click">
                        <i class="fa fa-pencil-square-o"></i> Clave
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnModificarImagen" ToolTip="Modificar Imagen" runat="server"
                                CssClass="btn btn-sm btn-success" onclick="btnModificarImagen_Click" > <i class="fa fa-pencil-square-o"></i> Imagen</asp:LinkButton>
                            <asp:LinkButton ID="btnResetearImagen" ToolTip="Resetear Imagen" runat="server" 
                                CssClass="btn btn-sm btn-primary" onclick="btnResetearImagen_Click"
                                > <i class="fa fa-power-off"></i> Imagen</asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class=" col-md-3">
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
    <!-- Modificar Clave Modal Starts here-->
    <div id="modificarClaveModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="upAdd" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel">
                            Modificar Clave</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class=" panel panel-success">
                            <div class=" panel-heading">
                                Modificar Clave
                            </div>
                            <div class=" panel-body">
                                <div class=" col-md-12">
                                    Contraseña Actual
                                    <asp:TextBox ID="txtClaveActual" runat="server" CssClass=" form-control" TextMode="Password"></asp:TextBox>
                                </div>
                                <div class=" col-md-12">
                                    Contraseña Nueva
                                    <asp:TextBox ID="txtClaveNueva" runat="server" CssClass=" form-control" TextMode="Password"></asp:TextBox>
                                </div>
                                <div class=" col-md-12">
                                    Confirmar Contraseña
                                    <asp:TextBox ID="txtClaveConfirmar" runat="server" CssClass=" form-control" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnModificarClaveModal" runat="server" Text="Modificar" CssClass="btn btn-success"
                            OnClick="btnModificarClaveModal_Click" />
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Modificar Clave Modal Ends here-->

        <!-- CambiarImagenModal Starts here-->
    <div id="cambiarImagenModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h3 id="H1">
                            Cambiar imagen Usuario</h3>
                    </div>
                    <div class=" modal-body ">
                        <div class="row">
                            <div class=" col-md-12">
                                <asp:FileUpload ID="FileUpload1" CssClass=" form-control" runat="server" /></div>
                            <div class=" col-md-12">
                                <p>
                                    Tamaño 100x100</p>
                            </div>
                            <div class=" col-md-12">
                                <asp:Button ID="btnAgregarImagen" runat="server" CssClass=" btn btn-block btn-success"
                                    Text="Agregar Imagen" OnClick="btnAgregarImagen_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                            Cancelar</button>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnAgregarImagen" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--CambiarImagenModal Ends here-->
</asp:Content>
