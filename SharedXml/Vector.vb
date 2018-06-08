Imports System.Runtime.CompilerServices
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Public Class Vector : Implements IXmlSerializable

    Public Property pivot As Double()

    Sub New()
    End Sub

    Public Sub ReadXml(reader As XmlReader) Implements IXmlSerializable.ReadXml
        Dim s = reader.ReadString
        reader.ReadEndElement()

        Dim v = s.GetStackValue("(", ")") _
                 .Split(","c) _
                 .Select(Function(t) Val(Trim(t))) _
                 .ToArray
        pivot = v
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub WriteXml(writer As XmlWriter) Implements IXmlSerializable.WriteXml
        Call writer.WriteString(ToString)
    End Sub

    Public Function GetSchema() As XMLSchema Implements IXmlSerializable.GetSchema
        Dim schema As New XmlSchema
        Dim xmlText As New XmlSchemaAny

        schema.Items.Add(xmlText)
        schema.Write(Console.Out)

        Return schema
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        Return $"({pivot.JoinBy(", ")})"
    End Function
End Class