Imports System.IO

Public Class _Default
    Inherits System.Web.UI.Page

    Dim currentDir As String = Server.MapPath(".")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        If fu.HasFile Then
            Dim reader As StreamReader = New StreamReader(fu.FileContent)

            Do
                Dim textLine As String = reader.ReadLine()
                Debug.WriteLine(textLine)
            Loop While reader.Peek() <> -1

            reader.Close()
        End If
    End Sub

End Class