Imports System.Windows.Forms
Imports WorldOfWarcraft.Plugins.WMV.ogre

Public Class SkeletonAlignment

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using Xml As New OpenFileDialog With {
            .Filter = "WoW Skeleton Xml(*.xml)|*.Xml"
        }
            If Xml.ShowDialog = DialogResult.OK Then
                Panel1.LoadModel(Xml.FileName.LoadXml(Of skeleton))
            End If
        End Using
    End Sub
End Class