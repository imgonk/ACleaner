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
''' Registry property class.
''' </summary>
''' <remarks></remarks>
Public Class RegistryInfoA
    Private _RegistryRoot As String = String.Empty
    ''' <summary>
    ''' Gets and sets a property indicating the root registry key to scan. "hkcr, hkcu, hklm, hku, hkcc"
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String.ToLower</returns>
    ''' <remarks>N/A</remarks>
    Public Property RegistryRoot As String
        Get
            Return _RegistryRoot.ToLower
        End Get
        Set(ByVal value As String)
            _RegistryRoot = value
        End Set
    End Property

    Private _RegistryPath As String = String.Empty
    ''' <summary>
    ''' Gets and sets a property indicating the registry path to scan
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String.ToLower</returns>
    ''' <remarks>N/A</remarks>
    Public Property RegistryPath As String
        Get
            Return _RegistryPath.ToLower
        End Get
        Set(ByVal value As String)
            _RegistryPath = value
        End Set
    End Property

    Private _Recursive As Boolean
    ''' <summary>
    ''' Gets or sets a property that indicates whether or not registry scanning should recurse
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks>N/A</remarks>
    Public Property Recursive As Boolean
        Get
            Return _Recursive
        End Get
        Set(ByVal value As Boolean)
            _Recursive = value
        End Set
    End Property

    Private _regValue As String = String.Empty
    ''' <summary>
    ''' Gets and sets a property indicating the value of the registry key scanned. This property is to only be set during
    ''' scanning and not prior to
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String.ToLower</returns>
    ''' <remarks>N/A</remarks>
    Public Property RegValue As String
        Get
            Return _regValue
        End Get
        Set(ByVal value As String)
            _regValue = value
        End Set
    End Property

    Private _DisplayName As String = String.Empty
    ''' <summary>
    ''' Gets and sets a property indicating the value of the display name for the LVW
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

    Private _RiImageIndex As Integer = 0
    ''' <summary>
    ''' Gets and sets a property indicating the value of the display name for the LVW
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks>N/A</remarks>
    Public Property RiImageIndex As Integer
        Get
            Return _RiImageIndex
        End Get
        Set(ByVal value As Integer)
            _RiImageIndex = value
        End Set
    End Property

    Public Sub New(ByVal regRoot As String, ByVal regPath As String, ByVal recursive As Boolean, ByVal DisplayName As String, ByVal RiImageIndex As Integer, Optional ByVal regValue As String = Nothing)
        _RegistryRoot = regRoot
        _RegistryPath = regPath
        _Recursive = recursive
        _DisplayName = DisplayName
        _RiImageIndex = RiImageIndex
        _regValue = regValue
    End Sub
End Class
