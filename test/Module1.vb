Imports MikuMikuDance.File.VMD

Module Module1

    Sub Main()
        Call vmdReaderTest()

    End Sub

    Sub vmdReaderTest()

        Dim path = "C:\Users\Evia\source\repos\VMD\DATA\MOTION.vmd"
        Dim vmd = Reader.OpenAuto(path)

        Pause()
    End Sub
End Module
