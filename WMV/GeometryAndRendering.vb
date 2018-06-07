Imports System.Xml.Serialization

Public Class GeometryAndRendering

End Class

Public Class Vertice

    <XmlAttribute>
    Public Property id As Integer
    Public Property pos As Vector
End Class

Public Class Vertices : Inherits List(Of Vertice)

    <XmlElement("Vertice")>
    Public Property Vertices As Vertice()
End Class