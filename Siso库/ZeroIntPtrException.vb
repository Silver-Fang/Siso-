Public Class ZeroIntPtrException
	Inherits Exception

	Public Sub New(message As String)
		MyBase.New(message)
	End Sub

	Public Sub New(message As String, innerException As Exception)
		MyBase.New(message, innerException)
	End Sub

	Public Sub New()
	End Sub
	Shared Function Check(指针 As IntPtr) As IntPtr
		If 指针 = IntPtr.Zero Then
			Throw New ZeroIntPtrException("指针为空")
		Else
			Return 指针
		End If
	End Function
End Class
