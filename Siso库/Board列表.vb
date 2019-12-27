Friend Class Board列表
	Inherits 动态加载列表(Of SisoBoard)
	ReadOnly siso_hal As SisoHal, fglib5 As FgLib5
	Protected Overrides Function 加载元素(索引 As Integer) As SisoBoard
		Return New SisoBoard(siso_hal.SHalInitDevice.Invoke(索引), siso_hal, fglib5)
	End Function
	Sub New(元素个数 As Byte, siso_hal As SisoHal, fglib5 As FgLib5)
		MyBase.New(元素个数)
		Me.siso_hal = siso_hal
		Me.fglib5 = fglib5
	End Sub
End Class
