Public MustInherit Class 非托管库
	Private Declare Function LoadLibraryW Lib "kernel32.dll" (<MarshalAs(UnmanagedType.LPWStr)> Dll路径 As String) As IntPtr
	Private Declare Function FreeLibrary Lib "kernel32.dll" (Dll指针 As IntPtr) As Boolean
	ReadOnly Dll指针 As IntPtr
	<DllImport("kernel32.dll", BestFitMapping:=False)> Private Shared Function GetProcAddress(Dll指针 As IntPtr, Api名称 As String) As IntPtr
	End Function
	Protected Sub New(Dll名称 As String)
		Dll指针 = LoadLibraryW(Dll名称)
		If Dll指针 = IntPtr.Zero Then Throw New Win32Exception(Marshal.GetLastWin32Error, "库加载失败")
	End Sub
	Protected Function GetProcAddress(Of TDelegate)(Api名称 As String) As TDelegate
		Dim a As IntPtr = GetProcAddress(Dll指针, Api名称)
		If a = IntPtr.Zero Then
			Throw New Win32Exception(Marshal.GetLastWin32Error, "过程加载失败")
		Else
			Return Marshal.GetDelegateForFunctionPointer(Of TDelegate)(a)
		End If
	End Function
	Protected Overrides Sub Finalize()
		FreeLibrary(Dll指针)
		MyBase.Finalize()
	End Sub
End Class
