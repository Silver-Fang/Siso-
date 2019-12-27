Imports Siso库
Module 主模块
	WithEvents Camera As CameraPort, Video As AviRef
	Async Sub Main()
		'Static 命令 As String()
		Dim Client As MicroDisplay = New MicroDisplay("C:\Program Files\SiliconSoftware\Runtime5.3.200")
		Dim Board As SisoBoard = Client.Boards(0)
		Camera = Board.InitFrameGrabber(Board.Applets.FindApplet("")).CameraPortA
		Camera.分配内存(20)
		Video = Client.CreateAvi("", 1024, 1024, 60)
		Camera.BeginAcquire(10)
		Console.Write("抓取中，按回车停止")
		Console.ReadLine()
		Await Camera.StopAcquire()
		Video.CloseAvi()
	End Sub

	Private Sub Camera_ImageAcquired(PicNr As Frameindex_t, ImagePtr As IntPtr, Timestamp As Long) Handles Camera.ImageAcquired
		Video.WriteAviPicture(PicNr, ImagePtr)
	End Sub
End Module