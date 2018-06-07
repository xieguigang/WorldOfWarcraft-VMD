Imports System.Runtime.InteropServices

''' <summary>
''' http://mikumikudance.wikia.com/wiki/VMD_file_format
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

    Public Property BoneKeyframeList As KeyFrameList(Of Bone)
    Public Property FaceKeyframeList As KeyFrameList(Of Face)
    Public Property CameraKeyframeList As KeyFrameList(Of Camera)

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

    Public Property BoneName As String

    Public Property X As Double
    Public Property Y As Double
    Public Property Z As Double

    Public Property rX As Double
    Public Property rY As Double
    Public Property rZ As Double
    Public Property rW As Double

    ''' <summary>
    ''' 64 bytes of frame interpolation data. 
    ''' </summary>
    ''' <returns></returns>
    Public Property Interpolation As Byte()
End Class

Public Class Face : Inherits KeyFrame

    Public Property FaceName As String
    Public Property Scale As Double
End Class

Public Class Camera : Inherits KeyFrame

    Public Property Length As Double

    Public Property X As Double
    Public Property Y As Double
    Public Property Z As Double

    Public Property rX As Double
    Public Property rY As Double
    Public Property rZ As Double

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

    Public Property Perspective As Byte
End Class