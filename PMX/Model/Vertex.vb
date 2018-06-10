Imports System.Xml.Serialization
Imports MikuMikuDance.File.PMX.Model.VertexData.DeformTypes

Namespace Model.VertexData

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

    Public Class vertexs : Inherits ListData(Of vertex)

        Public Overrides Function ToString() As String
            Return $"{size} vertex"
        End Function
    End Class

    Namespace DeformTypes

        Public Enum WeightDeformTypes As Byte
            BDEF1 = 0
            BDEF2 = 1
            BDEF4 = 2
            SDEF = 3
            QDEF = 4
        End Enum

        Public Structure DeformType

            <XmlAttribute>
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

            <XmlAttribute>
            Public Property index1 As Integer
            Public Property weight1 As Single

            Sub New()
                weight1 = 1
            End Sub
        End Class

        Public Class BDEF2 : Inherits BDEF1

            <XmlAttribute> Public Property index2 As Integer

            Public ReadOnly Property weight2 As Single
                Get
                    Return 1S - weight1
                End Get
            End Property
        End Class

        Public Class BDEF4 : Inherits BDEF1
            <XmlAttribute> Public Property index2 As Integer
            <XmlAttribute> Public Property index3 As Integer
            <XmlAttribute> Public Property index4 As Integer
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
End Namespace