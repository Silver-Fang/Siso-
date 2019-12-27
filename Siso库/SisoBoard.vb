Public Class SisoBoard
	ReadOnly siso_hal As SisoHal, fglib5 As FgLib5
	''' <summary>
	''' 从这里枚举所有的Applet信息，供用户选择，支持根据路径查找
	''' </summary>
	ReadOnly Property Applets As Applet列表
	ReadOnly Property 指针 As IntPtr
	Friend Sub New(指针 As IntPtr, siso_hal As SisoHal, fglib5 As FgLib5)
		Me.指针 = ZeroIntPtrException.Check(指针)
		Me.siso_hal = siso_hal
		Me.fglib5 = fglib5
		Dim b As New TypedIntPtr(Of IntPtr)(AddressOf Marshal.ReadIntPtr)
		Applets = New Applet列表(fglib5.Fg_getAppletIterator.Invoke(Index, FgAppletIteratorSource.FG_AIS_FILESYSTEM, b, 0), fglib5, b.值)
	End Sub
	ReadOnly Property Index As Byte
		Get
			Return siso_hal.SHalGetBoardIndex.Invoke(指针, 0)
		End Get
	End Property
	ReadOnly Property Info(信息名称 As String) As Integer
		Get
			Return siso_hal.SHalGetBoardInfoInt.Invoke(指针, 信息名称)
		End Get
	End Property
	ReadOnly Property InfoString(信息名称 As String) As String
		Get
			Return Marshal.PtrToStringAnsi(siso_hal.SHalGetBoardInfoString.Invoke(指针, 信息名称))
		End Get
	End Property
	Protected Overrides Sub Finalize()
		siso_hal.SHalFreeDevice.Invoke(指针)
	End Sub
	''' <summary>
	''' 加载用户选择的Applet，启动FrameGrabber
	''' </summary>
	''' <param name="Applet">用户选择的Applet</param>
	''' <returns>被启动的FrameGrabber</returns>
	Function InitFrameGrabber(Applet As Applet) As FrameGrabber
		Return New FrameGrabber(fglib5.Fg_Init.Invoke(Applet.StringProperty(FgAppletStringProperty.FG_AP_STRING_APPLET_PATH), Index), fglib5)
	End Function
End Class