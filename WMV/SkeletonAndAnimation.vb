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

Public Class Animations : Inherits List(Of Animation)

    <XmlElement("Animation")>
    Public Property Animations As Animation()
End Class

Public Class Animation

    <XmlAttribute>
    Public Property id As Integer

    Public Property animID As Integer
    Public Property animName As String
    Public Property length As Integer
    Public Property moveSpeed As Integer
    Public Property flags As Integer
    Public Property probability As Integer
    Public Property d1 As Integer
    Public Property d2 As Integer
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

Public Class Bones : Inherits List(Of Bone)

    <XmlElement("Bone")>
    Public Property Bones As Bone()
End Class

Public Class KeyBoneLookups : Inherits List(Of Lookup)

    <XmlElement("KeyBoneLookup")>
    Public Property KeyBoneLookups As Lookup()
End Class

Public Class BoneLookups : Inherits List(Of Lookup)

    <XmlElement("BoneLookup")>
    Public Property BoneLookups As Lookup()
End Class

Public Class Bone
    Public Property keyboneid As Integer
    Public Property billboard As Integer
    Public Property parent As Integer
    Public Property geoid As Integer
    Public Property unknown As Integer
    Public Property trans As transform
    Public Property rot As transform
    Public Property scale As transform

    ''' <summary>
    ''' 骨骼节点的坐标
    ''' </summary>
    ''' <returns></returns>
    Public Property pivot As Vector
End Class

Public Class transform
    Public Property type As Integer
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

