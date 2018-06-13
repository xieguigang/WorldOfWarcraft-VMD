Imports System.Windows.Forms
Imports MikuMikuDance.File.PMX
Imports WorldOfWarcraft.Plugins.WMV.ogre

Public Class SkeletonAlignment

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using Xml As New OpenFileDialog With {
            .Filter = "WoW Skeleton Xml(*.xml)|*.Xml"
        }
            If Xml.ShowDialog = DialogResult.OK Then
                Panel1.LoadModel(skeleton.LoadSkeletonXml(Xml.FileName))
                Panel1.Run()
            End If
        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Using Pmx As New OpenFileDialog With {
            .Filter = "MMD model data(*.pmx)|*.pmx"
        }
            If Pmx.ShowDialog = DialogResult.OK Then
                Panel2.LoadModel(PMXReader.Open(Pmx.FileName))
                Panel2.Run()
            End If
        End Using
    End Sub
End Class