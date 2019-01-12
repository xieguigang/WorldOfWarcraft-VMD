Imports System.Xml.Serialization

Namespace Model

    ''' <summary>
    ''' # VMD file format
    ''' 
    ''' > http://mikumikudance.wikia.com/wiki/VMD_file_format
    ''' 
    ''' The Vocaloid Motion Data (VMD) file format is the file format used to 
    ''' store animations for models used in the MikuMikuDance (Polygon Movie 
    ''' Maker) animation program.
    ''' </summary>
    <XmlType("mikumikudance-VMD", [Namespace]:=VMD.MMDOfficial)>
    Public Class VMDFile

        ''' <summary>
        ''' The file begins with a 30-character "magic byte" sequence which 
        ''' can also be used to determine the version of the software used 
        ''' to create the file. 
        ''' 
        ''' The signature 
        ''' 
        ''' + is **"Vocaloid Motion Data file"** if the VMD was created with 
        '''   MikuMikuDance 1.30 (prior to the "Multi-Model" version). 
        ''' + It is **"Vocaloid Motion Data 0002"** if the VMD was created 
        '''   with later versions (Multi-Model Edition) of MikuMikuDance.
        '''   
        ''' </summary>
        ''' 
        Public Property magicHeader As String

        ''' <summary>
        ''' Following the magic bytes, there is a fixed-length string which tells 
        ''' the name of the model that this VMD is compatible with. 
        ''' 
        ''' **This string is 10 bytes in the old version of VMD, and 20 bytes 
        ''' in the new version.** If the name of the currently-loaded model does 
        ''' not match the name of the compatible model, the message "This motion 
        ''' file is the data for [current model name]. You can regist the motion 
        ''' only same bone name. Are you OK?" will appear. This means that it is 
        ''' possible to load the VMD, but only the bones described in the VMD whose 
        ''' names match bones in the currently-loaded model will be able to be 
        ''' registered.
        ''' </summary>
        ''' <returns></returns>
        Public Property modelName As String

        ''' <summary>
        ''' Now we get to the bone keyframe list, which starts with a 4-byte unsigned 
        ''' int that tells how many keyframes are listed in the file. Note that the 
        ''' position coordinates are relative to the "bind pose", or the model's default 
        ''' pose. The position data of the bones in the PMD model are relative to the 
        ''' world's origin, and the position data here is relative to that. So, for 
        ''' example, the bind pose of a bone is (1, 2, 3) and the VMD gives (10, 25, 30). 
        ''' The final world position would be (11, 27, 33). 
        ''' </summary>
        ''' <returns></returns>
        Public Property boneList As KeyFrameList(Of bone)
        Public Property faceList As KeyFrameList(Of face)
        Public Property cameraList As KeyFrameList(Of camera)

        Public ReadOnly Property version As Versions
            Get
                If magicHeader = Information.MikuMikuDanceMagicHeader130 Then
                    Return Versions.MikuMikuDance130
                ElseIf magicHeader = Information.MikuMikuDanceMagicHeaderNew Then
                    Return Versions.MikuMikuDanceNewer
                Else
                    Return Versions.Unknown
                End If
            End Get
        End Property

        <XmlNamespaceDeclarations()>
        Public xmlns As XmlSerializerNamespaces

        Sub New()
            xmlns = New XmlSerializerNamespaces
            xmlns.Add("mmd", MMDOfficial)
        End Sub

        Public Overrides Function ToString() As String
            Return modelName
        End Function

        Public Function GetAllBoneNames() As String()
            Return boneList.keyframes.Select(Function(b) b.boneName).Distinct.ToArray
        End Function

        Public Function GetAllFaceNames() As String()
            Return faceList.keyframes.Select(Function(f) f.faceName).Distinct.ToArray
        End Function

    End Class
End Namespace