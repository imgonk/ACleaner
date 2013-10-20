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

''' <summary>
''' Directory property class
''' </summary>
''' <remarks></remarks>
Public Class DirectoryInfoA

#Region " Properties "
    Private _DirectoryPath As String
    ''' <summary>
    ''' Directory
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>Directory string</returns>
    ''' <remarks>N/A</remarks>
    Public Property DirectoryPath As String
        Get
            Return _DirectoryPath
        End Get
        Set(ByVal value As String)
            _DirectoryPath = value
        End Set
    End Property

    Private _DirectoryFilter As String = "*.*"
    ''' <summary>
    ''' Filter for the directory
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>Filter</returns>
    ''' <remarks>N/A</remarks>
    Public Property DirectoryFilter As String
        Get
            Return _DirectoryFilter
        End Get
        Set(ByVal value As String)
            _DirectoryFilter = value
        End Set
    End Property

    Private _DisplayName As String = String.Empty
    ''' <summary>
    ''' DisplayName for ListView
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>N/A</remarks>
    Public Property DisplayName As String
        Get
            Return _DisplayName
        End Get
        Set(ByVal value As String)
            _DisplayName = value
        End Set
    End Property

    Private _diImageIndex As Integer = 0
    ''' <summary>
    ''' DirectoryInfo Image Index
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>ImageIndex</returns>
    ''' <remarks>N/A</remarks>
    Public Property DiImageIndex As Integer
        Get
            Return _diImageIndex
        End Get
        Set(ByVal value As Integer)
            _diImageIndex = value
        End Set
    End Property

    Private _DirectoryIsRecursive As Boolean = False
    ''' <summary>
    ''' Returns True if directory scanning is recursive.
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>True/False</returns>
    ''' <remarks>N/A</remarks>
    Public Property DirectoryIsRecursive As Boolean
        Get
            Return _DirectoryIsRecursive
        End Get
        Set(ByVal value As Boolean)
            _DirectoryIsRecursive = value
        End Set
    End Property
#End Region

    Public Sub New(ByVal _DirectoryPath As String, ByVal _DisplayName As String, ByVal _DiImageIndex As Integer, Optional ByVal _DirectoryFilter As String = "*.*", Optional ByVal _DirectoryIsRecursive As Boolean = False)
        DirectoryPath = _DirectoryPath
        DirectoryFilter = _DirectoryFilter
        DirectoryIsRecursive = _DirectoryIsRecursive
        DisplayName = _DisplayName
        DiImageIndex = _DiImageIndex
    End Sub

End Class
