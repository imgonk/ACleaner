Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Runtime.CompilerServices

Module Extensions

    Private Delegate Sub SetThreadSafePropertyDelegate(Of T)(ByVal control As Control, ByVal [property] As Expression(Of Func(Of T)), ByVal value As T)
    Private Delegate Function GetThreadSaferopertyDelegate(Of T)(ByVal control As Control, ByVal [property] As Expression(Of Func(Of T))) As T
    Private Delegate Sub InvokeThreadSafeMethodDelegate(ByVal control As Control, ByVal method As Expression(Of Action))
    Private Delegate Function InvokeThreadSafeFunctionDelegate(Of T)(ByVal control As Control, ByVal [function] As Expression(Of Func(Of T))) As T

    ''' <summary>
    ''' Sets the specified property of this control to the specified value safely across threads by invoking a delegate if necessary.
    ''' </summary>
    ''' <typeparam name="T">The type of the property. Can usually be inferred from usage.</typeparam>
    ''' <param name="control">The control to set the property on.</param>
    ''' <param name="property">The property to set as a lambda expression.</param>
    ''' <param name="value">The new value of the property.</param>
    <Extension()> _
    Public Sub SetThreadSafeProperty(Of T)(ByVal control As Control, ByVal [property] As Expression(Of Func(Of T)), ByVal value As T)
        If (control.InvokeRequired) Then
            Dim del = New SetThreadSafePropertyDelegate(Of T)(AddressOf SetThreadSafeProperty)
            control.Invoke(del, control, [property], value)
        Else
            Dim propertyInfo = GetPropertyInfo([property])
            If (propertyInfo IsNot Nothing) Then
                propertyInfo.SetValue(control, value, Nothing)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Gets the value of the specified property of this control safely across threads by invoking a delegate if necessary.
    ''' </summary>
    ''' <typeparam name="T">The type of the property. Can usually be inferred from usage.</typeparam>
    ''' <param name="control">The control to get the property from.</param>
    ''' <param name="property">The property to get the value from as a lambda expression.</param>
    ''' <returns>The value of the specified property.</returns>
    <Extension()> _
    Public Function GetThreadSafeProperty(Of T)(ByVal control As Control, ByVal [property] As Expression(Of Func(Of T))) As T
        If (control.InvokeRequired) Then
            Dim del = New GetThreadSaferopertyDelegate(Of T)(AddressOf GetThreadSafeProperty)
            Return DirectCast(control.Invoke(del, control, [property]), T)
        Else
            Dim propertyInfo = GetPropertyInfo([property])
            If (propertyInfo IsNot Nothing) Then
                Return DirectCast(propertyInfo.GetValue(control, Nothing), T)
            End If
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Invokes a method of this control safely across threads by invoking a delegate if necessary.
    ''' </summary>
    ''' <param name="control">The control to invoke the method on.</param>
    ''' <param name="method">The method to invoke as an expression.</param>
    <Extension()> _
    Public Sub InvokeThreadSafeMethod(ByVal control As Control, ByVal method As Expression(Of Action))
        If (control.InvokeRequired) Then
            Dim del = New InvokeThreadSafeMethodDelegate(AddressOf InvokeThreadSafeMethod)
            control.Invoke(del, control, method)
        Else
            method.Compile().DynamicInvoke()
        End If
    End Sub

    ''' <summary>
    ''' Invokes a function of this control safely across threads by invoking a delegate if necessary.
    ''' </summary>
    ''' <typeparam name="T">The return type of the function to invoke. Can usually be inferred from usage.</typeparam>
    ''' <param name="control">The control to invoke the function on.</param>
    ''' <param name="function">The function to invoke as an expression.</param>
    ''' <returns>The result of the function to invoke.</returns>
    <Extension()> _
    Public Function InvokeThreadSafeFunction(Of T)(ByVal control As Control, ByVal [function] As Expression(Of Func(Of T))) As T
        If (control.InvokeRequired) Then
            Dim del = New InvokeThreadSafeFunctionDelegate(Of T)(AddressOf InvokeThreadSafeFunction)
            Return DirectCast(control.Invoke(del, control, [function]), T)
        Else
            Return DirectCast([function].Compile().DynamicInvoke(), T)
        End If
    End Function

    Private Function GetMemberInfo(ByVal expression As Expression) As MemberInfo
        Dim memberExpression As MemberExpression
        Dim lambda = DirectCast(expression, LambdaExpression)
        If (TypeOf lambda.Body Is UnaryExpression) Then
            memberExpression = DirectCast(DirectCast(lambda.Body, UnaryExpression).Operand, MemberExpression)
        Else
            memberExpression = DirectCast(lambda.Body, MemberExpression)
        End If
        Return memberExpression.Member
    End Function

    Private Function GetPropertyInfo(ByVal expression As Expression) As PropertyInfo
        Dim memberInfo = GetMemberInfo(expression)
        If (memberInfo.MemberType = Reflection.MemberTypes.Property) Then
            Return DirectCast(memberInfo, PropertyInfo)
        End If
        Return Nothing
    End Function

End Module