' --- GameOverForm.vb - 12/27/2008 ---

' --- LastMod: 08/07/2020

Public Class GameOverForm

    Private Sub YesButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles YesButton.Click
        Me.DialogResult = DialogResult.Yes
        Close()
    End Sub

    Private Sub YesButton_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles YesButton.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.DialogResult = DialogResult.Yes
            Close()
        End If
    End Sub

    Private Sub NoButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NoButton.Click
        Me.DialogResult = DialogResult.No
        Close()
    End Sub

    Private Sub NoButton_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles NoButton.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.DialogResult = DialogResult.No
            Close()
        End If
    End Sub

End Class
