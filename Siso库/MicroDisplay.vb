Imports System.IO.Path
''' <summary>
''' 标准入口类，满足常规需求。不建议直接使用底层API。
''' </summary>
Public Class MicroDisplay
	ReadOnly iolibrt As IoLibRt
	''' <summary>
	''' 从这里枚举所有可用的Board
	''' </summary>
	ReadOnly Property Boards As IReadOnlyList(Of SisoBoard)
	''' <summary>
	''' The SetupDiGetClassDevsEx function returns a handle to a device information set that contains requested device information elements for a local or a remote computer.
	''' </summary>
	''' <param name="ClassGuid">A pointer to the GUID for a device setup class or a device interface class.</param>
	''' <param name="Enumerator">A pointer to a NULL-terminated string.</param>
	''' <param name="hwndParent">A handle to the top-level window to be used for a user interface that is associated with installing a device instance in the device information set.</param>
	''' <param name="Flags">A variable of type DWORD that specifies control options that filter the device information elements that are added to the device information set.</param>
	''' <param name="DeviceInfoSet">The handle to an existing device information set to which SetupDiGetClassDevsEx adds the requested device information elements.</param>
	''' <param name="MachineName">A pointer to a constant string that contains the name of a remote computer on which the devices reside.</param>
	''' <param name="Reserved">Reserved for internal use. This parameter must be set to NULL.</param>
	''' <returns></returns>
	Private Declare Function SetupDiGetClassDevsExW Lib "SetupAPI.dll" (ClassGuid As IntPtr, Enumerator As IntPtr, hwndParent As IntPtr, Flags As Integer, DeviceInfoSet As IntPtr, MachineName As IntPtr, Reserved As IntPtr) As IntPtr
	Private Declare Function SetupDiEnumDeviceInfo Lib "SetupAPI.dll" (DeviceInfoSet As IntPtr, MemberIndex As Integer, DeviceInfoData As IntPtr) As Boolean
	Private Declare Function CM_Enable_DevNode_Ex Lib "CfgMgr32.dll" (dnDevInst As Integer, ulFlags As ULong, hMachine As IntPtr) As Integer
	Private Declare Function CM_Disable_DevNode_Ex Lib "CfgMgr32.dll" (dnDevInst As Integer, ulFlags As ULong, hMachine As IntPtr) As Integer
	Private Structure SP_DEVINFO_DATA
		Property cbSize As Integer
		Property ClassGuid As Guid
		Property DevInst As Integer
		Property Reserved As ULong
	End Structure
	''' <summary>
	''' 新建MicroDisplay入口类
	''' </summary>
	''' <param name="Runtime目录">SiliconSoftware运行时的目录，如"C:\Program Files\SiliconSoftware\Runtime5.7.0"</param>
	''' <param name="ClassGuid">设备的类GUID。如果设备加载失败，将尝试用这个GUID重启设备。</param>
	Sub New(Runtime目录 As String, ClassGuid As Guid)
		Dim a As String = Combine(Runtime目录, "bin"), fglib5 As New FgLib5(Combine(a, "fglib5.dll")), siso_hal As New SisoHal(Combine(a, "siso_hal.dll"))
		fglib5.Fg_InitLibrariesEx.Invoke(Nothing, 17, "siso-microdisplay-master", 0)
		Dim b As Byte = siso_hal.SHalGetDeviceCount.Invoke
		If b = 0 Then
			Dim c As Byte = 0, d As New TypedIntPtr(Of SP_DEVINFO_DATA), e As SP_DEVINFO_DATA
			While SetupDiEnumDeviceInfo(SetupDiGetClassDevsExW(New TypedIntPtr(Of Guid)(ClassGuid), Nothing, Nothing, 12, Nothing, Nothing, Nothing), 0, d)
				e = d.值
				If e.ClassGuid = ClassGuid Then
					CM_Disable_DevNode_Ex(e.DevInst, 4, Nothing)
					CM_Enable_DevNode_Ex(e.DevInst, 0, Nothing)
					Exit While
				Else
					c += 1
				End If
			End While
		End If
		Boards = New Board列表(siso_hal.SHalGetDeviceCount.Invoke, siso_hal, fglib5)
		iolibrt = New IoLibRt(Combine(a, "iolibrt.dll"))
	End Sub
	''' <summary>
	''' 新建MicroDisplay入口类
	''' </summary>
	''' <param name="Runtime目录">SiliconSoftware运行时的目录，如"C:\Program Files\SiliconSoftware\Runtime5.7.0"</param>
	Sub New(Runtime目录 As String)
		Dim a As String = Combine(Runtime目录, "bin"), fglib5 As New FgLib5(Combine(a, "fglib5.dll")), siso_hal As New SisoHal(Combine(a, "siso_hal.dll"))
		fglib5.Fg_InitLibrariesEx.Invoke(Nothing, 17, "siso-microdisplay-master", 0)
		Boards = New Board列表(siso_hal.SHalGetDeviceCount.Invoke, siso_hal, fglib5)
		iolibrt = New IoLibRt(Combine(a, "iolibrt.dll"))
	End Sub
	''' <summary>
	''' 创建一个可写出文件的AVI对象
	''' </summary>
	''' <param name="路径">AVI路径</param>
	''' <param name="width">宽度</param>
	''' <param name="height">高度</param>
	''' <param name="fps">帧率</param>
	''' <returns>AVI对象</returns>
	Function CreateAvi(路径 As String, width As Integer, height As Integer, fps As Double) As AviRef
		Return New AviRef(iolibrt, 路径, width, height, fps)
	End Function
End Class
