Imports System.Xml.Serialization

Namespace Model

    Public Class ListData(Of T)

        <XmlAttribute>
        Public Property size As Integer
        <XmlElement>
        Public Property data As T()

    End Class

    Public Structure vec2

        <XmlAttribute> Dim x, y As Single

        Public Overrides Function ToString() As String
            Return $"({x}, {y})"
        End Function
    End Structure

    Public Structure vec3

        <XmlAttribute> Dim x, y, z As Single

        Public Shared ReadOnly Property UnitX As New vec3(1, 0, 0)
        Public Shared ReadOnly Property UnitY As New vec3(0, 1, 0)
        Public Shared ReadOnly Property UnitZ As New vec3(0, 0, 1)

        Sub New(x!, y!, z!)
            Me.x = x
            Me.y = y
            Me.z = z
        End Sub

        Public Function Cross(b As vec3) As vec3

        End Function

        Public Overrides Function ToString() As String
            Return $"({x}, {y}, {z})"
        End Function
    End Structure

    Public Structure vec4

        <XmlAttribute> Dim x, y, z, w As Single

        Public Overrides Function ToString() As String
            Return $"({x}, {y}, {z}, {w})"
        End Function
    End Structure

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