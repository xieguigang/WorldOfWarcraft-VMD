Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Public Class Sequences : Inherits List(Of Integer)

    <XmlElement>
    Public Property Sequence As Integer()
End Class

Public Class SkeletonAndAnimation

    Public Property GlobalSequences As Sequences
    Public Property Animations As Animations
    Public Property AnimationLookups As AnimationLookups

End Class

Public Class List(Of T)

    <XmlAttribute>
    Public Property size As Integer
End Class

Public Class Animations : Inherits List(Of Animation)

    <XmlElement("Animation")>
    Public Property Animations As Animation()
End Class

Public Class Animation

    <XmlAttribute>
    Public Property id As Integer

    Public Property animID As Integer
    Public Property animName As String
    Public Property length As Integer
    Public Property moveSpeed As Integer
    Public Property Flags As Integer
    Public Property probability As Integer
    Public Property d1 As Integer
    Public Property d2 As Integer
    Public Property playSpeed As Integer
    Public Property boxA As String
    Public Property boxB As String
    Public Property rad As Double
    Public Property NextAnimation As Integer
    Public Property Index As Integer

End Class

Public Class AnimationLookup
    <XmlAttribute>
    Public Property id As Integer
    <XmlText>
    Public Property value As Integer
End Class

Public Class AnimationLookups : Inherits List(Of AnimationLookup)

    <XmlElement("AnimationLookup")>
    Public Property AnimationLookups As AnimationLookup()
End Class

Public Class Bone
    Public Property keyboneid As Integer
    Public Property billboard As Integer
    Public Property parent As Integer
    Public Property geoid As Integer
    Public Property unknown As Integer
End Class

Public Class Vector : Implements IXmlSerializable

    Public Property pivot As Double()

    Public Sub ReadXml(reader As XmlReader) Implements IXmlSerializable.ReadXml
        Throw New NotImplementedException()
    End Sub

    Public Sub WriteXml(writer As XmlWriter) Implements IXmlSerializable.WriteXml
        Throw New NotImplementedException()
    End Sub

    Public Function GetSchema() As XmlSchema Implements IXmlSerializable.GetSchema
        Throw New NotImplementedException()
    End Function
End Class