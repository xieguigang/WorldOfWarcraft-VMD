Imports System.Xml.Serialization
Imports MikuMikuDance.Math3D

Namespace ogre

    Public Class bone

        <XmlAttribute> Public Property id As Integer
        <XmlAttribute> Public Property name As String

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
    End Structure
End Namespace