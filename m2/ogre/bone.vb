Imports System.Xml.Serialization
Imports MikuMikuDance.Math3D

Namespace ogre

    Public Class bone

        <XmlAttribute> Public Property id As Integer
        <XmlAttribute> Public Property name As String

        ''' <summary>
        ''' 这个骨骼节点的三维空间位置
        ''' </summary>
        ''' <returns></returns>
        Public Property position As vec3
        Public Property rotation As rotation

        Public Overrides Function ToString() As String
            Return $"{name} @ {position}"
        End Function

    End Class

    Public Structure rotation
        <XmlAttribute>
        Dim angle As Double
        Dim axis As vec3

        Public Overrides Function ToString() As String
            Return $"rotate {angle}° on axis={axis}"
        End Function
    End Structure

    Public Structure boneparent
        <XmlAttribute> Dim bone As Integer
        <XmlAttribute> Dim parent As Integer

        Public Overrides Function ToString() As String
            Return $"{parent} -> {bone}"
        End Function
    End Structure
End Namespace