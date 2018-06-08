Imports System.Text

Public Module Information

    Public Const MikuMikuDanceMagicHeader130$ = "Vocaloid Motion Data file"
    Public Const MikuMikuDanceMagicHeaderNew$ = "Vocaloid Motion Data 0002"

    ''' <summary>
    ''' MMD之中的日文编码
    ''' </summary>
    Friend ReadOnly Shift_JIS932 As Encoding = Encoding.GetEncoding("shift_jis")

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
End Module
