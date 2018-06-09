Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Text
Imports MikuMikuDance.File.PMX.DeformTypes
Imports MikuMikuDance.File.PMX.globals

Public Module PMXReader

    ''' <summary>
    ''' MMD之中的日文编码
    ''' </summary>
    ReadOnly Shift_JIS932 As Encoding = System.Text.Encoding.GetEncoding("shift_jis")

    Public Function Open(pmx As String) As PMXFile
        Using file As BinaryDataReader = pmx.OpenBinaryReader
            Dim header As header = file.readHeader
            Dim globals = header.globals

            Return New PMXFile With {
                .header = header,
                .modelInfo = file.readModelInfo(globals.encoding),
                .vertexData = New VertexData With {
                    .size = file.ReadUInt32,
                    .data = file.populateVertex(
                        n:= .size,
                        indexSize:=globals.vertexIndexSize
                    ).ToArray
                }
            }
        End Using
    End Function

    Const byte1 As Byte = 1

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
        Dim encoding As Encoding = Encoding.Unicode Or UTF8.When(encodingByte = byte1)
        Dim nameJp$ = pmx.ReadString(pmx.ReadUInt32, encoding)
        Dim nameEn$ = pmx.ReadString(pmx.ReadUInt32, encoding)
        Dim commentJp$ = pmx.ReadString(pmx.ReadUInt32, encoding)
        Dim commentsEn$ = pmx.ReadString(pmx.ReadUInt32, encoding)

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

#Region "Model Data"

    <Extension>
    Private Iterator Function populateVertex(pmx As BinaryDataReader, n%, indexSize As IndexSize) As IEnumerable(Of vertex)
        For i As Integer = 0 To n - 1
            Dim xyz As New vec3 With {
                .x = pmx.ReadSingle,
                .y = pmx.ReadSingle,
                .z = pmx.ReadSingle
            }
            Dim UV As New vec2 With {
                .x = pmx.ReadSingle,
                .y = pmx.ReadSingle
            }
            Dim appendixUV As New vec4 With {
                .x = pmx.ReadSingle,
                .y = pmx.ReadSingle,
                .z = pmx.ReadSingle,
                .w = pmx.ReadSingle
            }
            Dim type As New DeformType With {
                .type = pmx.ReadByte,
                .BDEF1 = pmx.readBDEF1(.type, indexSize),
                .BDEF2 = pmx.readBDEF2(.type, indexSize),
                .BDEF4 = pmx.readBDEF4(.type, indexSize),
                .SDEF = pmx.readSDEF(.type, indexSize),
                .QDEF = pmx.readQDEF(.type, indexSize)
            }

            Yield New vertex With {
                .position = xyz,
                .UVtextureCoordinate = UV,
                .appendixUV = appendixUV,
                .weightDeform = type,
                .edgeScale = pmx.ReadSingle
            }
        Next
    End Function

#Region "DeformTypes"

    <Extension>
    Private Function readIndex(pmx As BinaryDataReader, type As IndexSize) As Integer
        Select Case type
            Case IndexSize.byte
                Return pmx.ReadByte
            Case IndexSize.short
                Return pmx.ReadUInt16
            Case IndexSize.int
                Return pmx.ReadUInt32
            Case Else
                Throw New NotImplementedException
        End Select
    End Function

    <Extension>
    Private Function readBDEF1(pmx As BinaryDataReader, type As WeightDeformTypes, size As IndexSize) As BDEF1
        If type <> WeightDeformTypes.BDEF1 Then
            Return Nothing
        Else
            Return New BDEF1 With {
                .index1 = pmx.readIndex(size),
                .weight1 = +1S
            }
        End If
    End Function

    <Extension>
    Private Function readBDEF2(pmx As BinaryDataReader, type As WeightDeformTypes, size As IndexSize) As BDEF2
        If type <> WeightDeformTypes.BDEF2 Then
            Return Nothing
        Else
            Return New BDEF2 With {
                .index1 = pmx.readIndex(size),
                .index2 = pmx.readIndex(size),
                .weight1 = pmx.ReadSingle
            }
        End If
    End Function

    <Extension>
    Private Function readBDEF4(pmx As BinaryDataReader, type As WeightDeformTypes, size As IndexSize) As BDEF4
        If type <> WeightDeformTypes.BDEF4 Then
            Return Nothing
        Else
            Return New BDEF4 With {
                .index1 = pmx.readIndex(size),
                .index2 = pmx.readIndex(size),
                .index3 = pmx.readIndex(size),
                .index4 = pmx.readIndex(size),
                .weight1 = pmx.ReadSingle,
                .weight2 = pmx.ReadSingle,
                .weight3 = pmx.ReadSingle,
                .weight4 = pmx.ReadSingle
            }
        End If
    End Function

    <Extension>
    Private Function readSDEF(pmx As BinaryDataReader, type As WeightDeformTypes, size As IndexSize) As SDEF
        If type <> WeightDeformTypes.SDEF Then
            Return Nothing
        Else
            Return New SDEF With {
                .index1 = pmx.readIndex(size),
                .index2 = pmx.readIndex(size),
                .weight1 = pmx.ReadSingle,
                .C = New vec3 With {.x = pmx.ReadSingle, .y = pmx.ReadSingle, .z = pmx.ReadSingle},
                .R0 = New vec3 With {.x = pmx.ReadSingle, .y = pmx.ReadSingle, .z = pmx.ReadSingle},
                .R1 = New vec3 With {.x = pmx.ReadSingle, .y = pmx.ReadSingle, .z = pmx.ReadSingle}
            }
        End If
    End Function

    <Extension>
    Private Function readQDEF(pmx As BinaryDataReader, type As WeightDeformTypes, size As IndexSize) As QDEF
        If type <> WeightDeformTypes.QDEF Then
            Return Nothing
        Else
            Return New QDEF With {
                .index1 = pmx.readIndex(size),
                .index2 = pmx.readIndex(size),
                .index3 = pmx.readIndex(size),
                .index4 = pmx.readIndex(size),
                .weight1 = pmx.ReadSingle,
                .weight2 = pmx.ReadSingle,
                .weight3 = pmx.ReadSingle,
                .weight4 = pmx.ReadSingle
            }
        End If
    End Function
#End Region
#End Region
End Module

Public Enum Versions As Byte
    Unknown = 0

    <Description("2.0")> v20 = 1
    <Description("2.1")> v21 = 2
End Enum