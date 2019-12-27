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
