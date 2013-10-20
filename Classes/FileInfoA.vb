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

Public Class FileInfoA
#Region " Properties "
    Private _FilePath As String
    ''' <summary>
    ''' File
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>File string.</returns>
    ''' <remarks>N/A</remarks>
    Public Property FilePath As String
        Get
            Return _FilePath
        End Get
        Set(ByVal value As String)
            _FilePath = value
        End Set
    End Property

    Private _FileFilter As String = "*.*"
    ''' <summary>
    ''' Filter for the file.
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>Filter</returns>
    ''' <remarks>N/A</remarks>
    Public Property FileFilter As String
        Get
            Return _FileFilter
        End Get
        Set(ByVal value As String)
            _FileFilter = value
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

    Private _FiImageIndex As Integer = 0
    ''' <summary>
    ''' FileInfoA Image Index
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>ImageIndex</returns>
    ''' <remarks>N/A</remarks>
    Public Property FiImageIndex As Integer
        Get
            Return _FiImageIndex
        End Get
        Set(ByVal value As Integer)
            _FiImageIndex = value
        End Set
    End Property
#End Region

    Public Sub New(ByVal _FilePath As String, ByVal _DisplayName As String, ByVal _FiImageIndex As Integer, Optional ByVal _FileFilter As String = "*.*")
        FilePath = _FilePath
        FileFilter = _FileFilter
        DisplayName = _DisplayName
        FiImageIndex = _FiImageIndex
    End Sub
End Class
