Public Class ListData(Of T)

    Public Property size As Integer
    Public Property data As T()

End Class

Public Structure vec2
    Dim x, y As Single

    Public Overrides Function ToString() As String
        Return $"({x}, {y})"
    End Function
End Structure

Public Structure vec3
    Dim x, y, z As Single

    Public Overrides Function ToString() As String
        Return $"({x}, {y}, {z})"
    End Function
End Structure

Public Structure vec4
    Dim x, y, z, w As Single

    Public Overrides Function ToString() As String
        Return $"({x}, {y}, {z}, {w})"
    End Function
End Structure