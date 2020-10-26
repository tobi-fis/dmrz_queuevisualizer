Imports System.Drawing
Imports System.IO

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        loadButton.Disabled = True 'Standardmäßig disabled bis Datei gewählt
    End Sub

    'loadButton Click Handler
    Protected Sub loadButton_Click(sender As Object, e As EventArgs)
        If FileUpload1.HasFile Then
            If Path.GetExtension(FileUpload1.FileName).ToLower() = ".txt" Then
                ReadFile()
            End If
        End If
    End Sub

    'Inhalt der log Datei auslesen und filtern
    Private Sub ReadFile()
        Dim fu As FileUpload = FileUpload1
        Dim terminatingEvents As ArrayList = New ArrayList()
        Dim maxVal As Integer = 0 'Höchstwert zur Berechnung der Breite

        If fu.HasFile Then
            outputImage.ImageUrl = Nothing
            Dim reader As StreamReader = New StreamReader(fu.FileContent)
            Try
                Do
                    Dim textLine As String = reader.ReadLine()
                    If textLine.Contains("COMPLETE") Then
                        Dim params() As String = Split(textLine, "|")
                        terminatingEvents.Add(params)

                        'Neuer Höchstwert erreicht?
                        Dim time As Integer = Integer.Parse(params(5)) + Integer.Parse(params(6))
                        maxVal = If(time > maxVal, time, maxVal)

                    ElseIf textLine.Contains("ABANDON") Then
                        Dim params() As String = Split(textLine, "|")
                        terminatingEvents.Add(params)

                        'Neuer Höchstwert erreicht?
                        Dim time As Integer = Integer.Parse(params(7))
                        maxVal = If(time > maxVal, time, maxVal)
                    End If
                Loop While reader.Peek() <> -1

                reader.Close()

                'Falls keine Einträge gefunden Fehler ausgeben, sonst Bitmap erstellen
                If terminatingEvents.Count > 0 Then
                    errorInfo.Text = ""
                    CreateBitmap(terminatingEvents, maxVal)
                Else
                    errorInfo.Text = "Keine verwertbaren Einträge im Log gefunden."
                End If

            Catch ex As Exception
                errorInfo.Text = "Fehlerhafte Log-Datei!"
            End Try
        End If
    End Sub

    'Bitmap zeichnen und rendern
    Private Sub CreateBitmap(events As ArrayList, maxVal As Integer)
        'Bitmap und Startkoordinaten initialisieren
        Dim drawX = 165
        Dim drawY = 10
        Dim b_height As Integer = events.Count * 30 + 10 'Höhe Abhängig von Anzahl der Balken
        Dim b_width As Integer = 175 + maxVal * 10  'Breite Abhängig von Höchstwert
        Dim charts As Bitmap = New Bitmap(b_width, b_height)
        Dim chartsGraphics As Graphics = Graphics.FromImage(charts)
        chartsGraphics.Clear(Color.WhiteSmoke) 'Hintergrundfarbe

        Dim drawFontID = New Font("Arial", 12)
        Dim drawFontTime = New Font("Arial", 10)

        For Each params As String() In events
            'ID platzieren
            chartsGraphics.DrawString("ID: " + params(1), drawFontID, New SolidBrush(Color.Black), 5, drawY + 2)

            If params(4).Contains("COMPLETE") Then 'Normal beendete Anrufe
                Dim waitLength As Integer = Integer.Parse(params(5)) * 10

                'Gelben Balken zeichnen für Wartezeit
                chartsGraphics.FillRectangle(Brushes.Yellow, drawX, drawY, waitLength, 20)

                'Grünen Balken zeichnen für Gesprächszeit
                chartsGraphics.FillRectangle(Brushes.Green, drawX + waitLength, drawY, Integer.Parse(params(6)) * 10, 20)

                'Wartezeit in Sekunden platzieren
                chartsGraphics.DrawString(params(5), drawFontTime, New SolidBrush(Color.Black), drawX + 2, drawY + 2)

                'Gesprächszeit in Sekunden platzieren
                chartsGraphics.DrawString(params(6), drawFontTime, New SolidBrush(Color.Black), drawX + 2 + waitLength, drawY + 2)

                drawY += 30

            Else 'Nicht beantwortete Anrufe
                'Roten Balken zeichnen
                chartsGraphics.FillRectangle(Brushes.Red, drawX, drawY, Integer.Parse(params(7)) * 10, 20)

                'Zeit in Sekunden platzieren
                chartsGraphics.DrawString(params(7), drawFontTime, New SolidBrush(Color.Black), drawX + 2, drawY + 2)

                drawY += 30
            End If
        Next

        Dim ms As MemoryStream = New MemoryStream()
        charts.Save(ms, Imaging.ImageFormat.Bmp)
        Dim byteImage As Byte() = ms.ToArray()
        Dim SigBase64 = Convert.ToBase64String(byteImage)
        outputImage.ImageUrl = "data:image/bmp;base64," + SigBase64
        ms.Close()
        charts.Dispose()
        chartsGraphics.Dispose()

    End Sub

End Class