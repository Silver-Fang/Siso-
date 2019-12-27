Imports System.Timers
''' <summary>
''' 这个模块是一个示例文档，演示标准调用流程，不是用来运行的
''' </summary>
Public Module 示例文档
	Private fglib5 As FgLib5, Fg As IntPtr, 内存头 As IntPtr, 捕捉中 As Boolean = False
	Private Sub Fg_getAppletProperty(fglib5 As FgLib5, item As IntPtr)
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_APPLET_PATH))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_APPLET_FILE))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_APPLET_NAME))
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_INFO)
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_FLAGS)
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_BOARD_USER_CODE)
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_BOARD_GROUP_MASK)
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_GROUP_CODE)
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_USER_CODE)
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_NR_OF_DMA)
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_NR_OF_CAMS)
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_LAG)
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_VERSION))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_APPLET_UID))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_BITSTREAM_UID))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_SUPPORTED_PLATFORMS))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_CATEGORY))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_DESCRIPTION))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_TAGS))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_RUNTIME_VERSION))
		Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(item, FgAppletStringProperty.FG_AP_STRING_ICON))
		fglib5.Fg_getAppletIntProperty.Invoke(item, FgAppletIntProperty.FG_AP_INT_ICON_SIZE)
	End Sub
	''' <summary>
	''' 处理内存溢出事件
	''' </summary>
	Private Function 溢出处理(events As ULong, data As IntPtr, info As IntPtr) As ErrorCodes
		Throw New OverflowException("FG_OVERFLOW_CAM0")
	End Function
	Private Sub 初始化抓取()
		fglib5.Fg_NumaPinThread.Invoke(Fg)
		'获取一些参数，非必需
		Static Integer指针 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(Of Integer))
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_FORMAT, Integer指针, 0)
		Marshal.ReadInt32(Integer指针)
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_GEN_ENABLE, Integer指针, 0)
		Marshal.ReadInt32(Integer指针)
		'开始抓图，必需
		fglib5.Fg_AcquireEx.Invoke(Fg, DmaIndex.CameraPortA, Frameindex_t.GRAB_INFINITE, AcquireFormats.ACQ_STANDARD, 内存头)
	End Sub
	Private Sub 图像抓取(宽度 As UShort, 高度 As UShort, 像素位 As Byte)
		'必需
		初始化抓取()
		Dim a As Long = 1, b(宽度 * 高度 * 像素位 / 8 - 1) As Byte
		Static 图片指针 As IntPtr
		While Threading.Volatile.Read(捕捉中)
			'这个调用将会阻塞线程，直到返回完整图像
			a = fglib5.Fg_getLastPicNumberBlockingEx.Invoke(Fg, a, 0, 10, 内存头)
			图片指针 = fglib5.Fg_getImagePtrEx.Invoke(Fg, a, 0, 内存头)
			'将图片指针读出作为托管的字节数组。如果你的应用可以直接使用指针，可以省略这一步。否则请输出这个字节数组然后转换成对应平台的图像对象。
			For c As Integer = 0 To b.GetUpperBound(0)
				b(c) = Marshal.ReadByte(图片指针, c)
			Next
			'获取图片的准确时间戳，非必需
			Static Integer指针 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(Of Integer))
			fglib5.Fg_getParameterEx.Invoke(Fg, ParameterID.FG_TIMESTAMP, Integer指针, 0, 内存头, a)
			Marshal.ReadInt32(Integer指针)
		End While
		'停止抓取，必需
		fglib5.Fg_stopAcquireEx.Invoke(Fg, DmaIndex.CameraPortA, 内存头, FgStopAcquireFlags.STOP_ASYNC)
	End Sub
	Private Sub 分配内存(单帧字节数 As UInteger, BufCnt As Byte, DmaBuffer As IntPtr())
		内存头 = fglib5.Fg_AllocMemHead.Invoke(Fg, 单帧字节数 * BufCnt, BufCnt)
		For b As Byte = 0 To BufCnt - 1
			DmaBuffer(b) = fglib5.Fg_NumaAllocDmaBuffer.Invoke(Fg, 单帧字节数)
			fglib5.Fg_AddMem.Invoke(Fg, DmaBuffer(b), 单帧字节数, b, 内存头)
		Next
	End Sub
	Private Sub 释放内存(BufCnt As Byte, DmaBuffer As IntPtr())
		For b As Byte = 0 To BufCnt - 1
			fglib5.Fg_DelMem.Invoke(Fg, 内存头, b)
			fglib5.Fg_NumaFreeDmaBuffer.Invoke(Fg, DmaBuffer(b))
		Next
		fglib5.Fg_FreeMemHead.Invoke(Fg, 内存头)
	End Sub
	Private Sub 视频捕捉(宽度 As UShort, 高度 As UShort, 像素位 As Byte, iolibrt As IoLibRt, AviRef As IntPtr)
		'必需
		初始化抓取()
		Dim a As Long = 1, b(宽度 * 高度 * 像素位 / 8 - 1) As Byte
		Static 图片指针 As IntPtr
		While Threading.Volatile.Read(捕捉中)
			'这个调用将会阻塞线程，直到返回完整图像
			a = fglib5.Fg_getLastPicNumberBlockingEx.Invoke(Fg, a, 0, 10, 内存头)
			'这里执行了两次完全相同的调用，官方应用就是这么干的，我只是照搬，不知为啥，不一定有必要
			图片指针 = fglib5.Fg_getImagePtrEx.Invoke(Fg, a, 0, 内存头)
			图片指针 = fglib5.Fg_getImagePtrEx.Invoke(Fg, a, 0, 内存头)
			iolibrt.IoWriteAVIPicture.Invoke(AviRef, a, 图片指针)
			'获取图片的准确时间戳，非必需
			Static Integer指针 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(Of Integer))
			fglib5.Fg_getParameterEx.Invoke(Fg, ParameterID.FG_TIMESTAMP, Integer指针, 0, 内存头, a)
			Marshal.ReadInt32(Integer指针)
		End While
		'停止抓取，必需
		fglib5.Fg_stopAcquireEx.Invoke(Fg, DmaIndex.CameraPortA, 内存头, FgStopAcquireFlags.STOP_ASYNC)
	End Sub
	''' <summary>
	''' 输入动态链接库包装对象和小程序文件名，演示标准调用流程。仅用于测试目的，请勿用于生产环境。本示例函数未进行任何GCHandle释放过程，实际应用中请务必进行释放，否则会造成内存泄漏。代码中标记为“非必需”的流程可能包含一些变量声明被后面的流程用到，请谨慎删除
	''' </summary>
	''' <param name="fglib5">位于Siso目录\bin下</param>
	''' <param name="siso_hal">位于Siso目录\bin下</param>
	''' <param name="小程序文件名">在Siso目录\Dll下查找</param>
	Public Async Sub 标准调用流程(fglib5 As FgLib5, siso_hal As SisoHal, 小程序文件名 As String, iolibrt As IoLibRt, Optional 视频位置 As String = ".")
		示例文档.fglib5 = fglib5
#Region "硬件初始化"
		'以下流程是必需的
		fglib5.Fg_InitLibrariesEx.Invoke(Nothing, 17, "siso-microdisplay-master", 0)
		Dim 通用指针3 As IntPtr
		通用指针3 = siso_hal.SHalInitDevice.Invoke(0)
		'以下流程是获取硬件信息，非必需。如果需要，请记录这些函数的返回值
		siso_hal.SHalGetBoardIndex.Invoke(通用指针3, 0)
		siso_hal.SHalGetBoardInfoInt.Invoke(通用指针3, "license/group")
		siso_hal.SHalGetBoardInfoInt.Invoke(通用指针3, "license/user")
		siso_hal.SHalGetBoardInfoInt.Invoke(通用指针3, "board/version")
		siso_hal.SHalGetBoardInfoInt.Invoke(通用指针3, "board/revision")
		siso_hal.SHalGetBoardInfoInt.Invoke(通用指针3, "board/type")
		Marshal.PtrToStringAnsi(siso_hal.SHalGetBoardInfoString.Invoke(通用指针3, "board/name"))
		siso_hal.SHalGetBoardInfoInt.Invoke(通用指针3, "board/serial")
		siso_hal.SHalGetBoardInfoInt.Invoke(通用指针3, "pocl")
		Dim String指针 As IntPtr = Marshal.AllocHGlobal(255), Integer指针 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(Of Integer))
		fglib5.Fg_getSystemInformation.Invoke(IntPtr.Zero, Fg_Info_Selector.INFO_DESIGN_ID, FgProperty.PROP_ID_VALUE, 0, String指针, Integer指针)
		Marshal.PtrToStringAnsi(String指针)
		Marshal.ReadInt32(Integer指针)
		fglib5.Fg_getSystemInformation.Invoke(IntPtr.Zero, Fg_Info_Selector.INFO_BITSTREAM_ID, FgProperty.PROP_ID_VALUE, 0, String指针, Integer指针)
		Marshal.PtrToStringAnsi(String指针)
		Marshal.ReadInt32(Integer指针)
		Marshal.PtrToStringAnsi(siso_hal.SHalGetBoardInfoString.Invoke(通用指针3, "board/caminterface"))
		'以下流程是枚举可用的小程序，非必需。如果需要，请记录这些函数的返回值。
		Dim 通用指针1 As IntPtr, 通用指针2 As IntPtr, IntPtr指针 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(Of IntPtr))
		Dim a As Integer = fglib5.Fg_getAppletIterator.Invoke(0, FgAppletIteratorSource.FG_AIS_FILESYSTEM, IntPtr指针, 0) - 1
		通用指针1 = Marshal.ReadIntPtr(IntPtr指针)
		For e As Byte = 0 To a
			通用指针2 = fglib5.Fg_getAppletIteratorItem.Invoke(通用指针1, e)
			Fg_getAppletProperty(fglib5, 通用指针2)
		Next
		fglib5.Fg_freeAppletIterator.Invoke(通用指针1)
		siso_hal.SHalFreeDevice.Invoke(通用指针3)
#End Region
		'在正式应用中，此处应当要求用户选择小程序
#Region "加载选定的小程序"
		'载入小程序模块，必需
		Fg = fglib5.Fg_Init.Invoke(小程序文件名, 0)
		'以下返回选定的小程序相关信息，非必需。如果需要，请记录这些函数的返回值。
		fglib5.Fg_getAppletIterator.Invoke(0, FgAppletIteratorSource.FG_AIS_FILESYSTEM, IntPtr指针, 0)
		通用指针1 = Marshal.ReadIntPtr(IntPtr指针)
		通用指针3 = fglib5.Fg_findAppletIteratorItem.Invoke(通用指针1, 小程序文件名)
		Fg_getAppletProperty(fglib5, 通用指针3)
		fglib5.Fg_freeAppletIterator.Invoke(通用指针1)
		fglib5.Fg_getParameterInfo.Invoke(Fg, 0)
		'以下根据图像尺寸尝试分配内存，用于检测内存是否充足，非必需
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_WIDTH, Integer指针, 0)
		Dim d As UInteger = Marshal.ReadInt32(Integer指针)
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_HEIGHT, Integer指针, 0)
		Dim g As UInteger = Marshal.ReadInt32(Integer指针)
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_FORMAT, Integer指针, 0)
		Dim f As ImageFormat = Marshal.ReadInt32(Integer指针)
		Const BufCnt As Byte = 16
		Dim DmaBuffer(15) As IntPtr
		Dim h As UInteger = d * g
		分配内存(h, BufCnt, DmaBuffer)
		Dim i As ULong = fglib5.Fg_getEventMask.Invoke(Fg, "FG_OVERFLOW_CAM0")
		fglib5.Fg_clearEvents.Invoke(Fg, i)
		fglib5.Fg_registerEventCallback.Invoke(Fg, i, AddressOf 溢出处理, IntPtr.Zero, FgEventControlFlags.FG_EVENT_DEFAULT_FLAGS, IntPtr.Zero)
		fglib5.Fg_activateEvents.Invoke(Fg, i, True)
		'以下获取小程序相关参数，非必需。参数较多不一一列举，可选的参数都在ParameterID枚举中有详细说明。
		'以下为获取数值类参数的标准流程
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_CAMSTATUS_EXTENDED, Integer指针, 0)
		Marshal.ReadInt32(Integer指针)
		通用指针3 = Marshal.StringToHGlobalAnsi("FG_CAMSTATUS_EXTENDED")
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_PARAM_DESCR, 通用指针3, 0)
		Marshal.PtrToStringAnsi(通用指针3)
		'以下为获取字符串参数的标准流程。字符串参数的ParameterID都被标记为"Fg_getParameterWithType, Fg_freeParameterStringWithType"。
		Marshal.WriteIntPtr(IntPtr指针, IntPtr.Zero)
		fglib5.Fg_getParameterWithType.Invoke(Fg, ParameterID.FG_HAP_FILE, IntPtr指针, 0, FgParamTypes.FG_PARAM_TYPE_CHAR_PTR_PTR)
		String指针 = Marshal.ReadIntPtr(IntPtr指针)
		Marshal.PtrToStringAnsi(String指针)
		fglib5.Fg_freeParameterStringWithType.Invoke(Fg, ParameterID.FG_HAP_FILE, String指针, 0, FgParamTypes.FG_PARAM_TYPE_CHAR_PTR_PTR)
#End Region
		'小程序加载完毕，但用户尚未开始录制，此时可定时检测相机状态，非必需。
		fglib5.Fg_getParameterWithType.Invoke(Fg, ParameterID.FG_CAMSTATUS_EXTENDED, Integer指针, 0, FgParamTypes.FG_PARAM_TYPE_UINT32_T)
		Marshal.ReadInt32(Integer指针)
		'根据你的需要选择图像捕捉或视频捕捉。
#Region "图像捕捉"

		'清理内存，重新分配。必需。
		释放内存(BufCnt, DmaBuffer)
		分配内存(h, BufCnt, DmaBuffer)
		'检查触发模式，非必需。
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_TRIGGERMODE, Integer指针, 0)
		Marshal.ReadInt32(Integer指针)
		'另开线程抓图，保证UI流畅。必需的。
		捕捉中 = True
		Dim 像素位 As New Dictionary(Of ImageFormat, Byte) From {{ImageFormat.FG_GRAY16, 16}, {ImageFormat.FG_GRAY, 8}}
		Dim j As Task = Task.Run(Sub() 图像抓取(d, g, 像素位(f)))
		'等待用户停止抓取
		Console.WriteLine("按回车停止抓图")
		Console.ReadLine()
		'停止抓取，必需
		Threading.Volatile.Write(捕捉中, False)
		Await j
		'获取一些参数，非必需
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_TRANSFER_LEN, Integer指针, DmaIndex.CameraPortA)
		Marshal.ReadInt32(Integer指针)
		fglib5.Fg_getParameterWithType.Invoke(Fg, ParameterID.FG_IMAGE_TAG, Integer指针, DmaIndex.CameraPortA, FgParamTypes.FG_PARAM_TYPE_UINT32_T)
		Marshal.ReadInt32(Integer指针)
#End Region
		'如果你只需要抓图，可以跳过视频捕捉部分。
		'等待用户操作期间可以定时检测相机状态，非必需
		fglib5.Fg_getParameterWithType.Invoke(Fg, ParameterID.FG_CAMSTATUS_EXTENDED, Integer指针, 0, FgParamTypes.FG_PARAM_TYPE_UINT32_T)
		Marshal.ReadInt32(Integer指针)
#Region "视频捕捉"
		'清理内存，重新分配。必需。
		释放内存(BufCnt, DmaBuffer)
		分配内存(h, BufCnt, DmaBuffer)
		'这个采样率可以根据具体需要调整
		Const 采样率 As Double = 60
		iolibrt.IoCreateAVIGrayW.Invoke(IntPtr指针, 视频位置, d, g, 采样率)
		'检查触发模式，非必需。
		fglib5.Fg_getParameter.Invoke(Fg, ParameterID.FG_TRIGGERMODE, Integer指针, 0)
		Marshal.ReadInt32(Integer指针)
		'另开线程进行捕捉，保证UI流畅。必需。
		捕捉中 = True
		Dim AviRef As IntPtr = Marshal.ReadIntPtr(IntPtr指针)
		j = Task.Run(Sub() 视频捕捉(d, g, 像素位(f), iolibrt, AviRef))
		'等待用户停止抓取
		Console.WriteLine("按回车停止视频")
		Console.ReadLine()
		'停止抓取，必需
		Threading.Volatile.Write(捕捉中, False)
		Await j
		iolibrt.IoCloseAVI.Invoke(AviRef)
#End Region
		'处理后事
		fglib5.Fg_activateEvents.Invoke(Fg, i, False)
		fglib5.Fg_registerEventCallback.Invoke(Fg, i, Nothing, IntPtr.Zero, FgEventControlFlags.FG_EVENT_DEFAULT_FLAGS, IntPtr.Zero)
		释放内存(BufCnt, DmaBuffer)
		fglib5.Fg_FreeGrabber.Invoke(Fg)
		fglib5.Fg_FreeLibraries.Invoke()
	End Sub
End Module