Imports System.Xml.Serialization

Namespace Model

    Public Class KeyFrameList(Of T As KeyFrame)

        ''' <summary>
        ''' keyframe list, which starts with a 4-byte unsigned int that tells how 
        ''' many keyframes are listed in the file.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute("size")>
        Public Property count As UInteger
        <XmlElement>
        Public Property keyframes As T()

        Public Overrides Function ToString() As String
            Return $"[{count}] {GetType(T).FullName}"
        End Function
    End Class

    Public MustInherit Class KeyFrame

        ''' <summary>
        ''' ``byte[4] (unsigned int)``
        ''' 
        ''' The frame number. Since keyframes are not necessarily stored for each actual 
        ''' frame, the animation software must interpolate between two adjacent keyframes 
        ''' with different frame indices.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute>
        Public Property index As UInteger

    End Class

    Public Class bone : Inherits KeyFrame

        ''' <summary>
        ''' A null-terminated string representing the name of the bone to which the transformation 
        ''' will be applied.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute>
        Public Property boneName As String

        ''' <summary>
        ''' + X-coordinate of the bone position
        ''' + Y-coordinate of the bone position
        ''' + Z-coordinate of the bone position
        ''' </summary>
        ''' <returns></returns>
        <XmlElement>
        Public Property position As Vector

        ''' <summary>
        ''' X-coordinate of the bone rotation (quaternion)
        ''' Y-coordinate of the bone rotation (quaternion)
        ''' Z-coordinate of the bone rotation (quaternion)
        ''' W-coordinate of the bone rotation (quaternion)
        ''' </summary>
        ''' <returns></returns>
        <XmlElement>
        Public Property rotation As Vector

        ''' <summary>
        ''' 64 bytes of frame interpolation data. 
        ''' </summary>
        ''' <returns></returns>
        Public Property interpolation As Byte()

        Public Overrides Function ToString() As String
            Return boneName
        End Function
    End Class

    Public Class face : Inherits KeyFrame

        ''' <summary>
        ''' A null-terminated string representing the name 
        ''' of the face to which the transformation will be 
        ''' applied.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute>
        Public Property faceName As String
        ''' <summary>
        ''' Weight - this value is on a scale of 0.0-1.0. 
        ''' It is used to scale how much a face morph should 
        ''' move a vertex based off of the maximum possible 
        ''' coordinate that it can move by (specified in the 
        ''' PMD)
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute>
        Public Property scale As Single

        Public Overrides Function ToString() As String
            Return faceName
        End Function
    End Class

    Public Class camera : Inherits KeyFrame

        <XmlAttribute>
        Public Property len As Single

        ''' <summary>
        ''' + X-coordinate of the bone position
        ''' + Y-coordinate of the bone position
        ''' + Z-coordinate of the bone position
        ''' </summary>
        ''' <returns></returns>
        Public Property position As Vector

        ''' <summary>
        ''' X-coordinate of the bone rotation (quaternion)
        ''' Y-coordinate of the bone rotation (quaternion)
        ''' Z-coordinate of the bone rotation (quaternion)
        ''' </summary>
        ''' <returns></returns>
        Public Property rotation As Vector

        ''' <summary>
        ''' 24 bytes of interpolation data.
        ''' </summary>
        ''' <returns></returns>
        Public Property interpolation As Byte()

        ''' <summary>
        ''' Camera FOV angle
        ''' </summary>
        ''' <returns></returns>
        ''' 
        <XmlAttribute>
        Public Property angle As UInteger

        ''' <summary>
        ''' Perspective
        ''' </summary>
        ''' <returns></returns>
        ''' 
        <XmlAttribute>
        Public Property perspective As Byte

        Public Overrides Function ToString() As String
            Return position.GetXml
        End Function
    End Class
End Namespace