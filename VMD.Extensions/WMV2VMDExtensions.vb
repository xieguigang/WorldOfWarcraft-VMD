Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
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

    ''' <summary>
    ''' 将WOW和MMD模型的骨骼尺寸调整到相同的尺度
    ''' </summary>
    ''' <param name="wow"></param>
    ''' <param name="mmd"></param>
    Private Sub rescale(wow As skeleton, mmd As PMXFile)
        Dim wowBounds As DoubleRange() = {
            wow.bones.Select(Function(b) CDbl(b.position.x)),
            wow.bones.Select(Function(b) CDbl(b.position.y)),
            wow.bones.Select(Function(b) CDbl(b.position.z))
        }
        Dim mmdBounds As DoubleRange() = {
            mmd.bones.data.Select(Function(b) CDbl(b.position.x)).ToArray,
            mmd.bones.data.Select(Function(b) CDbl(b.position.y)).ToArray,
            mmd.bones.data.Select(Function(b) CDbl(b.position.z)).ToArray
        }
        Dim maxScaleX, maxScaleY, maxScaleZ As DoubleRange

        If wowBounds(0).Length > mmdBounds(0).Length Then
            maxScaleX = wowBounds(0)
        Else
            maxScaleX = mmdBounds(0)
        End If
        If wowBounds(1).Length > mmdBounds(1).Length Then
            maxScaleY = wowBounds(1)
        Else
            maxScaleY = mmdBounds(1)
        End If
        If wowBounds(2).Length > mmdBounds(2).Length Then
            maxScaleZ = wowBounds(2)
        Else
            maxScaleZ = mmdBounds(2)
        End If


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
