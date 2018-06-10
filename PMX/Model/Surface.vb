Imports System.Xml.Serialization

Namespace Model

    Public Class Surface

    End Class

    Public Class Face

        <XmlAttribute>
        Public Property size As Integer
        Public Property vertexIndices As Integer()

        Public Overrides Function ToString() As String
            Return $"{size} vertex index"
        End Function

    End Class
End Namespace