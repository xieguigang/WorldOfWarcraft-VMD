Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Imaging.Drawing3D.Device
Imports Microsoft.VisualBasic.Imaging.Drawing3D.Math3D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Parallel.Tasks
Imports MikuMikuDance.File.PMX.Model
Imports WorldOfWarcraft.Plugins.WMV.ogre

Public Class SkeletonCanvas

    Dim frameThread As New UpdateThread(300, AddressOf updateAction)
    Dim _camera As New Camera With {
        .angleX = 0,
        .angleY = 0,
        .angleZ = 0,
        .fov = 256,
        .screen = Size,
        .ViewDistance = -40
    }

    ''' <summary>
    ''' MMD模型会转换为ogre模型进行显示
    ''' </summary>
    Dim skeleton As skeleton
    Dim skeletonLine As New Pen(Drawing.Color.Black, 5) With {
        .DashStyle = DashStyle.Solid,
        .EndCap = LineCap.ArrowAnchor
    }
    Dim mouse As Mouse

    Public Property NodeRadius As Single = 5

    Private Sub updateAction()
        Static oldCamera As New Camera

        'If oldCamera.angleX <> _camera.angleX OrElse
        '   oldCamera.angleY <> _camera.angleY OrElse
        '   oldCamera.angleZ <> _camera.angleZ OrElse
        '   oldCamera.fov <> _camera.fov OrElse
        '   oldCamera.ViewDistance <> _camera.ViewDistance Then

        '    oldCamera.angleX = _camera.angleX
        '    oldCamera.angleY = _camera.angleY
        '    oldCamera.angleZ = _camera.angleZ
        '    oldCamera.fov = _camera.fov
        '    oldCamera.ViewDistance = _camera.ViewDistance

        ' Call $"skeleton of {Name} is nothing? {skeleton Is Nothing}".__DEBUG_ECHO

        Call Me.Invalidate()
        'End If
    End Sub

    Private Sub SkeletonCanvas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mouse = New Mouse(Me, _camera)

        Call frameThread.Start()
    End Sub

    Public Sub Run()
        Call frameThread.Start()
    End Sub

    Public Overloads Sub LoadModel(wow As skeleton)
        skeleton = wow
        skeleton.bones = skeleton.bones _
            .OrderBy(Function(b) b.id) _
            .ToArray
    End Sub

    Public Overloads Sub LoadModel(mmd As PMXFile)
        Call LoadModel(mmd.AsOgre)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If skeleton Is Nothing Then
            Return
        End If

        ' 得到骨骼点
        Dim points As Point3D() = skeleton.bones _
            .Select(Function(b)
                        Return New Point3D(b.position.x, b.position.y, b.position.z)
                    End Function) _
            .ToArray

        ' 对骨骼点进行旋转与投影
        points = New Vector3D(points) _
            .RotateX(_camera.angleX) _
            .RotateY(_camera.angleY) _
            .RotateZ(_camera.angleZ) _
            .Project(Width, Height, _camera.fov, _camera.ViewDistance) _
            .ToArray

        ' 进行绘制顺序的计算
        Dim zOrder = points.OrderProvider(Function(p) p.Z)
        Dim size As Size = Me.Size
        Dim point2D() = New PointF(points.Length - 1) {}
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle

        ' 绘制骨骼节点
        For Each i As Integer In zOrder
            point2D(i) = points(i).PointXY(size)
            rect = New Rectangle With {
                .Width = NodeRadius * 2,
                .Height = .Width,
                .X = point2D(i).X - NodeRadius,
                .Y = point2D(i).Y - NodeRadius
            }
            g.FillPie(Brushes.Black, rect, 0, 360)
        Next

        ' 绘制骨骼连线
        For Each link In skeleton.bonehierarchy.SafeQuery
            Dim a = point2D(link.bone)
            Dim b = point2D(link.parent)

            Call g.DrawLine(skeletonLine, b, a)
        Next

        ' 为了防止被节点和线给覆盖掉
        ' 标签在最后进行绘制
        For Each i As Integer In zOrder
            g.DrawString(skeleton.bones(i).name, Font, Brushes.White, point2D(i))
        Next
    End Sub

    Private Sub SkeletonCanvas_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        _camera.ViewDistance += Math.Sign(e.Delta) / 4
        Console.WriteLine(_camera.ViewDistance)
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        _camera.angleX = 0
        _camera.angleY = 0
        _camera.angleZ = 0
    End Sub
End Class
