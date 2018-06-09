Imports MikuMikuDance.File.PMX
Imports MikuMikuDance.File.VMD
Imports WorldOfWarcraft

Module Module1

    Sub Main()
        'Call vmdReaderTest()
        ' Call vmdWriteTest()

        Call pmxReadertest()

        Call loadWMVtest()
    End Sub

    Sub pmxReadertest()
        Dim pmx = PMXReader.Open("../DATA/vdproject.pmx")

        Pause()
    End Sub

    Sub loadWMVtest()
        Dim path = "C:\Users\Evia\source\repos\VMD\DATA\ModelInfo.xml"
        Dim wmv = Plugins.WMV.ModelInfo.Load(path)

        Pause()
    End Sub

    Sub vmdReaderTest()

        Dim path = "C:\Users\Evia\source\repos\VMD\DATA\MOTION.vmd"
        Dim vmd = Reader.OpenAuto(path)

        Call New Xml With {.VMD = vmd}.GetXml.SaveTo("./test.vmd.xml")

        Pause()
    End Sub

    Sub vmdWriteTest()
        Dim path = "C:\Users\Evia\source\repos\VMD\DATA\MOTION.vmd"
        Dim vmd = Reader.OpenAuto(path)

        Call vmd.Save("./130.vmd", Information.Versions.MikuMikuDance130)
        Call vmd.Save("./newer.vmd", Information.Versions.MikuMikuDanceNewer)

        Dim v130 = Reader.Open130Version("./130.vmd")
        Dim vnewer = Reader.OpenNewerVersion("./newer.vmd")

        Pause()
    End Sub
End Module
