Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel
Imports MikuMikuDance.File.PMX.Model

<XmlType("mikumikudance-pmx", [Namespace]:="http://plugins.mmd.lilithaf.me/file/vmd " & MMDOfficial)>
Public Class Xml : Inherits XmlDataModel

    Public Property PMX As PMXFile

    Public Overrides Function ToString() As String
        Return PMX.ToString
    End Function
End Class
