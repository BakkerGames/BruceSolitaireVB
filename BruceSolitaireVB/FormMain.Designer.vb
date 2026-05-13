<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InstructionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainPanel = New System.Windows.Forms.Panel()
        Me.FloatPictureBox = New System.Windows.Forms.PictureBox()
        Me.MainPicture = New System.Windows.Forms.PictureBox()
        Me.TickTimer = New System.Windows.Forms.Timer(Me.components)
        Me.StatusPanel = New System.Windows.Forms.Panel()
        Me.MainStatus = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ShowHintsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu.SuspendLayout()
        Me.MainPanel.SuspendLayout()
        CType(Me.FloatPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MainPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusPanel.SuspendLayout()
        Me.MainStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu
        '
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(792, 24)
        Me.MainMenu.TabIndex = 0
        Me.MainMenu.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.toolStripSeparator1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Image = CType(resources.GetObject("NewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.NewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(138, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionsToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.ToolsToolStripMenuItem.Text = "&Tools"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowHintsToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.OptionsToolStripMenuItem.Text = "&Options"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InstructionsToolStripMenuItem, Me.toolStripSeparator5, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'InstructionsToolStripMenuItem
        '
        Me.InstructionsToolStripMenuItem.Name = "InstructionsToolStripMenuItem"
        Me.InstructionsToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.InstructionsToolStripMenuItem.Text = "Instructions"
        '
        'toolStripSeparator5
        '
        Me.toolStripSeparator5.Name = "toolStripSeparator5"
        Me.toolStripSeparator5.Size = New System.Drawing.Size(133, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'MainPanel
        '
        Me.MainPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainPanel.AutoScroll = True
        Me.MainPanel.BackColor = System.Drawing.SystemColors.Control
        Me.MainPanel.Controls.Add(Me.FloatPictureBox)
        Me.MainPanel.Controls.Add(Me.MainPicture)
        Me.MainPanel.Location = New System.Drawing.Point(0, 24)
        Me.MainPanel.Name = "MainPanel"
        Me.MainPanel.Size = New System.Drawing.Size(792, 520)
        Me.MainPanel.TabIndex = 2
        '
        'FloatPictureBox
        '
        Me.FloatPictureBox.Location = New System.Drawing.Point(346, 235)
        Me.FloatPictureBox.Name = "FloatPictureBox"
        Me.FloatPictureBox.Size = New System.Drawing.Size(100, 50)
        Me.FloatPictureBox.TabIndex = 5
        Me.FloatPictureBox.TabStop = False
        Me.FloatPictureBox.Visible = False
        '
        'MainPicture
        '
        Me.MainPicture.BackColor = System.Drawing.SystemColors.Control
        Me.MainPicture.Dock = System.Windows.Forms.DockStyle.Top
        Me.MainPicture.Location = New System.Drawing.Point(0, 0)
        Me.MainPicture.Name = "MainPicture"
        Me.MainPicture.Size = New System.Drawing.Size(792, 429)
        Me.MainPicture.TabIndex = 0
        Me.MainPicture.TabStop = False
        '
        'TickTimer
        '
        Me.TickTimer.Interval = 8
        '
        'StatusPanel
        '
        Me.StatusPanel.Controls.Add(Me.MainStatus)
        Me.StatusPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.StatusPanel.Location = New System.Drawing.Point(0, 544)
        Me.StatusPanel.Name = "StatusPanel"
        Me.StatusPanel.Size = New System.Drawing.Size(792, 22)
        Me.StatusPanel.TabIndex = 3
        '
        'MainStatus
        '
        Me.MainStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
        Me.MainStatus.Location = New System.Drawing.Point(0, 0)
        Me.MainStatus.Name = "MainStatus"
        Me.MainStatus.Size = New System.Drawing.Size(792, 22)
        Me.MainStatus.TabIndex = 0
        Me.MainStatus.Text = "MainStatus"
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(0, 17)
        '
        'ShowHintsToolStripMenuItem
        '
        Me.ShowHintsToolStripMenuItem.CheckOnClick = True
        Me.ShowHintsToolStripMenuItem.Name = "ShowHintsToolStripMenuItem"
        Me.ShowHintsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ShowHintsToolStripMenuItem.Text = "&Show Hints"
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(792, 566)
        Me.Controls.Add(Me.StatusPanel)
        Me.Controls.Add(Me.MainPanel)
        Me.Controls.Add(Me.MainMenu)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MainMenu
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bruce's Solitaire"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.MainPanel.ResumeLayout(False)
        CType(Me.FloatPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MainPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusPanel.ResumeLayout(False)
        Me.StatusPanel.PerformLayout()
        Me.MainStatus.ResumeLayout(False)
        Me.MainStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainPanel As System.Windows.Forms.Panel
    Friend WithEvents MainPicture As System.Windows.Forms.PictureBox
    Friend WithEvents TickTimer As System.Windows.Forms.Timer
    Friend WithEvents StatusPanel As System.Windows.Forms.Panel
    Friend WithEvents MainStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents FloatPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents InstructionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowHintsToolStripMenuItem As ToolStripMenuItem
End Class
