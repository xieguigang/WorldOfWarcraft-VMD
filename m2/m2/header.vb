
Namespace m2

    ''' <summary>
    ''' WOW ``*.m2`` file header
    ''' </summary>
    Public Class header

        Public Property nameLength As Integer
        Public Property nameOfs As Integer
        Public Property name As String

        ''' <summary>
        ''' 1: tilt x, 2: tilt y, 4:, 8: add another field in header, 16: ; (no other flags as of 3.1.1);
        ''' </summary>
        ''' <returns></returns>
        Public Property GlobalModelFlags As Integer
        ''' <summary>
        ''' AnimationRelated
        ''' </summary>
        ''' <returns></returns>
        Public Property nGlobalSequences As Integer
        ''' <summary>
        ''' A list of timestamps.
        ''' </summary>
        ''' <returns></returns>
        Public Property ofsGlobalSequences As Integer
        ''' <summary>
        ''' AnimationRelated
        ''' </summary>
        ''' <returns></returns>
        Public Property nAnimations As Integer
        ''' <summary>
        ''' Information about the animations in the model.
        ''' </summary>
        ''' <returns></returns>
        Public Property ofsAnimations As Integer
        ''' <summary>
        ''' AnimationRelated
        ''' </summary>
        ''' <returns></returns>
        Public Property nAnimationLookup As Integer
        ''' <summary>
        ''' Mapping of global IDs to the entries in the Animation sequences block.
        ''' </summary>
        ''' <returns></returns>
        Public Property ofsAnimationLookup As Integer
        Public Property nBones As Integer
        Public Property ofsBones As Integer
        Public Property nKeyBoneLookup As Integer
        Public Property ofsKeyBoneLookup As Integer
        Public Property nVertices As Integer
        Public Property ofsVertices As Integer
        Public Property nViews As Integer
        Public Property lodname As String
        Public Property nColors As Integer
        Public Property ofsColors As Integer
        Public Property nTextures As Integer
        Public Property ofsTextures As Integer
        Public Property nTransparency As Integer
        Public Property ofsTransparency As Integer
        Public Property nTexAnims As Integer
        Public Property ofsTexAnims As Integer
        Public Property nTexReplace As Integer
        Public Property ofsTexReplace As Integer
        Public Property nTexFlags As Integer
        Public Property ofsTexFlags As Integer
        Public Property nBoneLookup As Integer
        Public Property ofsBoneLookup As Integer
        Public Property nTexLookup As Integer
        Public Property ofsTexLookup As Integer
        Public Property nTexUnitLookup As Integer
        Public Property ofsTexUnitLookup As Integer
        Public Property nTransparencyLookup As Integer
        Public Property ofsTransparencyLookup As Integer
        Public Property nTexAnimLookup As Integer
        Public Property ofsTexAnimLookup As Integer
        Public Property collisionSphere As Sphere
        Public Property boundSphere As Sphere
        Public Property nBoundingTriangles As Integer
        Public Property ofsBoundingTriangles As Integer
        Public Property nBoundingVertices As Integer
        Public Property ofsBoundingVertices As Integer
        Public Property nBoundingNormals As Integer
        Public Property ofsBoundingNormals As Integer
        Public Property nAttachments As Integer
        Public Property ofsAttachments As Integer
        Public Property nAttachLookup As Integer
        Public Property ofsAttachLookup As Integer
        Public Property nEvents As Integer
        Public Property ofsEvents As Integer
        Public Property nLights As Integer
        Public Property ofsLights As Integer
        Public Property nCameras As Integer
        Public Property ofsCameras As Integer
        Public Property nCameraLookup As Integer
        Public Property ofsCameraLookup As Integer
        Public Property nRibbonEmitters As Integer
        Public Property ofsRibbonEmitters As Integer
        Public Property nParticleEmitters As Integer
        Public Property ofsParticleEmitters As Integer
    End Class

    Public Class Sphere
        Public Property min As String
        Public Property max As String
        Public Property radius As Double
    End Class
End Namespace