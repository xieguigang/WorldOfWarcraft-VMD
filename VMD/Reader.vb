Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO

Public Module Reader

    Const MikuMikuDanceMagicHeader130$ = "Vocaloid Motion Data file"
    Const MikuMikuDanceMagicHeaderNew$ = "Vocaloid Motion Data 0002"

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
        Dim file As New FileStream(vmd, FileMode.Open)
        Dim magic$ = New BinaryDataReader(file).ReadString(format:=BinaryStringFormat.ZeroTerminated)

        If magic = MikuMikuDanceMagicHeader130 Then
            Return Open130Version(path:=vmd)
        ElseIf magic = MikuMikuDanceMagicHeaderNew Then
            Return OpenNewerVersion(path:=vmd)
        Else
            Throw New InvalidProgramException("Unknown magic: " & magic)
        End If
    End Function

    Public Function Open130Version(path As String) As VMD

    End Function

    Public Function OpenNewerVersion(path As String) As VMD

    End Function

End Module
