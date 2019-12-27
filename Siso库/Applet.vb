Public Structure Applet
	ReadOnly fglib5 As FgLib5
	ReadOnly Property 指针 As IntPtr
	Friend Sub New(指针 As IntPtr, fglib5 As FgLib5)
		Me.指针 = ZeroIntPtrException.Check(指针)
		Me.fglib5 = fglib5
	End Sub
	ReadOnly Property StringProperty([property] As FgAppletStringProperty) As String
		Get
			Return Marshal.PtrToStringAnsi(fglib5.Fg_getAppletStringProperty.Invoke(指针, [property]))
		End Get
	End Property
	ReadOnly Property IntProperty([property] As FgAppletIntProperty) As Long
		Get
			Return fglib5.Fg_getAppletIntProperty.Invoke(指针, [property])
		End Get
	End Property
	Public Overrides Function Equals(obj As Object) As Boolean
		Dim a As Applet = obj
		Return 指针 = a.指针
	End Function

	Public Shared Operator =(left As Applet, right As Applet) As Boolean
		Return left.Equals(right)
	End Operator

	Public Shared Operator <>(left As Applet, right As Applet) As Boolean
		Return Not left = right
	End Operator
End Structure
