''' <summary>
''' 这个类库未公开文档，根据ApiMonitor监视结果猜测编写。建议使用MicroDisplay入口类，不要直接调用这里的底层API。
''' </summary>
Public Class SisoHal
	Inherits CppDllAdapter.CppDllAdapter
	Delegate Function SHalGetDeviceCount委托() As UInteger
	Delegate Function SHalInitDevice委托(设备序号 As Integer) As IntPtr
	Delegate Function SHalGetBoardIndex委托(设备指针 As IntPtr, 空 As Integer) As UInteger
	Delegate Function SHalGetBoardInfoInt委托(设备指针 As IntPtr, <MarshalAs(UnmanagedType.LPStr)> 信息名称 As String) As Integer
	Delegate Function SHalGetBoardInfoString委托(设备指针 As IntPtr, <MarshalAs(UnmanagedType.LPStr)> 信息名称 As String) As IntPtr
	Delegate Sub SHalFreeDevice委托(设备指针 As IntPtr)
	ReadOnly Property SHalGetDeviceCount As SHalGetDeviceCount委托 = GetProcDelegate(Of SHalGetDeviceCount委托)("SHalGetDeviceCount")
	ReadOnly Property SHalInitDevice As SHalInitDevice委托 = GetProcDelegate(Of SHalInitDevice委托)("SHalInitDevice")
	ReadOnly Property SHalGetBoardIndex As SHalGetBoardIndex委托 = GetProcDelegate(Of SHalGetBoardIndex委托)("SHalGetBoardIndex")
	ReadOnly Property SHalGetBoardInfoInt As SHalGetBoardInfoInt委托 = GetProcDelegate(Of SHalGetBoardInfoInt委托)("SHalGetBoardInfoInt")
	ReadOnly Property SHalGetBoardInfoString As SHalGetBoardInfoString委托 = GetProcDelegate(Of SHalGetBoardInfoString委托)("SHalGetBoardInfoString")
	ReadOnly Property SHalFreeDevice As SHalFreeDevice委托 = GetProcDelegate(Of SHalFreeDevice委托)("SHalFreeDevice")
	Sub New(Dll路径 As String)
		MyBase.New(Dll路径)
	End Sub
End Class
