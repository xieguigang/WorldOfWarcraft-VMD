Imports System.Runtime.CompilerServices

Public Module Reader

    Const MikuMikuDanceMagicHeader130$ = "Vocaloid Motion Data file"
    Const MikuMikuDanceMagicHeaderNew$ = "Vocaloid Motion Data 0002"

    Public Enum Versions As Integer
        MikuMikuDance130 = 10
        MikuMikuDanceNewer = 20
    End Enum

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Open(vmd$, Optional version As Versions = Versions.MikuMikuDanceNewer) As VMD
        If version = Versions.MikuMikuDance130 Then
            Return Open130Version(path:=vmd)
        Else
            Return OpenNewerVersion(path:=vmd)
        End If
    End Function

    Public Function Open130Version(path As String) As VMD

    End Function

    Public Function OpenNewerVersion(path As String) As VMD

    End Function

End Module
