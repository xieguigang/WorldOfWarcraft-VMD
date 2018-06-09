﻿Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.Collection

Public Class Texture

    Public Property size As Integer

    Public Property fileNames As String()
        Get
            Return index.Objects
        End Get
        Set(value As String())
            index = value
        End Set
    End Property

    <XmlIgnore>
    Public Property index As Index(Of String)

    Public Overrides Function ToString() As String
        Return $"{size} textures"
    End Function

End Class