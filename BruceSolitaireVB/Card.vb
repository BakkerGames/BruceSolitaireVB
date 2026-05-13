' --- Card.vb - 03/13/2009 ---

Public Enum Suits
    Spade = 1
    Heart = 2
    Club = 3
    Diamond = 4
End Enum

Public Class Card

    Public Const MinCardNum As Integer = 0
    Public Const MaxCardNum As Integer = 51
    Public Const TotalCards As Integer = MaxCardNum + 1
    Public Const CardsPerSuit As Integer = 13

    Public Sub New()
    End Sub

    Public Sub New(ByVal CardNumber As Integer)
        Me.CardNum = CardNumber
    End Sub

    Private _CardNum As Integer = 0
    Public Property CardNum() As Integer
        Get
            Return _CardNum
        End Get
        Set(ByVal value As Integer)
            If value < MinCardNum OrElse value > MaxCardNum Then
                Throw New ArgumentOutOfRangeException("CardNum", "Invalid Card Number: " + value.ToString)
            End If
            _CardNum = value
        End Set
    End Property

    Public ReadOnly Property Rank() As Integer
        Get
            Return (_CardNum Mod CardsPerSuit) + 1
        End Get
    End Property

    Public ReadOnly Property Suit() As Suits
        Get
            Return CType((_CardNum \ CardsPerSuit) + 1, Suits)
        End Get
    End Property

    Private _FaceUp As Boolean = False
    Public Property FaceUp() As Boolean
        Get
            Return _FaceUp
        End Get
        Set(ByVal value As Boolean)
            _FaceUp = value
        End Set
    End Property

    Private _Highlighted As Boolean = False
    Public Property Highlighted() As Boolean
        Get
            Return _Highlighted
        End Get
        Set(ByVal value As Boolean)
            _Highlighted = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Dim Result As String = ""
        Select Case Me.Rank
            Case 1
                Result += "A"
            Case 2 To 10
                Result += Me.Rank.ToString
            Case 11
                Result += "J"
            Case 12
                Result += "Q"
            Case 13
                Result += "K"
        End Select
        Select Case Me.Suit
            Case Suits.Spade
                Result += "S"
            Case Suits.Heart
                Result += "H"
            Case Suits.Club
                Result += "C"
            Case Suits.Diamond
                Result += "D"
        End Select
        Return Result
    End Function

End Class
