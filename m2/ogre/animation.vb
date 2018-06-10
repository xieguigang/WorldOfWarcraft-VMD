Imports System.Xml.Serialization
Imports MikuMikuDance.Math3D

Namespace ogre

    Public Class animation

        <XmlAttribute> Public Property name As String
        <XmlAttribute> Public Property length As Double

        Public Property tracks As track()

        Public Overrides Function ToString() As String
            Return name
        End Function
    End Class

    Public Class track
        <XmlAttribute>
        Public Property bone As Integer
        Public Property keyframes As keyframe()

        Public Overrides Function ToString() As String
            Return $"{bone} with {keyframes.Length} keyframes"
        End Function
    End Class

    Public Structure keyframe

        <XmlAttribute>
        Dim time As Double
        Dim translate As vec3
        Dim rotate As rotation

        Public Overrides Function ToString() As String
            Return $"{time}:  {translate} | {rotate}"
        End Function
    End Structure
End Namespace