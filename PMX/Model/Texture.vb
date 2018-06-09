Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.Collection

Namespace Model

    Public Class Texture

        Public Property size As Integer

        Public Property fileNames As String()
            Get
                Return index.Objects
            End Get
            Set(value As String())
                index = value
            End Set
        End Property

        <XmlIgnore>
        Public Property index As Index(Of String)

        Public Function GetTextureName(index As Integer) As String
            If index >= 0 AndAlso index < size Then
                Return fileNames(index)
            Else
                Return ""
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"{size} textures"
        End Function

    End Class

End Namespace