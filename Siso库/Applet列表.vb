Public Class Applet列表
	Inherits 动态加载列表(Of Applet)
	ReadOnly fglib5 As FgLib5
	ReadOnly Property 指针 As IntPtr
	Protected Overrides Function 加载元素(索引 As Integer) As Applet
		Return New Applet(fglib5.Fg_getAppletIteratorItem.Invoke(指针, 索引), fglib5)
	End Function
	Sub New(元素个数 As Byte, fglib5 As FgLib5, 指针 As IntPtr)
		MyBase.New(元素个数)
		Me.fglib5 = fglib5
		Me.指针 = ZeroIntPtrException.Check(指针)
	End Sub
	Protected Overrides Sub Finalize()
		fglib5.Fg_freeAppletIterator.Invoke(指针)
	End Sub
	''' <summary>
	''' 根据路径查找Applet。
	''' </summary>
	''' <param name="路径">Applet的路径，通常在Dll子目录下</param>
	''' <returns>找到的Applet</returns>
	Function FindApplet(路径 As String) As Applet
		Return New Applet(fglib5.Fg_findAppletIteratorItem.Invoke(指针, 路径), fglib5)
	End Function
End Class
