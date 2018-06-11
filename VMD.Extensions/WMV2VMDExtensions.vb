Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Imaging.Drawing3D.Math3D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Math
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
    Private Function ResetSkeleton(skeleton As skeleton) As skeleton
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
    Private Function ResetSkeleton(mmd As PMXFile) As PMXFile
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

    ''' <summary>
    ''' 将WOW和MMD模型的骨骼尺寸调整到相同的尺度
    ''' </summary>
    ''' <param name="wow"></param>
    ''' <param name="mmd"></param>
    Private Sub rescale(wow As skeleton, mmd As PMXFile)
        Dim wowBounds As DoubleRange() = {
            wow.bones.Select(Function(b) CDbl(b.position.x)).ToArray,
            wow.bones.Select(Function(b) CDbl(b.position.y)).ToArray,
            wow.bones.Select(Function(b) CDbl(b.position.z)).ToArray
        }
        Dim mmdBounds As DoubleRange() = {
            mmd.bones.data.Select(Function(b) CDbl(b.position.x)).ToArray,
            mmd.bones.data.Select(Function(b) CDbl(b.position.y)).ToArray,
            mmd.bones.data.Select(Function(b) CDbl(b.position.z)).ToArray
        }
        Dim scaleWOWx, scaleWOWy, scaleWOWz As Double
        Dim scaleMMDx, scaleMMDy, scaleMMDz As Double

        If wowBounds(0).Length > mmdBounds(0).Length Then
            scaleWOWx = 1
            scaleMMDx = wowBounds(0).Length / mmdBounds(0).Length
        Else
            scaleWOWx = mmdBounds(0).Length / wowBounds(0).Length
            scaleMMDx = 1
        End If
        If wowBounds(1).Length > mmdBounds(1).Length Then
            scaleWOWy = 1
            scaleMMDy = wowBounds(1).Length / mmdBounds(1).Length
        Else
            scaleWOWy = mmdBounds(1).Length / wowBounds(1).Length
            scaleMMDy = 1
        End If
        If wowBounds(2).Length > mmdBounds(2).Length Then
            scaleWOWz = 1
            scaleMMDz = wowBounds(2).Length / mmdBounds(2).Length
        Else
            scaleWOWz = mmdBounds(2).Length / wowBounds(2).Length
            scaleMMDz = 1
        End If

        Dim o As New Point3D

        For Each bone As bone In wow.bones
            Dim p3 As New Point3D(bone.position.x, bone.position.y, bone.position.z)
            p3 = p3.Scale(o, scaleWOWx, scaleWOWy, scaleWOWz)
            bone.position = New vec3 With {.x = p3.X, .y = p3.Y, .z = p3.Z}
        Next

        For Each bone In mmd.bones.data
            Dim p3 As New Point3D(bone.position.x, bone.position.y, bone.position.z)
            p3 = p3.Scale(o, scaleMMDx, scaleMMDy, scaleMMDz)
            bone.position = New vec3 With {.x = p3.X, .y = p3.Y, .z = p3.Z}
        Next
    End Sub

    ''' <summary>
    ''' 使用最小欧几里得距离进行骨骼的匹配操作
    ''' 
    ''' ``{WOW bone name -> MMD bone name}``
    ''' </summary>
    ''' <param name="wow"></param>
    ''' <param name="mmd"></param>
    ''' <returns></returns>
    <Extension>
    Public Function MatchBones(wow As skeleton, mmd As PMXFile) As Dictionary(Of String, String)
        Dim mapping As New Dictionary(Of String, String)

        Call rescale(wow, mmd)

        wow = wow.ResetSkeleton
        mmd = mmd.ResetSkeleton

        For Each bone As bone In wow.bones
            Dim query As Vector = {
                bone.position.x,
                bone.position.y,
                bone.position.z
            }
            Dim calcOrder = mmd.bones _
                .data _
                .OrderBy(Function(b)
                             Dim target As Vector = {
                                 b.position.x,
                                 b.position.y,
                                 b.position.z
                             }
                             Dim dist# = query.EuclideanDistance(target)
                             Return dist
                         End Function) _
                .ToArray
            Dim best = calcOrder.First
            Dim wowID = bone.name Or CStr(bone.id).AsDefault

            mapping(wowID) = best.name
        Next

        Return mapping
    End Function
End Module
