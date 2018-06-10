Imports System.Runtime.CompilerServices

Namespace ogre

    Public Class skeleton

        Public Property bones As bone()
        Public Property bonehierarchy As boneparent()
        Public Property animations As animation()

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function LoadSkeletonXml(path As String) As skeleton
            Return path _
                .ReadAllText _
                .CreateObjectFromXmlFragment(Of skeleton)(
                    Function(xml)
                        Return xml.Replace("#QNAN", "")
                    End Function)
        End Function
    End Class
End Namespace