<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="default.aspx.vb" Inherits="dmrz_queuevisualizer._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="style.css" rel="stylesheet" type="text/css" />
    <title>Asterisk Queue Visualizer</title>
</head>
<body>
    <div class="header">
        <div id="title" class="title">Asterisk Queue Visualizer</div>

    </div>

    <form id="mainForm" runat="server">
        <div class="mainSection">
            <div class="uploadSection">
                <p style="margin-bottom: 15px">Bitte wählen sie eine Log-Datei als .txt aus, um die Anrufe grafisch darstellen zu können:</p>
                <div>
                    <asp:FileUpload ID="FileUpload1" runat="server" accept=".txt" onchange="activateButton('loadButton')" />
                    <asp:RegularExpressionValidator ID="RegExValFileUploadFileType" runat="server"
                        ControlToValidate="FileUpload1"
                        ErrorMessage="Bitte .txt Datei auswählen."
                        ForeColor="Red"
                        Font-Names="Arial"
                        Font-Size="14px"
                        ValidationExpression="(.*?)\.(txt)$"></asp:RegularExpressionValidator>
                </div>
                <div class="uploadBottom">
                    <asp:Label ID="errorInfo" CssClass="errorLabel" runat="server" Text=""></asp:Label>
                    <button runat="server" id="loadButton" text="Button" onserverclick="loadButton_Click">Log visualisieren</button>
                </div>
            </div>

            <div class="imgDiv">
                <asp:Image ID="outputImage" runat="server" />
            </div>
        </div>
    </form>

    <!-- loadButton erst aktivieren, wenn Dateiendung stimmt -->
    <script type="text/javascript">
        function activateButton(btn) {
            var ending = document.getElementById('FileUpload1').value.split('.')[1];
            if (ending == 'txt') {
                document.getElementById(btn).disabled = false;
            }
        }
    </script>
</body>
</html>
