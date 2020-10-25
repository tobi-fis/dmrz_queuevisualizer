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
                <p class="infoText">Bitte wählen sie eine Log-Datei als .txt aus, um die Queue als Bitmap darzustellen zu können:</p>
                <div>
                    <asp:FileUpload ID="FileUpload1" runat="server" accept=".txt" onchange="activateButton('loadButton')" />
                    <asp:RegularExpressionValidator ID="RegExValFileUploadFileType" runat="server"
                        ControlToValidate="FileUpload1"
                        ErrorMessage="Bitte .txt Datei auswählen."
                        ForeColor="Red"
                        Font-Size="Medium"
                        ValidationExpression="(.*?)\.(txt)$"></asp:RegularExpressionValidator>
                </div>
               
                    <button runat="server" id="loadButton" class="btn1" text="Button" onserverclick="loadButton_Click">Log visualisieren</button>
                

            </div>
        </div>
    </form>

    <!-- loadButton erst aktivieren, wenn Dateiendung stimmt -->
    <script type="text/javascript">
        function activateButton(btn) {
            console.log('activebtn called');
            var ending = document.getElementById('FileUpload1').value.split('.')[1];
            console.log(ending);
            if (ending == 'txt') {
                console.log('true');
                document.getElementById(btn).disabled = false;
            }
        }
    </script>
</body>
</html>
