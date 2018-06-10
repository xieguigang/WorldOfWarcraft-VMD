Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports MikuMikuDance.File.PMX.Model
Imports WorldOfWarcraft.Plugins.WMV.ogre

Public Module WMV2VMDExtensions

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
            xrange.Min + xrange.Length / 2,
            yrange.Min + yrange.Length / 2,
            minZ
        }

    End Function

    <Extension>
    Public Function ResetSkeleton(mmd As PMXFile) As PMXFile

    End Function
End Module
