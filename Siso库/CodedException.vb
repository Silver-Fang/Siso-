Public Class CodedException
	Inherits Exception
	ReadOnly Property 错误代码 As ErrorCodes
	Sub New(message As String)
		MyBase.New(message)
	End Sub

	Sub New(message As String, innerException As Exception)
		MyBase.New(message, innerException)
	End Sub
	Sub New(错误代码 As ErrorCodes)
		Me.错误代码 = 错误代码
	End Sub

	Sub New()
	End Sub
	Shared Sub Attempt(错误代码 As ErrorCodes)
		If 错误代码 < ErrorCodes.FG_OK Then Throw New CodedException(错误代码)
	End Sub
End Class