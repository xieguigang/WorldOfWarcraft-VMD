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
                .header = file.readHeader,
                .modelInfo = file.readModelInfo(.header.globals.encoding)
            }
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pmx"></param>
    ''' <param name="encodingByte">
    ''' + 0 = UTF-16
    ''' + 1 = UTF-8
    ''' </param>
    ''' <returns></returns>
    <Extension>
    Private Function readModelInfo(pmx As BinaryDataReader, encodingByte As Byte) As modelInfo
        Dim encoding As Encoding = If(encodingByte = 0, Encoding.Unicode, TextEncodings.UTF8WithoutBOM)
        Dim nameJp$ = pmx.ReadString(BinaryStringFormat.DwordLengthPrefix, encoding)
        Dim nameEn$ = pmx.ReadString(BinaryStringFormat.ZeroTerminated)
        Dim commentJp$ = pmx.ReadString(BinaryStringFormat.ZeroTerminated, encoding)
        Dim commentsEn$ = pmx.ReadString(BinaryStringFormat.ZeroTerminated)

        Return New modelInfo With {
            .modelNameJp = nameJp,
            .modelNameUniversal = nameEn,
            .commentsJp = commentJp,
            .commentsUniversal = commentsEn
        }
    End Function

    <Extension>
    Private Function readHeader(pmx As BinaryDataReader) As header
        Dim magic$ = pmx.ReadString(4)
        Dim version = pmx.ReadSingle
        Dim count = pmx.ReadByte
        Dim globals As Byte() = pmx.ReadBytes(count)

        Return New header With {
            .signature = magic,
            .version = version,
            .count = count,
            .globals = New globals(globals)
        }
    End Function
End Module

Public Enum Versions As Byte
    Unknown = 0

    <Description("2.0")> v20 = 1
    <Description("2.1")> v21 = 2
End Enum