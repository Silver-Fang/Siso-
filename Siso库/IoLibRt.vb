Public Class IoLibRt
	Inherits CppDllAdapter.CppDllAdapter
	Delegate Function IoCreateAVIGrayW委托(AviRef As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> filename As String, width As Integer, height As Integer, fps As Double) As Integer
	Delegate Function IoWriteAVIPicture委托(AviRef As IntPtr, PicNr As Integer, buffer As IntPtr) As Integer
	Delegate Function IoCloseAVI委托(AviRef As IntPtr) As Integer
	ReadOnly Property IoCreateAVIGrayW As IoCreateAVIGrayW委托 = GetProcDelegate(Of IoCreateAVIGrayW委托)("IoCreateAVIGrayW")
	ReadOnly Property IoWriteAVIPicture As IoWriteAVIPicture委托 = GetProcDelegate(Of IoWriteAVIPicture委托)("IoWriteAVIPicture")
	ReadOnly Property IoCloseAVI As IoCloseAVI委托 = GetProcDelegate(Of IoCloseAVI委托)("IoCloseAVI")
	Sub New(Dll路径 As String)
		MyBase.New(Dll路径)
	End Sub
End Class
