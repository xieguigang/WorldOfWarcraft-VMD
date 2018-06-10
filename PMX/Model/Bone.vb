Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON
Imports MikuMikuDance.Math3D

Namespace Model.BoneData

    Public Class Bone

        <XmlAttribute> Public Property name As String
        <XmlAttribute> Public Property enUS As String

        Public Property position As vec3
        Public Property boneFlag As BoneFlags
        <XmlAttribute> Public Property parentIndex As Integer
        Public Property connectedTo As connectTo
        <XmlAttribute> Public Property appendParent As Integer
        <XmlAttribute> Public Property appendRatio As Single
        Public Property axis As vec3
        <XmlAttribute> Public Property level As Integer
        Public Property localX As vec3
        Public Property localY As vec3
        Public Property localZ As vec3
        <XmlAttribute> Public Property extKey As Integer
        Public Property IK As IK
        Public Property IKtype As IKKinds

        Public Overrides Function ToString() As String
            Return $"{name} @ {position}"
        End Function

    End Class

    Public Structure connectTo
        <XmlAttribute>
        Dim toBoneIndex As Integer
        Dim toOffset As vec3
    End Structure

    Public Class bones : Inherits ListData(Of Bone)

        Public Overrides Function ToString() As String
            Return $"{size} bones"
        End Function
    End Class

    Public Structure IK

        <XmlAttribute> Dim target As Integer
        <XmlAttribute> Dim loops As Integer
        <XmlAttribute> Dim angle As Single

        Dim linklist As Link()

        Public Overrides Function ToString() As String
            If linklist Is Nothing Then
                Return "null"
            Else
                Dim linkTo$ = linklist _
                .Select(Function(l) l.boneIndex) _
                .ToArray _
                .GetJson

                Return $"{target} => {linkTo}"
            End If
        End Function
    End Structure

    Public Structure Link
        Dim boneIndex As Integer
        Dim isLimited As Boolean

        Dim low, high As vec3
    End Structure

    <Flags> Public Enum BoneFlags As UShort
        None = 0
        ToBone = 1
        Rotation = 2
        Translation = 4
        Visible = 8
        Enable = &H10
        IK = &H20
        AppendRotation = &H100
        AppendTranslation = &H200
        FixAxis = &H400
        LocalFrame = &H800
        AfterPhysics = &H1000
        ExtParent = &H2000
    End Enum

    Public Enum IKKinds
        None = 0
        IK = 1
        Target = 2
        Link = 3
    End Enum
End Namespace