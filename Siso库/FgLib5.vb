Public Enum ErrorCodes As Short
	''' <summary>
	''' Transmission was not started.
	''' </summary>
	FG_TRANSFER_NOT_ACTIVE = -3010
	''' <summary>
	''' The frame grabber cannot be initialized
	''' </summary>
	FG_CANNOT_INIT_MICROENABLE = -3000
	FG_NOT_IMPLEMENTED = -2130
	''' <summary>
	''' Timeout occurs.
	''' </summary>
	FG_TIMEOUT_ERR = -2120
	FG_OPERATION_ABORTED = -2091
	''' <summary>
	''' Grabbing is already started and can't be started twice.
	''' </summary>
	FG_ALREADY_STARTED = -2090
	''' <summary>
	''' The filename is not a valid filename
	''' </summary>
	FG_INVALID_FILENAME = -2076
	''' <summary>
	''' An APC from a previous acquisition is still active
	''' </summary>
	FG_ILLEGAL_WHILE_APC = -2071
	FG_INVALID_PARAMETER = -2070
	''' <summary>
	''' Image memory is already used by another DMA channel.
	''' </summary>
	FG_MEMORY_IN_USE = -2061
	''' <summary>
	''' Image memory was not allocated.
	''' </summary>
	FG_INVALID_MEMORY = -2060
	''' <summary>
	''' Frame grabber could not stop.
	''' </summary>
	FG_CANNOT_STOP = -2042
	FG_MEMORY_ALREADY_ALLOCATED = -2024
	''' <summary>
	''' Insufficient image memory was allocated for specified image parameter.
	''' </summary>
	FG_NOT_ENOUGH_MEMORY = -2020
	FG_NOT_INIT = -2001
	''' <summary>
	''' The system memory is insufficient for loading control structures
	''' </summary>
	FG_NOT_ENOUGH_MEM = -500
	''' <summary>
	''' The file cannot be found
	''' </summary>
	FG_FILE_NOT_FOUND = -101
	''' <summary>
	''' The Applet file (HAP file) is not a valid HAP file
	''' </summary>
	FG_HAP_FILE_NOT_LOAD = -100
	FG_EXCEPTION_IN_APPLET = -99
	FG_SISODIR5_NOT_SET = -5
	FG_OK = 0
	FG_TIMEOUT = 600
End Enum
Public Enum Fg_Info_Selector As Short
	INFO_DESIGN_ID = 1200
	INFO_BITSTREAM_ID = 1201
End Enum
Public Enum FgProperty As Byte
	PROP_ID_VALUE = 0
End Enum
Public Enum FgAppletIteratorSource As Byte
	FG_AIS_BOARD
	FG_AIS_FILESYSTEM
End Enum
Public Enum FgAppletStringProperty As Byte
	FG_AP_STRING_APPLET_UID
	FG_AP_STRING_BITSTREAM_UID
	FG_AP_STRING_DESIGN_NAME
	FG_AP_STRING_APPLET_NAME
	FG_AP_STRING_DESCRIPTION
	FG_AP_STRING_CATEGORY
	FG_AP_STRING_APPLET_PATH
	FG_AP_STRING_ICON
	FG_AP_STRING_SUPPORTED_PLATFORMS
	FG_AP_STRING_TAGS
	FG_AP_STRING_VERSION
	FG_AP_STRING_APPLET_FILE
	FG_AP_STRING_RUNTIME_VERSION
End Enum
Public Enum FgAppletIntProperty As Byte
	FG_AP_INT_FLAGS
	FG_AP_INT_INFO
	FG_AP_INT_PARTITION
	FG_AP_INT_NR_OF_DMA
	FG_AP_INT_NR_OF_CAMS
	FG_AP_INT_GROUP_CODE
	FG_AP_INT_USER_CODE
	FG_AP_INT_BOARD_GROUP_MASK
	FG_AP_INT_BOARD_USER_CODE
	FG_AP_INT_ICON_SIZE
	FG_AP_INT_LAG
	FG_AP_INT_DESIGN_VERSION
	FG_AP_INT_DESIGN_REVISION
End Enum
''' <summary>
''' 注意：这个列表不完整，因为实际可用的ParameterID太多没时间一一列举。
''' </summary>
Public Enum ParameterID
	FG_WIDTH = 100
	FG_HEIGHT = 200
	''' <summary>
	''' Internal framegrabber image timeout. This is independend of the runtime timeout value.
	''' </summary>
	FG_TIMEOUT = 600
	''' <summary>
	''' Pixel format of the output image.
	''' </summary>
	FG_FORMAT = 700
	''' <summary>
	''' Returns the status of a connected CameraLink camera. For CoaXPress this is not of interest.
	''' </summary>
	FG_CAMSTATUS = 2000
	''' <summary>
	''' Returns the extended status of the camera. Bit = meaning: 0 = CameraPixelClk, 1 = CameraLval, 2 = CameraFval, 3 = Camera CC1 Signal, 4 = ExTrg / external trigger, 5 = BufferOverflow, 6 = BufferStatus-LSB, 7 = BufferStatus-MSB
	''' </summary>
	FG_CAMSTATUS_EXTENDED = 2050
	''' <summary>
	''' Enables a maximum DMA performance
	''' </summary>
	FG_TURBO_DMA_MODE = 3051
	''' <summary>
	''' Internal processing bit depth of the applet.
	''' </summary>
	FG_PIXELDEPTH = 4000
	''' <summary>
	''' Defines if the output data are right aligned or left aligned.
	''' </summary>
	FG_BITALIGNMENT = 4010
	FG_TRANSFER_LEN = 5210
	FG_TRIGGERMODE = 8100
	FG_IMAGE_TAG = 22000
	FG_TIMESTAMP = 22020
	''' <summary>
	''' Returns the applet id.
	''' </summary>
	FG_APPLET_ID = 24010
	''' <summary>
	''' Returns the applet version.
	''' </summary>
	FG_APPLET_VERSION = 24020
	''' <summary>
	''' Returns the applet revision.
	''' </summary>
	FG_APPLET_REVISION = 24030
	''' <summary>
	''' Returns the status of the DMA transmission.
	''' </summary>
	FG_DMASTATUS = 24092
	''' <summary>
	''' Fg_getParameterWithType, Fg_freeParameterStringWithType
	''' </summary>
	FG_HAP_FILE = 24108
	FG_PARAM_DESCR = 24114
	FG_GEN_ENABLE = 30099
	''' <summary>
	''' Enable or disable the noise filter.
	''' </summary>
	FG_NOISEFILTER = 110016
	''' <summary>
	''' Enables the LUT to be load with custom values or use the applet's processor.
	''' </summary>
	FG_LUT_TYPE = 110017
	''' <summary>
	''' Fg_getParameterWithType, Fg_freeParameterStringWithType
	''' </summary>
	FG_LUT_SAVE_FILE = 110021
	''' <summary>
	''' Fg_getParameterWithType, Fg_freeParameterStringWithType
	''' </summary>
	FG_LUT_CUSTOM_FILE = 300000
	''' <summary>
	''' Field with LUT Values.
	''' </summary>
	FG_LUT_VALUE = 300001
	''' <summary>
	''' Gain correction value. Available when LUT functionality is enabled.
	''' </summary>
	FG_PROCESSING_GAIN = 300002
	''' <summary>
	''' Gamma correction value. Available when LUT functionality is enabled.
	''' </summary>
	FG_PROCESSING_GAMMA = 300003
	''' <summary>
	''' Offset correction value. Available when LUT functionality is enabled.
	''' </summary>
	FG_PROCESSING_OFFSET = 300004
	''' <summary>
	''' Invert output. Available when LUT functionality is enabled.
	''' </summary>
	FG_PROCESSING_INVERT = 300005
	''' <summary>
	''' Type of LUT implementation.
	''' </summary>
	FG_LUT_IMPLEMENTATION_TYPE = 300006
	''' <summary>
	''' Pixel bit depth at LUT input.
	''' </summary>
	FG_LUT_IN_BITS = 300007
	''' <summary>
	''' Pixel bit depth at LUT output.
	''' </summary>
	FG_LUT_OUT_BITS = 300008
	''' <summary>
	''' Enables the gain correction.
	''' </summary>
	FG_SHADING_GAIN_ENABLE = 300100
	''' <summary>
	''' Enables the dead pixel interpolation.
	''' </summary>
	FG_SHADING_DEAD_PIXEL_INTERPOLATION_ENABLE = 300104
End Enum
Public Enum FgEventControlFlags As Byte
	FG_EVENT_DEFAULT_FLAGS = 0
End Enum
Public Enum FgParamTypes As Byte
	FG_PARAM_TYPE_UINT32_T = 2
	FG_PARAM_TYPE_SIZE_T = 7
	FG_PARAM_TYPE_CHAR_PTR_PTR = &H16
End Enum
Public Enum AcquireFormats As Byte
	ACQ_STANDARD = 1
End Enum
Public Enum Frameindex_t As Long
	GRAB_INFINITE = -1
End Enum
Public Enum DmaIndex As Byte
	CameraPortA
	CameraPortB
End Enum
Public Enum ImageFormat As Byte
	FG_GRAY16 = 1
	FG_GRAY = 3
End Enum
Public Enum FgStopAcquireFlags
	STOP_ASYNC = 0
	STOP_SYNC = &H80000000
End Enum
Public Structure Fg_event_info
	Property Version As UInteger
	Property Pad As UInteger
	Property Notify As <MarshalAs(UnmanagedType.LPArray, SizeConst:=64)> UInteger()
	Property Timestamp As <MarshalAs(UnmanagedType.LPArray, SizeConst:=64)> ULong()
	Property Length As UInteger
	Property Data As <MarshalAs(UnmanagedType.LPArray, SizeConst:=254)> UShort()
	Function Init() As Fg_event_info
		ReDim Notify(63)
		ReDim Timestamp(63)
		ReDim Data(253)
		Version = 2
		Pad = 0
		For a As Byte = 0 To 63
			Notify(a) = 0
			Timestamp(a) = 0
		Next
		Length = 0
		For a As Byte = 0 To 253
			Data(a) = 0
		Next
		Return Me
	End Function
	Public Overrides Function Equals(obj As Object) As Boolean
		Throw New NotImplementedException()
	End Function

	Public Shared Operator =(left As Fg_event_info, right As Fg_event_info) As Boolean
		Return left.Equals(right)
	End Operator

	Public Shared Operator <>(left As Fg_event_info, right As Fg_event_info) As Boolean
		Return Not left = right
	End Operator
End Structure
Public Delegate Sub Fg_EventFunc(events As ULong, data As IntPtr, info As IntPtr)
Public Delegate Function Fg_EventFunc_t(events As ULong, data As IntPtr, info As IntPtr) As ErrorCodes
''' <summary>
''' 建议使用MicroDisplay入口类，不要直接调用这里的底层API。
''' </summary>
Public Class FgLib5
	Inherits CppDllAdapter.CppDllAdapter
	Delegate Function Fg_InitLibrariesEx委托(<MarshalAs(UnmanagedType.LPStr)> sisoDir As String, flags As UInteger, <MarshalAs(UnmanagedType.LPStr)> id As String, timeout As UInteger) As ErrorCodes
	Delegate Sub Fg_FreeLibraries委托()
	Delegate Function Fg_getSystemInformation委托(Fg As IntPtr, selector As Fg_Info_Selector, propertyId As FgProperty, param1 As Integer, buffer As IntPtr, bufLen As IntPtr) As ErrorCodes
	Delegate Function Fg_getAppletIterator委托(boardIndex As Integer, src As FgAppletIteratorSource, iter As IntPtr, flags As Integer) As Integer
	Delegate Function Fg_getAppletIteratorItem委托(iter As IntPtr, index As Integer) As IntPtr
	Delegate Function Fg_getAppletStringProperty委托(item As IntPtr, [property] As FgAppletStringProperty) As IntPtr
	Delegate Function Fg_getAppletIntProperty委托(item As IntPtr, [property] As FgAppletIntProperty) As Long
	Delegate Function Fg_freeAppletIterator委托(iter As IntPtr) As Integer
	Delegate Function Fg_Init委托(<MarshalAs(UnmanagedType.LPStr)> FileName As String, BoardIndex As UInteger) As IntPtr
	Delegate Function Fg_findAppletIteratorItem委托(iter As IntPtr, <MarshalAs(UnmanagedType.LPStr)> path As String) As IntPtr
	Delegate Function Fg_getParameterInfo委托(Fg As IntPtr, CamPort As UInteger) As IntPtr
	Delegate Function Fg_getParameter委托(Fg As IntPtr, Parameter As ParameterID, Value As IntPtr, DmaIndex As DmaIndex) As ErrorCodes
	Delegate Function Fg_AllocMemHead委托(Fg As IntPtr, Size As ULong, BufCnt As Frameindex_t) As IntPtr
	Delegate Function Fg_NumaAllocDmaBuffer委托(Fg As IntPtr, Size As ULong) As IntPtr
	Delegate Function Fg_AddMem委托(Fg As IntPtr, pBuffer As IntPtr, Size As ULong, bufferIndex As Frameindex_t, memHandle As IntPtr) As Integer
	Delegate Function Fg_getEventMask委托(Fg As IntPtr, <MarshalAs(UnmanagedType.LPStr)> name As String) As ULong
	Delegate Function Fg_clearEvents委托(Fg As IntPtr, mask As ULong) As ErrorCodes
	Delegate Function Fg_registerEventCallback委托(Fg As IntPtr, mask As ULong, handler As Fg_EventFunc_t, data As IntPtr, flags As FgEventControlFlags, info As IntPtr) As ErrorCodes
	Delegate Function Fg_activateEvents委托(Fg As IntPtr, mask As ULong, enable As Boolean) As ErrorCodes
	Delegate Function Fg_getParameterWithType委托(Fg As IntPtr, Parameter As ParameterID, Value As IntPtr, DmaIndex As DmaIndex, type As FgParamTypes) As ErrorCodes
	Delegate Function Fg_freeParameterStringWithType委托(Fg As IntPtr, Parameter As ParameterID, Value As IntPtr, DmaIndex As DmaIndex, type As FgParamTypes) As ErrorCodes
	Delegate Function Fg_DelMem委托(Fg As IntPtr, memHandle As IntPtr, bufferIndex As Frameindex_t) As ErrorCodes
	Delegate Function Fg_NumaFreeDmaBuffer委托(Fg As IntPtr, Buffer As IntPtr) As ErrorCodes
	Delegate Function Fg_FreeMemHead委托(Fg As IntPtr, memHandle As IntPtr) As ErrorCodes
	Delegate Function Fg_NumaPinThread委托(Fg As IntPtr) As ErrorCodes
	Delegate Function Fg_AcquireEx委托(Fg As IntPtr, DmaIndex As DmaIndex, PicCount As Frameindex_t, nFlag As AcquireFormats, memHandle As IntPtr) As ErrorCodes
	''' <summary>
	''' 
	''' </summary>
	''' <param name="Fg"></param>
	''' <param name="PicNr"></param>
	''' <param name="DmaIndex"></param>
	''' <param name="Timeout">超时秒数</param>
	''' <param name="pMem"></param>
	''' <returns></returns>
	Delegate Function Fg_getLastPicNumberBlockingEx委托(Fg As IntPtr, PicNr As Frameindex_t, DmaIndex As DmaIndex, Timeout As Integer, pMem As IntPtr) As ErrorCodes
	Delegate Function Fg_getImagePtrEx委托(Fg As IntPtr, PicNr As Frameindex_t, DmaIndex As DmaIndex, pMem As IntPtr) As IntPtr
	Delegate Function Fg_getParameterEx委托(Fg As IntPtr, Parameter As ParameterID, Value As IntPtr, DmaIndex As DmaIndex, pMem As IntPtr, ImgNr As Frameindex_t) As ErrorCodes
	Delegate Function Fg_stopAcquireEx委托(Fg As IntPtr, DmaIndex As DmaIndex, memHandle As IntPtr, nFlag As FgStopAcquireFlags) As ErrorCodes
	Delegate Function Fg_FreeGrabber委托(Fg As IntPtr) As ErrorCodes
	ReadOnly Property Fg_InitLibrariesEx As Fg_InitLibrariesEx委托 = GetProcDelegate(Of Fg_InitLibrariesEx委托)("Fg_InitLibrariesEx")
	ReadOnly Property Fg_FreeLibraries As Fg_FreeLibraries委托 = GetProcDelegate(Of Fg_FreeLibraries委托)("Fg_FreeLibraries")
	ReadOnly Property Fg_getSystemInformation As Fg_getSystemInformation委托 = GetProcDelegate(Of Fg_getSystemInformation委托)("Fg_getSystemInformation")
	ReadOnly Property Fg_getAppletIterator As Fg_getAppletIterator委托 = GetProcDelegate(Of Fg_getAppletIterator委托)("Fg_getAppletIterator")
	ReadOnly Property Fg_getAppletIteratorItem As Fg_getAppletIteratorItem委托 = GetProcDelegate(Of Fg_getAppletIteratorItem委托)("Fg_getAppletIteratorItem")
	ReadOnly Property Fg_getAppletStringProperty As Fg_getAppletStringProperty委托 = GetProcDelegate(Of Fg_getAppletStringProperty委托)("Fg_getAppletStringProperty")
	ReadOnly Property Fg_getAppletIntProperty As Fg_getAppletIntProperty委托 = GetProcDelegate(Of Fg_getAppletIntProperty委托)("Fg_getAppletIntProperty")
	ReadOnly Property Fg_freeAppletIterator As Fg_freeAppletIterator委托 = GetProcDelegate(Of Fg_freeAppletIterator委托)("Fg_freeAppletIterator")
	ReadOnly Property Fg_Init As Fg_Init委托 = GetProcDelegate(Of Fg_Init委托)("Fg_Init")
	ReadOnly Property Fg_findAppletIteratorItem As Fg_findAppletIteratorItem委托 = GetProcDelegate(Of Fg_findAppletIteratorItem委托)("Fg_findAppletIteratorItem")
	ReadOnly Property Fg_getParameterInfo As Fg_getParameterInfo委托 = GetProcDelegate(Of Fg_getParameterInfo委托)("Fg_getParameterInfo")
	ReadOnly Property Fg_getParameter As Fg_getParameter委托 = GetProcDelegate(Of Fg_getParameter委托)("Fg_getParameter")
	ReadOnly Property Fg_AllocMemHead As Fg_AllocMemHead委托 = GetProcDelegate(Of Fg_AllocMemHead委托)("Fg_AllocMemHead")
	ReadOnly Property Fg_NumaAllocDmaBuffer As Fg_NumaAllocDmaBuffer委托 = GetProcDelegate(Of Fg_NumaAllocDmaBuffer委托)("Fg_NumaAllocDmaBuffer")
	ReadOnly Property Fg_AddMem As Fg_AddMem委托 = GetProcDelegate(Of Fg_AddMem委托)("Fg_AddMem")
	ReadOnly Property Fg_getEventMask As Fg_getEventMask委托 = GetProcDelegate(Of Fg_getEventMask委托)("Fg_getEventMask")
	ReadOnly Property Fg_clearEvents As Fg_clearEvents委托 = GetProcDelegate(Of Fg_clearEvents委托)("Fg_clearEvents")
	ReadOnly Property Fg_registerEventCallback As Fg_registerEventCallback委托 = GetProcDelegate(Of Fg_registerEventCallback委托)("Fg_registerEventCallback")
	ReadOnly Property Fg_activateEvents As Fg_activateEvents委托 = GetProcDelegate(Of Fg_activateEvents委托)("Fg_activateEvents")
	ReadOnly Property Fg_getParameterWithType As Fg_getParameterWithType委托 = GetProcDelegate(Of Fg_getParameterWithType委托)("Fg_getParameterWithType")
	ReadOnly Property Fg_freeParameterStringWithType As Fg_freeParameterStringWithType委托 = GetProcDelegate(Of Fg_freeParameterStringWithType委托)("Fg_freeParameterStringWithType")
	ReadOnly Property Fg_DelMem As Fg_DelMem委托 = GetProcDelegate(Of Fg_DelMem委托)("Fg_DelMem")
	ReadOnly Property Fg_NumaFreeDmaBuffer As Fg_NumaFreeDmaBuffer委托 = GetProcDelegate(Of Fg_NumaFreeDmaBuffer委托)("Fg_NumaFreeDmaBuffer")
	ReadOnly Property Fg_FreeMemHead As Fg_FreeMemHead委托 = GetProcDelegate(Of Fg_FreeMemHead委托)("Fg_FreeMemHead")
	ReadOnly Property Fg_NumaPinThread As Fg_NumaPinThread委托 = GetProcDelegate(Of Fg_NumaPinThread委托)("Fg_NumaPinThread")
	ReadOnly Property Fg_AcquireEx As Fg_AcquireEx委托 = GetProcDelegate(Of Fg_AcquireEx委托)("Fg_AcquireEx")
	ReadOnly Property Fg_getLastPicNumberBlockingEx As Fg_getLastPicNumberBlockingEx委托 = GetProcDelegate(Of Fg_getLastPicNumberBlockingEx委托)("Fg_getLastPicNumberBlockingEx")
	ReadOnly Property Fg_getImagePtrEx As Fg_getImagePtrEx委托 = GetProcDelegate(Of Fg_getImagePtrEx委托)("Fg_getImagePtrEx")
	ReadOnly Property Fg_getParameterEx As Fg_getParameterEx委托 = GetProcDelegate(Of Fg_getParameterEx委托)("Fg_getParameterEx")
	ReadOnly Property Fg_stopAcquireEx As Fg_stopAcquireEx委托 = GetProcDelegate(Of Fg_stopAcquireEx委托)("Fg_stopAcquireEx")
	ReadOnly Property Fg_FreeGrabber As Fg_FreeGrabber委托 = GetProcDelegate(Of Fg_FreeGrabber委托)("Fg_FreeGrabber")
	Sub New(Dll路径 As String)
		MyBase.New(Dll路径)
	End Sub
	Protected Overrides Sub Finalize()
		Fg_FreeLibraries.Invoke
		MyBase.Finalize()
	End Sub
End Class
