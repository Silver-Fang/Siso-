Public MustInherit Class 动态加载列表(Of T)
	Implements IReadOnlyList(Of T)
	ReadOnly 缓存 As T()
	Protected MustOverride Function 加载元素(索引 As Integer) As T
	Default ReadOnly Property Item(index As Integer) As T Implements IReadOnlyList(Of T).Item
		Get
			If 缓存(index) Is Nothing Then 缓存(index) = 加载元素(index)
			Return 缓存(index)
		End Get
	End Property

	Private ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of T).Count

	Private Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
		For a = 0 To Count - 1
			If 缓存(a) Is Nothing Then 缓存(a) = 加载元素(a)
		Next
		Return 缓存.GetEnumerator
	End Function

	Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
		Return GetEnumerator()
	End Function
	Protected Sub New(元素个数 As Integer)
		ReDim 缓存(元素个数 - 1)
		Count = 元素个数
	End Sub
End Class
