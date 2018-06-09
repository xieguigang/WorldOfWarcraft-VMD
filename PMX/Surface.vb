Public Class Surface

End Class

Public Class Face

    Public Property size As Integer
    Public Property VertexIndex As Integer()

    Public Overrides Function ToString() As String
        Return $"{size} vertex index"
    End Function

End Class