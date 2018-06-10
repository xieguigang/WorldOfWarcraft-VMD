![](Wiki-wordmark.png)

-----------------------------------

+ [PMX 2.0](PMX2.0.md)
+ [PMX 2.1](PMX2.1.md)

# VMD file format

> http://mikumikudance.wikia.com/wiki/VMD_file_format

The Vocaloid Motion Data (VMD) file format is the file format used to store animations for models used in the MikuMikuDance (Polygon Movie Maker) animation program.

Any fixed-length strings whose values are described as shorter than the actual length, assume to be padded with null byte (0).

## File header

The file begins with a 30-character "magic byte" sequence which can also be used to determine the version of the software used to create the file. The signature is "Vocaloid Motion Data file" if the VMD was created with MikuMikuDance 1.30 (prior to the "Multi-Model" version). It is "Vocaloid Motion Data 0002" if the VMD was created with later versions (Multi-Model Edition) of MikuMikuDance.

Following the magic bytes, there is a fixed-length string which tells the name of the model that this VMD is compatible with. This string is 10 bytes in the old version of VMD, and 20 bytes in the new version. If the name of the currently-loaded model does not match the name of the compatible model, the message "This motion file is the data for [current model name]. You can regist the motion only same bone name. Are you OK?" will appear. This means that it is possible to load the VMD, but only the bones described in the VMD whose names match bones in the currently-loaded model will be able to be registered.

## Key Frame List

Now we get to the bone keyframe list, which starts with a 4-byte unsigned int that tells how many keyframes are listed in the file. Note that the position coordinates are relative to the "bind pose", or the model's default pose. The position data of the bones in the PMD model are relative to the world's origin, and the position data here is relative to that. So, for example, the bind pose of a bone is (1, 2, 3) and the VMD gives (10, 25, 30). The final world position would be (11, 27, 33). Following the keyframe count, for each keyframe there is this structure:

| Data type                      | Value                                                                                                                                                                                    |
|:-------------------------------|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| char[15] (fixed length string) | A null-terminated string representing the name of the bone to which the transformation will be applied.                                                                                  |
| byte[4] (unsigned int)         | The frame number. Since keyframes are not necessarily stored for each actual frame, the animation software must interpolate between two adjacent keyframes with different frame indices. |
| byte[4] (float)                | X-coordinate of the bone position                                                                                                                                                        |
| byte[4] (float)                | Y-coordinate of the bone position                                                                                                                                                        |
| byte[4] (float)                | Z-coordinate of the bone position                                                                                                                                                        |
| byte[4] (float)                | X-coordinate of the bone rotation (quaternion)                                                                                                                                           |
| byte[4] (float)                | Y-coordinate of the bone rotation (quaternion)                                                                                                                                           |
| byte[4] (float)                | Z-coordinate of the bone rotation (quaternion)                                                                                                                                           |
| byte[4] (float)                | W-coordinate of the bone rotation (quaternion)                                                                                                                                           |
| byte[64]                       | 64 bytes of frame interpolation data. Editor note: Re:VB-P knows more about this than I do, so hopefully he will get around to describing it here...                                     |

After the bone keyframe list, there is the face keyframe list (special thanks to Re:VB-P for helping me understand this part!) Like the bone keyframe list, it begins with a 4-byte unsigned int that tells how many keyframes are listed. Then, for each keyframe:

| Data type                      | Value                                                                                                                                                                                                |
|:-------------------------------|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| char[15] (fixed length string) | A null-terminated string representing the name of the face to which the transformation will be applied.                                                                                              |
| byte[4] (unsigned int)         | The index of the frame.                                                                                                                                                                              |
| byte[4] (float)                | Weight - this value is on a scale of 0.0-1.0. It is used to scale how much a face morph should move a vertex based off of the maximum possible coordinate that it can move by (specified in the PMD) |

Following the face keyframe list is the camera keyframe list. Similar to the other two lists, it begins with a 4-byte unsigned int that tells how many keyframes are listed. Then, for each keyframe:

| Data type              | Value                                                                                       |
|:-----------------------|:--------------------------------------------------------------------------------------------|
| byte[4] (unsigned int) | The index of the frame.                                                                     |
| byte[4] (float)        | Length. Editor note: I am not certain what this value does.                                 |
| byte[4] (float)        | X-coordinate of camera position                                                             |
| byte[4] (float)        | Y-coordinate of camera position                                                             |
| byte[4] (float)        | Z-coordinate of camera position                                                             |
| byte[4] (float)        | X-coordinate of camera rotation                                                             |
| byte[4] (float)        | Y-coordinate of camera rotation                                                             |
| byte[4] (float)        | Z-coordinate of camera rotation                                                             |
| byte[24]               | 24 bytes of interpolation data. Editor note: I am uncertain what kind of data this entails. |
| byte[4] (unsigned int) | Camera FOV angle                                                                            |
| byte[1]                | Perspective. Editor note: I believe this is to toggle a perspective camera.                 |
