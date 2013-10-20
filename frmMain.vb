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

Imports System.Threading
Imports System.IO

Public Class frmMain
    Private clean As New UniducksCleaner
    Private hasTreeNodesExpanded As Boolean = False

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim info As New UniducksInformation

        For Each n As TreeNode In Me.tvwBrowsers.Nodes
            n.Checked = True
        Next
        For Each n As TreeNode In Me.tvwUserSystem.Nodes
            n.Checked = True
        Next
        For Each n As TreeNode In Me.tvwOther.Nodes
            n.Checked = True
        Next

        hasTreeNodesExpanded = True

        Me.tvwBrowsers.ExpandAll()
        Me.tvwUserSystem.ExpandAll()
        Me.tvwOther.ExpandAll()

        Me.tvwBrowsers.Nodes(0).EnsureVisible()
        Me.tvwUserSystem.Nodes(0).EnsureVisible()
        Me.tvwOther.Nodes(0).EnsureVisible()

        Me.lblInformation.Text = info.GetComputerInformation
    End Sub

    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        Me.lvwTemps.Items.Clear()
        Me.btnClean.Enabled = False
        Me.btnScan.Enabled = False
        Me.lblResults.Text = String.Empty

        Dim t As New Thread(AddressOf Initiate)
        t.Start()
    End Sub
    Private Sub btnClean_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClean.Click
        MessageBox.Show("Cleaning isn't done yet! I'm not focusing on cleaning until I get all the directories done!")

        Me.progressScanning.Style = ProgressBarStyle.Blocks
        Me.progressScanning.Value = 0

        Me.progressScanning.Maximum = Me.clean.FilesToDelete.Count

        For Each f In Me.clean.FilesToDelete
            Dim fi As New FileInfo(f)
            progressScanning.PerformStep()
        Next
    End Sub

    Private Sub Initiate()
        Me.progressScanning.InvokeThreadSafeMethod(Sub() Me.progressScanning.Style = ProgressBarStyle.Marquee)
        clean.Clear()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''' Internet Explorer ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(0, 0, Me.tvwBrowsers) Then
            If Directory.Exists(InternetExplorerCookies) Then
                Dim di As New DirectoryInfoA(InternetExplorerCookies, "Internet Explorer - Cookies", 3, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(InternetExplorerCookiesDomStore) Then
                Dim di As New DirectoryInfoA(InternetExplorerCookiesDomStore, "Internet Explorer - Cookies (DOM Store)", 3, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(0, 1, Me.tvwBrowsers) Then
            If Directory.Exists(InternetExplorerTemps) Then
                Dim di As New DirectoryInfoA(InternetExplorerTemps, "Internet Explorer - Temporary Interent Files", 3, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(0, 2, Me.tvwBrowsers) Then
            If Directory.Exists(InternetExplorerHistory) Then
                Dim di As New DirectoryInfoA(InternetExplorerHistory, "Internet Explorer - History", 3, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(0, 3, Me.tvwBrowsers) Then
            Dim ri As New RegistryInfoA("hkcu", InternetExplorerRecentlyTypedUrls, False, "Internet Explorer - Recently Typed URLs", 3)
            clean.Add(ri)
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Chrome '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(1, 0, Me.tvwBrowsers) Then
            If Directory.Exists(GoogleChromeCookiesLocalStorage) Then
                Dim di As New DirectoryInfoA(GoogleChromeCookiesLocalStorage, "Google Chrome - Cookies (Local Storage)", 1, "*.*", False)
                clean.Add(di)
            End If
            If Directory.Exists(GoogleChromeCookiesDBs) Then
                Dim di As New DirectoryInfoA(GoogleChromeCookiesDBs, "Google Chrome - Cookies (DB)", 1, "*.*", True)
                clean.Add(di)
            End If
            If File.Exists(GoogleChromeCookies) Then
                Dim fi As New FileInfoA(GoogleChromeCookies, "Google Chrome - Cookies", 1, "*.*")
                clean.Add(fi)
            End If
        End If
        If isTreeNodeChecked(1, 1, Me.tvwBrowsers) Then
            If Directory.Exists(GoogleChromeCache) Then
                Dim di As New DirectoryInfoA(GoogleChromeCache, "Google Chrome - Cache", 1, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(1, 2, Me.tvwBrowsers) Then
            If Directory.Exists(GoogleChromeInternetHistory) Then
                Dim di As New DirectoryInfoA(GoogleChromeInternetHistory, "Google Chrome - Internet History", 1, "*History*", False)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Firefox ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(2, 0, Me.tvwBrowsers) Then
            If File.Exists(MozillaFireFoxCookies) Then
                Dim fi As New FileInfoA(MozillaFireFoxCookies, "Mozilla Firefox - Cookies", 2, "*.*")
                clean.Add(fi)
            End If
            If File.Exists(MozillaFireFoxCookiesWebAppsStore) Then
                Dim fi As New FileInfoA(MozillaFireFoxCookiesWebAppsStore, "Mozilla Firefox - Cookies (Web Apps Store)", 2, "*.*")
                clean.Add(fi)
            End If
        End If
        If isTreeNodeChecked(2, 1, Me.tvwBrowsers) Then
            If File.Exists(MozillaFireFoxDownloads) Then
                Dim fi As New FileInfoA(MozillaFireFoxDownloads, "Mozilla Firefox - Download History", 2, "*.*")
                clean.Add(fi)
            End If
        End If
        If isTreeNodeChecked(2, 2, Me.tvwBrowsers) Then
            If File.Exists(MozillaFireFoxFormHistory) Then
                Dim fi As New FileInfoA(MozillaFireFoxFormHistory, "Mozilla Firefox - Form History", 2, "*.*")
                clean.Add(fi)
            End If
        End If
        If isTreeNodeChecked(2, 3, Me.tvwBrowsers) Then
            If Directory.Exists(MozillaFireFoxCache) Then
                Dim di As New DirectoryInfoA(MozillaFireFoxCache, "Mozilla Firefox - Cache", 2, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(2, 4, Me.tvwBrowsers) Then
            If File.Exists(MozillaFirefoxSessionStore) Then
                Dim fi As New FileInfoA(MozillaFirefoxSessionStore, "Mozilla Firefox - Session Store", 2, "*.*")
                clean.Add(fi)
            End If
            If File.Exists(MozillaFirefoxSessionStoreBackup) Then
                Dim fi As New FileInfoA(MozillaFirefoxSessionStoreBackup, "Mozilla Firefox - Session Store (Backup)", 2, "*.*")
                clean.Add(fi)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Safari '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(3, 0, Me.tvwBrowsers) Then
            If File.Exists(SafariCache) Then
                Dim fi As New FileInfoA(SafariCache, "Safari - Cache", 4, "*.*")
                clean.Add(fi)
            End If
        End If
        If isTreeNodeChecked(3, 1, Me.tvwBrowsers) Then
            If Directory.Exists(SafariHistory) Then
                Dim di As New DirectoryInfoA(SafariHistory, "Safari - History", 4, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(3, 2, Me.tvwBrowsers) Then
            If Directory.Exists(SafariWebpagePreviews) Then
                Dim di As New DirectoryInfoA(SafariWebpagePreviews, "Safari - Webpage Previews", 4, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(3, 3, Me.tvwBrowsers) Then
            If File.Exists(SafariHistoryDownloadsPlist) Then
                Dim fi As New DirectoryInfoA(SafariHistoryDownloadsPlist, "Safari - Download History", 4, "*.*")
                clean.Add(fi)
            End If
        End If
        If isTreeNodeChecked(3, 4, Me.tvwBrowsers) Then
            If File.Exists(SafariHistoryLastSessionPlist) Then
                Dim fi As New DirectoryInfoA(SafariHistoryLastSessionPlist, "Safari - Last Session", 4, "*.*")
                clean.Add(fi)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Opera ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(4, 0, Me.tvwBrowsers) Then
            If Directory.Exists(OperaCache) Then
                Dim di As New DirectoryInfoA(OperaCache, "Opera - Cache", 5, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(OperaOPCache) Then
                Dim di As New DirectoryInfoA(OperaOPCache, "Opera - Cache (OP)", 5, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(OperaIconCache) Then
                Dim di As New DirectoryInfoA(OperaIconCache, "Opera - Cache (Icons)", 5, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(4, 1, Me.tvwBrowsers) Then
            If Directory.Exists(OperaInternetHistory_1) Then
                Dim di As New DirectoryInfoA(OperaInternetHistory_1, "Opera - Internet History (1)", 5, "*.*", True)
                clean.Add(di)
            End If
            If File.Exists(OperaInternetHistory_2) Then
                Dim fi As New FileInfoA(OperaInternetHistory_2, "Opera - Internet History (2)", 5, "*.*")
                clean.Add(fi)
            End If
            If File.Exists(OperaInternetHistory_3) Then
                Dim fi As New FileInfoA(OperaInternetHistory_3, "Opera - Internet History (3)", 5, "*.*")
                clean.Add(fi)
            End If
            If File.Exists(OperaInternetHistory_4) Then
                Dim fi As New FileInfoA(OperaInternetHistory_4, "Opera - Internet History (4)", 5, "*.*")
                clean.Add(fi)
            End If
            If File.Exists(OperaInternetHistory_5) Then
                Dim fi As New FileInfoA(OperaInternetHistory_5, "Opera - Internet History (5)", 5, "*.*")
                clean.Add(fi)
            End If
            If File.Exists(OperaInternetHistory_6) Then
                Dim fi As New FileInfoA(OperaInternetHistory_6, "Opera - Internet History (6)", 5, "*.*")
                clean.Add(fi)
            End If
        End If
        If isTreeNodeChecked(4, 2, Me.tvwBrowsers) Then
            If File.Exists(OperaCookies) Then
                Dim fi As New FileInfoA(OperaCookies, "Opera - Cookies", 5, "*.*")
                clean.Add(fi)
            End If
        End If
        If isTreeNodeChecked(4, 3, Me.tvwBrowsers) Then
            If Directory.Exists(OperaWebsiteIcon) Then
                Dim di As New DirectoryInfoA(OperaWebsiteIcon, "Opera - Website Icons", 5, "*.*", False)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(4, 4, Me.tvwBrowsers) Then
            If Directory.Exists(OperaCacheTempDownloads) Then
                Dim di As New DirectoryInfoA(OperaCacheTempDownloads, "Opera - Download History", 5, "*.*", True)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Explorer '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(0, 0, Me.tvwUserSystem) Then
            If Directory.Exists(WindowsExplorerRecent) Then
                Dim di As New DirectoryInfoA(WindowsExplorerRecent, "Windows Explorer - Recent Documents", 6, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(0, 1, Me.tvwUserSystem) Then
            If Directory.Exists(WindowsExplorerThumbnailCache) Then
                Dim di As New DirectoryInfoA(WindowsExplorerThumbnailCache, "Windows Explorer - Thumbnail Cache", 6, "*.*", True)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' System '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(1, 0, Me.tvwUserSystem) Then
            If Directory.Exists(WindowsTempFiles) Then
                Dim di As New DirectoryInfoA(WindowsTempFiles, "System - Windows Temp Files", 7, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(1, 1, Me.tvwUserSystem) Then
            If Directory.Exists(UserTemps) Then
                Dim di As New DirectoryInfoA(UserTemps, "System - User Temp Files", 7, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(1, 2, Me.tvwUserSystem) Then
            If Directory.Exists(MiniDumps) Then
                Dim di As New DirectoryInfoA(MiniDumps, "System - Memory Dumps", 7, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(1, 3, Me.tvwUserSystem) Then
            If Directory.Exists(ChkDskFileFragments) Then
                Dim di As New DirectoryInfoA(ChkDskFileFragments, "System - ChkDsk File Fragments", 7, "*.chk", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(1, 4, Me.tvwUserSystem) Then
            If Directory.Exists(WindowsLogFiles) Then
                Dim di As New DirectoryInfoA(WindowsLogFiles, "System - Windows Log Files", 7, "*.log", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(1, 5, Me.tvwUserSystem) Then
            If Directory.Exists(WindowsErrorReporting_1) Then
                Dim di As New DirectoryInfoA(WindowsErrorReporting_1, "System - Windows Error Reporting - 1", 7, "*.log", True)
                clean.Add(di)
            End If
            If Directory.Exists(WindowsErrorReporting_2) Then
                Dim di As New DirectoryInfoA(WindowsErrorReporting_2, "System - Windows Error Reporting - 2", 7, "*.log", True)
                clean.Add(di)
            End If
            If Directory.Exists(WindowsErrorReporting_3) Then
                Dim di As New DirectoryInfoA(WindowsErrorReporting_3, "System - Windows Error Reporting - 3", 7, "*.log", True)
                clean.Add(di)
            End If
            If Directory.Exists(WindowsErrorReporting_4) Then
                Dim di As New DirectoryInfoA(WindowsErrorReporting_4, "System - Windows Error Reporting - 4", 7, "*.log", True)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Advanced '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(2, 0, Me.tvwUserSystem) Then
            If Directory.Exists(PrefetchData) Then
                Dim di As New DirectoryInfoA(PrefetchData, "Advanced - Prefetch Data", 8, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(2, 1, Me.tvwUserSystem) Then
            If Directory.Exists(IISLogs_1) Then
                Dim di As New DirectoryInfoA(IISLogs_1, "Advanced - IIS Logs", 8, "*.log", True)
                clean.Add(di)
                Dim di2 As New DirectoryInfoA(IISLogs_1, "Advanced - IIS Logs Files - ETL", 8, "*.etl", True)
                clean.Add(di2)
            End If
            If Directory.Exists(IISLogs_2) Then
                Dim di As New DirectoryInfoA(IISLogs_2, "Advanced - IIS Logs Files - 2", 8, "*.log", True)
                clean.Add(di)
                Dim di2 As New DirectoryInfoA(IISLogs_2, "Advanced - IIS Logs Files - 2 - ETL", 8, "*.etl", True)
                clean.Add(di2)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''' uTorrent '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(0, 0, Me.tvwOther) Then
            If Directory.Exists(uTorrentDllImageCache) Then
                Dim di As New DirectoryInfoA(uTorrentDllImageCache, "uTorrent - Image Cache", 9, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(uTorrentTempFiles) Then
                Dim di As New DirectoryInfoA(uTorrentTempFiles, "uTorrent - Temp Files", 9, "*.*", True)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Sun Java '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(0, 1, Me.tvwOther) Then
            If Directory.Exists(SunJavaCache) Then
                Dim di As New DirectoryInfoA(SunJavaCache, "Sun Java - Cache", 9, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(SunJavaSystemCache) Then
                Dim di As New DirectoryInfoA(SunJavaSystemCache, "Sun Java - System Cache", 9, "*.*", True)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''' FileZilla '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(0, 2, Me.tvwOther) Then
            If File.Exists(FileZillaRecentServers) Then
                Dim fi As New FileInfoA(FileZillaRecentServers, "FileZilla - Recent Servers", 9, "*.*")
                clean.Add(fi)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Paint.NET '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(1, 0, Me.tvwOther) Then
            If Directory.Exists(PaintNet) Then
                Dim di As New DirectoryInfoA(PaintNet, "Paint.NET - Temp Files", 10, "*.*", True)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''' Win Media Player '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(2, 0, Me.tvwOther) Then
            If Directory.Exists(WindowsMediaPlayer) Then
                Dim di As New DirectoryInfoA(WindowsMediaPlayer, "Windows Media Player - Temp Files", 11, "*.*", True)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''' QuickTime Player '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(2, 1, Me.tvwOther) Then
            If Directory.Exists(QuickTimePlayerCache) Then
                Dim di As New DirectoryInfoA(QuickTimePlayerCache, "Quick Time Player - Cache", 11, "*.*", True)
                clean.Add(di)
            End If
            If File.Exists(QuickTimerPlayer) Then
                Dim fi As New FileInfoA(QuickTimerPlayer, "Quick Time Player - Player Session", 11, "*.*")
                clean.Add(fi)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''' Adobe Flash Player '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(2, 2, Me.tvwOther) Then
            If Directory.Exists(AdobeFlashPlayer) Then
                Dim di As New DirectoryInfoA(AdobeFlashPlayer, "Adobe Flash Player - Temp Files", 11, "*.*", True)
                clean.Add(di)
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' AVG Antivirus '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(3, 0, Me.tvwOther) Then
            If Directory.Exists(AvgAntivirus10Log) Then
                Dim di As New DirectoryInfoA(AvgAntivirus10Log, "AVG Antivirus - Log Files", 12, "*.log*", True)
                clean.Add(di)
                Dim di2 As New DirectoryInfoA(AvgAntivirus10Log, "AVG Antivirus - Log Files - XML", 12, "*.xml*", True)
                clean.Add(di2)
            End If
            If Directory.Exists(AvgAntivirus10Backup) Then
                Dim di As New DirectoryInfoA(AvgAntivirus10Backup, "AVG Antivirus - Backup Files", 12, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(AvgAntivirus10Misc) Then
                Dim di As New DirectoryInfoA(AvgAntivirus10Misc, "AVG Antivirus - Misc Files", 12, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(AvgAntivirus10Temps) Then
                Dim di As New DirectoryInfoA(AvgAntivirus10Misc, "AVG Antivirus - Temp Files", 12, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(3, 1, Me.tvwOther) Then
            If Directory.Exists(MalwareBytesLogs) Then
                Dim di As New DirectoryInfoA(MalwareBytesLogs, "MalwareBytes - Logs", 12, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(MalwareBytesQuarantineBackup) Then
                Dim di As New DirectoryInfoA(MalwareBytesQuarantineBackup, "MalwareBytes - Quarantine", 12, "QUAR", True)
                clean.Add(di)
                Dim di2 As New DirectoryInfoA(MalwareBytesQuarantineBackup, "MalwareBytes - Backup Files", 12, "BACKUP", True)
                clean.Add(di2)
            End If
        End If
        If isTreeNodeChecked(3, 2, Me.tvwOther) Then
            If Directory.Exists(WindowsDefenderHistoryResource) Then
                Dim di As New DirectoryInfoA(WindowsDefenderHistoryResource, "Windows Defender - History Resource", 12, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(WindowsDefenderHistoryQuick) Then
                Dim di As New DirectoryInfoA(WindowsDefenderHistoryQuick, "Windows Defender - Quick History", 12, "*.*", True)
                clean.Add(di)
            End If
        End If
        If isTreeNodeChecked(3, 3, Me.tvwOther) Then
            If Directory.Exists(SpybotSdLogs) Then
                Dim di As New DirectoryInfoA(SpybotSdLogs, "Spybot S&D - Logs", 12, "*.*", True)
                clean.Add(di)
            End If
            If Directory.Exists(SpybotSdBackups) Then
                Dim di As New DirectoryInfoA(SpybotSdBackups, "Spybot S&D - Backups", 12, "*.*", True)
                clean.Add(di)
            End If
            If File.Exists(SpybotSdIni) Then
                Dim fi As New FileInfoA(SpybotSdIni, "Spybot S&D - Statistics", 12, "*.*")
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' Visual Studio '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If isTreeNodeChecked(4, 0, Me.tvwOther) Then
            Dim ri As New RegistryInfoA("hkcu", VisualStudio2010FileMRUList, False, "Visual Studio - VS 2010 File MRU List", 13)
            Dim ri2 As New RegistryInfoA("hkcu", VisualStudio2010ProjectMRUList, False, "Visual Studio - VS 2010 Project MRU List", 13)

            clean.Add(ri)
            clean.Add(ri2)
        End If
        If isTreeNodeChecked(4, 1, Me.tvwOther) Then
            Dim ri As New RegistryInfoA("hkcu", VisualStudio2008FileMRUList, False, "Visual Studio - VS 2008 File MRU List", 13)
            Dim ri2 As New RegistryInfoA("hkcu", VisualStudio2008ProjectMRUList, False, "Visual Studio - VS 2008 Project MRU List", 13)

            clean.Add(ri)
            clean.Add(ri2)
        End If
        If isTreeNodeChecked(4, 2, Me.tvwOther) Then
            Dim ri As New RegistryInfoA("hkcu", VisualStudio2005FileMRUList, False, "Visual Studio - VS 2005 File MRU List", 13)
            Dim ri2 As New RegistryInfoA("hkcu", VisualStudio2005ProjectMRUList, False, "Visual Studio - VS 2005 Project MRU List", 13)

            clean.Add(ri)
            clean.Add(ri2)
        End If

        clean.Scan(lvwTemps)

        Me.progressScanning.InvokeThreadSafeMethod(Sub() Me.progressScanning.Style = ProgressBarStyle.Blocks)

        Me.btnScan.InvokeThreadSafeMethod(Sub() Me.btnScan.Enabled = True)
        Me.btnClean.InvokeThreadSafeMethod(Sub() Me.btnClean.Enabled = True)

        Me.progressScanning.InvokeThreadSafeMethod(Sub() Me.progressScanning.PerformStep())

        Me.lblResults.InvokeThreadSafeMethod(Sub() Me.lblResults.Text = "Initial Scan Size      " & clean.DisplaySize & Environment.NewLine & "Initial File Count       " & CStr(clean.FileCount))

    End Sub

    Private Function isTreeNodeChecked(ByVal indexParent As Integer, ByVal indexChild As Integer, ByVal tvw As TreeView) As Boolean
        For Each tn As TreeNode In tvw.GetThreadSafeProperty(Function() tvw.Nodes)
            If tn.Checked And tn.Index = indexParent Then
                For Each tn2 As TreeNode In tn.Nodes
                    If tn2.Index = indexChild Then
                        If tn2.Checked Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                Next
            End If
        Next
    End Function

    Private Sub tvwBrowsers_AfterCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwBrowsers.AfterCheck, tvwUserSystem.AfterCheck, tvwOther.AfterCheck
        If e.Node.Checked Then
            For Each n As TreeNode In e.Node.Nodes
                n.Checked = True
            Next
        Else
            If hasTreeNodesExpanded Then
                For Each n As TreeNode In e.Node.Nodes
                    n.Checked = False
                Next
            End If
        End If
    End Sub
End Class
