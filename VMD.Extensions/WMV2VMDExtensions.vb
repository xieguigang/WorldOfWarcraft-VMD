Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports MikuMikuDance.File.PMX.Model
Imports MikuMikuDance.Math3D
Imports WorldOfWarcraft.Plugins.WMV.ogre

Public Module WMV2VMDExtensions

    ''' <summary>
    ''' Reset WOW skeleton
    ''' </summary>
    ''' <param name="skeleton"></param>
    ''' <returns></returns>
    <Extension>
    Public Function ResetSkeleton(skeleton As skeleton) As skeleton
        Dim minZ# = skeleton.bones _
                            .Select(Function(b) b.position.z) _
                            .Min
        Dim xrange As DoubleRange = skeleton _
            .bones _
            .Select(Function(b) CDbl(b.position.x)) _
            .ToArray
        Dim yrange As DoubleRange = skeleton _
            .bones _
            .Select(Function(b) CDbl(b.position.y)) _
            .ToArray
        Dim offset As Vector = {
            xrange.Length / 2,
            yrange.Length / 2,
            minZ
        }
        Dim x, y, z As Single

        For Each bone As bone In skeleton.bones
            x = bone.position.x
            y = bone.position.y
            z = bone.position.z

            bone.position = New vec3 With {
                .x = x - offset(0),
                .y = y - offset(1),
                .z = z - offset(2)
            }
        Next

        Return skeleton
    End Function

    ''' <summary>
    ''' Reset MMD skeleton
    ''' </summary>
    ''' <param name="mmd"></param>
    ''' <returns></returns>
    <Extension>
    Public Function ResetSkeleton(mmd As PMXFile) As PMXFile
        Dim minZ# = mmd.bones.data.Select(Function(b) b.position.z).Min
        Dim xrange As DoubleRange = mmd.bones.data.Select(Function(b) CDbl(b.position.x)).ToArray
        Dim yrange As DoubleRange = mmd.bones.data.Select(Function(b) CDbl(b.position.y)).ToArray
        Dim offset As Vector = {
            xrange.Length / 2,
            yrange.Length / 2,
            minZ
        }
        Dim x, y, z As Single

        For Each bone In mmd.bones.data
            x = bone.position.x
            y = bone.position.y
            z = bone.position.z

            bone.position = New vec3 With {
                .x = x - offset(0),
                .y = y - offset(1),
                .z = z - offset(2)
            }
        Next

        Return mmd
    End Function
End Module
