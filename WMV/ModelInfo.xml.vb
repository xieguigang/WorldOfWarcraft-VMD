﻿Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Text

''' <summary>
''' ``m2`` model info
''' </summary>
<XmlType("m2")> Public Class ModelInfo

    Public Property info As info
    Public Property header As header

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function Load(path$, Optional encoding As Encoding = Nothing) As ModelInfo
        Return path.ReadAllText(encoding Or UTF8).CreateObjectFromXmlFragment(Of ModelInfo)
    End Function
End Class

Public Class info
    Public Property modelname As String
End Class



