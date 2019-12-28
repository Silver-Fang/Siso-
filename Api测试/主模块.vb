Imports Siso库
Module 主模块
	WithEvents Camera As CameraPort, Video As AviRef
	Sub Main()
		'Static 命令 As String()
		Dim Client As MicroDisplay = New MicroDisplay("C:\Program Files\SiliconSoftware\Runtime5.3.200", New Guid("{4d36e971-e325-11ce-bfc1-08002be10318}"))
		Dim Board As SisoBoard = Client.Boards(0)
		Camera = Board.InitFrameGrabber(Board.Applets.FindApplet("C:\Program Files\SiliconSoftware\Runtime5.3.200\Dll\mE4AD4-CL\Acq_FullAreaGray10.dll")).CameraPortA
		Camera.分配内存(20)
		Video = Client.CreateAvi("D:\VTF", 1024, 1024, 60)
		Camera.BeginAcquire(10)
		Console.Write("抓取中，按回车停止")
		Console.ReadLine()
		Camera.StopAcquire().Wait()
		Video.CloseAvi()
	End Sub

	Private Sub Camera_ImageAcquired(PicNr As Frameindex_t, ImagePtr As IntPtr, Timestamp As Long) Handles Camera.ImageAcquired
		Video.WriteAviPicture(PicNr, ImagePtr)
	End Sub
End Module