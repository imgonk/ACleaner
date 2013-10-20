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
Option Strict On

Imports System.IO
Imports Microsoft.Win32

''' <summary>
''' File and directory cleaning class.
''' </summary>
''' <remarks></remarks>
Public Class UniducksCleaner

#Region " Properties "
    Private _DirectoryList As New List(Of DirectoryInfoA)
    ''' <summary>
    ''' List of directories.
    ''' </summary>
    ''' <value>List</value>
    ''' <returns>List(Of FileDirectoryInfo)</returns>
    ''' <remarks>N/A</remarks>
    Private Property Directories As List(Of DirectoryInfoA)
        Get
            Return _DirectoryList
        End Get
        Set(ByVal value As List(Of DirectoryInfoA))
            _DirectoryList = value
        End Set
    End Property

    Private _FileList As New List(Of FileInfoA)
    ''' <summary>
    ''' List of files.
    ''' </summary>
    ''' <value>List</value>
    ''' <returns>List(Of FileDirectoryInfo)</returns>
    ''' <remarks>N/A</remarks>
    Private Property Files As List(Of FileInfoA)
        Get
            Return _FileList
        End Get
        Set(ByVal value As List(Of FileInfoA))
            _FileList = value
        End Set
    End Property

    Private _Registry As New List(Of RegistryInfoA)
    ''' <summary>
    ''' List of registry entries
    ''' </summary>
    ''' <value>List</value>
    ''' <returns>List(Of RegistryInfo)</returns>
    ''' <remarks>N/A</remarks>
    Private Property [Registry] As List(Of RegistryInfoA)
        Get
            Return _Registry
        End Get
        Set(ByVal value As List(Of RegistryInfoA))
            _Registry = value
        End Set
    End Property

    Private _DisplayName As String = String.Empty
    ''' <summary>
    ''' Name to display in ListView
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>N/A</remarks>
    Private Property DisplayName As String
        Get
            Return _DisplayName
        End Get
        Set(ByVal value As String)
            _DisplayName = value
        End Set
    End Property

    Private _FilesToDelete As New List(Of String)
    ''' <summary>
    ''' Files and directories to delete
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>List(Of String)</returns>
    ''' <remarks>N/A</remarks>
    Public Property FilesToDelete As List(Of String)
        Get
            Return _FilesToDelete
        End Get
        Set(ByVal value As List(Of String))
            _FilesToDelete = value
        End Set
    End Property

    Private _RegistryValuesToDelete As New List(Of RegistryInfoA)
    ''' <summary>
    ''' Registry values to delete
    ''' </summary>
    ''' <value>RegistryInfoA</value>
    ''' <returns>List(Of RegistryInfoA)</returns>
    ''' <remarks>N/A</remarks>
    Public Property RegistryValuesToDelete As List(Of RegistryInfoA)
        Get
            Return _RegistryValuesToDelete
        End Get
        Set(ByVal value As List(Of RegistryInfoA))
            _RegistryValuesToDelete = value
        End Set
    End Property

    Private _Size As UInt64 = 0
    ''' <summary>
    ''' Internal property used to get total size of files scanned in bytes
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Size As UInt64
        Get
            Return _Size
        End Get
        Set(ByVal value As UInt64)
            _Size = value
        End Set
    End Property

    Private _DisplaySize As String = String.Empty
    ''' <summary>
    ''' Size property in UniduckCleaner class returns size as UInt64 and this property returns it as a string.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DisplaySize As String
        Get
            Return returnSize(Me.Size)
        End Get
        Set(ByVal value As String)
            _DisplaySize = value
        End Set
    End Property

    Private _FileCount As Integer = 0
    ''' <summary>
    ''' Returns total number of files scanned
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    ''' <remarks>N/A</remarks>
    Public Property FileCount As Integer
        Get
            Return _FileCount
        End Get
        Set(ByVal value As Integer)
            _FileCount = value
        End Set
    End Property
#End Region

#Region " Constructors "
    Public Sub New()

    End Sub
#End Region

#Region " Methods "
    ''' <summary>
    ''' Adds a DirectoryInfoA class and adds it to one of the appropriate list.
    ''' </summary>
    ''' <param name="di">DirectoryInfo</param>
    ''' <remarks>N/A</remarks>
    Public Sub Add(ByVal di As DirectoryInfoA)
        Directories.Add(di)
    End Sub

    ''' <summary>
    ''' Adds a FileInfoA class into the appropriate list
    ''' </summary>
    ''' <param name="fi"></param>
    ''' <remarks></remarks>
    Public Sub Add(ByVal fi As FileInfoA)
        Files.Add(fi)
    End Sub

    ''' <summary>
    ''' Adds a RegistryInfoA class into the appropriate list.
    ''' </summary>
    ''' <param name="ri">RegistryInfo</param>
    ''' <remarks>N/A</remarks>
    Public Sub Add(ByVal ri As RegistryInfoA)
        Registry.Add(ri)
    End Sub

    ''' <summary>
    ''' Start the scanning process
    ''' </summary>
    ''' <param name="lvw">ListView</param>
    ''' <remarks></remarks>
    Public Sub Scan(ByVal lvw As ListView)
        If Directories.Count = 0 And Files.Count = 0 And Registry.Count = 0 Then
            MessageBox.Show("Whoah, Jim! What's going on, buddy? You can't start the scanning process without choosing some items to scan!",
                            "Stop... In the Name of Love...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            If Directories.Count > 0 Then
                scan_Directories(lvw)
            End If
            If Files.Count > 0 Then
                scan_Files(lvw)
            End If
            If Registry.Count > 0 Then
                scan_Registry(lvw)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Scan directories
    ''' </summary>
    ''' <param name="lvw">ListView</param>
    ''' <remarks>N/A</remarks>
    Private Sub scan_Directories(ByVal lvw As ListView)
        For Each di As DirectoryInfoA In Directories
            Try
                MasterScanner("d", lvw, di)
            Catch ex As AccessViolationException

            End Try
        Next
    End Sub

    ''' <summary>
    ''' Scan files
    ''' </summary>
    ''' <param name="lvw">ListView</param>
    ''' <remarks>N/A</remarks>
    Private Sub scan_Files(ByVal lvw As ListView)
        For Each fi As FileInfoA In Files
            MasterScanner("f", lvw, Nothing, fi)
        Next
    End Sub

    ''' <summary>
    ''' Scan registry keys
    ''' </summary>
    ''' <param name="lvw">ListView</param>
    ''' <remarks>N/A</remarks>
    Private Sub scan_Registry(ByVal lvw As ListView)
        For Each ri As RegistryInfoA In Registry
            MasterScanner("r", lvw, Nothing, Nothing, ri)
        Next
    End Sub

    Private Function MasterDirectoryScanner(ByVal DirectroyInfos As DirectoryInfo, ByVal filter As String) As List(Of UInt64)
        Dim FileInformation As New List(Of UInt64)

        Dim file_size As UInt64 = 0
        Dim file_count As Integer = 0

        For Each fi In DirectroyInfos.GetFiles(filter)
            Try
                file_count = file_count + 1
                file_size = CULng(file_size + fi.Length)
                FilesToDelete.Add(fi.FullName)
            Catch ex As UnauthorizedAccessException
                'There's really no pretty way to handle this exception
            Catch ex As FileNotFoundException
                'There's really no pretty way to handle this exception
            End Try
        Next

        For Each di As DirectoryInfo In DirectroyInfos.GetDirectories
            MasterDirectoryScanner(di, filter)
        Next

        FileInformation.Add(CULng(file_count))
        FileInformation.Add(file_size)

        Return FileInformation
    End Function

    Private Sub MasterScanner(ByVal FileRegOrDirectory As String, ByVal lvw As ListView, Optional ByVal di As DirectoryInfoA = Nothing, Optional ByVal fi As FileInfoA = Nothing, Optional ByVal ri As RegistryInfoA = Nothing)
        Dim file_size As UInt64 = 0
        Dim file_count As Integer = 0

        Select Case FileRegOrDirectory.ToLower
            Case "f"
                Dim _fi As New FileInfo(fi.FilePath)
                Try
                    file_count = file_count + 1
                    file_size = CULng(file_size + _fi.Length)
                    FilesToDelete.Add(_fi.FullName)
                Catch ex As UnauthorizedAccessException
                    'There's really no pretty way to handle this exception
                Catch ex As FileNotFoundException
                    'There's really no pretty way to handle this exception
                End Try

                If Not file_count <= 0 Then
                    Dim lvi As New ListViewItem
                    lvi.Text = fi.DisplayName
                    lvi.SubItems.Add(CStr(file_count))
                    lvi.SubItems.Add(CStr(returnSize(file_size)))
                    lvi.ImageIndex = fi.FiImageIndex

                    lvw.InvokeThreadSafeMethod(Sub() lvw.Items.Add(lvi))
                End If
            Case "d"
                If di.DirectoryIsRecursive Then
                    'Normally this is done at the end of this method, but we need to do it here because the data isn't accessible
                    'to it, when scanning with this method.
                    Dim tempFileCount As UInt64 = MasterDirectoryScanner(New DirectoryInfo(di.DirectoryPath), di.DirectoryFilter).Item(0)
                    Dim tempSize As UInt64 = MasterDirectoryScanner(New DirectoryInfo(di.DirectoryPath), di.DirectoryFilter).Item(1)

                    If Not tempFileCount <= 0 Then
                        Dim lvi As New ListViewItem
                        lvi.Text = di.DisplayName
                        lvi.SubItems.Add(CStr(tempFileCount))
                        lvi.SubItems.Add(returnSize(tempSize))
                        lvi.ImageIndex = di.DiImageIndex
                        lvw.InvokeThreadSafeMethod(Sub() lvw.Items.Add(lvi))

                        Me.FileCount = CInt(Me.FileCount + tempFileCount)
                        Me.Size = Me.Size + tempSize
                    End If
                Else
                    Dim current_directory As New DirectoryInfo(di.DirectoryPath)
                    For Each _fi In current_directory.GetFiles(di.DirectoryFilter)
                        Try
                            file_count = file_count + 1
                            file_size = CULng(file_size + _fi.Length)
                            FilesToDelete.Add(_fi.FullName)
                        Catch ex As UnauthorizedAccessException
                            'There's really no pretty way to handle this exception
                        Catch ex As FileNotFoundException
                            'There's really no pretty way to handle this exception
                        End Try
                    Next

                    If Not file_count <= 0 Then
                        Dim lvi As New ListViewItem
                        lvi.Text = di.DisplayName
                        lvi.SubItems.Add(CStr(file_count))
                        lvi.SubItems.Add(CStr(returnSize(file_size)))
                        lvi.ImageIndex = di.DiImageIndex
                        lvw.InvokeThreadSafeMethod(Sub() lvw.Items.Add(lvi))
                    End If
                End If
            Case "r"
                If ri.Recursive Then

                Else
                    Dim root_key As RegistryKey

                    Select Case ri.RegistryRoot.ToLower
                        Case "hkcr"
                            root_key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ri.RegistryPath, False)
                        Case "hkcu"
                            root_key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(ri.RegistryPath, False)
                        Case "hklm"
                            root_key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(ri.RegistryPath, False)
                        Case "hku"
                            root_key = Microsoft.Win32.Registry.Users.OpenSubKey(ri.RegistryPath, False)
                        Case "hkcc"
                            root_key = Microsoft.Win32.Registry.CurrentConfig.OpenSubKey(ri.RegistryPath, False)
                        Case Else
                            root_key = Nothing
                    End Select

                    If Not root_key Is Nothing Then
                        For Each keyname As String In root_key.GetValueNames()
                            Dim riToDelete As New RegistryInfoA(ri.RegistryRoot, ri.RegistryPath, Nothing, ri.DisplayName, ri.RiImageIndex, keyname)
                            RegistryValuesToDelete.Add(riToDelete)
                            file_count = file_count + 1
                        Next

                        If Not file_count <= 0 Then
                            Dim lvi As New ListViewItem
                            lvi.Text = ri.DisplayName
                            lvi.SubItems.Add(CStr(file_count))
                            lvi.SubItems.Add(returnSize(file_size))
                            lvi.ImageIndex = ri.RiImageIndex
                            lvw.InvokeThreadSafeMethod(Sub() lvw.Items.Add(lvi))
                        End If
                    End If

                End If
        End Select

        Me.Size = Me.Size + file_size
        Me.FileCount = Me.FileCount + file_count
    End Sub

    ''' <summary>
    ''' Clears all FileDirectoryInfos, RegistryInfos and files to delete
    ''' </summary>
    ''' <remarks>N/A</remarks>
    Public Sub Clear()
        For i As Integer = Directories.Count - 1 To 0 Step -1
            If Directories.Count > 0 Then
                Directories.RemoveAt(i)
            End If
        Next
        For i As Integer = Files.Count - 1 To 0 Step -1
            If Files.Count > 0 Then
                Files.RemoveAt(i)
            End If
        Next
        For i As Integer = Registry.Count - 1 To 0 Step -1
            If Registry.Count > 0 Then
                Registry.RemoveAt(i)
            End If
        Next
        For i As Integer = FilesToDelete.Count - 1 To 0 Step -1
            If FilesToDelete.Count > 0 Then
                FilesToDelete.RemoveAt(i)
            End If
        Next

        Me.Size = 0
        Me.FileCount = 0
    End Sub

    Private Function returnSize(ByVal int As UInt64) As String
        Dim KiloBytes As Integer = CInt(int / 1024)
        Dim MegaBytes As Integer = CInt(KiloBytes / 1024)
        Dim GigBytes As Integer = CInt(MegaBytes / 1024)
        If KiloBytes >= 1024 Then
            If MegaBytes >= 1024 Then
                'GigaBytes
                Return CStr(Math.Round(GigBytes) & " GB")
            Else
                'Megabytes
                Return CStr(Math.Round(MegaBytes) & " MB")
            End If
        Else
            'Kilobytes
            Return CStr(Math.Round(KiloBytes) & " KB")
        End If
    End Function
#End Region

End Class
