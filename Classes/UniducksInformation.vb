'ACleaner - Temp file scanning and cleaning software
'Copyright (C) 2011  Ernest Jr
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program.  If not, see <http://www.gnu.org/licenses/>

Imports Microsoft.Win32
Imports System.Management

Public Class UniducksInformation

    Public Function GetComputerInformation() As String
        Return getOS() & ", " & getProcessor() & ", " & returnSize(CInt(My.Computer.Info.TotalPhysicalMemory)) & ", " & GetGraphicsCardName()
    End Function

    Private Function getOS() As String
        Return My.Computer.Info.OSFullName & getServicePack.ToString
    End Function

    Private Function getProcessor() As String
        Dim SoftwareKey As String = "HARDWARE\DESCRIPTION\System\CentralProcessor\0"
        Using rk As RegistryKey = Registry.LocalMachine.OpenSubKey(SoftwareKey)
            Dim propertiesKey As RegistryKey = Registry.LocalMachine.OpenSubKey(SoftwareKey, False)

            Dim name = propertiesKey.GetValue("ProcessorNameString")
            Return CStr(name)
            propertiesKey.Close()
        End Using
    End Function

    Private Function GetGraphicsCardName() As String
        Dim GraphicsCardName = String.Empty
        Try
            Dim WmiSelect As New ManagementObjectSearcher _
            ("root\CIMV2", "SELECT * FROM Win32_VideoController")

            For Each WmiResults As ManagementObject In WmiSelect.Get()
                GraphicsCardName = CStr(WmiResults.GetPropertyValue("Name"))

                If (Not String.IsNullOrEmpty(GraphicsCardName)) Then
                    Exit For
                End If
            Next

        Catch ex As ManagementException

        End Try

        Return GraphicsCardName

    End Function

    Public Function returnSize(ByVal int As Integer) As String
        Dim KiloBytes As Integer = CInt(int / 1024)
        Dim MegaBytes As Integer = CInt(KiloBytes / 1024)
        Dim GigBytes As Integer = CInt(MegaBytes / 1024)
        If KiloBytes >= 1024 Then
            If MegaBytes >= 1024 Then
                'GigaBytes
                Return Math.Round(GigBytes) & " GB".ToString
            Else
                'Megabytes
                Return Math.Round(MegaBytes) & " MB".ToString
            End If
        Else
            'Kilobytes
            Return Math.Round(KiloBytes) & " KB".ToString
        End If
    End Function

    Private Declare Function GetVersionExA Lib "kernel32" (ByRef lpVersionInformation As OSVERSIONINFO) As Short
    Private Function getServicePack() As String
        Dim osinfo As OSVERSIONINFO
        Dim retvalue As Short
        osinfo.dwOSVersionInfoSize = 148
        retvalue = GetVersionExA(osinfo)
        If Len(osinfo.szCSDVersion) = 0 Then
            Return ("")
        Else
            Return " - " & (CStr(osinfo.szCSDVersion))
        End If
    End Function
    Private Structure OSVERSIONINFO
        Dim dwOSVersionInfoSize As Integer
        Dim dwMajorVersion As Integer
        Dim dwMinorVersion As Integer
        Dim dwBuildNumber As Integer
        Dim dwPlatformId As Integer
        <VBFixedString(128), _
          System.Runtime.InteropServices.MarshalAs _
               (System.Runtime.InteropServices.UnmanagedType.ByValTStr, _
            SizeConst:=128)> Dim szCSDVersion As String

    End Structure
End Class
