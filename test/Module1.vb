Imports MikuMikuDance.File
Imports MikuMikuDance.File.PMX
Imports MikuMikuDance.File.PMX.Model
Imports MikuMikuDance.File.VMD
Imports WorldOfWarcraft.Plugins.WMV

Module Module1

    Sub Main()
        'Call vmdReaderTest()
        ' Call vmdWriteTest()

        Call testLoadOgre()

        Call pmxReadertest()

        Call loadWMVtest()
    End Sub

    Sub testLoadOgre()
        Dim ske = ogre.skeleton.LoadSkeletonXml("../DATA/ogre/bloodelffemale.skeleton.xml")


        Pause()
    End Sub

    Sub pmxReadertest()
        Dim xi As PMXFile = PMXReader.Open("../DATA/vdproject.pmx")
        ' Dim pmx2 = PMXReader.Open("F:\MikuMikuDanceE_v926x64\小桃初代女仆v1\小桃初代女仆v1\小桃初代女仆v1.pmx")

        Call New PMX.Xml With {.PMX = xi}.GetXml.SaveTo("./xiv1.xml")

        Pause()
    End Sub

    Sub loadWMVtest()
        Dim path = "C:\Users\Evia\source\repos\VMD\DATA\ModelInfo.xml"
        Dim wmv = m2.ModelInfo.Load(path)

        Pause()
    End Sub

    Sub vmdReaderTest()

        Dim path = "C:\Users\Evia\source\repos\VMD\DATA\MOTION.vmd"
        Dim vmdData = Reader.OpenAuto(path)

        Call New VMD.Xml With {.VMD = vmdData}.GetXml.SaveTo("./test.vmd.xml")

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
