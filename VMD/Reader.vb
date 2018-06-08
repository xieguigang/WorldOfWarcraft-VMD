Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Text

Public Module Reader

    Const MikuMikuDanceMagicHeader130$ = "Vocaloid Motion Data file"
    Const MikuMikuDanceMagicHeaderNew$ = "Vocaloid Motion Data 0002"

    ''' <summary>
    ''' MMD之中的日文编码
    ''' </summary>
    ReadOnly Shift_JIS932 As Encoding = Encoding.GetEncoding("shift_jis")

    Public Enum Versions As Integer
        Unknown = 0

        ''' <summary>
        ''' 10 bytes in the old version of VMD
        ''' </summary>
        MikuMikuDance130 = 10
        ''' <summary>
        ''' 20 bytes in the new version
        ''' </summary>
        MikuMikuDanceNewer = 20
    End Enum

    ''' <summary>
    ''' 手动指定读取的版本
    ''' </summary>
    ''' <param name="vmd$"></param>
    ''' <param name="version"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Open(vmd$, Optional version As Versions = Versions.MikuMikuDanceNewer) As VMD
        If version = Versions.MikuMikuDance130 Then
            Return Open130Version(path:=vmd)
        Else
            Return OpenNewerVersion(path:=vmd)
        End If
    End Function

    ''' <summary>
    ''' Open *.VMD file with version detected automaticlly
    ''' </summary>
    ''' <param name="vmd"></param>
    ''' <returns></returns>
    Public Function OpenAuto(vmd As String) As VMD
        Dim file As New BinaryDataReader(New FileStream(vmd, FileMode.Open))
        Dim magic$ = file.ReadString(format:=BinaryStringFormat.ZeroTerminated)

        Call file.Close()
        Call file.Dispose()

        If magic = MikuMikuDanceMagicHeader130 Then
            Return Open130Version(path:=vmd)
        ElseIf magic = MikuMikuDanceMagicHeaderNew Then
            Return OpenNewerVersion(path:=vmd)
        Else
            Throw New InvalidProgramException("Unknown magic: " & magic)
        End If
    End Function

    Public Function Open130Version(path As String) As VMD
        Using file As New BinaryDataReader(path.Open(FileMode.Open, doClear:=False))
            Dim modelName$
            Dim magicBytes$ = file.ReadString(format:=BinaryStringFormat.ZeroTerminated)

            If magicBytes <> MikuMikuDanceMagicHeader130 Then
                Throw New InvalidProgramException("Mismatch program version!")
            End If

            ' 前30个字节是magic bytes
            file.Seek(30, SeekOrigin.Begin)
            ' 后20个字节是模型的名称
            modelName = file.ReadString(10, Shift_JIS932)

            Return file.vmdReaderInternal(magicBytes, modelName)
        End Using
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Private Function vmdReaderInternal(file As BinaryDataReader, magicBytes$, modelName$) As VMD
        Return New VMD With {
            .MagicHeader = magicBytes,
            .ModelName = modelName,
            .BoneKeyframeList = New KeyFrameList(Of Bone) With {
                .Count = file.ReadUInt32,
                .Keyframes = file _
                    .populateBones(n:= .Count) _
                    .ToArray
            },
            .FaceKeyframeList = New KeyFrameList(Of Face) With {
                .Count = file.ReadUInt32,
                .Keyframes = file _
                    .populateFaces(n:= .Count) _
                    .ToArray
            },
            .CameraKeyframeList = New KeyFrameList(Of Camera) With {
                .Count = file.ReadUInt32,
                .Keyframes = file _
                    .populateCamera(n:= .Count) _
                    .ToArray
            }
        }
    End Function

    Public Function OpenNewerVersion(path As String) As VMD
        Using file As New BinaryDataReader(path.Open(FileMode.Open, doClear:=False))
            Dim modelName$
            Dim magicBytes$ = file.ReadString(format:=BinaryStringFormat.ZeroTerminated)

            If magicBytes <> MikuMikuDanceMagicHeaderNew Then
                Throw New InvalidProgramException("Mismatch program version!")
            End If

            ' 前30个字节是magic bytes
            file.Seek(30, SeekOrigin.Begin)
            ' 后20个字节是模型的名称
            modelName = file.ReadString(20, Shift_JIS932)

            Return file.vmdReaderInternal(magicBytes, modelName)
        End Using
    End Function

#Region "Common data structure"

    ' part 1. bones
    ' int(4) list length
    '
    ' list element:
    '
    ' char(15)
    ' int(4)
    ' single(4) x
    ' single(4) y
    ' single(4) z
    ' single(4) rx
    ' single(4) ry
    ' single(4) rz
    ' single(4) rw
    ' bytes(64)
    <Extension>
    Private Iterator Function populateBones(vmd As BinaryDataReader, n%) As IEnumerable(Of Bone)
        For i As Integer = 0 To n - 1
            Dim boneName$ = vmd.ReadString(15, Shift_JIS932)?.Trim(ASCII.NUL)
            Dim index As UInteger = vmd.ReadUInt32
            Dim x! = vmd.ReadSingle
            Dim y! = vmd.ReadSingle
            Dim z! = vmd.ReadSingle
            Dim rx! = vmd.ReadSingle
            Dim ry! = vmd.ReadSingle
            Dim rz! = vmd.ReadSingle
            Dim rw! = vmd.ReadSingle
            Dim interpolation As Byte() = vmd.ReadBytes(64)

            Yield New Bone With {
                .BoneName = boneName,
                .Index = index,
                .Interpolation = interpolation,
                .X = x,
                .Y = y,
                .Z = z,
                .rX = rx,
                .rY = ry,
                .rZ = rz,
                .rW = rw
            }
        Next
    End Function

    <Extension>
    Private Iterator Function populateFaces(vmd As BinaryDataReader, n%) As IEnumerable(Of Face)
        For i As Integer = 0 To n - 1
            Dim faceName$ = vmd.ReadString(15, Shift_JIS932)?.Trim(ASCII.NUL)
            Dim index As UInteger = vmd.ReadUInt32
            Dim weight! = vmd.ReadSingle

            Yield New Face With {
                .FaceName = faceName,
                .Index = index,
                .Scale = weight
            }
        Next
    End Function

    <Extension>
    Private Iterator Function populateCamera(vmd As BinaryDataReader, n%) As IEnumerable(Of Camera)
        For i As Integer = 0 To n - 1
            Dim index As UInteger = vmd.ReadUInt32
            Dim len! = vmd.ReadSingle
            Dim x! = vmd.ReadSingle
            Dim y! = vmd.ReadSingle
            Dim z! = vmd.ReadSingle
            Dim rx! = vmd.ReadSingle
            Dim ry! = vmd.ReadSingle
            Dim rz! = vmd.ReadSingle
            Dim interpolation As Byte() = vmd.ReadBytes(24)
            Dim angle As UInteger = vmd.ReadUInt32
            Dim perspective_toggle As Byte = vmd.ReadByte

            Yield New Camera With {
                .Index = index,
                .Length = len,
                .X = x,
                .Y = y,
                .Z = z,
                .rX = rx,
                .rY = ry,
                .rZ = rz,
                .Interpolation = interpolation,
                .Angle = angle,
                .Perspective = perspective_toggle
            }
        Next
    End Function
#End Region

End Module
