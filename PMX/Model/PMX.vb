Imports System.ComponentModel
Imports Microsoft.VisualBasic.Language

Public Class PMXFile

    Public Property header As header
    Public Property modelInfo As modelInfo
    Public Property vertexData As VertexData
    Public Property faceVertex As Face
    Public Property textureTable As Texture
    Public Property materials As MaterialList

    Public Overrides Function ToString() As String
        Return $"[{header}] {modelInfo}"
    End Function

End Class

Public Class header

    ''' <summary>
    ''' The very first magic bytes at the begining of the pmx file.
    ''' </summary>
    ''' <returns></returns>
    Public Property magics As Char()
    Public Property version As Single
    Public Property count As Byte
    ''' <summary>
    ''' 长度为<see cref="count"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property globals As globals

    Public Function GetVersion() As Versions
        If version = 2.0! Then
            Return Versions.v20
        ElseIf version = 2.1! Then
            Return Versions.v21
        Else
            Return Versions.Unknown
        End If
    End Function

    Public Overrides Function ToString() As String
        Return magics.CharString & GetVersion.Description
    End Function

End Class

Public Enum Versions As Byte
    <Description("NA")> Unknown = 0
    <Description("ver2.0")> v20 = 1
    <Description("ver2.1")> v21 = 2
End Enum

Public Class globals

    ''' <summary>
    ''' + 0 = UTF-16
    ''' + 1 = UTF-8
    ''' </summary>
    ''' <returns></returns>
    Public Property encoding As Byte
    Public Property addiVec4 As Byte
    Public Property vertexIndexSize As IndexSize
    Public Property textureIndexSize As IndexSize
    Public Property materialIndexSize As IndexSize
    Public Property boneIndexSize As IndexSize
    Public Property morphIndexSize As IndexSize
    Public Property rigidBodyIndexSize As IndexSize

    Public Enum IndexSize As Byte
        [byte] = 1
        [short] = 2
        [int] = 4
    End Enum

    Sub New()
    End Sub

    Sub New(bytes As Byte())
        Dim i As int = Scan0

        encoding = bytes(++i)
        addiVec4 = bytes(++i)
        vertexIndexSize = bytes(++i)
        textureIndexSize = bytes(++i)
        materialIndexSize = bytes(++i)
        boneIndexSize = bytes(++i)
        morphIndexSize = bytes(++i)
        rigidBodyIndexSize = bytes(++i)
    End Sub
End Class

''' <summary>
''' Fields with type text begins with an int (32 bit) with how many bytes of 
''' text the section is. The encoding of the text is either UTF-8 or UTF-16, 
''' as specified by the <see cref="globals.encoding"/> byte in the header.
''' </summary>
Public Class modelInfo

    Public Property modelNameJp As String
    Public Property modelNameUniversal As String
    Public Property commentsJp As String
    Public Property commentsUniversal As String

    Public Overrides Function ToString() As String
        Return modelNameJp
    End Function
End Class