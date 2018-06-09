Imports System.Drawing
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Model

    Public Class Material

        Public Property name As String
        Public Property enUS As String
        Public Property diffuseColor As Color
        Public Property specularColor As Color
        Public Property specularity As Single
        Public Property ambientColor As Color
        Public Property drawingMode As DrawingModes
        Public Property edgeColor As Color
        Public Property edgeSize As Single
        Public Property textureIndex As NamedValue(Of Integer)
        Public Property sphereIndex As NamedValue(Of Integer)
        Public Property sphereMode As SphereModes
        Public Property toonFlag As Byte
        Public Property toonIndex As NamedValue(Of Integer)
        Public Property memo As String
        Public Property faceCount As Integer

        Public Overrides Function ToString() As String
            Return $"{name} ({textureIndex.Name})"
        End Function

        Public Shared Function GetToonName(n As Integer) As String
            If (n < 0) Then
                Return "toon0.bmp"
            Else
                Return ("toon" & (n + 1).ToString("00") & ".bmp")
            End If
        End Function

    End Class

    Public Class MaterialList : Inherits ListData(Of Material)

        Public Overrides Function ToString() As String
            Return $"{size} materials"
        End Function
    End Class

    Public Enum DrawingModes As Byte
        None = 0
        DoubleSided = &H1
        Shadow = &H2
        SelfShadowMap = &H4
        SelfShadow = &H8
        DrawEdges = &H10
    End Enum

    Public Enum SphereModes As Byte
        None = 0
        Mul = 1
        Add = 2
        SubTex = 3
    End Enum
End Namespace