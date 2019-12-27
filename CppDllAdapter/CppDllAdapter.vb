Imports System.Runtime.InteropServices, System.ComponentModel
''' <summary>
''' 一个Dll文件的.Net表示
''' </summary>
Public Class CppDllAdapter
	Private Declare Function LoadLibraryW Lib "kernel32.dll" (<MarshalAs(UnmanagedType.LPWStr)> Dll路径 As String) As IntPtr
	Private Declare Function FreeLibrary Lib "kernel32.dll" (Dll指针 As IntPtr) As Boolean
	ReadOnly Dll指针 As IntPtr
	<DllImport("kernel32.dll", BestFitMapping:=False)> Private Shared Function GetProcAddress(Dll指针 As IntPtr, Api名称 As String) As IntPtr
	End Function
	''' <summary>
	''' 通过Dll文件的路径创建Adapter
	''' </summary>
	''' <param name="Dll路径">Dll文件路径</param>
	Sub New(Dll路径 As String)
		Dll指针 = LoadLibraryW(Dll路径)
		If Dll指针 = IntPtr.Zero Then Throw New Win32Exception(Marshal.GetLastWin32Error, "库加载失败")
	End Sub
	''' <summary>
	''' 返回给定名称Api函数的委托对象。你需要自行定义一个符合目标函数签名的委托，作为此方法的类型参数。
	''' </summary>
	''' <typeparam name="TDelegate">目标函数的委托类型</typeparam>
	''' <param name="Api名称">目标函数的名称</param>
	''' <returns>委托对象</returns>
	Function GetProcDelegate(Of TDelegate)(Api名称 As String) As TDelegate
		Dim a As IntPtr = GetProcAddress(Dll指针, Api名称)
		If a = IntPtr.Zero Then
			Throw New Win32Exception(Marshal.GetLastWin32Error, "过程加载失败")
		Else
			Return Marshal.GetDelegateForFunctionPointer(Of TDelegate)(a)
		End If
	End Function
	''' <summary>
	''' 如果重写此方法，应当在最后加上MyBase.Finalize()
	''' </summary>
	Protected Overrides Sub Finalize()
		FreeLibrary(Dll指针)
		MyBase.Finalize()
	End Sub
End Class
