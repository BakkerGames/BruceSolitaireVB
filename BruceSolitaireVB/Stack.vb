' --- Stack.vb - 03/13/2009 ---

Public Class Stack

    Private Const MaxItems As Integer = Card.TotalCards
    Private Const LastItem As Integer = MaxItems - 1

    Private _Items(MaxItems) As Card

    Public Sub New(ByVal Name As String)
        _Name = Name
    End Sub

    Public Property Name() As String = ""

    Public Property CardSize() As New Size(0, 0)

    Public Location As New Point(0, 0)

    Public Property FaceDownOfs() As New Size(0, 0)

    Public Property FaceUpOfs() As New Size(0, 0)

    Private _Rotation As Integer = 0
    Public Property Rotation() As Integer
        Get
            Return _Rotation
        End Get
        Set(ByVal value As Integer)
            If value < 0 OrElse value >= 360 Then
                Throw New ArgumentOutOfRangeException("Rotation", "Invalid angle: " + value.ToString)
            End If
            _Rotation = value
        End Set
    End Property

    Public Property Visible() As Boolean = True

    Public Property Changed() As Boolean = False

    Private _LastCount As Integer = 0
    Public ReadOnly Property LastCount() As Integer
        Get
            Return _LastCount
        End Get
    End Property

    Private _LastStackSize As New Size(0, 0)
    Public ReadOnly Property LastStackSize() As Size
        Get
            Return _LastStackSize
        End Get
    End Property

    Public Property Item(ByVal ItemNum As Integer) As Card
        Get
            If ItemNum < 0 OrElse ItemNum >= MaxItems Then
                Throw New ArgumentOutOfRangeException("Item", "Invalid Item Number: " + ItemNum.ToString)
            End If
            If _Items(ItemNum) Is Nothing Then
                Throw New ArgumentNullException("Item", "No card at Item Number: " + ItemNum.ToString)
            End If
            Return _Items(ItemNum)
        End Get
        Set(ByVal value As Card)
            If _Items(ItemNum) Is Nothing OrElse value Is Nothing Then
                SaveLast()
            End If
            _Items(ItemNum) = value
        End Set
    End Property

    Public ReadOnly Property Items() As List(Of Card)
        Get
            Dim Result As New List(Of Card)
            For CardNum As Integer = 0 To LastItem
                If _Items(CardNum) IsNot Nothing Then
                    Result.Add(_Items(CardNum))
                End If
            Next
            Return Result
        End Get
    End Property

    Public Sub Clear()
        SaveLast()
        For CardNum As Integer = 0 To LastItem
            _Items(CardNum) = Nothing
        Next
        Me.Changed = True
    End Sub

    Public ReadOnly Property Count() As Integer
        Get
            Dim Result As Integer = 0
            For CardNum As Integer = 0 To LastItem
                If _Items(CardNum) IsNot Nothing Then
                    Result += 1
                End If
            Next
            Return Result
        End Get
    End Property

    Public ReadOnly Property StackSize() As Size
        Get
            Dim Result As Size = CardSize
            For ItemNum As Integer = 1 To Me.Count - 1 ' skip first card
                If _Items(ItemNum).FaceUp Then
                    Result.Width += _FaceUpOfs.Width
                    Result.Height += _FaceUpOfs.Height
                Else
                    Result.Width += _FaceDownOfs.Width
                    Result.Height += _FaceDownOfs.Height
                End If
            Next
            Return Result
        End Get
    End Property

    Public Sub Add(ByVal Value As Card)
        If Value Is Nothing Then
            Throw New ArgumentNullException("Add")
        End If
        SaveLast()
        For ItemNum As Integer = LastItem To 0 Step -1
            If _Items(ItemNum) IsNot Nothing Then
                _Items(ItemNum + 1) = Value
                Me.Changed = True
                Exit Sub
            End If
        Next
        _Items(0) = Value
        Me.Changed = True
    End Sub

    Public Sub InsertAt(ByVal ItemNum As Integer, ByVal Value As Card)
        If ItemNum < 0 OrElse ItemNum >= LastItem Then
            Throw New ArgumentOutOfRangeException("InsertAt", "Invalid Item Number: " + ItemNum.ToString)
        End If
        If Value Is Nothing Then
            Throw New ArgumentNullException("InsertAt")
        End If
        If _Items(ItemNum) Is Nothing Then
            Throw New ArgumentNullException("InsertAt", "No card at Item Number: " + ItemNum.ToString)
        End If
        If _Items(LastItem) IsNot Nothing Then
            Throw New ArgumentException("InsertAt", "Stack is full!")
        End If
        SaveLast()
        For CardNum As Integer = LastItem To ItemNum + 1 Step -1
            _Items(CardNum) = _Items(CardNum - 1)
            _Items(CardNum - 1) = Nothing
        Next
        _Items(ItemNum) = Value
        Me.Changed = True
    End Sub

    Public Function RemoveTop() As Card
        SaveLast()
        For ItemNum As Integer = LastItem To 0 Step -1
            If _Items(ItemNum) IsNot Nothing Then
                Dim Result As Card = _Items(ItemNum)
                _Items(ItemNum) = Nothing
                Me.Changed = True
                Return Result
            End If
        Next
        Return Nothing
    End Function

    Public Function ViewTop() As Card
        For ItemNum As Integer = LastItem To 0 Step -1
            If _Items(ItemNum) IsNot Nothing Then
                Return _Items(ItemNum)
            End If
        Next
        Return Nothing
    End Function

    Public Function RemoveFrom(ByVal ItemNum As Integer) As List(Of Card)
        Dim Result As New List(Of Card)
        If _Items(ItemNum) Is Nothing Then
            Throw New ArgumentOutOfRangeException("RemoveFrom", "Item Number past top of stack: " + ItemNum.ToString)
        End If
        SaveLast()
        For CardNum As Integer = ItemNum To LastItem
            If _Items(CardNum) IsNot Nothing Then
                Result.Add(_Items(CardNum))
                _Items(CardNum) = Nothing
            End If
        Next
        Me.Changed = True
        Return Result
    End Function

    Public Sub Shuffle()
        Dim RandSeq As New Random
        Dim FromItem As Integer
        Dim ToItem As Integer
        Dim TempCard As Card
        SaveLast()
        For ShuffleNum As Integer = 1 To 10000
            FromItem = CInt(Int(RandSeq.NextDouble * MaxItems))
            ToItem = CInt(Int(RandSeq.NextDouble * MaxItems))
            If FromItem <> ToItem Then
                TempCard = _Items(FromItem)
                _Items(FromItem) = _Items(ToItem)
                _Items(ToItem) = TempCard
            End If
        Next
        Me.Changed = True
    End Sub

    Public Function InStack(ByVal X As Integer, ByVal Y As Integer) As Boolean
        If X < Location.X OrElse Y < Location.Y Then
            Return False
        End If
        Dim CurrSize As Size = Me.StackSize
        If X > Location.X + CurrSize.Width OrElse Y > Location.Y + CurrSize.Height Then
            Return False
        End If
        Return True
    End Function

    Public Function ClickedCardNum(ByVal X As Integer, ByVal Y As Integer) As Integer
        Dim Result As Integer = 0
        Dim CurrX As Integer = Location.X
        Dim CurrY As Integer = Location.Y
        ' --- This gets the highest numbered card which was clicked ---
        For LoopNum As Integer = 0 To Me.Count - 1
            If X >= CurrX AndAlso X <= CurrX + CardSize.Width AndAlso Y >= CurrY AndAlso Y <= CurrY + CardSize.Height Then
                Result = LoopNum
            End If
            ' --- Move the offsets to the next card ---
            If _Items(LoopNum).FaceUp Then
                CurrX += _FaceUpOfs.Width
                CurrY += _FaceUpOfs.Height
            Else
                CurrX += _FaceDownOfs.Width
                CurrY += _FaceDownOfs.Height
            End If
        Next
        Return Result
    End Function

    Public Function ClickedCardOfs(ByVal X As Integer, ByVal Y As Integer) As Size
        Dim Result As New Size(X - Location.X, Y - Location.Y)
        Dim CurrX As Integer = Location.X
        Dim CurrY As Integer = Location.Y
        ' --- This gets the highest numbered card which was clicked ---
        For LoopNum As Integer = 0 To Me.Count - 1
            If X >= CurrX AndAlso X <= CurrX + CardSize.Width AndAlso Y >= CurrY AndAlso Y <= CurrY + CardSize.Height Then
                Result.Width = X - CurrX
                Result.Height = Y - CurrY
            End If
            ' --- Move the offsets to the next card ---
            If _Items(LoopNum).FaceUp Then
                CurrX += _FaceUpOfs.Width
                CurrY += _FaceUpOfs.Height
            Else
                CurrX += _FaceDownOfs.Width
                CurrY += _FaceDownOfs.Height
            End If
        Next
        Return Result
    End Function

    Private Sub SaveLast()
        _LastCount = Me.Count
        _LastStackSize = Me.StackSize
    End Sub

End Class
