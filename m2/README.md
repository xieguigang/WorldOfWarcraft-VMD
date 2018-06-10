在此，给出WotLK的头文件定义

```cpp
    char id[4];
    uint8 version[4];
    uint32 nameLength;
    uint32 nameOfs;
    uint32 GlobalModelFlags; // 1: tilt x, 2: tilt y, 4:, 8: add another field in header, 16: ; (no other flags as of 3.1.1);
    uint32 nGlobalSequences; // AnimationRelated
    uint32 ofsGlobalSequences; // A list of timestamps.
    uint32 nAnimations; // AnimationRelated
    uint32 ofsAnimations; // Information about the animations in the model.
    uint32 nAnimationLookup; // AnimationRelated
    uint32 ofsAnimationLookup; // Mapping of global IDs to the entries in the Animation sequences block.
    //uint32 nD;
    //uint32 ofsD;
    uint32 nBones; // BonesAndLookups
    uint32 ofsBones; // Information about the bones in this model.
    uint32 nKeyBoneLookup; // BonesAndLookups
    uint32 ofsKeyBoneLookup; // Lookup table for key skeletal bones.

    uint32 nVertices; // GeometryAndRendering
    uint32 ofsVertices; // Vertices of the model.
    uint32 nViews; // GeometryAndRendering
    //uint32 ofsViews; // Views (LOD) are now in .skins.

    uint32 nColors; // ColorsAndTransparency
    uint32 ofsColors; // Color definitions.

    uint32 nTextures; // TextureAndTheifAnimation
    uint32 ofsTextures; // Textures of this model.

    uint32 nTransparency; // H,  ColorsAndTransparency
    uint32 ofsTransparency; // Transparency of textures.
    //uint32 nI;   // always unused ?
    //uint32 ofsI;
    uint32 nTexAnims;    // J, TextureAndTheifAnimation
    uint32 ofsTexAnims;
    uint32 nTexReplace; // TextureAndTheifAnimation
    uint32 ofsTexReplace; // Replaceable Textures.

    uint32 nTexFlags; // Render Flags
    uint32 ofsTexFlags; // Blending modes / render flags.
    uint32 nBoneLookup; // BonesAndLookups
    uint32 ofsBoneLookup; // A bone lookup table.

    uint32 nTexLookup; // TextureAndTheifAnimation
    uint32 ofsTexLookup; // The same for textures.

    uint32 nTexUnitLookup;        // L, TextureAndTheifAnimation, seems gone after Cataclysm
    uint32 ofsTexUnitLookup; // And texture units. Somewhere they have to be too.
    uint32 nTransparencyLookup; // M, ColorsAndTransparency
    uint32 ofsTransparencyLookup; // Everything needs its lookup. Here are the transparencies.
    uint32 nTexAnimLookup; // TextureAndTheifAnimation
    uint32 ofsTexAnimLookup; // Wait. Do we have animated Textures? Wasn't ofsTexAnims deleted? oO

    Sphere collisionSphere;
    Sphere boundSphere;

    uint32 nBoundingTriangles; // Miscellaneous
    uint32 ofsBoundingTriangles;
    uint32 nBoundingVertices; // Miscellaneous
    uint32 ofsBoundingVertices;
    uint32 nBoundingNormals; // Miscellaneous
    uint32 ofsBoundingNormals;

    uint32 nAttachments; // O, Miscellaneous
    uint32 ofsAttachments; // Attachments are for weapons etc.
    uint32 nAttachLookup; // P, Miscellaneous
    uint32 ofsAttachLookup; // Of course with a lookup.
    uint32 nEvents; //
    uint32 ofsEvents; // Used for playing sounds when dying and a lot else.
    uint32 nLights; // R
    uint32 ofsLights; // Lights are mainly used in loginscreens but in wands and some doodads too.
    uint32 nCameras; // S, Miscellaneous
    uint32 ofsCameras; // The cameras are present in most models for having a model in the Character-Tab.
    uint32 nCameraLookup; // Miscellaneous
    uint32 ofsCameraLookup; // And lookup-time again, unit16
    uint32 nRibbonEmitters; // U, Effects
    uint32 ofsRibbonEmitters; // Things swirling around. See the CoT-entrance for light-trails.
    uint32 nParticleEmitters; // V, Effects
    uint32 ofsParticleEmitters; // Spells and weapons, doodads and loginscreens use them. Blood dripping of a blade?

Particles.
};
```

在说明之前，有几个约定需要讲解一下，以便简单。 结构体中的 nXXXXX表示，有多少个这样的数据单元。而ofsXXXXXX表示，在哪里读取这个数据。  而每一个数据单元具体的大小和信息，则需要由额外的地方来定义。在解释的时候，就不对nXXXXX和ofsXXXX多作解释了。

下面逐一说明各变量的作用的含意

> + id：  必然是 'M' 'D' '2' '0'
> + version：  用来检查文件版本的。 可以是以下值
   ```c
     // 10 1 0 0 = WoW 5.0 models (as of 15464)
    // 10 1 0 0 = WoW 4.0.0.12319 models
    // 9 1 0 0 = WoW 4.0 models
    // 8 1 0 0 = WoW 3.0 models
    // 4 1 0 0 = WoW 2.0 models
    // 0 1 0 0 = WoW 1.0 models
    ```
> + nameLength和nameOfs 在WMV中，除了看到拿来检测数据合法性外，没有看到拿来读取数据的地方
> + GlobalModelFlags 模型的全局标志位，在WMV中除了看到用于输出外，没有看到有其它地方使用
> + nGlobalSequences和ofsGlobalSequences 一个全局数据序列，数据单元类型为UINT32
> + nAnimations和ofsAnimations 动画数据信息，数据单元类型由ModelAnimation定义，此定义在WMV中如下。

```cpp
struct ModelAnimation
{
    uint32 animID; // AnimationDataDB.ID
    uint32 timeStart;
    uint32 timeEnd;

    float moveSpeed;

    uint32 flags;
    uint16 probability;
    uint16 unused;
    uint32 d1;
    uint32 d2;
    uint32 playSpeed;  // note: this can't be play speed because it's 0 for some models

    Sphere boundSphere;

    int16 NextAnimation;
    int16 Index;
};
```

它主要是定义一个动画的相关参数，比如ID，开始结束时间等等。

+ nAnimationLookup，动画数据查看表，主要是给外部提供一个查询的便利性，数据单元类型为UINT16
+ nBones，ofsBones 骨骼数据，数据单元类型为ModelBoneDef 其定义大致如下

```cpp
struct ModelBoneDef {
    int32 keyboneid; // Back-reference to the key bone lookup table. -1 if this is no key bone.
    int32 flags; // Only known flags: 8 - billboarded and 512 - transformed
    int16 parent; // parent bone index
    int16 geoid; // A geoset for this bone.
    int32 unknown; // new int added to the bone definitions.  Added in WoW 2.0
    AnimationBlock translation; // (Vec3D)
    AnimationBlock rotation; // (QuatS)
    AnimationBlock scaling; // (Vec3D)
    Vec3D pivot;
};
```

可以看出，每个骨头都有一个ID，以及一些标志位，同时记录了其父骨骼的索引。 而骨骼本身，则有平移，旋转，缩放和锚点等数据。

+ nKeyBoneLookup也是一个提供快速查询的数据。 M2中很多对应的信息，都提供了这样的LOOK UP TABLE。 典型的以空间换时间的做法。

nVertices，ofsVertices 顶点信息，其数据单元定义如下

```cpp
struct ModelVertex
{
    Vec3D pos;
    uint8 weights[4];
    uint8 bones[4];
    Vec3D normal;
    Vec2D texcoords;
    int unk1, unk2; // always 0,0 so this is probably unused
};
```

每一个顶点数据，有一个位置信息，4个骨骼索引和对应的权重 （其实貌似权重存3个就可以了。） 法线（法线貌似也只存两个FLOAT就可以了。） 纹理坐标 以及两个没有摸索出用途的INT。  值得注意的是，WOW中的坐标用的是Z向上，Y向里的坐标。 如果要将WOW中的坐标转换到左手坐标系（D3D默认）中。  则 X0,Y0,Z0 = X,Z,Y  若转换成右手坐标系（OPENGL默认） 则 X0，Y0,Z0 = X,Z,-Y.  这个在前面分析数据的时候有说过。  因为在WMV中，就有转换坐标系相关的操作。

nViews， 此值表示模型有多少个LOD数据。 在WotLK版本以后，LOD数据全部被放入了 *.skin文件中。 不再在M2文件中读取。
假设一个模型为 ooxx.m2  那其对应的LOD文件信息可以为 ooxx00.skin  ooxx01.skin ooxx02.skin ooxx03.skin，而此M2模型的具体子

模型划分等细节，都在skin文件中了。

nColors，ofsColors 此模型用到的颜色序列，用于实现模型动态变色效果 其数据单元定义为

```cpp
struct ModelColorDef {
    AnimationBlock color; // (Vec3D) Three floats. One for each color.
    AnimationBlock opacity; // (UInt16) 0 - transparent, 0x7FFF - opaque.
};

struct AnimationBlock {
    int16 type;        // interpolation type (0=none, 1=linear, 2=hermite)
    int16 seq;        // global sequence id or -1
    uint32 nRanges;
    uint32 ofsRanges;
    uint32 nTimes;
    uint32 ofsTimes;
    uint32 nKeys;
    uint32 ofsKeys;
};
```

nTextures，ofsTextures定义了此模型用到的纹理序列，其结构定义如下

```cpp
#define    TEXTURE_MAX    32
struct ModelTextureDef
{
    uint32 type;
    uint32 flags;
    uint32 nameLen;
    uint32 nameOfs;
};
```

关于纹理相关的内容，得专门有一篇文章讲解一下才行。这个内容有点多，但是思路却很清楚清晰

nTransparency，ofsTransparency用于实现透明变化效果，其读取结构定义如下

```cpp
struct ModelTransDef
{
    AnimationBlock trans; // (UInt16)
};
```

AnimationBlock的定义上面已经给出

nTexAnims 纹理动画，结构体定义如下

```cpp
struct ModelTexAnimDef {
    AnimationBlock trans; // (Vec3D)
    AnimationBlock rot; // (QuatS)
    AnimationBlock scale; // (Vec3D)
};
```

这个表示在不同的情况下，纹理矩阵作用的效果，一些爆布，火盆上的火焰或者流动的岩浆就是通过这个实现的。

+ nTexReplace 字面上是可替换的纹理，在WMV中没有发现具体的用法。
+ nTexFlags 纹理标记位，在WMV中没有发现具体用法
+ nBoneLookup 骨骼查询表，在WMV中，除了拿来显示以外，没有看到特别的作用。
+ nTexLookup 纹理查询表，用于快速定位一个nTextures中读出来的纹理。
+ nTexUnitLookup 纹理单元查询表，和上面的功能类似，貌似CTM版本就没有使用到了。
+ nTransparencyLookup 透明信息查询表
+ nTexAnimLookup 纹理信息查询表
+ collisionSphere 碰撞球
+ boundSphere 包围球
+ nBoundingTriangles 构成包围网格的三角形数据 每个数据单元是UINT16
+ nBoundingVertices 构成包围网格的顶点数据 每个数据单元是Vec3D，即三个FLOAT
+ nBoundingNormals 构成包围网格的法线数据 数据同上
+ nAttachments挂接点的信息  每个挂接点的信息定义如下

```cpp
struct ModelAttachmentDef
{
    uint32 id; // Just an id. Is referenced in the enum POSITION_SLOTS.
    uint32 bone; // Somewhere it has to be attached.
    Vec3D pos; // Relative to that bone of course.
    AnimationBlock unk; // (Int32) Its an integer in the data. It has been 1 on all models I saw. Whatever.
};
```

+ nAttachLookup 挂接点查询表，用于快速定位某个挂接点
+ nEvents 动画播放时的事件触发，用于完成一些特殊的，比如音效的播放，攻击方与受击方的动画吻合等。 定义如下

```cpp
struct ModelEventDef
{
    char id[4]; // This is a (actually 3 character) name for the event with a $ in front.
    int32 dbid; // This data is passed when the event is fired.
    int32 bone; // Somewhere it has to be attached.
    Vec3D pos; // Relative to that bone of course.
    int16 type; // This is some fake-AnimationBlock.
    int16 seq; // Built up like a real one but without timestamps(?). What the fuck?
    uint32 nTimes; // See the documentation on AnimationBlocks at this topic.
    uint32 ofsTimes; // This points to a list of timestamps for each animation given.
};
```

关于ID的值，WMV中列出了一些摸索到的。

```cpp

/*
There are a lot more of them. I did not list all up to now.
ID     Data     Description
DEST         exploding ballista, that one has a really fucked up block. Oo
POIN     unk     something alliance gunship related (flying in icecrown)
WHEE     601+     Used on wheels at vehicles.
$tsp         p is {0 to 3} (position); t is {W, S, B, F (feet) or R} (type); s is {R or L} (right or left); this is

used when running through snow for example.
$AHx         UnitCombat_C, x is {0 to 3}
$BRT         Plays some sound.
$BTH         Used for bubbles or breath. ("In front of head")
$BWP         UnitCombat_C
$BWR         Something with bow and rifle. Used in AttackRifle, AttackBow etc. "shoot now"?
$CAH         UnitCombat_C
$Cxx         UnitCombat_C, x is {P or S}
$CSD    SoundEntries.dbc     Emote sounds?
$CVS    SoundEntriesAdvanced.dbc     Sound
$DSE       
$DSL    SoundEntries.dbc     Sound with something special. Use another one if you always want to have it playing..
$DSO    SoundEntries.dbc     Sound
$DTH         UnitCombat_C, death, this plays death sounds and more.
$EMV         MapLoad.cpp
$ESD         Plays some emote sound.
$EWT         MapLoad.cpp
$FDx         x is {1 to 5}. Calls some function in the Object VMT. Also plays some sound.
$FDx         x is {6 to 9}. Calls some function in the Object VMT.
$FDX         Should do nothing. But is existant.
$FSD         Plays some sound.
$GCx         Play gameobject custom sound referenced in GameObjectDisplayInfo.dbc. x can be from {0 to 3}: {Custom0,

Custom1, Custom2, Custom3}
$GOx         Play gameobject sound referenced in GameObjectDisplayInfo.dbc. x can be from {0 to 5}: {Stand, Open,

Loop, Close, Destroy, Opened}
$HIT         Get hit?
$KVS         MapLoad.cpp
$SCD         Plays some sound.
$SHK    SpellEffectCameraShakes.dbc     Add a camera shake
$SHx         x is {L or R}, fired on Sheath and SheathHip. "Left/right shoulder" was in the old list.
$SMD         Plays some sound.
$SMG         Plays some sound.
$SND    SoundEntries.dbc     Sound
$TRD         Does something with a spell, a sound and a spellvisual.
$VGx         UnitVehicle_C, x is {0 to 8}
$VTx         UnitVehicle_C, x is {0 to 8}
$WxG         x is {W or N}. Calls some function in the Object VMT.
-------     ----------------------------------     - Old documentation (?) ----------------------------------------------
$CSx         x is {L or R} ("Left/right hand") (?)
$CFM       
$CHD         ("Head") (?)
$CCH         ("Bust") (?)
$TRD         ("Crotch") (?)
$CCH         ("Bust") (?)
$BWR         ("Right hand") (?)
$CAH       
$CST
*/
```

nLights 光照信息，标记了，模型的哪个骨骼上，挂接了灯光。 结构定义如下

```cpp
struct ModelLightDef {
    int16 type; // 0: Directional, 1: Point light
    int16 bone; // If its attached to a bone, this is the bone. Else here is a nice -1.
    Vec3D pos; // Position, Where is this light?
    AnimationBlock ambientColor; // (Vec3D) The ambient color. Three floats for RGB.
    AnimationBlock ambientIntensity; // (Float) A float for the intensity.
    AnimationBlock diffuseColor; // (Vec3D) The diffuse color. Three floats for RGB.
    AnimationBlock diffuseIntensity; // (Float) A float for the intensity again.
    AnimationBlock attenuationStart; // (Float) This defines, where the light starts to be.
    AnimationBlock attenuationEnd; // (Float) And where it stops.
    AnimationBlock useAttenuation; // (Uint32) Its an integer and usually 1.
};
```

nCameras 真的不是太懂这个。 结构体定义如下

```cpp
struct ModelCameraDef {
    int32 id; // 0 is potrait camera, 1 characterinfo camera; -1 if none; referenced in CamLookup_Table
    float fov; // No radians, no degrees. Multiply by 35 to get degrees.
    float farclip; // Where it stops to be drawn.
    float nearclip; // Far and near. Both of them.
    AnimationBlock transPos; // (Vec3D) How the cameras position moves. Should be 3*3 floats. (? WoW parses 36 bytes = 3*3*sizeof(float))
    Vec3D pos; // float, Where the camera is located.
    AnimationBlock transTarget; // (Vec3D) How the target moves. Should be 3*3 floats. (?)
    Vec3D target; // float, Where the camera points to.
    AnimationBlock rot; // (Quat) The camera can have some roll-effect. Its 0 to 2*Pi.
};
```

nCameraLookup 摄相机信息查询表

nRibbonEmitters 此模型身上的多边形轨迹（缎带）效果数目。 结构体定义如下

```cpp
struct ModelRibbonEmitterDef {
    int32 id;
    int32 bone;
    Vec3D pos;
    int32 nTextures;
    int32 ofsTextures;
    int32 nUnknown;
    int32 ofsUnknown;
    AnimationBlock color; // (Vec3D)
    AnimationBlock opacity; // (UInt16) And an alpha value in a short, where: 0 - transparent, 0x7FFF - opaque.
    AnimationBlock above; // (Float) The height above.
    AnimationBlock below; // (Float) The height below. Do not set these to the same!
    float res; // This defines how smooth the ribbon is. A low value may produce a lot of edges.
    float length; // The length aka Lifespan.
    float Emissionangle; // use arcsin(val) to get the angle in degree
    int16 s1, s2;
    AnimationBlock unk1; // (short)
    AnimationBlock unk2; // (boolean)
    int32 unknown; // This looks much like just some Padding to the fill up the 0x10 Bytes, always 0
};
```

最后一个值unknown是WotLK版本后新增的，不知道拿来干什么。 但可以肯定，WLK版本，加强了这个效果类型的表现力。

nParticleEmitters 粒子系统，结构体定义如下。

```cpp
struct ModelParticleEmitterDefV10
{
    int32 id;
    int32 flags;
    Vec3D pos; // The position. Relative to the following bone.
    int16 bone; // The bone its attached to.
    int16 texture; // And the texture that is used.
    int32 nModelFileName;
    int32 ofsModelFileName;
    int32 nParticleFileName;
    int32 ofsParticleFileName; // TODO
    int8 blend;
    int8 EmitterType; // EmitterType     1 - Plane (rectangle), 2 - Sphere, 3 - Spline? (can't be bothered to find one)
    int16 ParticleColor; // This one is used so you can assign a color to specific particles. They loop over all
                         // particles and compare +0x2A to 11, 12 and 13. If that matches, the colors from the dbc get applied.
    int8 ParticleType; // 0 "normal" particle,
                       // 1 large quad from the particle's origin to its position (used in Moonwell water effects)
                       // 2 seems to be the same as 0 (found some in the Deeprun Tram blinky-lights-sign thing)
    int8 HeaderTail; // 0 - Head, 1 - Tail, 2 - Both
    int16 TextureTileRotation; // TODO, Rotation for the texture tile. (Values: -1,0,1)
    int16 cols; // How many different frames are on that texture? People should learn what rows and cols are.
    int16 rows; // (2, 2) means slice texture to 2*2 pieces
    AnimationBlock EmissionSpeed; // (Float) All of the following blocks should be floats.
    AnimationBlock SpeedVariation; // (Float) Variation in the flying-speed. (range: 0 to 1)
    AnimationBlock VerticalRange; // (Float) Drifting away vertically. (range: 0 to pi)
    AnimationBlock HorizontalRange; // (Float) They can do it horizontally too! (range: 0 to 2*pi)
    AnimationBlock Gravity; // (Float) Fall down, apple!
    AnimationBlock Lifespan; // (Float) Everyone has to die.
    int32 unknown;
    AnimationBlock EmissionRate; // (Float) Stread your particles, emitter.
    int32 unknown2;
    AnimationBlock EmissionAreaLength; // (Float) Well, you can do that in this area.
    AnimationBlock EmissionAreaWidth; // (Float)
    AnimationBlock Gravity2; // (Float) A second gravity? Its strong.
    ModelParticleParams p;
    AnimationBlock en; // (UInt16), seems unused in cataclysm
    int32 unknown3; // 12319, cataclysm
    int32 unknown4; // 12319, cataclysm
    int32 unknown5; // 12319, cataclysm
    int32 unknown6; // 12319, cataclysm
};
```