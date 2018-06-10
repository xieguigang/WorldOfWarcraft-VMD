Imports System.Xml.Serialization

Namespace Model

    Public Class ListData(Of T)

        <XmlAttribute>
        Public Property size As Integer
        <XmlElement>
        Public Property data As T()

    End Class

    Public Structure Color

        <XmlAttribute> Dim a, r, g, b As Single

        Sub New(values!())
            If values Is Nothing Then
                Return
            End If
            If values.Length = 4 Then
                a = values(0)
                r = values(1)
                g = values(2)
                b = values(3)
            ElseIf values.Length = 3 Then
                r = values(0)
                g = values(1)
                b = values(2)
            Else
                Throw New InvalidCastException
            End If
        End Sub
    End Structure
End Namespace