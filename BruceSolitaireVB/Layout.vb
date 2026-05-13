' --- Layout.vb - 05/17/2008 ---

Public Class Layout

    Private _LayoutSize As New Size(0, 0)
    Public Property LayoutSize() As Size
        Get
            Return _LayoutSize
        End Get
        Set(ByVal value As Size)
            _LayoutSize = value
        End Set
    End Property

    Private _MinSize As New Size(0, 0)
    Public Property MinSize() As Size
        Get
            Return _MinSize
        End Get
        Set(ByVal value As Size)
            _MinSize = value
        End Set
    End Property

    Private _CardSize As New Size(0, 0)
    Public Property CardSize() As Size
        Get
            Return _CardSize
        End Get
        Set(ByVal value As Size)
            _CardSize = value
        End Set
    End Property

    Public Stacks As New ArrayList

    Public Sub Click(ByVal X As Integer, ByVal Y As Integer)
        For Each TempStack As Stack In Stacks
            If X >= TempStack.Location.X AndAlso X < TempStack.Location.X + _CardSize.Width Then
                If Y >= TempStack.Location.Y AndAlso Y < TempStack.Location.Y + _CardSize.Height Then
                    If TempStack.Item(TempStack.Count - 1).FaceUp = False Then
                        TempStack.Item(TempStack.Count - 1).FaceUp = True
                    End If
                End If
            End If
        Next
    End Sub

    Public Function GetStack(ByVal Name As String) As Stack
        For Each TempStack As Stack In Stacks
            If TempStack.Name.ToUpper = Name.ToUpper Then
                Return TempStack
            End If
        Next
        Return Nothing
    End Function

End Class
