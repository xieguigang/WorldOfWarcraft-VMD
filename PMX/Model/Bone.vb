Public Class Bone

    Public Property name As String
    Public Property enUS As String
    Public Property boneFlag As BoneFlags
    Public Property parentIndex As Integer
    Public Property connectedToIndex As Integer
    Public Property appendParent As Integer
    Public Property appendRatio As Single
    Public Property level As Integer
    Public Property local As vec3
    Public Property IK As IK
    Public Property IKtype As IKKinds

    Public Overrides Function ToString() As String
        Return name
    End Function

End Class

Public Structure IK
    Dim target As Integer
    Dim loops As Integer
    Dim angle As Single
    Dim linklist As List(Of Link)
End Structure

Public Structure Link
    Dim boneIndex As Integer
    Dim isLimited As Boolean
End Structure

<Flags> Public Enum BoneFlags
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