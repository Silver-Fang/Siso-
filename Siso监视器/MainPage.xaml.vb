' https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板
Imports Windows.Storage
Imports Siso库
''' <summary>
''' 可用于自身或导航至 Frame 内部的空白页。
''' </summary>
Public NotInheritable Class MainPage
	Inherits Page
	Const 未来访问列表_Silicon运行时 As String = "Silicon运行时", 未来访问列表_临时文件 As String = "临时文件", 本地设置_帧缓冲区数 As String = "帧缓冲区数", 本地设置_采集超时 As String = "采集超时"
	ReadOnly 本地设置 As IPropertySet = ApplicationData.Current.LocalSettings.Values, 未来访问列表 As AccessCache.StorageItemAccessList = AccessCache.StorageApplicationPermissions.FutureAccessList, 库未找到对话框 As New ContentDialog With {.CloseButtonText = "确定", .Title = "未找到Silicon库"}, 文件夹选取器 As New Pickers.FolderPicker, 系统错误对话框 As New ContentDialog With {.CloseButtonText = "确定", .Title = "系统错误"}, 库未加载对话框 As New ContentDialog With {.CloseButtonText = "确定", .Title = "Silicon库未正确加载"}, 无设备对话框 As New ContentDialog With {.CloseButtonText = "确定", .Title = "未找到Silicon设备", .Content = "请检查设备连接，尝试在设备管理器中禁用-启用设备"}
	Private Silicon运行时文件夹 As StorageFolder, 帧抓取器库 As FgLib5, SisoHal As SisoHal, 临时文件夹 As StorageFolder

	Private Sub 载入小程序_Click(sender As Object, e As RoutedEventArgs) Handles 载入小程序.Click
		If 帧抓取器库 Is Nothing Then
			库未加载对话框.Content = "帧抓取器库fglib5.dll加载失败，请检查Silicon目录"
			Call 库未加载对话框.ShowAsync()
			Exit Sub
		Else
			Dim a As ErrorCodes = 帧抓取器库.Fg_InitLibrariesEx(Nothing, 17, "siso-microdisplay-master", 0)
			If a <> ErrorCodes.FG_OK Then
				库未加载对话框.Content = "fglib5.dll初始化失败：" & a.ToString
				Call 库未加载对话框.ShowAsync()
			End If
		End If
		If SisoHal Is Nothing Then
			库未加载对话框.Content = "SisoHal库siso_hal.dll加载失败，请检查Silicon目录"
			Call 库未加载对话框.ShowAsync()
			Exit Sub
		Else
			'If SisoHal.GetDeviceCount = 0 Then
			'	Call 无设备对话框.ShowAsync()
			'Else
			'	SisoHal.SHalInitDevice.Invoke(0)
			'End If
		End If
	End Sub

	Private Sub 采集超时_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 采集超时.SelectionChanged
		本地设置.Item(本地设置_采集超时) = 采集超时.SelectedValue
	End Sub

	Private Sub 帧缓冲区数_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 帧缓冲区数.SelectionChanged
		本地设置.Item(本地设置_帧缓冲区数) = 帧缓冲区数.SelectedValue
	End Sub
	Private Async Sub 选择临时文件夹_Click(sender As Object, e As RoutedEventArgs) Handles 选择临时文件夹.Click
		Dim a As StorageFolder = Await 文件夹选取器.PickSingleFolderAsync
		If a IsNot Nothing Then
			临时文件夹 = a
			临时文件.Text = a.Path
			未来访问列表.AddOrReplace(未来访问列表_临时文件, a)
		End If
	End Sub

	Private Async Sub 选择Silicon文件夹_Click(sender As Object, e As RoutedEventArgs) Handles 选择Silicon文件夹.Click
		Dim a As StorageFolder = Await 文件夹选取器.PickSingleFolderAsync
		If a IsNot Nothing Then 设置Silicon运行时文件夹(a)
	End Sub
	Private Async Sub 设置Silicon运行时文件夹(文件夹 As StorageFolder)
		Dim a As StorageFolder = Await 文件夹.TryGetItemAsync("bin")
		If a Is Nothing Then
			库未找到对话框.Content = "指定的Silicon目录下未找到bin目录"
			Call 库未找到对话框.ShowAsync()
		Else
			Dim c As StorageFile = Await a.TryGetItemAsync("fglib5.dll")
			If c Is Nothing Then
				库未找到对话框.Content = "指定的Silicon目录下未找到bin\fglib5.dll"
				Call 库未找到对话框.ShowAsync()
				Exit Sub
			Else
				Try
					帧抓取器库 = New FgLib5(c.Path)
				Catch ex As Win32Exception
					系统错误对话框.Content = ex.Message & "：" & ex.ErrorCode
					Call 系统错误对话框.ShowAsync()
					Exit Sub
				End Try
			End If
			c = Await a.TryGetItemAsync("siso_hal.dll")
			If c Is Nothing Then
				库未找到对话框.Content = "指定的Silicon目录下未找到bin\siso_hal.dll"
				Call 库未找到对话框.ShowAsync()
				Exit Sub
			Else
				Try
					SisoHal = New SisoHal(c.Path)
				Catch ex As Win32Exception
					系统错误对话框.Content = ex.Message & "：" & ex.ErrorCode
					Call 系统错误对话框.ShowAsync()
					Exit Sub
				End Try
			End If
			Silicon运行时文件夹 = 文件夹
			Silicon运行时.Text = Silicon运行时文件夹.Path
			未来访问列表.AddOrReplace(未来访问列表_Silicon运行时, 文件夹)
		End If
	End Sub
	Private Async Sub 异步初始化()
		If 未来访问列表.ContainsItem(未来访问列表_Silicon运行时) Then 设置Silicon运行时文件夹(Await 未来访问列表.GetFolderAsync(未来访问列表_Silicon运行时))
		If 未来访问列表.ContainsItem(未来访问列表_临时文件) Then
			临时文件夹 = Await 未来访问列表.GetFolderAsync(未来访问列表_临时文件)
			临时文件.Text = 临时文件夹.Path
		End If
	End Sub
	Sub New()

		' 此调用是设计器所必需的。
		InitializeComponent()

		' 在 InitializeComponent() 调用之后添加任何初始化。
		Const 最小帧缓冲区数 As Byte = 2, 最大帧缓冲区数 As UInteger = 134217728
		Dim a As UInteger = 最小帧缓冲区数
		While a < 最大帧缓冲区数
			帧缓冲区数.Items.Add(a)
			a <<= 1
		End While
		Const 最小采集超时 As Byte = 2, 最大采集超时 As UInteger = 100000
		a = 最小采集超时
		While a < 最大采集超时
			采集超时.Items.Add(a)
			a <<= 1
		End While
		If 本地设置.ContainsKey(本地设置_帧缓冲区数) Then 帧缓冲区数.SelectedValue = 本地设置.Item(本地设置_帧缓冲区数)
		If 本地设置.ContainsKey(本地设置_采集超时) Then 采集超时.SelectedValue = 本地设置.Item(本地设置_采集超时)
		异步初始化()
		文件夹选取器.FileTypeFilter.Add("*")
	End Sub

End Class
