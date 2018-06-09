Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Text
Imports MikuMikuDance.File.PMX.Model
Imports MikuMikuDance.File.PMX.Model.BoneData
Imports MikuMikuDance.File.PMX.Model.Information
Imports MikuMikuDance.File.PMX.Model.Information.globals
Imports MikuMikuDance.File.PMX.Model.VertexData
Imports MikuMikuDance.File.PMX.Model.VertexData.DeformTypes

Public Module PMXReader

    ''' <summary>
    ''' MMD之中的日文编码
    ''' </summary>
    ReadOnly Shift_JIS932 As Encoding = System.Text.Encoding.GetEncoding("shift_jis")

    Public Function Open(pmx As String) As PMXFile
        Using file As BinaryDataReader = pmx.OpenBinaryReader
            Dim header As header = file.readHeader
            Dim globals = header.globals
            Dim textures As New Value(Of Texture)

            Return New PMXFile With {
                .header = header,
                .modelInfo = file.readModelInfo(globals.encoding),
                .vertexData = New vertexs With {
                    .size = file.ReadInt32,
                    .data = file.populateVertex(
                        n:= .size,
                        globals:=globals
                    ).ToArray
                },
                .faceVertex = file.readFaceVertex(globals),
                .textureTable = (textures = file.readTextureTable(globals)),
                .materials = New MaterialList With {
                    .size = file.ReadInt32,
                    .data = file.readMaterials(
                        n:= .size,
                        globals:=globals,
                        textures:=textures
                    ).ToArray
                },
                .bones = New bones With {
                    .size = file.ReadInt32,
                    .data = file.readBones(
                        n:= .size,
                        globals:=globals
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
            .magics = magic,
            .version = version,
            .count = count,
            .globals = New globals(globals)
        }
    End Function

#Region "Model Data"

    <Extension>
    Private Iterator Function readBones(pmx As BinaryDataReader, n%, globals As globals) As IEnumerable(Of Bone)
        Dim encoding As Encoding = Encoding.Unicode Or UTF8.When(globals.encoding = byte1)

        For i As Integer = 0 To n - 1
            Dim name$ = pmx.ReadString(BinaryStringFormat.UInt32LengthPrefix, encoding)
            Dim enUS$ = pmx.ReadString(BinaryStringFormat.UInt32LengthPrefix, encoding)
            Dim position As New vec3 With {
                .x = pmx.ReadSingle,
                .y = pmx.ReadSingle,
                .z = pmx.ReadSingle
            }
            Dim parentIndex% = pmx.readIndex(globals.boneIndexSize)
            Dim level% = pmx.ReadInt32
            Dim flag As BoneFlags = pmx.ReadUInt16
            Dim connectedToBone%
            Dim connectedToOffset As vec3

            If flag.CheckFlag(BoneFlags.ToBone) Then
                connectedToBone = pmx.readIndex(globals.boneIndexSize)
            Else
                connectedToOffset = New vec3 With {
                    .x = pmx.ReadSingle,
                    .y = pmx.ReadSingle,
                    .z = pmx.ReadSingle
                }
            End If

            Dim appendParent%
            Dim appendRatio!

            If flag.CheckFlag(BoneFlags.AppendRotation) OrElse flag.CheckFlag(BoneFlags.AppendTranslation) Then
                appendParent = pmx.readIndex(globals.boneIndexSize)
                appendRatio = pmx.ReadSingle
            End If

            Dim axis As vec3

            If flag.CheckFlag(BoneFlags.FixAxis) Then
                axis = New vec3 With {
                    .x = pmx.ReadSingle,
                    .y = pmx.ReadSingle,
                    .z = pmx.ReadSingle
                }
            End If

            Dim localX = vec3.UnitX, localY = vec3.UnitY, localZ = vec3.UnitZ

            If flag.CheckFlag(BoneFlags.LocalFrame) Then
                localX = New vec3 With {
                    .x = pmx.ReadSingle,
                    .y = pmx.ReadSingle,
                    .z = pmx.ReadSingle
                }
                localZ = New vec3 With {
                    .x = pmx.ReadSingle,
                    .y = pmx.ReadSingle,
                    .z = pmx.ReadSingle
                }

                localY = localZ.Cross(localX)
                localZ = localX.Cross(localY)
            End If

            Dim extKey%

            If flag.CheckFlag(BoneFlags.ExtParent) Then
                extKey = pmx.ReadInt32
            End If

            Dim IK As IK = Nothing

            If flag.CheckFlag(BoneFlags.IK) Then
                IK = pmx.readIK(globals)
            End If

            Yield New Bone With {
                .name = name,
                .enUS = enUS,
                .position = position,
                .parentIndex = parentIndex,
                .level = level,
                .boneFlag = flag,
                .connectedTo = New connectTo With {
                    .toBoneIndex = connectedToBone,
                    .toOffset = connectedToOffset
                },
                .appendParent = appendParent,
                .appendRatio = appendRatio,
                .axis = axis,
                .localX = localX,
                .localY = localY,
                .localZ = localZ,
                .extKey = extKey,
                .IK = IK
            }
        Next
    End Function

    <Extension>
    Private Function readIK(pmx As BinaryDataReader, globals As globals) As IK
        Dim target% = pmx.readIndex(globals.boneIndexSize)
        Dim loops = pmx.ReadInt32
        Dim angle! = pmx.ReadSingle

        Return New IK With {
            .angle = angle,
            .loops = loops,
            .target = target,
            .linklist = pmx.readLinks(
                n:=pmx.ReadInt32,
                globals:=globals
            ).ToArray
        }
    End Function

    <Extension>
    Private Iterator Function readLinks(pmx As BinaryDataReader, n%, globals As globals) As IEnumerable(Of Link)
        For i As Integer = 0 To n - 1
            Dim boneIndex = pmx.readIndex(globals.boneIndexSize)
            Dim isLimited = pmx.ReadByte <> 0
            Dim low, high As vec3

            If isLimited Then
                low = New vec3 With {
                    .x = pmx.ReadSingle,
                    .y = pmx.ReadSingle,
                    .z = pmx.ReadSingle
                }
                high = New vec3 With {
                    .x = pmx.ReadSingle,
                    .y = pmx.ReadSingle,
                    .z = pmx.ReadSingle
                }
            End If

            Yield New Link With {
                .boneIndex = boneIndex,
                .high = high,
                .low = low,
                .isLimited = isLimited
            }
        Next
    End Function

    <Extension>
    Public Function CheckFlag(flags As BoneFlags, check As BoneFlags) As Boolean
        Return (check And flags) = check
    End Function

    <Extension>
    Private Iterator Function readMaterials(pmx As BinaryDataReader, n%, globals As globals, textures As Texture) As IEnumerable(Of Material)
        Dim encoding As Encoding = Encoding.Unicode Or UTF8.When(globals.encoding = byte1)

        For i As Integer = 0 To n - 1
            Dim name$ = pmx.ReadString(BinaryStringFormat.UInt32LengthPrefix, encoding)
            Dim enName$ = pmx.ReadString(BinaryStringFormat.UInt32LengthPrefix, encoding)
            ' argb
            Dim argb!() = pmx.ReadSingles(4)
            Dim diffuse As Color = Color.FromArgb(argb(0), argb(1), argb(2), argb(3))
            ' rgb
            argb = pmx.ReadSingles(3)
            Dim specular As Color = Color.FromArgb(argb(0), argb(1), argb(2))
            Dim specularity! = pmx.ReadSingle
            argb = pmx.ReadSingles(3)
            Dim ambient As Color = Color.FromArgb(argb(0), argb(1), argb(2))
            Dim flag As DrawingModes = pmx.ReadByte
            argb = pmx.ReadSingles(4)
            Dim edgeColor As Color = Color.FromArgb(argb(0), argb(1), argb(2), argb(3))
            Dim edgeSize! = pmx.ReadSingle
            Dim texNameIndex = pmx.readIndex(globals.textureIndexSize)
            Dim sphereIndex = pmx.readIndex(globals.textureIndexSize)
            Dim sphereMode = DirectCast(pmx.ReadByte, SphereModes)
            Dim toon As NamedValue(Of Integer)
            Dim toonFlag = pmx.ReadByte

            If toonFlag = 0 Then
                toon = New NamedValue(Of Integer) With {
                    .Value = pmx.readIndex(globals.textureIndexSize),
                    .Name = textures.GetTextureName(.Value)
                }
            Else
                toon = New NamedValue(Of Integer) With {
                    .Value = pmx.ReadByte,
                    .Name = Material.GetToonName(.Value)
                }
            End If

            Dim memo = pmx.ReadString(BinaryStringFormat.UInt32LengthPrefix, encoding)
            Dim faces = pmx.ReadInt32

            Yield New Material With {
                .name = name,
                .enUS = enName,
                .diffuseColor = diffuse,
                .specularColor = specular,
                .specularity = specularity,
                .ambientColor = ambient,
                .drawingMode = flag,
                .edgeColor = edgeColor,
                .edgeSize = edgeSize,
                .textureIndex = New NamedValue(Of Integer) With {
                    .Name = textures.GetTextureName(texNameIndex),
                    .Value = texNameIndex
                },
                .sphereIndex = New NamedValue(Of Integer) With {
                    .Name = textures.GetTextureName(sphereIndex),
                    .Value = sphereIndex
                },
                .sphereMode = sphereMode,
                .toonFlag = toonFlag,
                .toonIndex = toon,
                .memo = memo,
                .faceCount = faces
            }
        Next
    End Function

    <Extension>
    Private Function readTextureTable(pmx As BinaryDataReader, globals As globals) As Texture
        Dim encoding As Encoding = Encoding.Unicode Or UTF8.When(globals.encoding = byte1)
        Dim numbers = pmx.ReadInt32
        Dim names$() = New String(numbers - 1) {}

        For i As Integer = 0 To numbers - 1
            names(i) = pmx.ReadString(BinaryStringFormat.UInt32LengthPrefix, encoding)
        Next

        Return New Texture With {
            .fileNames = names,
            .size = numbers
        }
    End Function

    <Extension>
    Private Function readFaceVertex(pmx As BinaryDataReader, globals As globals) As Face
        Dim numbers% = pmx.ReadInt32
        Dim indexReader As Func(Of Integer) = pmx.indexReader(globals.vertexIndexSize)
        Dim index%() = New Integer(numbers - 1) {}

        For i As Integer = 0 To numbers - 1
            index(i) = indexReader()
        Next

        Return New Face With {
            .size = numbers,
            .vertexIndices = index
        }
    End Function

    <Extension>
    Private Iterator Function populateVertex(pmx As BinaryDataReader, n%, globals As globals) As IEnumerable(Of vertex)
        For i As Integer = 0 To n - 1
            Dim xyz As New vec3 With {
                .x = pmx.ReadSingle,
                .y = pmx.ReadSingle,
                .z = -pmx.ReadSingle
            }
            Dim normal As New vec3 With {
                .x = pmx.ReadSingle,
                .y = pmx.ReadSingle,
                .z = -pmx.ReadSingle
            }
            Dim UV As New vec2 With {
                .x = pmx.ReadSingle,
                .y = pmx.ReadSingle
            }
            Dim appendixUVVector!() = pmx _
                .ReadBytes(4 * globals.addiVec4) _
                .Split(4, False) _
                .Select(Function(b) BitConverter.ToSingle(b, Scan0)) _
                .ToArray
            Dim appendixUV As New vec4 With {
                .x = appendixUVVector.ElementAtOrDefault(0),
                .y = appendixUVVector.ElementAtOrDefault(1),
                .z = appendixUVVector.ElementAtOrDefault(2),
                .w = appendixUVVector.ElementAtOrDefault(3)
            }
            Dim type As New DeformType With {
                .type = pmx.ReadByte,
                .BDEF1 = pmx.readBDEF1(.type, globals.boneIndexSize),
                .BDEF2 = pmx.readBDEF2(.type, globals.boneIndexSize),
                .BDEF4 = pmx.readBDEF4(.type, globals.boneIndexSize),
                .SDEF = pmx.readSDEF(.type, globals.boneIndexSize),
                .QDEF = pmx.readQDEF(.type, globals.boneIndexSize)
            }

            Yield New vertex With {
                .position = xyz,
                .normal = normal,
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
                Return pmx.ReadInt16
            Case IndexSize.int
                Return pmx.ReadInt32
            Case Else
                Throw New NotImplementedException
        End Select
    End Function

    <Extension>
    Private Function indexReader(pmx As BinaryDataReader, type As IndexSize) As Func(Of Integer)
        Select Case type
            Case IndexSize.byte
                Return Function() pmx.ReadByte
            Case IndexSize.short
                Return Function() pmx.ReadInt16
            Case IndexSize.int
                Return Function() pmx.ReadInt32
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