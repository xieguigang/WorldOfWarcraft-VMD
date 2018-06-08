Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel

<XmlType("VMD-xml", [Namespace]:="http://plugins.mmd.lilithaf.me/file/vmd")>
Public Class Xml : Inherits XmlDataModel

    Public Property VMD As VMD

    Public Function WriteVMD(path$, Optional version As Versions = Versions.MikuMikuDanceNewer) As Boolean
        Return VMD.Save(path, version)
    End Function

    Public Overrides Function ToString() As String
        Return VMD.ToString
    End Function
End Class

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
Public Class VMD

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

    Public ReadOnly Property Version As Versions
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

    Public Const MMDOfficial$ = "http://www.geocities.jp/higuchuu4/"

    Sub New()
        xmlns = New XmlSerializerNamespaces
        xmlns.Add("mmd", MMDOfficial)
    End Sub

    Public Overrides Function ToString() As String
        Return modelName
    End Function

End Class

Public Class KeyFrameList(Of T As KeyFrame)

    ''' <summary>
    ''' keyframe list, which starts with a 4-byte unsigned int that tells how 
    ''' many keyframes are listed in the file.
    ''' </summary>
    ''' <returns></returns>
    <XmlAttribute("size")>
    Public Property count As UInteger
    <XmlElement>
    Public Property keyframes As T()

    Public Overrides Function ToString() As String
        Return $"[{count}] {GetType(T).FullName}"
    End Function
End Class

Public MustInherit Class KeyFrame

    ''' <summary>
    ''' ``byte[4] (unsigned int)``
    ''' 
    ''' The frame number. Since keyframes are not necessarily stored for each actual 
    ''' frame, the animation software must interpolate between two adjacent keyframes 
    ''' with different frame indices.
    ''' </summary>
    ''' <returns></returns>
    <XmlAttribute>
    Public Property index As UInteger

End Class

Public Class bone : Inherits KeyFrame

    ''' <summary>
    ''' A null-terminated string representing the name of the bone to which the transformation 
    ''' will be applied.
    ''' </summary>
    ''' <returns></returns>
    <XmlAttribute>
    Public Property boneName As String

    ''' <summary>
    ''' + X-coordinate of the bone position
    ''' + Y-coordinate of the bone position
    ''' + Z-coordinate of the bone position
    ''' </summary>
    ''' <returns></returns>
    <XmlElement>
    Public Property position As Vector

    ''' <summary>
    ''' X-coordinate of the bone rotation (quaternion)
    ''' Y-coordinate of the bone rotation (quaternion)
    ''' Z-coordinate of the bone rotation (quaternion)
    ''' W-coordinate of the bone rotation (quaternion)
    ''' </summary>
    ''' <returns></returns>
    <XmlElement>
    Public Property rotation As Vector

    ''' <summary>
    ''' 64 bytes of frame interpolation data. 
    ''' </summary>
    ''' <returns></returns>
    Public Property interpolation As Byte()

    Public Overrides Function ToString() As String
        Return boneName
    End Function
End Class

Public Class face : Inherits KeyFrame

    ''' <summary>
    ''' A null-terminated string representing the name 
    ''' of the face to which the transformation will be 
    ''' applied.
    ''' </summary>
    ''' <returns></returns>
    <XmlAttribute>
    Public Property faceName As String
    ''' <summary>
    ''' Weight - this value is on a scale of 0.0-1.0. 
    ''' It is used to scale how much a face morph should 
    ''' move a vertex based off of the maximum possible 
    ''' coordinate that it can move by (specified in the 
    ''' PMD)
    ''' </summary>
    ''' <returns></returns>
    <XmlAttribute>
    Public Property scale As Single

    Public Overrides Function ToString() As String
        Return faceName
    End Function
End Class

Public Class camera : Inherits KeyFrame

    <XmlAttribute>
    Public Property len As Single

    ''' <summary>
    ''' + X-coordinate of the bone position
    ''' + Y-coordinate of the bone position
    ''' + Z-coordinate of the bone position
    ''' </summary>
    ''' <returns></returns>
    Public Property position As Vector

    ''' <summary>
    ''' X-coordinate of the bone rotation (quaternion)
    ''' Y-coordinate of the bone rotation (quaternion)
    ''' Z-coordinate of the bone rotation (quaternion)
    ''' </summary>
    ''' <returns></returns>
    Public Property rotation As Vector

    ''' <summary>
    ''' 24 bytes of interpolation data.
    ''' </summary>
    ''' <returns></returns>
    Public Property interpolation As Byte()

    ''' <summary>
    ''' Camera FOV angle
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <XmlAttribute>
    Public Property angle As UInteger

    ''' <summary>
    ''' Perspective
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <XmlAttribute>
    Public Property perspective As Byte

    Public Overrides Function ToString() As String
        Return position.GetXml
    End Function
End Class