Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO

Public Module PMXReader

    Public Function Open(pmx As String) As PMXFile
        Using file As BinaryDataReader = pmx.OpenBinaryReader
            Return New PMXFile With {
                .header = file.readHeader
            }
        End Using
    End Function

    <Extension>
    Private Function readHeader(pmx As BinaryDataReader) As header
        Dim magic$ = pmx.ReadString(4)
        Dim version = pmx.ReadSingle
        Dim count = pmx.ReadByte
        Dim globals As Byte() = pmx.ReadBytes(count)
        Dim nameJp$ = pmx.ReadString(format:=BinaryStringFormat.DwordLengthPrefix)
        Dim nameEn$ = pmx.ReadString(format:=BinaryStringFormat.DwordLengthPrefix)
        Dim commentJp$ = pmx.ReadString(format:=BinaryStringFormat.DwordLengthPrefix)
        Dim commentsEn$ = pmx.ReadString(format:=BinaryStringFormat.DwordLengthPrefix)

        Return New header With {
            .signature = magic,
            .version = version,
            .count = count,
            .globals = New globals(globals),
            .modelNameJp = nameJp,
            .modelNameUniversal = nameEn,
            .commentsJp = commentJp,
            .commentsUniversal = commentsEn
        }
    End Function
End Module

Public Enum Versions As Byte
    Unknown = 0

    <Description("2.0")> v20 = 1
    <Description("2.1")> v21 = 2
End Enum