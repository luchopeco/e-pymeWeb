<%@ page language="C#" autoeventwireup="true" inherits="_Login, App_Web_x2nqc0yb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="Es">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>Wiphala sistemas | e-Pyme</title>
    <!-- CSS -->
    <link href="/e-PymeWeb/css/bootstrap.css" rel="stylesheet" type="text/css">
    <link href="/e-PymeWeb/css/font-awesome.css" rel="stylesheet" type="text/css">
    <link href="/e-PymeWeb/css/main.css" rel="stylesheet" type="text/css">
    <!-- Javascript -->
    <script src="/e-PymeWeb/js/jquery-2.1.0.min.js"></script>
    <script src="/e-PymeWeb/js/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <script src="/e-PymeWeb/js/bootstrap.js"></script>
    <script src="/e-PymeWeb/js/modernizr.js"></script>
    <script src="/e-PymeWeb/js/bootstrap-tour.custom.js"></script>
    <script src="/e-PymeWeb/js/king-common.min.js"></script>
    <script src="/e-PymeWeb/js/stat/jquery.easypiechart.min.js"></script>
    <script src="/e-PymeWeb/js/raphael-2.1.0.min.js"></script>
    <script src="/e-PymeWeb/js/stat/flot/jquery.flot.min.js"></script>
    <script src="/e-PymeWeb/js/stat/flot/jquery.flot.resize.min.js"></script>
    <script src="/e-PymeWeb/js/stat/flot/jquery.flot.time.min.js"></script>
    <script src="/e-PymeWeb/js/stat/flot/jquery.flot.pie.min.js"></script>
    <script src="/e-PymeWeb/js/stat/flot/jquery.flot.tooltip.min.js"></script>
    <script src="/e-PymeWeb/js/jquery.sparkline.min.js"></script>
    <script src="/e-PymeWeb/js/datatable/jquery.dataTables.min.js"></script>
    <script src="/e-PymeWeb/js/datatable/jquery.dataTables.bootstrap.js"></script>
    <script src="/e-PymeWeb/js/jquery.mapael.js"></script>
    <script src="/e-PymeWeb/js/king-chart-stat.min.js"></script>
    <script src="/e-PymeWeb/js/king-table.min.js"></script>
    <script src="/e-PymeWeb/js/king-components.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var availableTags = [
        "DNI",
        "CI",
        "LE",
        "LC",
        "PASS"
    ];
            var availableTagsavailableTagsProvincia = [
        "CAPITAL FEDERAL",
        "BUENOS AIRES",
        "SANTA FE",
        "ENTRE RIOS",
        "CORRIENTES",
        "MISIONES",
        "CHACO",
        "FORMOSA",
        "SANTIAGO DEL ESTERO",
        "SALTA",
        "JUJUY",
        "TUCUMAN",
       "CATAMARCA",
       "LA RIOJA",
       "CORDOBA",
       "SAN JUAN",
       "SAN LUIS",
       "NEUQUEN",
       "CHUBUT",
       "LA PAMPA",
       "RIO NEGRO",
       "SANTA CRUZ",
       "TIERRA DEL FUEGO"
    ];
            $("#MainContent_txtTipoDocumento").autocomplete({
                source: availableTags
            });
            $("#MainContent_txtProvincia").autocomplete({
                source: availableTagsavailableTagsProvincia
            });
            $('#addModal').modal('show');
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Edit Modal Starts here -->
    <div id="addModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="editModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-header text-center">
            <h2>::Wiphala Sistemas - e-Pyme::</h2>
                <h3>
                    Inicia Sesion</h3>
            </div>
            <asp:UpdatePanel ID="upEdit" runat="server">
                <ContentTemplate>
                    <div class="modal-body">
                        <asp:Panel ID="panelError" runat="server">
                            <div class="alert alert-dismissable alert-danger">
                                <button type="button" class="close" data-dismiss="alert">
                                    ×</button>
                                <strong>
                                    <asp:Label ID="labelError" runat="server" Text="Label"></asp:Label></strong>
                            </div>
                        </asp:Panel>
                        <table class="table">
                            <tr>
                                <td>
                                    Usuario<asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator1" runat="server"
                                        ErrorMessage="Dato Obligatorio" ControlToValidate="txtUsuario"></asp:RequiredFieldValidator>
                                    <div class="form-group input-group">
                                        <span class="input-group-addon">@ </span>
                                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contraseña<asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator2"
                                        runat="server" ErrorMessage="Dato Obligatorio" ControlToValidate="txtPass"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSave" runat="server" Text="Aceptar" CssClass="btn btn-success" OnClick="btnSave_Click" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Edit Modal Ends here -->
    </form>
</body>
</html>
