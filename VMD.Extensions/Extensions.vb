Imports System.Runtime.CompilerServices
Imports MikuMikuDance.File.PMX.Model
Imports WorldOfWarcraft.Plugins.WMV.ogre

Public Module Extensions

    <Extension> Public Function AsOgre(mmd As PMXFile) As skeleton
        Return New skeleton With {
            .bonehierarchy = mmd.bones _
                .data _
                .Select(Function(b)
                            Return New boneparent With {.parent = b.parentIndex}
                        End Function) _
                .ToArray,
            .bones = mmd.bones _
                .data _
                .Select(Function(b)
                            Return New bone With {
                                .name = b.name,
                                .position = b.position
                            }
                        End Function) _
                .ToArray
        }
    End Function
End Module
