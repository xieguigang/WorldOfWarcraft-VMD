Imports System.Runtime.CompilerServices
Imports MikuMikuDance.File.PMX.Model
Imports WorldOfWarcraft.Plugins.WMV.ogre

Public Module Extensions

    <Extension> Public Function AsOgre(mmd As PMXFile) As skeleton
        '.bonehierarchy = mmd.bones _
        '    .data _
        '    .Select(Function(b)
        '                Return New boneparent With {.parent = b.parentIndex}
        '            End Function) _
        '    .ToArray,

        Return New skeleton With {
            .bones = mmd.bones _
                .data _
                .Select(Function(b, i)
                            Return New bone With {
                                .name = b.name,
                                .position = b.position,
                                .id = i
                            }
                        End Function) _
                .ToArray
        }
    End Function
End Module
