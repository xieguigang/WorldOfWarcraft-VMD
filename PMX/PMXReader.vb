Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Text

Public Module PMXReader

    ''' <summary>
    ''' MMD之中的日文编码
    ''' </summary>
    ReadOnly Shift_JIS932 As Encoding = Encoding.GetEncoding("shift_jis")

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
        Dim nameJp$ = pmx.ReadString(format:=BinaryStringFormat.DwordLengthPrefix, encoding:=Encoding.Unicode)
        Dim nameEn$ = pmx.ReadString(format:=BinaryStringFormat.ZeroTerminated)
        Dim commentJp$ = pmx.ReadString(format:=BinaryStringFormat.ZeroTerminated, encoding:=Encoding.Unicode)
        Dim commentsEn$ = pmx.ReadString(format:=BinaryStringFormat.ZeroTerminated)

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