
Public Delegate Function 读指针(Of T)(指针 As IntPtr) As T
''' <summary>
''' 基类提供一些现成的指针读取器
''' </summary>
Public MustInherit Class TypedIntPtr
	Protected ReadOnly 指针 As IntPtr
	''' <summary>
	''' 从8字节内存中读取最后1字节作为Byte
	''' </summary>
	Shared ReadOnly Property Offset7ByteReader As 读指针(Of Byte) = Function(指针 As IntPtr) As Byte
																	 Return Marshal.ReadByte(指针, 7)
																 End Function
	''' <summary>
	''' 从8字节内存中读取最后2字节作为Short
	''' </summary>
	Shared ReadOnly Property Offset6ShortReader As 读指针(Of Short) = Function(指针 As IntPtr) As Short
																	   Return Marshal.ReadInt16(指针, 6)
																   End Function
	''' <summary>
	''' 从8字节内存中读取最后4字节作为Integer
	''' </summary>
	Shared ReadOnly Property Offset4IntegerReader As 读指针(Of Integer) = Function(指针 As IntPtr) As Integer
																		   Return Marshal.ReadInt32(指针, 4)
																	   End Function
	''' <summary>
	''' 读取8字节作为Long
	''' </summary>
	Shared ReadOnly Property LongReader As 读指针(Of Long) = Function(指针 As IntPtr) As Short
															  Return Marshal.ReadInt64(指针)
														  End Function
	''' <summary>
	''' 读取8字节作为IntPtr
	''' </summary>
	Shared ReadOnly Property IntPtrReader As 读指针(Of IntPtr) = Function(指针 As IntPtr) As IntPtr
																  Return Marshal.ReadIntPtr(指针)
															  End Function
	Protected Sub New(指针 As IntPtr)
		Me.指针 = 指针
	End Sub
	Protected Overrides Sub Finalize()
		Marshal.FreeHGlobal(指针)
	End Sub
	Shared Narrowing Operator CType(类型指针 As TypedIntPtr) As IntPtr
		Return 类型指针.指针
	End Operator
End Class
''' <summary>
''' 整合了非托管指针的分配、读取和释放。释放是自动的，请勿将生命周期长于此对象的指针包入该类。
''' </summary>
''' <typeparam name="T">指针指向对象类型</typeparam>
Public Class TypedIntPtr(Of T)
	Inherits TypedIntPtr
	ReadOnly 读取器 As 读指针(Of T)
	''' <summary>
	''' 创建一个指向8字节内存的指针
	''' </summary>
	''' <param name="读取器">读取器可以选择基类中现成的，也可以自定义</param>
	Sub New(读取器 As 读指针(Of T))
		MyBase.New(Marshal.AllocHGlobal(Marshal.SizeOf(Of IntPtr)))
		Me.读取器 = 读取器
	End Sub
	Sub New(读取器 As 读指针(Of T), 字节数 As Integer)
		MyBase.New(Marshal.AllocHGlobal(字节数))
		Me.读取器 = 读取器
	End Sub
	Sub New(读取器 As 读指针(Of T), 指针 As IntPtr)
		MyBase.New(指针)
		Me.读取器 = 读取器
	End Sub
	ReadOnly Property 值 As T
		Get
			读取器.Invoke(指针)
		End Get
	End Property
End Class
