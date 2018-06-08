Imports System.Runtime.InteropServices

''' <summary>
''' # VMD file format
''' 
''' > http://mikumikudance.wikia.com/wiki/VMD_file_format
''' 
''' The Vocaloid Motion Data (VMD) file format is the file format used to 
''' store animations for models used in the MikuMikuDance (Polygon Movie 
''' Maker) animation program.
''' </summary>
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
    Public Property MagicHeader As String

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
    Public Property ModelName As String

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
    Public Property BoneKeyframeList As KeyFrameList(Of Bone)
    Public Property FaceKeyframeList As KeyFrameList(Of Face)
    Public Property CameraKeyframeList As KeyFrameList(Of Camera)

    Public Overrides Function ToString() As String
        Return ModelName
    End Function

End Class

Public Class KeyFrameList(Of T As KeyFrame)

    ''' <summary>
    ''' keyframe list, which starts with a 4-byte unsigned int that tells how 
    ''' many keyframes are listed in the file.
    ''' </summary>
    ''' <returns></returns>
    Public Property Count As Integer

    Public Property Keyframes As T()

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
    Public Property Index As Integer

End Class

Public Class Bone : Inherits KeyFrame

    ''' <summary>
    ''' A null-terminated string representing the name of the bone to which the transformation 
    ''' will be applied.
    ''' </summary>
    ''' <returns></returns>
    Public Property BoneName As String

    ''' <summary>
    ''' X-coordinate of the bone position
    ''' </summary>
    ''' <returns></returns>
    Public Property X As Single
    ''' <summary>
    ''' Y-coordinate of the bone position
    ''' </summary>
    ''' <returns></returns>
    Public Property Y As Single
    ''' <summary>
    ''' Z-coordinate of the bone position
    ''' </summary>
    ''' <returns></returns>
    Public Property Z As Single

    ''' <summary>
    ''' X-coordinate of the bone rotation (quaternion)
    ''' </summary>
    ''' <returns></returns>
    Public Property rX As Single
    ''' <summary>
    ''' Y-coordinate of the bone rotation (quaternion)
    ''' </summary>
    ''' <returns></returns>
    Public Property rY As Single
    ''' <summary>
    ''' Z-coordinate of the bone rotation (quaternion)
    ''' </summary>
    ''' <returns></returns>
    Public Property rZ As Single
    ''' <summary>
    ''' W-coordinate of the bone rotation (quaternion)
    ''' </summary>
    ''' <returns></returns>
    Public Property rW As Single

    ''' <summary>
    ''' 64 bytes of frame interpolation data. 
    ''' </summary>
    ''' <returns></returns>
    Public Property Interpolation As Byte()

    Public Overrides Function ToString() As String
        Return BoneName
    End Function
End Class

Public Class Face : Inherits KeyFrame

    ''' <summary>
    ''' A null-terminated string representing the name 
    ''' of the face to which the transformation will be 
    ''' applied.
    ''' </summary>
    ''' <returns></returns>
    Public Property FaceName As String
    ''' <summary>
    ''' Weight - this value is on a scale of 0.0-1.0. 
    ''' It is used to scale how much a face morph should 
    ''' move a vertex based off of the maximum possible 
    ''' coordinate that it can move by (specified in the 
    ''' PMD)
    ''' </summary>
    ''' <returns></returns>
    Public Property Scale As Single

    Public Overrides Function ToString() As String
        Return FaceName
    End Function
End Class

Public Class Camera : Inherits KeyFrame

    Public Property Length As Single

    ''' <summary>
    ''' X-coordinate of camera position
    ''' </summary>
    ''' <returns></returns>
    Public Property X As Single
    ''' <summary>
    ''' Y-coordinate of camera position
    ''' </summary>
    ''' <returns></returns>
    Public Property Y As Single
    ''' <summary>
    ''' Z-coordinate of camera position
    ''' </summary>
    ''' <returns></returns>
    Public Property Z As Single

    ''' <summary>
    ''' X-coordinate of camera rotation
    ''' </summary>
    ''' <returns></returns>
    Public Property rX As Single
    ''' <summary>
    ''' Y-coordinate of camera rotation
    ''' </summary>
    ''' <returns></returns>
    Public Property rY As Single
    ''' <summary>
    ''' Z-coordinate of camera rotation
    ''' </summary>
    ''' <returns></returns>
    Public Property rZ As Single

    ''' <summary>
    ''' 24 bytes of interpolation data.
    ''' </summary>
    ''' <returns></returns>
    Public Property Interpolation As Byte()

    ''' <summary>
    ''' Camera FOV angle
    ''' </summary>
    ''' <returns></returns>
    Public Property Angle As Integer

    ''' <summary>
    ''' Perspective
    ''' </summary>
    ''' <returns></returns>
    Public Property Perspective As Byte

    Public Overrides Function ToString() As String
        Return $"[{X}, {Y}, {Z}]"
    End Function
End Class