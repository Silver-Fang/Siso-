''' <summary>
''' 每张抓取到的图片如何处理？
''' </summary>
''' <param name="PicNr">帧编号</param>
''' <param name="ImagePtr">图像指针</param>
''' <param name="Timestamp">时间戳</param>
Public Delegate Sub ImageAcquiredHandler(PicNr As Frameindex_t, ImagePtr As IntPtr, Timestamp As Long)
Public Class CameraPort
	''' <summary>
	''' 这里保存对象而不是指针，避免意外的Finalize。需要手动分配内存。
	''' </summary>
	ReadOnly Fg As FrameGrabber
	ReadOnly DmaIndex As DmaIndex, fglib5 As FgLib5
	Private 内存头 As IntPtr = IntPtr.Zero, DmaBuffer As IntPtr(), Acquiring As Boolean = False, AcquireTask As Task
	Event 溢出 As Fg_EventFunc
	''' <summary>
	''' 处理此事件以获取抓取到的图像
	''' </summary>
	Event ImageAcquired As ImageAcquiredHandler
	''' <summary>
	''' 判断抓取是否正在进行
	''' </summary>
	ReadOnly Property 抓取中 As Boolean
		Get
			Return Acquiring
		End Get
	End Property
	''' <summary>
	''' 如果没有分配内存，返回0
	''' </summary>
	''' <returns>内存中可存储的图片帧数</returns>
	ReadOnly Property 缓冲区大小 As UShort
		Get
			If 内存头 = IntPtr.Zero Then
				Return 0
			Else
				Return DmaBuffer.Length
			End If
		End Get
	End Property
	Private Function Overflow(events As ULong, data As IntPtr, info As IntPtr) As ErrorCodes
		RaiseEvent 溢出(events, data, info)
		Return ErrorCodes.FG_OK
	End Function
	Friend Sub New(Fg As FrameGrabber, DmaIndex As DmaIndex, fglib5 As FgLib5)
		Me.Fg = Fg
		Me.DmaIndex = DmaIndex
		Me.fglib5 = fglib5
		Dim a As ULong = fglib5.Fg_getEventMask.Invoke(Fg.指针, "FG_OVERFLOW_CAM" & DmaIndex)
		CodedException.Attempt(fglib5.Fg_clearEvents.Invoke(Fg.指针, a))
		CodedException.Attempt(fglib5.Fg_registerEventCallback.Invoke(Fg.指针, a, AddressOf Overflow, IntPtr.Zero, FgEventControlFlags.FG_EVENT_DEFAULT_FLAGS, IntPtr.Zero))
		CodedException.Attempt(fglib5.Fg_activateEvents.Invoke(Fg.指针, a, True))
	End Sub
	''' <summary>
	''' 获取一个参数
	''' </summary>
	''' <typeparam name="T">参数的输出类型</typeparam>
	''' <param name="ID">哪个参数</param>
	''' <param name="指针">保存参数的指针</param>
	''' <returns>参数值</returns>
	Function GetParameter(Of T)(ID As ParameterID, 指针 As TypedIntPtr(Of T)) As T
		CodedException.Attempt(fglib5.Fg_getParameter.Invoke(Fg.指针, ID, 指针, DmaIndex))
		Return 指针.值
	End Function
	''' <summary>
	''' 如果用这个函数获取字符串参数，需要使用FreeParameterStringWithType手动释放
	''' </summary>
	''' <typeparam name="T">参数的输出类型</typeparam>
	''' <param name="ID">哪个参数</param>
	''' <param name="指针">保存参数的指针</param>
	''' <param name="类型">参数类型</param>
	''' <returns>参数值</returns>
	Function GetParameterWithType(Of T)(ID As ParameterID, 指针 As TypedIntPtr(Of T), 类型 As FgParamTypes) As T
		CodedException.Attempt(fglib5.Fg_getParameterWithType.Invoke(Fg.指针, ID, 指针, DmaIndex, 类型))
		Return 指针.值
	End Function
	''' <summary>
	''' 释放GetParameterWithType获取的字符串参数
	''' </summary>
	''' <param name="ID"></param>
	''' <param name="指针"></param>
	''' <param name="类型"></param>
	Sub FreeParameterStringWithType(ID As ParameterID, 指针 As IntPtr, 类型 As FgParamTypes)
		CodedException.Attempt(fglib5.Fg_freeParameterStringWithType.Invoke(Fg.指针, ID, 指针, DmaIndex, 类型))
	End Sub
	ReadOnly Property Width As UShort
		Get
			Return GetParameter(ParameterID.FG_WIDTH, New TypedIntPtr(Of Short)(TypedIntPtr.Offset6ShortReader))
		End Get
	End Property
	ReadOnly Property Height As UShort
		Get
			Return GetParameter(ParameterID.FG_HEIGHT, New TypedIntPtr(Of Short)(TypedIntPtr.Offset6ShortReader))
		End Get
	End Property
	ReadOnly Property Format As ImageFormat
		Get
			Return GetParameter(ParameterID.FG_FORMAT, New TypedIntPtr(Of Byte)(TypedIntPtr.Offset7ByteReader))
		End Get
	End Property
	ReadOnly Property 像素位数 As Byte
		Get
			Static 像素位 As New Dictionary(Of ImageFormat, Byte) From {{ImageFormat.FG_GRAY16, 16}, {ImageFormat.FG_GRAY, 8}}
			Return 像素位(Format)
		End Get
	End Property
	''' <summary>
	''' 该对象支持自动释放内存，一般不需要手动释放。即使没有分配过内存，释放也不会出错。
	''' </summary>
	Sub 释放内存()
		If 内存头 <> IntPtr.Zero Then
			For a As Frameindex_t = 0 To DmaBuffer.GetUpperBound(0)
				CodedException.Attempt(fglib5.Fg_DelMem.Invoke(Fg.指针, 内存头, a))
				CodedException.Attempt(fglib5.Fg_NumaFreeDmaBuffer.Invoke(Fg.指针, DmaBuffer(a)))
			Next
			CodedException.Attempt(fglib5.Fg_FreeMemHead.Invoke(Fg.指针, 内存头))
			内存头 = IntPtr.Zero
		End If
	End Sub
	''' <summary>
	''' 如果已经分配过，将会先释放旧的内存
	''' </summary>
	''' <param name="帧数">内存中要保存的图片帧数</param>
	Sub 分配内存(帧数 As Frameindex_t)
		释放内存()
		Dim b As UInteger = Width * Height * 像素位数 / 8
		内存头 = fglib5.Fg_AllocMemHead.Invoke(Fg.指针, b * 帧数, 帧数)
		ReDim DmaBuffer(帧数 - 1)
		For a As Frameindex_t = 0 To 帧数 - 1
			DmaBuffer(a) = fglib5.Fg_NumaAllocDmaBuffer.Invoke(Fg.指针, b)
			CodedException.Attempt(fglib5.Fg_AddMem.Invoke(Fg.指针, DmaBuffer(a), b, a, 内存头))
		Next
	End Sub
	Protected Overrides Sub Finalize()
		释放内存()
	End Sub
	Private Sub AcquireEx(Timeout As Integer)
		CodedException.Attempt(fglib5.Fg_NumaPinThread.Invoke(Fg.指针))
		CodedException.Attempt(fglib5.Fg_AcquireEx.Invoke(Fg.指针, DmaIndex, Frameindex_t.GRAB_INFINITE, AcquireFormats.ACQ_STANDARD, 内存头))
		Dim a As Frameindex_t = 1, b As IntPtr, c As New TypedIntPtr(Of Long)(TypedIntPtr.LongReader)
		While Threading.Volatile.Read(Acquiring)
			a = fglib5.Fg_getLastPicNumberBlockingEx.Invoke(Fg.指针, a, DmaIndex, Timeout, 内存头)
			b = fglib5.Fg_getImagePtrEx.Invoke(Fg.指针, a, DmaIndex, 内存头)
			fglib5.Fg_getParameterEx.Invoke(Fg.指针, ParameterID.FG_TIMESTAMP, c, DmaIndex, 内存头, a)
			RaiseEvent ImageAcquired(a, b, c.值)
			a += 1
		End While
		CodedException.Attempt(fglib5.Fg_stopAcquireEx.Invoke(Fg.指针, DmaIndex, 内存头, FgStopAcquireFlags.STOP_ASYNC))
	End Sub
	''' <summary>
	''' 调用前请检查缓冲区大小。如果抓取正在进行时调用此方法，新的Timeout不会生效
	''' </summary>
	''' <param name="Timeout">等待新图片到来的超时</param>
	Sub BeginAcquire(Timeout As Integer)
		If Not Acquiring Then
			Acquiring = True
			AcquireTask = Task.Run(Sub() AcquireEx(Timeout))
		End If
	End Sub
	''' <summary>
	''' 如果未抓取时调用此方法，将返回一个已完成的Task或Nothing
	''' </summary>
	''' <returns></returns>
	Function StopAcquire() As Task
		Acquiring = False
		Return AcquireTask
	End Function
End Class
