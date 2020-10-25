Imports System.Drawing
Imports System.IO

Public Class _Default
    Inherits Page

    Dim currentDir As String = Server.MapPath(".")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        loadButton.Disabled = True
    End Sub

    'loadButton Click Handler
    Protected Sub loadButton_Click(sender As Object, e As EventArgs)
        'TODO: Dateityp auch hier validieren
        If FileUpload1.HasFile Then

            If Path.GetExtension(FileUpload1.FileName).ToLower() = ".txt" Then

                ReadFile()

            End If

        Else

        End If
    End Sub

    'Inhalt der log Datei auslesen
    Private Sub ReadFile()
        Dim fu As FileUpload = FileUpload1
        Dim terminatingEvents As ArrayList = New ArrayList()
        If fu.HasFile Then
            Dim reader As StreamReader = New StreamReader(fu.FileContent)
            Do
                Dim textLine As String = reader.ReadLine()
                If textLine.Contains("COMPLETE") Or textLine.Contains("ABANDON") Then
                    terminatingEvents.Add(textLine)
                End If
            Loop While reader.Peek() <> -1

            reader.Close()

            If terminatingEvents.Count > 0 Then
                CreateBitmap(terminatingEvents)
            Else
                errorInfo.Text = "Keine verwertbaren Einträge im Log gefunden."
            End If

        End If
    End Sub

    'Bitmap 
    Private Sub CreateBitmap(calls As ArrayList)
        Dim charts As Bitmap = New Bitmap(1100, 600)
        Dim chartsGraphics As Graphics = Graphics.FromImage(charts)
        Dim red As Integer = 0
        Dim white As Integer = 11
        'Testbild erstellen
        While white <= 100
            chartsGraphics.FillRectangle(Brushes.Red, 0, red, 200, 10)
            chartsGraphics.FillRectangle(Brushes.White, 0, white, 200, 10)
            red += 20
            white += 20
        End While

        Dim ms As MemoryStream = New MemoryStream()
        charts.Save(ms, Imaging.ImageFormat.Jpeg)
        Dim byteImage As Byte() = ms.ToArray()
        Dim SigBase64 = Convert.ToBase64String(byteImage)

        outputImage.ImageUrl = "data:image/png;base64," + SigBase64
    End Sub

End Class