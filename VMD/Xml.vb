Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel
Imports MikuMikuDance.File.VMD.Model

<XmlType("VMD-xml", [Namespace]:="http://plugins.mmd.lilithaf.me/file/vmd")>
Public Class Xml : Inherits XmlDataModel

    Public Property VMD As VMDFile

    Public Function WriteVMD(path$, Optional version As Versions = Versions.MikuMikuDanceNewer) As Boolean
        Return VMD.Save(path, version)
    End Function

    Public Overrides Function ToString() As String
        Return VMD.ToString
    End Function
End Class
