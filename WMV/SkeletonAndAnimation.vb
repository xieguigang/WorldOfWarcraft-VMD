Imports System.Xml.Serialization

Public Class Sequences : Inherits List(Of Integer)

    <XmlElement>
    Public Property Sequence As Integer()
End Class

Public Class SkeletonAndAnimation

    Public Property GlobalSequences As Sequences
    Public Property Animations As Animations
    Public Property AnimationLookups As AnimationLookups
    Public Property Bones As Bones
    Public Property BoneLookups As BoneLookups
    Public Property KeyBoneLookups As KeyBoneLookups

End Class

Public Class List(Of T)

    <XmlAttribute>
    Public Property size As Integer
End Class

Public Class Animations : Inherits List(Of ModelAnimation)

    <XmlElement("Animation")>
    Public Property Animations As ModelAnimation()
End Class

Public Class ModelAnimation

    <XmlAttribute>
    Public Property id As Integer

    ''' <summary>
    ''' AnimationDataDB.ID
    ''' </summary>
    ''' <returns></returns>
    Public Property animID As Integer
    Public Property animName As String
    Public Property length As Integer
    Public Property moveSpeed As Double
    Public Property flags As Integer
    Public Property probability As Integer
    Public Property d1 As Integer
    Public Property d2 As Integer
    ''' <summary>
    ''' note: this can't be play speed because it's 0 for some models
    ''' </summary>
    ''' <returns></returns>
    Public Property playSpeed As Integer
    Public Property boxA As Vector
    Public Property boxB As Vector
    Public Property rad As Double
    Public Property NextAnimation As Integer
    Public Property Index As Integer

End Class

Public Class Lookup

    ''' <summary>
    ''' 仅仅起到排序的作用
    ''' </summary>
    ''' <returns></returns>
    <XmlAttribute>
    Public Property id As Integer
    ''' <summary>
    ''' 真正所需要的指向资源的指针数据
    ''' </summary>
    ''' <returns></returns>
    <XmlText>
    Public Property value As Integer
End Class

Public Class AnimationLookups : Inherits List(Of Lookup)

    <XmlElement("AnimationLookup")>
    Public Property AnimationLookups As Lookup()
End Class

Public Class Bones : Inherits List(Of ModelBoneDef)

    <XmlElement("Bone")>
    Public Property Bones As ModelBoneDef()
End Class

Public Class KeyBoneLookups : Inherits List(Of Lookup)

    <XmlElement("KeyBoneLookup")>
    Public Property KeyBoneLookups As Lookup()
End Class

Public Class BoneLookups : Inherits List(Of Lookup)

    <XmlElement("BoneLookup")>
    Public Property BoneLookups As Lookup()
End Class

Public Class ModelBoneDef

    ''' <summary>
    ''' Back-reference to the key bone lookup table. -1 if this is no key bone.
    ''' </summary>
    ''' <returns></returns>
    Public Property keyboneid As Integer
    Public Property billboard As Integer
    ''' <summary>
    ''' parent bone index
    ''' </summary>
    ''' <returns></returns>
    Public Property parent As Integer
    ''' <summary>
    ''' A geoset for this bone.
    ''' </summary>
    ''' <returns></returns>
    Public Property geoid As Integer
    ''' <summary>
    ''' new int added to the bone definitions.  
    ''' Added in WoW 2.0
    ''' </summary>
    ''' <returns></returns>
    Public Property unknown As Integer
    ''' <summary>
    ''' (Vec3D)
    ''' </summary>
    ''' <returns></returns>
    Public Property trans As AnimationBlock
    ''' <summary>
    ''' (QuatS)
    ''' </summary>
    ''' <returns></returns>
    Public Property rot As AnimationBlock
    ''' <summary>
    ''' (Vec3D)
    ''' </summary>
    ''' <returns></returns>
    Public Property scale As AnimationBlock

    ''' <summary>
    ''' 骨骼节点的坐标
    ''' </summary>
    ''' <returns></returns>
    Public Property pivot As Vector
End Class

Public Class AnimationBlock

    ''' <summary>
    ''' interpolation type (0=none, 1=linear, 2=hermite)
    ''' </summary>
    ''' <returns></returns>
    Public Property type As Integer
    ''' <summary>
    ''' global sequence id or -1
    ''' </summary>
    ''' <returns></returns>
    Public Property seq As Integer
    Public Property anims As anims
End Class

Public Class anims

    <XmlElement("anim")>
    Public Property anims As anim()
End Class

Public Class anim : Inherits List(Of anim_data)

    <XmlAttribute>
    Public Property id As Integer
    <XmlElement("data")>
    Public Property data As anim_data()

End Class

Public Class anim_data
    <XmlAttribute>
    Public Property time As Integer
    <XmlText>
    Public Property pivot As Vector
End Class

