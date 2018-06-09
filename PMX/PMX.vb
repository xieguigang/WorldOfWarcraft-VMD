Imports Microsoft.VisualBasic.Language
Imports MikuMikuDance.File.PMX.DeformTypes

Public Class PMXFile

    Public Property header As header
    Public Property modelInfo As modelInfo
    Public Property vertexData As VertexData

    Public Overrides Function ToString() As String
        Return $"[{header}] {modelInfo}"
    End Function

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

Public Class vertex
    Public Property position As vec3
    Public Property normal As vec3
    Public Property UVtextureCoordinate As vec2
    Public Property appendixUV As vec4
    Public Property weightDeform As DeformType
    Public Property edgeScale As Single

    Public Overrides Function ToString() As String
        Return $"[{weightDeform}] {position}"
    End Function
End Class

Public Class VertexData
    Public Property size As Integer
    Public Property data As vertex()

    Public Overrides Function ToString() As String
        Return $"{size} vertex"
    End Function
End Class

Namespace DeformTypes

    Public Enum WeightDeformTypes As Byte
        BDEF1 = 0
        BDEF2 = 1
        BDEF4 = 2
        SDEF = 4
        QDEF = 8
    End Enum

    Public Structure DeformType

        Dim type As WeightDeformTypes

        Dim BDEF1 As BDEF1
        Dim BDEF2 As BDEF2
        Dim BDEF4 As BDEF4
        Dim SDEF As SDEF
        Dim QDEF As QDEF

        Public Overrides Function ToString() As String
            Return type.Description
        End Function
    End Structure

#Region "Version 2.0"

    Public Class BDEF1

        Public Property index1 As Integer
        Public Property weight1 As Single

        Sub New()
            weight1 = 1
        End Sub
    End Class

    Public Class BDEF2 : Inherits BDEF1

        Public Property index2 As Integer

        Public ReadOnly Property weight2 As Single
            Get
                Return 1S - weight1
            End Get
        End Property
    End Class

    Public Class BDEF4 : Inherits BDEF1
        Public Property index2 As Integer
        Public Property index3 As Integer
        Public Property index4 As Integer
        Public Property weight2 As Single
        Public Property weight3 As Single
        Public Property weight4 As Single
    End Class

    ''' <summary>
    ''' Spherical deform blending
    ''' </summary>
    Public Class SDEF : Inherits BDEF2
        Public C As vec3
        Public R0 As vec3
        Public R1 As vec3
    End Class
#End Region

#Region "Version 2.1"

    ''' <summary>
    ''' Dual quaternion deform blending
    ''' </summary>
    Public Class QDEF : Inherits BDEF4
    End Class
#End Region
End Namespace

Public Class header

    Public Property signature As Char()
    Public Property version As Single
    Public Property count As Byte
    ''' <summary>
    ''' 长度为<see cref="count"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property globals As globals

    Public Overrides Function ToString() As String
        Return signature.CharString & version.ToString("F1")
    End Function

End Class

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