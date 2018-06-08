Imports MikuMikuDance.File.VMD

Module Module1

    Sub Main()
        Call vmdReaderTest()

    End Sub

    Sub vmdReaderTest()

        Dim path = "C:\Users\Evia\source\repos\VMD\DATA\MOTION.vmd"
        Dim vmd = Reader.OpenAuto(path)

        Call New Xml With {.VMD = vmd}.GetXml.SaveTo("./test.vmd.xml")

        Pause()
    End Sub
End Module
