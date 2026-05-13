' -------------------------------------------
' --- BruceSolitaireLogic.vb - 08/08/2020 ---
' -------------------------------------------

' ----------------------------------------------------------------------------------------------------
' 03/15/2014 - SBakker
'            - Added Bootstrapping to a a local area instead of using the ClickOnce install.
'            - Added the Settings Provider to save settings in the local area with the program.
'            - Added AboutMain.vb which shows the current path in the Status Bar.
' 11/30/2015 - SBakker
'            - Added auto flipping of a face-down card.
'            - Added left-click to move cards to Build stacks.
' 08/08/2020 - Replaced Click and RightClick with single routine DoClick. Allows all card clicking
'              to be managed by a single routine, so left and right clicks can both work the same.
'            - Added MovingCards() so calling program knows if cards are on the FLOAT stack.
'            - Changed CardOfs from 20 to 24 so cards can be read more easily.
' ----------------------------------------------------------------------------------------------------

Imports System.Math

Public Class BruceSolitaireEngine

    Private Shared ReadOnly ObjName As String = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName

    Public MyLayout As Layout = Nothing
    Public CardSize As Size = Nothing

    Private SepWidth As Integer = 20
    Private SepHeight As Integer = 20
    Private CardOfs As Integer = 24

    Private MoveState As Integer = 0
    Private MoveDist As Integer = 0

    Private Const MoveDistDeal As Integer = 40
    Private Const MoveDistFlip As Integer = 20

    Public Event ClearBack()
    Public Event StackChanged(ByVal ChangedStack As Stack)
    Public Event FloatChanged(ByVal ChangedStack As Stack)
    Public Event LostGame()
    Public Event WonGame()
    Public Event DebugMessage(ByVal Msg As String)

    Private CardsCameFrom As Stack = Nothing
    Private DragOfs As Size = Nothing

    Public Sub Init(ByVal NewLayout As Layout, ByVal NewCardSize As Size)
        MyLayout = NewLayout
        CardSize = NewCardSize
        Dim TempStack As Stack
        With Me.MyLayout
            .CardSize = CardSize
            TempStack = New Stack("DECK")
            TempStack.CardSize = NewCardSize
            TempStack.Location = New Point((CardSize.Width * 4) + (SepWidth * 5), (SepHeight * 9) + (CardSize.Height * 2))
            .Stacks.Add(TempStack)
            TempStack = New Stack("FLIP1")
            TempStack.CardSize = NewCardSize
            TempStack.Location = New Point(SepWidth, (SepHeight * 3) + (CardSize.Height * 2))
            .Stacks.Add(TempStack)
            TempStack = New Stack("FLIP2")
            TempStack.CardSize = NewCardSize
            TempStack.Location = New Point(SepWidth, (SepHeight * 4) + (CardSize.Height * 3))
            .Stacks.Add(TempStack)
            TempStack = New Stack("FLIP3")
            TempStack.CardSize = NewCardSize
            TempStack.Location = New Point(SepWidth, (SepHeight * 5) + (CardSize.Height * 4))
            .Stacks.Add(TempStack)
            For StackNum As Integer = 1 To 7
                TempStack = New Stack("PLAY" + StackNum.ToString)
                TempStack.CardSize = NewCardSize
                TempStack.Location = New Point((CardSize.Width * StackNum) + (SepWidth * (StackNum + 1)), SepHeight)
                TempStack.FaceDownOfs = New Size(0, CardOfs)
                TempStack.FaceUpOfs = New Size(0, CardOfs)
                .Stacks.Add(TempStack)
            Next
            For StackNum As Integer = 1 To 4
                TempStack = New Stack("BUILD" + StackNum.ToString)
                TempStack.CardSize = NewCardSize
                TempStack.Location = New Point((CardSize.Width * 8) + (SepWidth * 9), SepHeight + ((CardSize.Height + SepHeight) * StackNum))
                TempStack.FaceDownOfs = New Size(0, 0)
                TempStack.FaceUpOfs = New Size(0, 0)
                .Stacks.Add(TempStack)
            Next
            ' --- Float Stack is last so it is always on top ---
            TempStack = New Stack("FLOAT")
            TempStack.CardSize = NewCardSize
            TempStack.Location = New Point(0, 0)
            TempStack.Visible = False ' can't see float stack yet
            .Stacks.Add(TempStack)
            ' --- Set the total layout size and minimum layout size ---
            .LayoutSize = New Size((CardSize.Width * 9) + (SepWidth * 10), (SepHeight * 2) + (CardOfs * 51) + CardSize.Height)
            .MinSize = New Size((CardSize.Width * 9) + (SepWidth * 10), (CardSize.Height * 5) + (SepWidth * 6))
        End With
    End Sub

    Public Function NewGame() As Boolean
        If MoveState <> 0 Then Return False
        MoveState = -1 ' holding, ignore ticks 
        RaiseEvent DebugMessage("")
        ' --- Clear the entire background --- 
        RaiseEvent ClearBack()
        ' --- Empty every stack --- 
        For Each TempStack As Stack In MyLayout.Stacks
            TempStack.Clear()
            If TempStack.Name <> "FLOAT" Then
                TempStack.Visible = True
                RaiseEvent StackChanged(TempStack)
            End If
        Next
        ' --- Fill the deck --- 
        Dim DeckStack As Stack = MyLayout.GetStack("DECK")
        For CardNum As Integer = Card.MinCardNum To Card.MaxCardNum
            Dim TempCard As New Card(CardNum)
            TempCard.FaceUp = False
            DeckStack.Add(TempCard)
        Next
        DeckStack.Shuffle()
        RaiseEvent StackChanged(DeckStack)
        ' --- Turn off the float stack --- 
        Dim FloatStack As Stack = MyLayout.GetStack("FLOAT")
        FloatStack.Clear()
        FloatStack.Visible = False
        RaiseEvent FloatChanged(FloatStack)
        ' --- Start dealing cards --- 
        MoveState = 1 ' start dealing cards
        Return True
    End Function

    Public Function DoClick(ByVal X As Integer, ByVal Y As Integer) As Boolean
        ' ------------------
        If MoveState <> 0 Then Return False
        ' --- Find the clicked card
        For Each TempStack As Stack In MyLayout.Stacks
            If TempStack.Count = 0 Then Continue For
            If TempStack.Name = "FLOAT" Then Continue For
            If Not TempStack.InStack(X, Y) Then Continue For
            If TempStack.Name.StartsWith("BUILD") Then
                Return False
            End If
            ' --- Found the clicked Card
            Dim CardNum As Integer = TempStack.ClickedCardNum(X, Y)
            Dim TempCard As Card = TempStack.Items(CardNum)
            If Not TempCard.FaceUp Then
                Return False
            End If
            ' --- Check if card can move directly to Build stack ---
            If CardNum = TempStack.Count - 1 Then
                For Each TempStack2 As Stack In MyLayout.Stacks
                    If Not TempStack2.Name.StartsWith("BUILD") Then Continue For
                    If TempStack2.Count = 0 Then
                        If TempCard.Rank <> 1 Then Continue For ' Not an Ace
                    ElseIf TempStack2.ViewTop.Suit <> TempCard.Suit Then
                        Continue For
                    ElseIf TempStack2.ViewTop.Rank <> TempCard.Rank - 1 Then
                        Continue For
                    End If
                    ' --- Move card to Build stack
                    Dim TempCard2 As Card = TempStack.RemoveTop
                    TempStack2.Add(TempCard2)
                    RaiseEvent StackChanged(TempStack)
                    RaiseEvent StackChanged(TempStack2)
                    DoCascadeMoves()
                    CheckGameOver()
                    Return True
                Next
            End If
            ' --- Check if card(s) can be moved to another Play stack on top of next card
            For Each TempStack2 As Stack In MyLayout.Stacks
                If Not TempStack2.Name.StartsWith("PLAY") Then Continue For
                If TempStack2.Name = TempStack.Name Then Continue For
                If TempStack2.Count = 0 Then Continue For
                Dim TempCard2 As Card = TempStack2.ViewTop()
                If TempCard2.Suit <> TempCard.Suit Then Continue For
                If TempCard2.Rank <> TempCard.Rank + 1 Then Continue For
                ' --- Move cards
                Dim TempList As List(Of Card) = TempStack.RemoveFrom(CardNum)
                For Each TempCard3 As Card In TempList
                    TempStack2.Add(TempCard3)
                Next
                RaiseEvent StackChanged(TempStack)
                RaiseEvent StackChanged(TempStack2)
                DoCascadeMoves()
                CheckGameOver()
                Return True
            Next
            ' --- Move the selected card onto the Float stack ---
            DragOfs = TempStack.ClickedCardOfs(X, Y)
            Dim FloatStack As Stack = MyLayout.GetStack("FLOAT")
            MoveState = 200 ' dragging some cards
            CardsCameFrom = TempStack
            Dim TempList2 As List(Of Card) = TempStack.RemoveFrom(CardNum)
            For Each TempCard4 As Card In TempList2
                FloatStack.Add(TempCard4)
            Next
            FloatStack.Location.X = X - DragOfs.Width
            FloatStack.Location.Y = Y - DragOfs.Height
            FloatStack.FaceUpOfs = TempStack.FaceUpOfs
            FloatStack.FaceDownOfs = TempStack.FaceDownOfs
            FloatStack.Visible = True
            RaiseEvent FloatChanged(FloatStack)
            RaiseEvent StackChanged(TempStack)
            Return True
        Next
        Return False
    End Function

    Public Function IsMovingCards() As Boolean
        Return (MoveState = 200)
    End Function

    Public Function Drag(ByVal X As Integer, ByVal Y As Integer) As Boolean
        If MoveState <> 200 Then Return False
        ' --- Dragging a stack ---
        Dim FloatStack As Stack = MyLayout.GetStack("FLOAT")
        FloatStack.Location.X = X - DragOfs.Width
        FloatStack.Location.Y = Y - DragOfs.Height
        RaiseEvent FloatChanged(FloatStack)
        Return True
    End Function

    Public Function Unclick(ByVal X As Integer, ByVal Y As Integer) As Boolean
        Dim TempCard As Card
        Dim TempList As List(Of Card)
        ' ---------------------------
        If MoveState <> 200 Then Return False
        ' --- See if correct place to drop cards ---
        Dim FloatStack As Stack = MyLayout.GetStack("FLOAT")
        ' --- Check if mouse or FloatStack corner over the correct stack ---
        For Each TempStack As Stack In MyLayout.Stacks
            If TempStack.Name = "FLOAT" Then Continue For
            If TempStack.Name = "DECK" Then Continue For
            If TempStack.InStack(X, Y) OrElse
               TempStack.InStack(FloatStack.Location.X, FloatStack.Location.Y) OrElse
               TempStack.InStack(FloatStack.Location.X + FloatStack.StackSize.Width, FloatStack.Location.Y) OrElse
               TempStack.InStack(FloatStack.Location.X, FloatStack.Location.Y + FloatStack.StackSize.Height) OrElse
               TempStack.InStack(FloatStack.Location.X + FloatStack.StackSize.Width, FloatStack.Location.Y + FloatStack.StackSize.Height) Then
                Select Case TempStack.Name
                    Case "BUILD1", "BUILD2", "BUILD3", "BUILD4"
                        If FloatStack.Count > 1 Then Exit For
                        If (TempStack.Count = 0 AndAlso FloatStack.Item(0).Rank = 1) OrElse
                               (TempStack.Count > 0 AndAlso
                                TempStack.Item(TempStack.Count - 1).Suit = FloatStack.Item(0).Suit AndAlso
                                TempStack.Item(TempStack.Count - 1).Rank + 1 = FloatStack.Item(0).Rank) Then
                            TempCard = FloatStack.RemoveTop
                            TempStack.Add(TempCard)
                            FloatStack.Visible = False
                            RaiseEvent FloatChanged(FloatStack)
                            RaiseEvent StackChanged(TempStack)
                            MoveState = 0
                            DoCascadeMoves()
                            CheckGameOver()
                            Return True
                        End If
                        Exit For
                    Case "PLAY1", "PLAY2", "PLAY3", "PLAY4", "PLAY5", "PLAY6", "PLAY7"
                        If (TempStack.Count = 0) OrElse
                               (TempStack.Item(TempStack.Count - 1).Suit = FloatStack.Item(0).Suit AndAlso
                                TempStack.Item(TempStack.Count - 1).Rank - 1 = FloatStack.Item(0).Rank) Then
                            TempList = FloatStack.RemoveFrom(0)
                            For Each TempCard In TempList
                                TempStack.Add(TempCard)
                            Next
                            FloatStack.Visible = False
                            RaiseEvent FloatChanged(FloatStack)
                            RaiseEvent StackChanged(TempStack)
                            MoveState = 0
                            DoCascadeMoves()
                            CheckGameOver()
                            Return True
                        End If
                End Select
            End If
        Next
        ' --- Wrong place to drop cards ---
        TempList = FloatStack.RemoveFrom(0)
        For Each TempCard In TempList
            CardsCameFrom.Add(TempCard)
        Next
        FloatStack.Visible = False
        RaiseEvent FloatChanged(FloatStack)
        RaiseEvent StackChanged(CardsCameFrom)
        MoveState = 0
        Return False
    End Function

#Region " --- Autonomous Routines --- "

    Private TargetStack As Stack

    Public Sub Tick()
        ' --- Raise events if anything changes ---
        If MoveState <= 0 Then Exit Sub
        Select Case MoveState
            Case 1 To 49
                MoveDist = MoveDistDeal
                Dim FloatStack As Stack = MyLayout.GetStack("FLOAT")
                Dim StackNum As Integer = ((MoveState - 1) Mod 7) + 1
                TargetStack = MyLayout.GetStack("PLAY" + StackNum.ToString)
                Dim TargetY As Integer = TargetStack.Location.Y + (TargetStack.Count * CardOfs)
                ' --- Get a card from the deck ---
                If FloatStack.Visible = False Then
                    Dim DeckStack As Stack = MyLayout.GetStack("DECK")
                    Dim TempCard As Card = DeckStack.RemoveTop
                    FloatStack.Location = DeckStack.Location
                    FloatStack.Add(TempCard)
                    FloatStack.Visible = True
                    RaiseEvent StackChanged(DeckStack)
                End If
                If MoveFloatTo(TargetStack.Location.X, TargetY) Then
                    Dim TempCard As Card = FloatStack.RemoveTop
                    If TargetStack.Count >= StackNum - 1 Then
                        TempCard.FaceUp = True
                    End If
                    TargetStack.Add(TempCard)
                    FloatStack.Visible = False
                    RaiseEvent FloatChanged(FloatStack)
                    RaiseEvent StackChanged(TargetStack)
                    MoveState += 1
                Else
                    RaiseEvent FloatChanged(FloatStack)
                End If
            Case 50 To 52
                MoveDist = MoveDistDeal
                Dim FloatStack As Stack = MyLayout.GetStack("FLOAT")
                Dim StackNum As Integer = MoveState - 49
                TargetStack = MyLayout.GetStack("FLIP" + StackNum.ToString)
                Dim TargetY As Integer = TargetStack.Location.Y + (TargetStack.Count * CardOfs)
                ' --- Get a card from the deck ---
                If FloatStack.Visible = False Then
                    Dim DeckStack As Stack = MyLayout.GetStack("DECK")
                    Dim TempCard As Card = DeckStack.RemoveTop
                    FloatStack.Location = DeckStack.Location
                    FloatStack.Add(TempCard)
                    FloatStack.Visible = True
                    RaiseEvent StackChanged(DeckStack)
                End If
                If MoveFloatTo(TargetStack.Location.X, TargetY) Then
                    Dim TempCard As Card = FloatStack.RemoveTop
                    TempCard.FaceUp = True
                    TargetStack.Add(TempCard)
                    FloatStack.Visible = False
                    RaiseEvent FloatChanged(FloatStack)
                    RaiseEvent StackChanged(TargetStack)
                    MoveState += 1
                Else
                    RaiseEvent FloatChanged(FloatStack)
                End If
            Case 53
                Dim DeckStack As Stack = MyLayout.GetStack("DECK")
                DeckStack.Visible = False
                RaiseEvent StackChanged(DeckStack)
                MoveState = 0
                DoCascadeMoves()
                CheckGameOver()
        End Select
    End Sub

    Private Function MoveFloatTo(ByVal TargetLoc As Point) As Boolean
        Return MoveFloatTo(TargetLoc.X, TargetLoc.Y)
    End Function

    Private Function MoveFloatTo(ByVal TargetX As Integer, ByVal TargetY As Integer) As Boolean
        ' --- Returns TRUE when done moving ---
        Dim FloatStack As Stack = MyLayout.GetStack("FLOAT")
        Dim XSign As Integer = Sign(TargetX - FloatStack.Location.X)
        Dim YSign As Integer = Sign(TargetY - FloatStack.Location.Y)
        Dim MoveDistX As Integer = MoveDist
        Dim MoveDistY As Integer = MoveDist
        If FloatStack.Location.X <> TargetX AndAlso FloatStack.Location.Y <> TargetY AndAlso
           Abs(TargetX - FloatStack.Location.X) <> Abs(TargetY - FloatStack.Location.Y) Then
            Dim Hypo As Double = Sqrt(((TargetX - FloatStack.Location.X) * (TargetX - FloatStack.Location.X)) +
                ((TargetY - FloatStack.Location.Y) * (TargetY - FloatStack.Location.Y)))
            If Hypo > MoveDist Then
                MoveDistX = CInt(Round(Abs(TargetX - FloatStack.Location.X) * MoveDist / Hypo))
                MoveDistY = CInt(Round(Abs(TargetY - FloatStack.Location.Y) * MoveDist / Hypo))
            End If
        End If
        If FloatStack.Location.X <> TargetX Then
            If Abs(TargetX - FloatStack.Location.X) > MoveDistX Then
                FloatStack.Location = New Point(FloatStack.Location.X + (MoveDistX * XSign), FloatStack.Location.Y)
            Else
                FloatStack.Location = New Point(TargetX, FloatStack.Location.Y)
            End If
        End If
        If FloatStack.Location.Y <> TargetY Then
            If Abs(TargetY - FloatStack.Location.Y) > MoveDistY Then
                FloatStack.Location = New Point(FloatStack.Location.X, FloatStack.Location.Y + (MoveDistY * YSign))
            Else
                FloatStack.Location = New Point(FloatStack.Location.X, TargetY)
            End If
        End If
        Return ((FloatStack.Location.X = TargetX) And (FloatStack.Location.Y = TargetY))
    End Function

#End Region

#Region " --- Internal Routines --- "

    Private Sub DoCascadeMoves()
        Dim CurrCard As Card
        Dim LoopAgain As Boolean
        ' ----------------------
        Do
            LoopAgain = False
            ' --- Check for cards that can be flipped ---
            For Each TempStack As Stack In MyLayout.Stacks
                If TempStack.Name.StartsWith("PLAY") Then
                    If TempStack.Count = 0 Then ' check empty stacks later
                        Continue For
                    End If
                    CurrCard = TempStack.ViewTop
                    If CurrCard.FaceUp = False Then ' can flip the card
                        CurrCard.FaceUp = True
                        RaiseEvent StackChanged(TempStack)
                        LoopAgain = True
                    End If
                End If
            Next
        Loop Until Not LoopAgain
    End Sub

    Private Function CheckGameOver() As Boolean
        Dim BuildCount As Integer
        Dim TempCard As Card
        Dim CurrCard As Card
        ' -----------------------
        ' --- Check if moving a card ---
        For Each TempStack As Stack In MyLayout.Stacks
            If TempStack.Name = "FLOAT" Then
                If TempStack.Count > 0 Then
                    RaiseEvent DebugMessage("Floating Card: " + TempStack.Name)
                    Return False
                End If
            End If
        Next
        ' --- check if all card on build piles ---
        BuildCount = 0
        For Each TempStack As Stack In MyLayout.Stacks
            If TempStack.Name.StartsWith("BUILD") Then
                BuildCount += TempStack.Count
            End If
        Next
        If BuildCount > 51 Then
            RaiseEvent DebugMessage("")
            RaiseEvent WonGame()
            Return True
        End If
        ' --- check for cards that can move to build stacks ---
        For Each TempStack As Stack In MyLayout.Stacks
            If TempStack.Name.StartsWith("BUILD") Then
                If TempStack.Count = 0 Then
                    ' --- Check if any Aces available ---
                    For Each TempStack2 As Stack In MyLayout.Stacks
                        If TempStack2.Name.StartsWith("PLAY") AndAlso TempStack2.Count > 0 Then
                            TempCard = TempStack2.ViewTop
                            If TempCard.FaceUp Then
                                If TempCard.Rank = 1 Then ' Found an Ace
                                    RaiseEvent DebugMessage("Found an Ace: " + TempStack2.Name + " on " + TempStack.Name)
                                    Return False
                                End If
                            End If
                        End If
                        If TempStack2.Name.StartsWith("FLIP") Then
                            For Each TempCard In TempStack2.Items
                                If TempCard.Rank = 1 Then ' Found an Ace
                                    RaiseEvent DebugMessage("Found an Ace: " + TempStack2.Name + " on " + TempStack.Name)
                                    Return False
                                End If
                            Next
                        End If
                    Next
                Else
                    ' --- Check if next higher card available ---
                    CurrCard = TempStack.ViewTop
                    For Each TempStack2 As Stack In MyLayout.Stacks
                        If TempStack2.Name.StartsWith("PLAY") AndAlso TempStack2.Count > 0 Then
                            TempCard = TempStack2.ViewTop
                            If TempCard.FaceUp Then
                                If TempCard.Suit = CurrCard.Suit AndAlso TempCard.Rank = CurrCard.Rank + 1 Then ' Found next card
                                    RaiseEvent DebugMessage("Found next card: " + TempStack2.Name + " on " + TempStack.Name)
                                    Return False
                                End If
                            End If
                        End If
                        If TempStack2.Name.StartsWith("FLIP") Then
                            For Each TempCard In TempStack2.Items
                                If TempCard.Suit = CurrCard.Suit AndAlso TempCard.Rank = CurrCard.Rank + 1 Then ' Found next card
                                    RaiseEvent DebugMessage("Found next card: " + TempStack2.Name + " on " + TempStack.Name)
                                    Return False
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        Next
        ' --- check for cards that can be moved to play stacks ---
        For Each TempStack As Stack In MyLayout.Stacks
            If TempStack.Name.StartsWith("PLAY") Then
                If TempStack.Count = 0 Then ' check empty stacks later
                    Continue For
                End If
                ' --- Check if next lower card available anywhere in stack ---
                CurrCard = TempStack.ViewTop
                For Each TempStack2 As Stack In MyLayout.Stacks
                    If TempStack2.Name = TempStack.Name Then ' same stack
                        Continue For
                    End If
                    If TempStack2.Name.StartsWith("PLAY") AndAlso TempStack2.Count > 0 Then
                        For Each TempCard In TempStack2.Items
                            If TempCard.FaceUp AndAlso TempCard.Suit = CurrCard.Suit AndAlso TempCard.Rank = CurrCard.Rank - 1 Then ' Found next card
                                RaiseEvent DebugMessage("Found next card: " + TempStack2.Name + " on " + TempStack.Name)
                                Return False
                            End If
                        Next
                    End If
                    If TempStack2.Name.StartsWith("FLIP") AndAlso TempStack2.Count > 0 Then
                        For Each TempCard In TempStack2.Items
                            If TempCard.Suit = CurrCard.Suit AndAlso TempCard.Rank = CurrCard.Rank - 1 Then ' Found next card
                                RaiseEvent DebugMessage("Found next card: " + TempStack2.Name + " on " + TempStack.Name)
                                Return False
                            End If
                        Next
                    End If
                Next
            End If
        Next
        ' --- check for empty play stacks ---
        For Each TempStack As Stack In MyLayout.Stacks
            If TempStack.Name.StartsWith("PLAY") Then
                If TempStack.Count = 0 Then ' can keep playing with an empty stack, but all cards might be in build stacks
                    RaiseEvent DebugMessage("Empty stack: " + TempStack.Name)
                    Return False
                End If
            End If
        Next
        ' --- no moves left! ---
        RaiseEvent LostGame()
        Return True
    End Function

#End Region

End Class
