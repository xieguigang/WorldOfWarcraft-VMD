Public Class PMXFile

    Public Property header As header

End Class

Public Structure vec2
    Dim x, y As Single
End Structure

Public Structure vec3
    Dim x, y, z As Single
End Structure

Public Structure vec4
    Dim x, y, z, w As Single
End Structure

Public Class header
    Public Property signature As Char()
    Public Property version As Single
    Public Property count As Byte
    ''' <summary>
    ''' 长度为<see cref="count"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property globals As globals
    Public Property modelNameJp As String
    Public Property modelNameUniversal As String
    Public Property commentsJp As String
    Public Property commentsUniversal As String
End Class

Public Class globals
    Public Property encoding As Byte
    Public Property addiVec4 As Byte
    Public Property vertexIndexSize As Byte
    Public Property textureIndexSize As Byte
    Public Property materialIndexSize As Byte
    Public Property boneIndexSize As Byte
    Public Property morphIndexSize As Byte
    Public Property rigidBodyIndexSize As Byte

    Sub New()
    End Sub

    Sub New(bytes As Byte())

    End Sub
End Class