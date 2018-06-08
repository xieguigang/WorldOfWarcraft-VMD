Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO

Public Module Writer

#Region "Common data structures"

    <Extension>
    Public Function Save(vmd As VMD, path$, Optional version As Versions = Versions.MikuMikuDanceNewer)
        Dim nameText As New FixLengthString(encoding:=Shift_JIS932)

        Using file As New BinaryDataWriter(path.Open())
            If version = Versions.MikuMikuDance130 Then
                Call file.Write(nameText.GetBytes(Reader.MikuMikuDanceMagicHeader130, 30))
                Call file.Write(nameText.GetBytes(vmd.modelName, 10))
            ElseIf version = Versions.MikuMikuDanceNewer Then
                Call file.Write(nameText.GetBytes(Reader.MikuMikuDanceMagicHeaderNew, 30))
                Call file.Write(nameText.GetBytes(vmd.modelName, 20))
            Else
                Throw New NotImplementedException
            End If

            Call file.writeBones(vmd.boneList)
            Call file.writeFaces(vmd.faceList)
            Call file.writeCameras(vmd.cameraList)
        End Using

        Return True
    End Function

    <Extension>
    Private Sub writeBones(vmd As BinaryDataWriter, bones As KeyFrameList(Of bone))
        Dim nameText As New FixLengthString(encoding:=Shift_JIS932)

        Call vmd.Write(bones.count)

        For Each bone As bone In bones.keyframes
            Call vmd.Write(nameText.GetBytes(bone.boneName, 15))
            Call vmd.Write(bone.index)
            Call vmd.Write(CSng(bone.position.pivot(0)))
            Call vmd.Write(CSng(bone.position.pivot(1)))
            Call vmd.Write(CSng(bone.position.pivot(2)))
            Call vmd.Write(CSng(bone.rotation.pivot(0)))
            Call vmd.Write(CSng(bone.rotation.pivot(1)))
            Call vmd.Write(CSng(bone.rotation.pivot(2)))
            Call vmd.Write(CSng(bone.rotation.pivot(3)))
            Call vmd.Write(bone.interpolation)
        Next
    End Sub

    <Extension>
    Private Sub writeFaces(vmd As BinaryDataWriter, faces As KeyFrameList(Of face))
        Dim nameText As New FixLengthString(encoding:=Shift_JIS932)

        Call vmd.Write(faces.count)

        For Each face As face In faces.keyframes
            Call vmd.Write(nameText.GetBytes(face.faceName, 15))
            Call vmd.Write(face.index)
            Call vmd.Write(face.scale)
        Next
    End Sub

    <Extension>
    Private Sub writeCameras(vmd As BinaryDataWriter, cameras As KeyFrameList(Of camera))
        Call vmd.Write(cameras.count)

        For Each camera As camera In cameras.keyframes
            Call vmd.Write(camera.index)
            Call vmd.Write(camera.len)
            Call vmd.Write(CSng(camera.position.pivot(0)))
            Call vmd.Write(CSng(camera.position.pivot(1)))
            Call vmd.Write(CSng(camera.position.pivot(2)))
            Call vmd.Write(CSng(camera.rotation.pivot(0)))
            Call vmd.Write(CSng(camera.rotation.pivot(1)))
            Call vmd.Write(CSng(camera.rotation.pivot(2)))
            Call vmd.Write(camera.interpolation)
            Call vmd.Write(camera.angle)
            Call vmd.Write(camera.perspective)
        Next
    End Sub
#End Region
End Module
