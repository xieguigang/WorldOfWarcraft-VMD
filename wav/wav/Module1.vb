Imports Microsoft.VisualBasic.Data.IO

Module Module1

    Sub Main()

        Dim wav = "X:\w.wav".OpenBinaryReader
        Dim header As Header = Header.ParseHeader(wav)


        Pause()
    End Sub

End Module

Public Class Header

    Public Property Magic As String
    Public Property fileSize As Integer
    Public Property FileType As String
    Public Property FormatChunkMarker As String
    Public Property VerifyLength As Integer
    Public Property Type As Integer
    Public Property Channels As Integer
    Public Property SampleRate As Integer
    Public Property Size As Integer
    Public Property Size2 As Integer
    Public Property BitsPerSample As Integer
    Public Property ChunkHeader As String
    Public Property DataSize As Integer

    Public Shared Function ParseHeader(wav As BinaryDataReader) As Header
        Return New Header With {
            .Magic = wav.ReadString(4),
            .fileSize = wav.ReadInt32,
            .FileType = wav.ReadString(4),
            .FormatChunkMarker = wav.ReadString(4),
            .VerifyLength = wav.ReadInt32,
            .Type = wav.ReadInt16,
            .Channels = wav.ReadInt16,
            .SampleRate = wav.ReadInt32,
            .Size = wav.ReadInt32,
            .Size2 = wav.ReadInt16,
            .BitsPerSample = wav.ReadInt16,
            .ChunkHeader = wav.ReadString(4),
            .DataSize = wav.ReadInt32
        }
    End Function
End Class