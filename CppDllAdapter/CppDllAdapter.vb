Imports System.Runtime.InteropServices, System.ComponentModel
''' <summary>
''' һ��Dll�ļ���.Net��ʾ
''' </summary>
Public Class CppDllAdapter
	Private Declare Function LoadLibraryW Lib "kernel32.dll" (<MarshalAs(UnmanagedType.LPWStr)> Dll·�� As String) As IntPtr
	Private Declare Function FreeLibrary Lib "kernel32.dll" (Dllָ�� As IntPtr) As Boolean
	ReadOnly Dllָ�� As IntPtr
	<DllImport("kernel32.dll", BestFitMapping:=False)> Private Shared Function GetProcAddress(Dllָ�� As IntPtr, Api���� As String) As IntPtr
	End Function
	''' <summary>
	''' ͨ��Dll�ļ���·������Adapter
	''' </summary>
	''' <param name="Dll·��">Dll�ļ�·��</param>
	Sub New(Dll·�� As String)
		Dllָ�� = LoadLibraryW(Dll·��)
		If Dllָ�� = IntPtr.Zero Then Throw New Win32Exception(Marshal.GetLastWin32Error, "�����ʧ��")
	End Sub
	''' <summary>
	''' ���ظ�������Api������ί�ж�������Ҫ���ж���һ������Ŀ�꺯��ǩ����ί�У���Ϊ�˷��������Ͳ�����
	''' </summary>
	''' <typeparam name="TDelegate">Ŀ�꺯����ί������</typeparam>
	''' <param name="Api����">Ŀ�꺯��������</param>
	''' <returns>ί�ж���</returns>
	Function GetProcDelegate(Of TDelegate)(Api���� As String) As TDelegate
		Dim a As IntPtr = GetProcAddress(Dllָ��, Api����)
		If a = IntPtr.Zero Then
			Throw New Win32Exception(Marshal.GetLastWin32Error, "���̼���ʧ��")
		Else
			Return Marshal.GetDelegateForFunctionPointer(Of TDelegate)(a)
		End If
	End Function
	''' <summary>
	''' �����д�˷�����Ӧ����������MyBase.Finalize()
	''' </summary>
	Protected Overrides Sub Finalize()
		FreeLibrary(Dllָ��)
		MyBase.Finalize()
	End Sub
End Class
