<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GameOverForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GameOverLabel = New System.Windows.Forms.Label()
        Me.PlayAgainLabel = New System.Windows.Forms.Label()
        Me.YesButton = New System.Windows.Forms.Button()
        Me.NoButton = New System.Windows.Forms.Button()
        Me.GamesWonLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'GameOverLabel
        '
        Me.GameOverLabel.BackColor = System.Drawing.Color.Transparent
        Me.GameOverLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GameOverLabel.Location = New System.Drawing.Point(12, 15)
        Me.GameOverLabel.Name = "GameOverLabel"
        Me.GameOverLabel.Size = New System.Drawing.Size(280, 40)
        Me.GameOverLabel.TabIndex = 0
        Me.GameOverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PlayAgainLabel
        '
        Me.PlayAgainLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PlayAgainLabel.BackColor = System.Drawing.Color.Transparent
        Me.PlayAgainLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PlayAgainLabel.Location = New System.Drawing.Point(12, 103)
        Me.PlayAgainLabel.Name = "PlayAgainLabel"
        Me.PlayAgainLabel.Size = New System.Drawing.Size(280, 23)
        Me.PlayAgainLabel.TabIndex = 1
        Me.PlayAgainLabel.Text = "Play again?"
        Me.PlayAgainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'YesButton
        '
        Me.YesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.YesButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.YesButton.Location = New System.Drawing.Point(51, 146)
        Me.YesButton.Name = "YesButton"
        Me.YesButton.Size = New System.Drawing.Size(75, 32)
        Me.YesButton.TabIndex = 2
        Me.YesButton.Text = "Yes!"
        Me.YesButton.UseVisualStyleBackColor = True
        '
        'NoButton
        '
        Me.NoButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NoButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.NoButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NoButton.Location = New System.Drawing.Point(178, 146)
        Me.NoButton.Name = "NoButton"
        Me.NoButton.Size = New System.Drawing.Size(75, 32)
        Me.NoButton.TabIndex = 3
        Me.NoButton.Text = "No"
        Me.NoButton.UseVisualStyleBackColor = True
        '
        'GamesWonLabel
        '
        Me.GamesWonLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.GamesWonLabel.BackColor = System.Drawing.Color.Transparent
        Me.GamesWonLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GamesWonLabel.Location = New System.Drawing.Point(12, 70)
        Me.GamesWonLabel.Name = "GamesWonLabel"
        Me.GamesWonLabel.Size = New System.Drawing.Size(280, 23)
        Me.GamesWonLabel.TabIndex = 4
        Me.GamesWonLabel.Text = "You have won 0 of 0 games"
        Me.GamesWonLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GameOverForm
        '
        Me.AcceptButton = Me.YesButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.NoButton
        Me.ClientSize = New System.Drawing.Size(304, 197)
        Me.ControlBox = False
        Me.Controls.Add(Me.GamesWonLabel)
        Me.Controls.Add(Me.NoButton)
        Me.Controls.Add(Me.YesButton)
        Me.Controls.Add(Me.PlayAgainLabel)
        Me.Controls.Add(Me.GameOverLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "GameOverForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Game Over"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GameOverLabel As System.Windows.Forms.Label
    Friend WithEvents PlayAgainLabel As System.Windows.Forms.Label
    Friend WithEvents YesButton As System.Windows.Forms.Button
    Friend WithEvents NoButton As System.Windows.Forms.Button
    Friend WithEvents GamesWonLabel As System.Windows.Forms.Label
End Class
