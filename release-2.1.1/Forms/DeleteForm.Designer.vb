<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DeleteForm
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
        Me.Cancel = New System.Windows.Forms.Button
        Me.OK = New System.Windows.Forms.Button
        Me.ReasonLabel = New System.Windows.Forms.Label
        Me.Reason = New System.Windows.Forms.ComboBox
        Me.DeletionLogLabel = New System.Windows.Forms.Label
        Me.DeletionLog = New Huggle.PageLog
        Me.Throbber = New Huggle.Throbber
        Me.Progress = New System.Windows.Forms.Label
        Me.Notify = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'Cancel
        '
        Me.Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel.Location = New System.Drawing.Point(399, 211)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Cancel.TabIndex = 8
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'OK
        '
        Me.OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK.Location = New System.Drawing.Point(318, 211)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 7
        Me.OK.Text = "OK"
        Me.OK.UseVisualStyleBackColor = True
        '
        'ReasonLabel
        '
        Me.ReasonLabel.AutoSize = True
        Me.ReasonLabel.Location = New System.Drawing.Point(12, 15)
        Me.ReasonLabel.Name = "ReasonLabel"
        Me.ReasonLabel.Size = New System.Drawing.Size(47, 13)
        Me.ReasonLabel.TabIndex = 0
        Me.ReasonLabel.Text = "Reason:"
        '
        'Reason
        '
        Me.Reason.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Reason.FormattingEnabled = True
        Me.Reason.Location = New System.Drawing.Point(65, 12)
        Me.Reason.MaxDropDownItems = 20
        Me.Reason.Name = "Reason"
        Me.Reason.Size = New System.Drawing.Size(409, 21)
        Me.Reason.TabIndex = 1
        '
        'DeletionLogLabel
        '
        Me.DeletionLogLabel.AutoSize = True
        Me.DeletionLogLabel.Location = New System.Drawing.Point(12, 83)
        Me.DeletionLogLabel.Name = "DeletionLogLabel"
        Me.DeletionLogLabel.Size = New System.Drawing.Size(66, 13)
        Me.DeletionLogLabel.TabIndex = 5
        Me.DeletionLogLabel.Text = "Deletion log:"
        '
        'DeletionLog
        '
        Me.DeletionLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DeletionLog.FullRowSelect = True
        Me.DeletionLog.GridLines = True
        Me.DeletionLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.DeletionLog.Location = New System.Drawing.Point(15, 99)
        Me.DeletionLog.Mode = Huggle.PageLog.ViewMode.Delete
        Me.DeletionLog.Name = "DeletionLog"
        Me.DeletionLog.Page = Nothing
        Me.DeletionLog.Size = New System.Drawing.Size(459, 106)
        Me.DeletionLog.TabIndex = 6
        Me.DeletionLog.UseCompatibleStateImageBehavior = False
        Me.DeletionLog.View = System.Windows.Forms.View.Details
        '
        'Throbber
        '
        Me.Throbber.BackColor = System.Drawing.Color.Gainsboro
        Me.Throbber.Location = New System.Drawing.Point(65, 40)
        Me.Throbber.Name = "Throbber"
        Me.Throbber.Size = New System.Drawing.Size(58, 10)
        Me.Throbber.TabIndex = 2
        '
        'Progress
        '
        Me.Progress.AutoSize = True
        Me.Progress.Location = New System.Drawing.Point(137, 40)
        Me.Progress.Name = "Progress"
        Me.Progress.Size = New System.Drawing.Size(10, 13)
        Me.Progress.TabIndex = 3
        Me.Progress.Text = " "
        '
        'Notify
        '
        Me.Notify.AutoSize = True
        Me.Notify.Enabled = False
        Me.Notify.Location = New System.Drawing.Point(65, 59)
        Me.Notify.Name = "Notify"
        Me.Notify.Size = New System.Drawing.Size(89, 17)
        Me.Notify.TabIndex = 4
        Me.Notify.Text = "Notify creator"
        Me.Notify.UseVisualStyleBackColor = True
        '
        'DeleteForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(486, 246)
        Me.Controls.Add(Me.Notify)
        Me.Controls.Add(Me.Progress)
        Me.Controls.Add(Me.Throbber)
        Me.Controls.Add(Me.DeletionLog)
        Me.Controls.Add(Me.DeletionLogLabel)
        Me.Controls.Add(Me.Reason)
        Me.Controls.Add(Me.ReasonLabel)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.Cancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DeleteForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Delete page"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents ReasonLabel As System.Windows.Forms.Label
    Friend WithEvents Reason As System.Windows.Forms.ComboBox
    Friend WithEvents DeletionLogLabel As System.Windows.Forms.Label
    Friend WithEvents DeletionLog As Huggle.PageLog
    Friend WithEvents Throbber As Huggle.Throbber
    Friend WithEvents Progress As System.Windows.Forms.Label
    Friend WithEvents Notify As System.Windows.Forms.CheckBox
End Class