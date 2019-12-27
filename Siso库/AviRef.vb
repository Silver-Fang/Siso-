Public Class AviRef
	ReadOnly iolibrt As IoLibRt
	ReadOnly Property 指针 As IntPtr
	Friend Sub New(iolibrt As IoLibRt, filename As String, width As Integer, height As Integer, fps As Double)
		Me.iolibrt = iolibrt
		Dim a As New TypedIntPtr(Of IntPtr)(TypedIntPtr.IntPtrReader)
		iolibrt.IoCreateAVIGrayW.Invoke(a, filename, width, height, fps)
		指针 = ZeroIntPtrException.Check(a.值)
	End Sub
	Sub WriteAviPicture(PicNr As Integer, buffer As IntPtr)
		IoLibRt.IoWriteAVIPicture.Invoke(指针, PicNr, buffer)
	End Sub
	Protected Overrides Sub Finalize()
		IoLibRt.IoCloseAVI.Invoke(指针)
	End Sub
	''' <summary>
	''' 手动释放对象，解除对输出AVI文件的占用。调用此方法后此对象将不再可用。
	''' </summary>
	Sub CloseAvi()
		Finalize()
	End Sub
End Class
