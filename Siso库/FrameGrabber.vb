''' <summary>
''' FrameGrabber包含两个CameraPort，操作主要在CameraPort中进行
''' </summary>
Public Class FrameGrabber
	ReadOnly fglib5 As FgLib5
	ReadOnly Property CameraPortA As CameraPort
	ReadOnly Property CameraPortB As CameraPort
	ReadOnly Property 指针 As IntPtr
	Friend Sub New(指针 As IntPtr, fglib5 As FgLib5)
		Me.指针 = ZeroIntPtrException.Check(指针)
		Me.fglib5 = fglib5
		CameraPortA = New CameraPort(Me, DmaIndex.CameraPortA, fglib5)
		CameraPortB = New CameraPort(Me, DmaIndex.CameraPortB, fglib5)
	End Sub
	Protected Overrides Sub Finalize()
		fglib5.Fg_FreeGrabber.Invoke(指针)
	End Sub
End Class
