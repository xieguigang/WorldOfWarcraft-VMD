Imports System.Drawing

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
    Public Property textureIndex As Integer
    Public Property sphereIndex As Integer
    Public Property sphereMode As SphereModes
    Public Property toonFlag As Byte
    Public Property toonIndex As Integer
    Public Property memo As String
    Public Property faceIndex As Integer

End Class

Public Enum DrawingModes
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