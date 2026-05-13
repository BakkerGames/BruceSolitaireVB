' --- FormMain.vb - 03/13/2009 ---

' --- LastMod: 08/08/2020

Imports System.Math

Public Class FormMain

    Private Shared ReadOnly ObjName As String = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName

    Private WithEvents MyEngine As New BruceSolitaireEngine

    Private Const CardWidth As Integer = 71
    Private Const CardHeight As Integer = 95

    Private BoardColor As Color = Color.ForestGreen
    Private BoardBrush As Brush = Brushes.ForestGreen

    Private BackPicture As Bitmap = Nothing
    Private FloatPicture As Bitmap = Nothing

    Private ScaleFactor As Double = 1.0 ' for scaling the image to the screen

    Public PlayAgain As Boolean = False

    ' --- Adjust the drawing rectangle to fit the minimum layout size on the screen ---
    Dim ScreenWidth As Integer = 0
    Dim ScreenHeight As Integer = 0
    Dim ScreenOfs As Integer = 0
    Dim ScreenRect As Rectangle

    Dim MouseIsDown As Boolean = False

#Region " --- FormMain Events --- "

    Private Sub FormMain_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        Static FuncName As String = ObjName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name

        ' --- First call Upgrade to load setting from last version ---
        If My.Settings.CallUpgrade Then
            My.Settings.Upgrade()
            My.Settings.CallUpgrade = False
            My.Settings.Save()
        End If

        ShowHintsToolStripMenuItem.Checked = My.Settings.ShowHints

        ' --- Set background colors ---
        MainPanel.BackColor = BoardColor
        MainPicture.BackColor = BoardColor

        ' --- Create the engine and layout ---
        MyEngine.Init(New Layout, New Size(CardWidth, CardHeight))

        ' --- Create the background image to match the layout size ---
        BackPicture = New Bitmap(MyEngine.MyLayout.LayoutSize.Width, MyEngine.MyLayout.LayoutSize.Height)

        ' --- Adjust the screen to fit the layout size ---
        AdjustScreen()

        ' --- Create a new game ---
        MyEngine.NewGame()

        ' --- Start the animation timer ---
        TickTimer.Enabled = True

    End Sub

#End Region

#Region " --- MainPanel Events --- "

    Private Sub MainPanel_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles MainPanel.Resize
        ' --- Adjust the screen to fit the layout size ---
        AdjustScreen()
    End Sub

    Private Sub MainPanel_Scroll(ByVal sender As Object, ByVal e As ScrollEventArgs) Handles MainPanel.Scroll
        FloatPictureBox.Top -= e.OldValue - e.NewValue
        MainPanel.Refresh()
        FloatPictureBox.Refresh()
    End Sub

#End Region

#Region " --- MainPicture Events --- "

    Private Sub MainPicture_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles MainPicture.Paint
        ' --- Get a graphics to start drawing with ---
        Dim g As Graphics = e.Graphics
        g.Clear(BoardColor)
        ' --- Draw the image with scaling ---
        g.DrawImage(BackPicture, ScreenRect, 0, 0, BackPicture.Width, BackPicture.Height, GraphicsUnit.Pixel, Nothing)
    End Sub

    Private Sub MainPicture_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles MainPicture.Resize
        ' --- Adjust the screen to fit the layout size ---
        AdjustScreen()
    End Sub

    Private Sub MainPicture_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MainPicture.MouseDown
        ' --- All click events are handled the same now, so left or right click work the same.
        Dim X As Integer = CInt(Round((e.X - ScreenOfs) / ScaleFactor))
        Dim Y As Integer = CInt(Round(e.Y / ScaleFactor))
        If MyEngine.DoClick(X, Y) Then
            If MyEngine.IsMovingCards Then
                MouseIsDown = True
            End If
        End If
    End Sub

    Private Sub MainPicture_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MainPicture.MouseMove
        If MouseIsDown Then
            Dim X As Integer = CInt(Round((e.X - ScreenOfs) / ScaleFactor))
            Dim Y As Integer = CInt(Round(e.Y / ScaleFactor))
            MyEngine.Drag(X, Y)
        End If
    End Sub

    Private Sub MainPicture_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MainPicture.MouseUp
        If MouseIsDown Then
            Dim X As Integer = CInt(Round((e.X - ScreenOfs) / ScaleFactor))
            Dim Y As Integer = CInt(Round(e.Y / ScaleFactor))
            MyEngine.Unclick(X, Y)
        End If
        MouseIsDown = False
    End Sub

#End Region

#Region " --- FloatPictureBox Events --- "

    Private Sub FloatPictureBox_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles FloatPictureBox.MouseDown
        If MouseIsDown Then
            Dim X As Integer = CInt(Round((e.X + FloatPictureBox.Left) / ScaleFactor))
            Dim Y As Integer = CInt(Round((e.Y + FloatPictureBox.Top) / ScaleFactor))
            MyEngine.Unclick(X, Y)
            MouseIsDown = False
        End If
    End Sub

    Private Sub FloatPictureBox_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles FloatPictureBox.MouseUp
        If MouseIsDown Then
            Dim X As Integer = CInt(Round(e.X / ScaleFactor))
            Dim Y As Integer = CInt(Round(e.Y / ScaleFactor))
            MyEngine.Unclick(X, Y)
            MouseIsDown = False
        End If
    End Sub

#End Region

#Region " --- MenuItem Events --- "

    Private Sub NewToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click
        If MessageBox.Show("Are you sure you want to start a new game?", "New game?", MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        If MyEngine.NewGame() Then
            MainPicture.Invalidate()
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim a As New AboutForm
        a.ShowDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

#End Region

#Region " --- Timer Events --- "

    Private Sub TickTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TickTimer.Tick
        MyEngine.Tick()
        ' '' --- scroll the screen if floating off the screen ---
        ''If MouseIsDown Then
        ''    If FloatPictureBox.Top < 0 Then
        ''        Dim TempTop As Integer = FloatPictureBox.Top
        ''        FloatPictureBox.Top = 0
        ''        MainPanel.VerticalScroll.Value += TempTop
        ''    ElseIf FloatPictureBox.Top + FloatPictureBox.Height - MainPanel.ClientSize.Height > 0 Then
        ''        Dim TempDiff As Integer = FloatPictureBox.Top + FloatPictureBox.Height - MainPanel.ClientSize.Height
        ''        FloatPictureBox.Top -= TempDiff
        ''        MainPanel.VerticalScroll.Value += TempDiff
        ''    End If
        ''End If
    End Sub

#End Region

#Region " --- MyEngine Events --- "

    Private Sub MyEngine_ClearBack() Handles MyEngine.ClearBack
        Dim g As Graphics = Graphics.FromImage(BackPicture)
        g.Clear(BoardColor)
        g.Dispose()
    End Sub

    Private Sub MyEngine_FloatChanged(ByVal ChangedStack As Stack) Handles MyEngine.FloatChanged
        Dim TempCard As Card
        ' ------------------
        If ChangedStack.Visible = False Then
            FloatPictureBox.Visible = False
            Exit Sub
        End If
        ' --- Calculate new left position of floating stack ---
        Dim NewLeft As Integer = CInt(ScreenOfs + Round(ChangedStack.Location.X * ScaleFactor))
        If NewLeft > 0 Then
            If NewLeft > MainPanel.ClientSize.Width - FloatPictureBox.Width Then
                NewLeft = MainPanel.ClientSize.Width - FloatPictureBox.Width
            End If
        End If
        If NewLeft < 0 Then
            NewLeft = 0
        End If
        FloatPictureBox.Left = NewLeft
        ' --- Calculate new top position of floating stack ---
        Dim NewTop As Integer = CInt(Round(ChangedStack.Location.Y * ScaleFactor) - MainPanel.VerticalScroll.Value)
        'TODO: Fix this to scroll up
        If NewTop < -MainPanel.VerticalScroll.Value Then
            NewTop = -MainPanel.VerticalScroll.Value
        End If
        'TODO: Fix this to scroll down
        If NewTop > MainPicture.Height - MainPanel.VerticalScroll.Value - FloatPictureBox.Height Then
            NewTop = MainPicture.Height - MainPanel.VerticalScroll.Value - FloatPictureBox.Height
        End If
        FloatPictureBox.Top = NewTop
        ' --- Fill the floating stack's image if it hasn't been done yet ---
        If FloatPictureBox.Visible = False Then
            ' --- Adjust the sizes ---
            If FloatPicture Is Nothing OrElse FloatPicture.Width <> ChangedStack.StackSize.Width OrElse FloatPicture.Height <> ChangedStack.StackSize.Height Then
                FloatPicture = New Bitmap(ChangedStack.StackSize.Width, ChangedStack.StackSize.Height)
            End If
            If FloatPictureBox.Image Is Nothing OrElse
               FloatPictureBox.Width <> Round(ChangedStack.StackSize.Width * ScaleFactor) OrElse
               FloatPictureBox.Height <> Round(ChangedStack.StackSize.Height * ScaleFactor) Then
                FloatPictureBox.Image = New Bitmap(CInt(Round(ChangedStack.StackSize.Width * ScaleFactor)), CInt(Round(ChangedStack.StackSize.Height * ScaleFactor)))
                FloatPictureBox.Width = CInt(Round(ChangedStack.StackSize.Width * ScaleFactor))
                FloatPictureBox.Height = CInt(Round(ChangedStack.StackSize.Height * ScaleFactor))
            End If
            ' --- Draw the floating stack ---
            Dim g As Graphics = Graphics.FromImage(FloatPicture)
            g.Clear(BoardColor)
            Dim StartNum As Integer = 0
            For LoopNum As Integer = StartNum To ChangedStack.Count - 1
                TempCard = ChangedStack.Item(LoopNum)
                If TempCard.FaceUp Then
                    g.DrawImage(My.Resources.CardDeck71x95,
                        ChangedStack.FaceUpOfs.Width * LoopNum,
                        ChangedStack.FaceUpOfs.Height * LoopNum,
                        New Rectangle((TempCard.Rank - 1) * CardWidth,
                                      (TempCard.Suit - 1) * CardHeight,
                                      CardWidth, CardHeight),
                        GraphicsUnit.Pixel)
                Else
                    g.DrawImageUnscaled(My.Resources.CardBack71x95,
                        ChangedStack.FaceDownOfs.Width * LoopNum,
                        ChangedStack.FaceDownOfs.Height * LoopNum,
                        CardWidth, CardHeight)
                End If
            Next
            g.Dispose()
            ' --- Draw the image with scaling ---
            Dim g1 As Graphics = Graphics.FromImage(FloatPictureBox.Image)
            Dim r As New Rectangle(0, 0, FloatPictureBox.Width, FloatPictureBox.Height)
            g1.DrawImage(FloatPicture, r, 0, 0, FloatPicture.Width, FloatPicture.Height, GraphicsUnit.Pixel, Nothing)
            g1.Dispose()
            FloatPictureBox.Visible = True
            FloatPictureBox.Refresh()
        End If
    End Sub

    Private Sub MyEngine_StackChanged(ByVal ChangedStack As Stack) Handles MyEngine.StackChanged
        Dim TempCard As Card
        ' ------------------
        Dim g As Graphics = Graphics.FromImage(BackPicture)
        If Not ChangedStack.Visible OrElse ChangedStack.LastCount > ChangedStack.Count Then
            g.FillRectangle(BoardBrush, ChangedStack.Location.X, ChangedStack.Location.Y,
                            ChangedStack.LastStackSize.Width + 1, ChangedStack.LastStackSize.Height + 1)
        End If
        If ChangedStack.Visible Then
            If ChangedStack.Count = 0 Then
                g.FillRectangle(BoardBrush, ChangedStack.Location.X, ChangedStack.Location.Y, CardWidth, CardHeight)
                g.DrawRectangle(Pens.Green, ChangedStack.Location.X, ChangedStack.Location.Y, CardWidth, CardHeight)
                g.DrawRectangle(Pens.Green, ChangedStack.Location.X + 1, ChangedStack.Location.Y + 1,
                                CardWidth - 2, CardHeight - 2)
            Else
                Dim StartNum As Integer = 0
                If ChangedStack.FaceUpOfs.Width = 0 AndAlso ChangedStack.FaceUpOfs.Height = 0 AndAlso
                    ChangedStack.FaceDownOfs.Width = 0 AndAlso ChangedStack.FaceDownOfs.Height = 0 Then
                    StartNum = ChangedStack.Count - 1
                End If
                For LoopNum As Integer = StartNum To ChangedStack.Count - 1
                    TempCard = ChangedStack.Item(LoopNum)
                    If TempCard.FaceUp Then
                        g.DrawImage(My.Resources.CardDeck71x95,
                            ChangedStack.Location.X + (ChangedStack.FaceUpOfs.Width * LoopNum),
                            ChangedStack.Location.Y + (ChangedStack.FaceUpOfs.Height * LoopNum),
                            New Rectangle((TempCard.Rank - 1) * CardWidth,
                                          (TempCard.Suit - 1) * CardHeight,
                                          CardWidth, CardHeight),
                            GraphicsUnit.Pixel)
                    Else
                        g.DrawImageUnscaled(My.Resources.CardBack71x95,
                            ChangedStack.Location.X + (ChangedStack.FaceDownOfs.Width * LoopNum),
                            ChangedStack.Location.Y + (ChangedStack.FaceDownOfs.Height * LoopNum),
                            CardWidth, CardHeight)
                    End If
                Next
            End If
        End If
        g.Dispose()
        MainPicture.Refresh()
    End Sub

    Private Sub MyEngine_WonGame() Handles MyEngine.WonGame
        TickTimer.Enabled = False
        ' --- update settings ---
        My.Settings.Reload()
        My.Settings.GamesPlayed += 1
        My.Settings.GamesWon += 1
        My.Settings.Save()
        ' --- check if they want to play again ---
        Dim MyGameOver As New GameOverForm
        MyGameOver.GameOverLabel.Text = "YOU WON!!!"
        MyGameOver.GameOverLabel.ForeColor = Color.Blue
        Dim Plural As String = "s"
        If My.Settings.GamesPlayed = 1 Then Plural = ""
        MyGameOver.GamesWonLabel.Text = "You have won " + My.Settings.GamesWon.ToString + " out of " + My.Settings.GamesPlayed.ToString + " game" + Plural
        If MyGameOver.ShowDialog(Me) <> Windows.Forms.DialogResult.Yes Then
            Me.Close()
            Exit Sub
        End If
        ' --- Create a new game ---
        MyEngine.NewGame()
        ' --- Start the animation timer ---
        TickTimer.Enabled = True
    End Sub

    Private Sub MyEngine_LostGame() Handles MyEngine.LostGame
        TickTimer.Enabled = False
        ' --- update settings ---
        My.Settings.Reload()
        My.Settings.GamesPlayed += 1
        My.Settings.Save()
        ' --- check if they want to play again ---
        Dim MyGameOver As New GameOverForm
        MyGameOver.GameOverLabel.Text = "No more moves!"
        MyGameOver.GameOverLabel.ForeColor = Color.Red
        Dim Plural As String = "s"
        If My.Settings.GamesPlayed = 1 Then Plural = ""
        MyGameOver.GamesWonLabel.Text = "You have won " + My.Settings.GamesWon.ToString + " out of " + My.Settings.GamesPlayed.ToString + " game" + Plural
        If MyGameOver.ShowDialog(Me) <> Windows.Forms.DialogResult.Yes Then
            Me.Close()
        End If
        ' --- Create a new game ---
        MyEngine.NewGame()
        ' --- Start the animation timer ---
        TickTimer.Enabled = True
    End Sub

    Private Sub MyEngine_DebugMessage(ByVal Msg As String) Handles MyEngine.DebugMessage
        If ShowHintsToolStripMenuItem.Checked Then
            StatusLabel.Text = Msg
        End If
    End Sub

#End Region

#Region " --- Internal Routines --- "

    Private Sub AdjustScreen()
        ' --- Adjust the Scale Factor and the height when the screen is resized ---
        If MyEngine Is Nothing Then Exit Sub
        If MyEngine.MyLayout Is Nothing Then Exit Sub
        If Me.WindowState = FormWindowState.Minimized Then Exit Sub
        ' --- Adjust the main picture to fit the layout ---
        ScaleFactor = MainPicture.Width / MyEngine.MyLayout.LayoutSize.Width
        If ScaleFactor > MainPanel.Height / MyEngine.MyLayout.MinSize.Height Then
            ScaleFactor = MainPanel.Height / MyEngine.MyLayout.MinSize.Height
        End If
        If MainPicture.Height <> Round(MyEngine.MyLayout.LayoutSize.Height * ScaleFactor) Then
            MainPicture.Height = CInt(Round(MyEngine.MyLayout.LayoutSize.Height * ScaleFactor))
        End If
        ' --- Figure the proper rectangle for the display ---
        ScreenWidth = CInt(Round(MyEngine.MyLayout.LayoutSize.Width * ScaleFactor))
        ScreenHeight = CInt(Round(MyEngine.MyLayout.LayoutSize.Height * ScaleFactor))
        ScreenOfs = CInt((MainPicture.Width - ScreenWidth) / 2)
        ScreenRect = New Rectangle(ScreenOfs, 0, ScreenWidth, ScreenHeight)
        ' --- Redraw the entire screen ---
        MainPicture.Refresh()
    End Sub

    Private Sub ShowHintsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowHintsToolStripMenuItem.Click
        My.Settings.ShowHints = ShowHintsToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

#End Region

End Class
