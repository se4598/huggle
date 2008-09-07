Partial Class Main

    Public Sub Configure()
        'This gets called when the application loads and when the user configuration is changed

        SetShortcutDisplayText()
        SetTooltipText()
        SetQueueSelector()

        CurrentTab.ShowNewEdits = Config.ShowNewEdits
        TrayIcon.Visible = Config.TrayIcon
        QueueContainer.Width = Config.QueueWidth + QueueScroll.Width
        Splitter.Panel2Collapsed = Not Config.ShowLog

        PageDelete.Visible = (Administrator AndAlso Config.Delete)
        PageDeleteB.Visible = (Administrator AndAlso Config.Delete)
        PagePatrol.Visible = Config.Patrol
        PageProtect.Visible = (Administrator AndAlso Config.Protect)
        PageRequestProtection.Visible = Config.ProtectionRequests
        PageTagDelete.Visible = (Config.Speedy OrElse Config.Prod OrElse Config.Xfd)
        PageTagDeleteB.Visible = (Config.Speedy OrElse Config.Prod OrElse Config.Xfd)
        PageTagProd.Visible = Config.Prod
        PageTagSpeedy.Visible = Config.Speedy
        PageXfd.Visible = Config.Xfd

        UserBlock.Visible = (Administrator AndAlso Config.UseAdminFunctions AndAlso Config.Block)
        UserBlockB.Visible = (Administrator AndAlso Config.UseAdminFunctions AndAlso Config.Block)
        UserEmail.Visible = Config.Email
        UserMessageWelcome.Visible = (Config.Welcome IsNot Nothing)
        UserReport.Visible = Config.AIV OrElse Config.UAA OrElse Config.TRR
        UserReportB.Visible = Config.AIV OrElse Config.UAA OrElse Config.TRR
        UserWarn.Visible = Config.WarningSeries.Count > 0

        WarnVandalism.Visible = Config.WarningSeries.Contains("warning")
        WarnSpam.Visible = Config.WarningSeries.Contains("spam")
        WarnAttack.Visible = Config.WarningSeries.Contains("attack")
        WarnTest.Visible = Config.WarningSeries.Contains("test")
        WarnDelete.Visible = Config.WarningSeries.Contains("delete")
        WarnNpov.Visible = Config.WarningSeries.Contains("npov")
        WarnError.Visible = Config.WarningSeries.Contains("error")

        RevertWarnVandalism.Visible = Config.WarningSeries.Contains("warning")
        RevertWarnSpam.Visible = Config.WarningSeries.Contains("spam")
        RevertWarnAttack.Visible = Config.WarningSeries.Contains("attack")
        RevertWarnTest.Visible = Config.WarningSeries.Contains("test")
        RevertWarnDelete.Visible = Config.WarningSeries.Contains("delete")
        RevertWarnNpov.Visible = Config.WarningSeries.Contains("npov")
        RevertWarnError.Visible = Config.WarningSeries.Contains("error")

        For Each Item As ToolStrip In New ToolStrip() {MainStrip, HistoryStrip, NavigationStrip, ActionsStrip}
            Item.ShowItemToolTips = Config.ShowToolTips
        Next Item

        'Clear custom revert summaries menu
        Dim j As Integer = 0

        While j < RevertMenu.Items.Count
            If RevertMenu.Items(j) IsNot Separator20 AndAlso RevertMenu.Items(j) IsNot DiffRevertSummary _
                Then RevertMenu.Items.RemoveAt(j) Else j += 1
        End While

        'Add custom revert summaries to menu
        For Each Item As String In Config.CustomRevertSummaries
            Dim NewItem As New ToolStripMenuItem
            NewItem.Text = TrimSummary(Item)
            NewItem.Tag = CObj(Item)
            AddHandler NewItem.Click, AddressOf RevertItem_Click
            RevertMenu.Items.Insert(0, NewItem)
        Next Item

        For i As Integer = 2 To TemplateMenu.Items.Count - 5
            If TemplateMenu.Items.Count > i Then TemplateMenu.Items.RemoveAt(i)
        Next i

        'Add template messages to menu
        For Each Item As String In Config.TemplateMessages
            Dim NewItem As New ToolStripMenuItem
            Item = Item.Replace("\;", Convert.ToChar(1))

            If Item.Contains(";") Then
                NewItem.Text = Item.Substring(0, Item.IndexOf(";")).Replace(Convert.ToChar(1), ";")
                NewItem.Tag = CObj(Item.Substring(Item.IndexOf(";") + 1).Replace(Convert.ToChar(1), ";"))
                AddHandler NewItem.Click, AddressOf TemplateItem_Click
                TemplateMenu.Items.Insert(2, NewItem)
            End If
        Next Item

        'Add pages to 'go to' menu
        GoSeparator.Visible = (Config.GoToPages.Count > 0)

        For Each Item As String In Config.GoToPages
            Dim NewItem As New ToolStripMenuItem
            Item = Item.Replace("\;", Convert.ToChar(1))

            If Item.Contains(";") Then
                NewItem.Text = Item.Substring(Item.IndexOf(";") + 1).Replace(Convert.ToChar(1), ";")
                NewItem.Tag = CObj(Item.Substring(0, Item.IndexOf(";")).Replace(Convert.ToChar(1), ";"))
                AddHandler NewItem.Click, AddressOf GoToItem_Click
                GoToMenu.DropDownItems.Add(NewItem)
            End If
        Next Item

        'Add speedy deletion criteria to menu
        For Each Item As SpeedyCriterion In SpeedyCriteria.Values
            Dim NewItem As New ToolStripMenuItem
            NewItem.Name = "Speedy" & Item.Code
            NewItem.Text = Item.DisplayCode & " - " & Item.Description
            AddHandler NewItem.Click, AddressOf Speedy_Click
            TagDeleteMenu.Items.Add(NewItem)
        Next Item

        RefreshInterface()
    End Sub

    Public Sub RefreshInterface()
        If Me Is Nothing OrElse Not Visible Then Exit Sub

        MenuPage.Enabled = (CurrentPage IsNot Nothing)
        MenuUser.Enabled = (CurrentUser IsNot Nothing)

        'Set queue alignment
        If Config.ShowQueue Then
            Tabs.Width = Tabs.Parent.Width - QueueContainer.Width
            QueueContainer.Visible = True

            If Config.RightAlignQueue Then
                Tabs.Left = 0
                QueueContainer.Left = Tabs.Width
            Else
                Tabs.Left = QueueContainer.Width
                QueueContainer.Left = 0
            End If
        Else
            Tabs.Left = 0
            Tabs.Width = Tabs.Parent.Width
            QueueContainer.Visible = False
        End If

        If CurrentEdit IsNot Nothing AndAlso CurrentPage IsNot Nothing AndAlso CurrentUser IsNot Nothing _
            AndAlso CurrentQueue IsNot Nothing Then

            If CurrentPage.IsArticleTalkPage Then
                PageSwitchTalk.Text = "Switch to article"
            ElseIf CurrentPage.IsTalkPage Then
                PageSwitchTalk.Text = "Switch to subject page"
            Else
                PageSwitchTalk.Text = "Switch to talk page"
            End If

            Select Case CurrentPage.Space.Name
                Case "" : PageXfd.Enabled = (Config.AfdLocation IsNot Nothing)
                Case "Category" : PageXfd.Enabled = (Config.CfdLocation IsNot Nothing)
                Case "Image" : PageXfd.Enabled = (Config.IfdLocation IsNot Nothing)
                Case "Template" : PageXfd.Enabled = (Config.TfdLocation IsNot Nothing)
                Case Else : PageXfd.Enabled = (Config.MfdLocation IsNot Nothing)
            End Select

            If CurrentQueue.Type = QueueType.FixedList _
                Then QueueClear.Text = "Reset" Else QueueClear.Text = "Clear current"

            Dim Editable As Boolean = (CurrentPage.EditLevel <> "sysop" OrElse Administrator)

            BrowserBack.Enabled = (CurrentTab.HistoryIndex < CurrentTab.History.Count - 1)
            BrowserBackB.Enabled = (CurrentTab.HistoryIndex < CurrentTab.History.Count - 1)
            BrowserForward.Enabled = (CurrentTab.HistoryIndex > 0)
            BrowserForwardB.Enabled = (CurrentTab.HistoryIndex > 0)
            BrowserNewContribs.Checked = CurrentTab.ShowNewContribs
            BrowserNewEdits.Checked = CurrentTab.ShowNewEdits
            BrowserNewTabB.Enabled = True
            BrowserOpenB.Enabled = (CurrentTab.CurrentUrl IsNot Nothing)
            ContribsB.Enabled = (CurrentEdit.User.FirstEdit Is Nothing)
            ContribsPrevB.Enabled = (CurrentEdit.PrevByUser IsNot Nothing)
            ContribsNextB.Enabled = (CurrentEdit.NextByUser IsNot Nothing)
            ContribsLastB.Enabled = (CurrentEdit.User.LastEdit IsNot Nothing _
                AndAlso CurrentEdit.User.LastEdit IsNot CurrentEdit)
            DiffNextB.Enabled = (CurrentQueue.Edits.Count > 0)
            DiffRevertB.Enabled = (CurrentEdit IsNot CurrentPage.FirstEdit AndAlso Not RevertTimer.Enabled _
                AndAlso Editable)
            HistoryB.Enabled = (CurrentEdit.Page.FirstEdit Is Nothing)
            HistoryDiffToCurB.Enabled = (Not CurrentEdit Is CurrentPage.LastEdit) AndAlso (Not CurrentEdit.Multiple)
            HistoryPrevB.Enabled = CurrentEdit.Prev IsNot NullEdit _
                AndAlso Not (CurrentEdit.Prev IsNot Nothing AndAlso CurrentEdit.Prev.Prev Is NullEdit) _
                AndAlso CurrentEdit.Prev IsNot CurrentPage.FirstEdit
            HistoryNextB.Enabled = (Not CurrentEdit Is CurrentPage.LastEdit)
            HistoryLastB.Enabled = (Not CurrentEdit Is CurrentPage.LastEdit)
            PageDelete.Enabled = True
            PageDeleteB.Enabled = True
            PageEdit.Enabled = Editable
            PageEditB.Enabled = Editable
            PageHistory.Enabled = HistoryB.Enabled
            PageMove.Enabled = CurrentPage.IsMovable
            PagePatrol.Enabled = (Not CurrentPage.Patrolled)
            PageProtect.Enabled = True
            PageRequestProtection.Enabled = True
            PageTag.Enabled = Editable
            PageTagB.Enabled = Editable
            PageTagDelete.Enabled = Editable
            PageTagDeleteB.Enabled = Editable
            PageTagProd.Enabled = Editable
            PageTagSpeedy.Enabled = Editable
            PageView.Enabled = True
            PageViewB.Enabled = True
            PageViewLatest.Enabled = True
            PageWatch.Enabled = True
            PageWatchB.Enabled = True
            PageXfd.Enabled = Editable
            RevertWarnB.Enabled = (DiffRevertB.Enabled AndAlso Not CurrentUser.Ignored AndAlso Editable _
                AndAlso Config.WarningSeries.Count > 0)
            QueueClear.Enabled = (CurrentQueue.Edits.Count > 0)
            QueueNext.Enabled = (CurrentQueue.Edits.Count > 0)
            QueueTrim.Enabled = (CurrentQueue.Edits.Count > 0)
            SystemReconnectIRC.Enabled = (Config.IrcMode = True)
            SystemShowQueue.Checked = Config.ShowQueue
            SystemShowLog.Checked = Config.ShowLog
            UndoB.Enabled = (Undo.Count > 0)
            UserBlock.Enabled = (Not CurrentUser.Ignored)
            UserBlockB.Enabled = (Not CurrentUser.Ignored)
            UserContribs.Enabled = ContribsB.Enabled
            UserEmail.Enabled = (Not CurrentUser.Anonymous)
            UserIgnore.Enabled = True
            UserIgnoreB.Enabled = True
            UserInfo.Enabled = True
            UserInfoB.Enabled = True
            UserMessage.Enabled = True
            UserMessageB.Enabled = True
            UserReport.Enabled = (Not CurrentUser.Ignored AndAlso CurrentUser.Level < UserLevel.Blocked)
            UserReportB.Enabled = (Not CurrentUser.Ignored AndAlso CurrentUser.Level < UserLevel.Blocked)
            UserTalk.Enabled = True
            UserTalkB.Enabled = True
            UserTemplateB.Enabled = (Not CurrentUser.Ignored)
            UserWarn.Enabled = (Not CurrentUser.Ignored AndAlso Config.WarningSeries.Count > 0)
            WarnB.Enabled = (Not CurrentUser.Ignored AndAlso Config.WarningSeries.Count > 0)

            For Each Item As ToolStripItem In WarnMenu.Items
                Item.Enabled = WarnB.Enabled
            Next Item

            For Each Item As ToolStripItem In RevertMenu.Items
                Item.Enabled = DiffRevertB.Enabled
            Next Item

            For Each Item As ToolStripItem In TemplateMenu.Items
                Item.Enabled = UserTemplateB.Enabled
            Next Item

            For Each Item As ToolStripItem In TagDeleteMenu.Items
                If Item.Name.StartsWith("Speedy") Then
                    Dim Code As String = Item.Name.ToUpper.Substring(6)

                    If Code = "G8" Then : Item.Visible = CurrentPage.IsTalkPage

                    ElseIf Code.StartsWith("A") Then : Item.Visible = (CurrentPage.IsArticle)
                    ElseIf Code.StartsWith("C") Then : Item.Visible = (CurrentPage.Space Is Space.Category)
                    ElseIf Code.StartsWith("I") Then : Item.Visible = (CurrentPage.Space Is Space.Image)
                    ElseIf Code.StartsWith("P") Then : Item.Visible = (CurrentPage.Space.Name = "Portal")
                    ElseIf Code.StartsWith("T") Then : Item.Visible = (CurrentPage.Space Is Space.Template)
                    ElseIf Code.StartsWith("U") Then : Item.Visible = (CurrentPage.Space Is Space.User _
                        OrElse CurrentPage.Space Is Space.UserTalk)
                    Else
                        Item.Visible = True
                    End If
                End If
            Next Item

            DrawQueue()
            UpdateWatchButton()
            UpdateIgnoreButton()
        End If
    End Sub

    Sub UpdateWatchButton()
        If Watchlist.Contains(CurrentEdit.Page.SubjectPage) AndAlso PageWatch.Text = "Watch" Then
            PageWatchB.Image = My.Resources.page_unwatch
            PageWatch.Text = "Unwatch"
        ElseIf PageWatch.Text = "Unwatch" AndAlso Not Watchlist.Contains(CurrentEdit.Page.SubjectPage) Then
            PageWatchB.Image = My.Resources.page_watch
            PageWatch.Text = "Watch"
        End If
    End Sub

    Sub UpdateIgnoreButton()
        If CurrentUser.Ignored AndAlso UserIgnore.Text = "Ignore" Then
            UserIgnoreB.Image = My.Resources.user_unwhitelist
            UserIgnore.Text = "Unignore"
        ElseIf Not CurrentUser.Ignored AndAlso UserIgnore.Text = "Unignore" Then
            UserIgnoreB.Image = My.Resources.user_whitelist
            UserIgnore.Text = "Ignore"
        End If
    End Sub

    Public Sub Main_KeyDown(ByVal s As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If UserB.Focused OrElse PageB.Focused OrElse e.Modifiers = Keys.Alt OrElse KeyDelayTimer.Enabled Then Exit Sub

        KeyDelayTimer.Start()

        Dim Shortcut As New Shortcut(e.KeyCode, e.Control, e.Alt, e.Shift)

        Select Case Shortcut
            Case Is = ShortcutKeys("About")
                AboutForm.ShowDialog()

            Case Is = ShortcutKeys("Cancel")
                If CancelB.Enabled Then CancelB_Click()

            Case Is = ShortcutKeys("Help")
                HelpDocs_Click()

            Case Is = ShortcutKeys("User information")
                UserInfo_Click()

            Case Is = ShortcutKeys("Clear queue")
                If QueueClear.Enabled Then QueueClear_Click()

            Case Is = ShortcutKeys("Show next diff")
                If DiffNextB.Enabled Then DiffNextB_Click()

            Case Is = ShortcutKeys("View user talk page")
                If UserTalkB.Enabled Then UserTalk_Click()

            Case Is = ShortcutKeys("Report / block user")
                If UserReportB.Enabled AndAlso UserReportB.Visible Then UserReportVandalism_Click()

            Case Is = ShortcutKeys("Latest contribution")
                If ContribsLastB.Enabled Then ContribsLast_Click()

            Case Is = ShortcutKeys("Current revision")
                If HistoryLastB.Enabled Then HistoryLast_Click()

            Case Is = ShortcutKeys("Delete page")
                If PageDeleteB.Enabled AndAlso PageDeleteB.Visible AndAlso Administrator Then PageDelete_Click()

            Case Is = ShortcutKeys("Diff to current revision")
                If HistoryDiffToCurB.Enabled Then HistoryDiffToCur_Click()

            Case Is = ShortcutKeys("Edit page")
                If PageEditB.Enabled Then EditPage_Click()

            Case Is = ShortcutKeys("Tag page")
                If PageTagB.Enabled Then TagPage_Click()

            Case Is = ShortcutKeys("Retrieve page history")
                If HistoryB.Enabled Then ViewHistory_Click()

            Case Is = ShortcutKeys("Ignore user")
                If UserIgnoreB.Enabled AndAlso CurrentEdit IsNot Nothing AndAlso CurrentEdit.User IsNot Nothing _
                    AndAlso Not CurrentEdit.User.Ignored Then UserIgnore_Click()

            Case Is = ShortcutKeys("Unignore user")
                If UserIgnoreB.Enabled AndAlso CurrentEdit IsNot Nothing AndAlso CurrentEdit.User IsNot Nothing _
                    AndAlso Not CurrentEdit.User.Ignored Then UserIgnore_Click()

            Case Is = ShortcutKeys("Toggle 'show new edits'")
                ShowNewEdits_Click()

            Case Is = ShortcutKeys("Watch page")
                If PageWatchB.Enabled Then WatchPage_Click()

            Case Is = ShortcutKeys("Show new messages")
                If SystemShowNewMessages.Enabled Then SystemShowNewMessages_Click()

            Case Is = ShortcutKeys("Message user")
                If UserMessageB.Enabled Then UserMessage_Click()

            Case Is = ShortcutKeys("Open page in external browser")
                BrowserOpen_Click()

            Case Is = ShortcutKeys("Mark page as patrolled")
                If PagePatrol.Enabled AndAlso Config.Patrol Then PageMarkPatrolled_Click()

            Case Is = ShortcutKeys("Proposed deletion")
                If PageTagProd.Enabled AndAlso Config.Prod Then PageTagProd_Click()

            Case Is = ShortcutKeys("Revert and warn")
                If RevertWarnB.Enabled Then RevertWarnB_ButtonClick()

            Case Is = ShortcutKeys("Revert")
                If DiffRevertB.Enabled Then Revert_Click()

            Case Is = ShortcutKeys("Nominate for deletion")
                If PageXfd.Enabled Then PageNominate_Click()

            Case Is = ShortcutKeys("Request deletion")
                If PageTagDeleteB.Enabled Then PageTagDeleteB.ShowDropDown()

            Case Is = ShortcutKeys("Post template message")
                If UserTemplateB.Enabled Then UserTemplateB.ShowDropDown()

            Case Is = ShortcutKeys("Retrieve user contributions")
                If ContribsB.Enabled Then UserContribs_Click()

            Case Is = ShortcutKeys("View current revision")
                If PageViewLatest.Enabled Then PageViewLatest_Click()

            Case Is = ShortcutKeys("View this revision")
                If PageViewB.Enabled Then PageView_Click()

            Case Is = ShortcutKeys("Warn")
                If WarnB.Enabled Then WarnB.ShowDropDown()

            Case Is = ShortcutKeys("Next contribution")
                If ContribsNextB.Enabled Then ContribsNext_Click()

            Case Is = ShortcutKeys("Next revision")
                If HistoryNextB.Enabled Then HistoryNext_Click()

            Case Is = ShortcutKeys("Revert with custom summary")
                If DiffRevertB.Enabled Then DiffRevertSummary_Click()

            Case Is = ShortcutKeys("Previous contribution")
                If ContribsPrevB.Enabled Then ContribsPrev_Click()

            Case Is = ShortcutKeys("Previous revision")
                If HistoryPrevB.Enabled Then HistoryPrev_Click()

            Case Is = ShortcutKeys("Browse back")
                If BrowserBackB.Enabled Then GoBack_Click()

            Case Is = ShortcutKeys("Browse forward")
                If BrowserForwardB.Enabled Then GoForward_Click()

            Case Is = ShortcutKeys("New tab")
                NewTab_Click()

            Case Is = ShortcutKeys("Close tab")
                If Tabs.TabPages.Count > 1 Then CloseTab_Click()

            Case Is = ShortcutKeys("Next tab")
                Tabs.SelectedIndex = (Tabs.SelectedIndex + 1) Mod Tabs.TabCount

            Case Is = ShortcutKeys("Previous tab")
                Tabs.SelectedIndex = (Tabs.SelectedIndex - 1) Mod Tabs.TabCount

            Case Is = ShortcutKeys("Warn - vandalism")
                If WarnB.Enabled AndAlso Config.WarningSeries.Contains("warning") Then WarnVandalism_Click()

            Case Is = ShortcutKeys("Warn - spam")
                If WarnB.Enabled AndAlso Config.WarningSeries.Contains("spam") Then WarnSpam_Click()

            Case Is = ShortcutKeys("Warn - editing tests")
                If WarnB.Enabled AndAlso Config.WarningSeries.Contains("test") Then WarnTest_Click()

            Case Is = ShortcutKeys("Warn - removing content")
                If WarnB.Enabled AndAlso Config.WarningSeries.Contains("delete") Then WarnDelete_Click()

            Case Is = ShortcutKeys("Warn - personal attacks")
                If WarnB.Enabled AndAlso Config.WarningSeries.Contains("attack") Then WarnAttacks_Click()

            Case Is = ShortcutKeys("Warn - factual errors")
                If WarnB.Enabled AndAlso Config.WarningSeries.Contains("error") Then WarnError_Click()

            Case Is = ShortcutKeys("Warn - biased content")
                If WarnB.Enabled AndAlso Config.WarningSeries.Contains("npov") Then WarnNpov_Click()

            Case Is = ShortcutKeys("Revert and warn - spam")
                If Config.WarningSeries.Contains("spam") Then RevertAndWarn("spam")

            Case Is = ShortcutKeys("Revert and warn - editing tests")
                If Config.WarningSeries.Contains("test") Then RevertAndWarn("test")

            Case Is = ShortcutKeys("Revert and warn - removing content")
                If Config.WarningSeries.Contains("delete") Then RevertAndWarn("delete")

            Case Is = ShortcutKeys("Revert and warn - personal attacks")
                If Config.WarningSeries.Contains("attack") Then RevertAndWarn("attack")

            Case Is = ShortcutKeys("Revert and warn - factual errors")
                If Config.WarningSeries.Contains("error") Then RevertAndWarn("error")

            Case Is = ShortcutKeys("Revert and warn - biased content")
                If Config.WarningSeries.Contains("npov") Then RevertAndWarn("npov")

            Case Is = ShortcutKeys("Revert and warn - unsourced content")
                If Config.WarningSeries.Contains("unsourced") Then RevertAndWarn("unsourced")

            Case Is = ShortcutKeys("Request protection")
                If PageRequestProtection.Enabled Then PageRequestProtection_Click()
        End Select

        e.Handled = True
        e.SuppressKeyPress = True
    End Sub

    Private Sub SetShortcutDisplayText()
        SetSDItem(BrowserBack, "Browse back")
        SetSDItem(BrowserForward, "Browse forward")
        SetSDItem(BrowserCloseTab, "Close tab")
        SetSDItem(BrowserCloseOthers, "Close other tabs")
        SetSDItem(BrowserNewTab, "New tab")
        SetSDItem(BrowserOpen, "Open page in external browser")
        SetSDItem(HelpAbout, "About")
        SetSDItem(HelpDocs, "Help")
        SetSDItem(PageDelete, "Delete page")
        SetSDItem(PageEdit, "Edit page")
        SetSDItem(PageHistory, "Retrieve page history")
        SetSDItem(PagePatrol, "Mark page as patrolled")
        SetSDItem(PageMove, "Move page")
        SetSDItem(PageXfd, "Nominate for deletion")
        SetSDItem(PageTagProd, "Proposed deletion")
        SetSDItem(PageProtect, "Protect page")
        SetSDItem(PageRequestProtection, "Request protection")
        SetSDItem(PageTag, "Tag page")
        SetSDItem(PageView, "View this revision")
        SetSDItem(PageViewLatest, "View current revision")
        SetSDItem(PageWatch, "Watch page")
        SetSDItem(QueueClear, "Clear queue")
        SetSDItem(QueueNext, "Show next diff")
        SetSDItem(SystemShowNewMessages, "Show new messages")
        SetSDItem(UserBlock, "Report / block user")
        SetSDItem(UserContribs, "Retrieve user contributions")
        SetSDItem(UserIgnore, "Ignore user")
        SetSDItem(UserInfo, "User information")
        SetSDItem(UserMessage, "Message user")
        SetSDItem(UserReport, "Report / block user")
        SetSDItem(UserTalk, "View user talk page")
        SetSDItem(WarnVandalism, "Warn - vandalism")
        SetSDItem(WarnSpam, "Warn - spam")
        SetSDItem(WarnTest, "Warn - editing tests")
        SetSDItem(WarnDelete, "Warn - removing content")
        SetSDItem(WarnAttack, "Warn - personal attacks")
        SetSDItem(WarnError, "Warn - factual errors")
        SetSDItem(WarnNpov, "Warn - biased content")
        SetSDItem(RevertWarnVandalism, "Revert and warn")
        SetSDItem(RevertWarnSpam, "Revert and warn - spam")
        SetSDItem(RevertWarnTest, "Revert and warn - editing tests")
        SetSDItem(RevertWarnDelete, "Revert and warn - removing content")
        SetSDItem(RevertWarnAttack, "Revert and warn - personal attacks")
        SetSDItem(RevertWarnError, "Revert and warn - factual errors")
        SetSDItem(RevertWarnNpov, "Revert and warn - biased content")
    End Sub

    Private Sub SetTooltipText()
        SetTTItem(BrowserBackB, "Browse back")
        SetTTItem(BrowserCloseTabB, "Close tab")
        SetTTItem(BrowserForwardB, "Browse forward")
        SetTTItem(BrowserNewTabB, "New tab")
        SetTTItem(BrowserOpenB, "Open page in external browser")
        SetTTItem(CancelB, "Cancel")
        SetTTItem(ContribsB, "Retrieve user contributions")
        SetTTItem(ContribsLastB, "Latest contribution")
        SetTTItem(ContribsNextB, "Next contribution")
        SetTTItem(ContribsPrevB, "Previous contribution")
        SetTTItem(DiffNextB, "Show next diff")
        SetTTItem(DiffRevertB, "Revert")
        SetTTItem(RevertWarnB, "Revert and warn")
        SetTTItem(HistoryB, "Retrieve page history")
        SetTTItem(HistoryDiffToCurB, "Diff to current revision")
        SetTTItem(HistoryLastB, "Current revision")
        SetTTItem(HistoryNextB, "Next revision")
        SetTTItem(HistoryPrevB, "Previous revision")
        SetTTItem(PageEditB, "Edit page")
        SetTTItem(PageDeleteB, "Delete page")
        SetTTItem(PageTagB, "Tag page")
        SetTTItem(PageTagDeleteB, "Request deletion")
        SetTTItem(PageViewB, "View this revision")
        SetTTItem(PageWatchB, "Watch page")
        SetTTItem(UserIgnoreB, "Ignore user")
        SetTTItem(UserInfoB, "User information")
        SetTTItem(UserMessageB, "Message user")
        SetTTItem(UserReportB, "Report user")
        SetTTItem(UserTalkB, "View user talk page")
        SetTTItem(UserTemplateB, "Post template message")
        SetTTItem(WarnB, "Warn")
    End Sub

    Private Sub SetSDItem(ByVal MenuItem As ToolStripMenuItem, ByVal Key As String)
        If ShortcutKeys.ContainsKey(Key) Then
            If ShortcutKeys(Key).Key = Keys.None Then MenuItem.ShortcutKeyDisplayString = "" _
                Else MenuItem.ShortcutKeyDisplayString = ShortcutKeys(Key).ToString
        End If
    End Sub

    Private Sub SetTTItem(ByVal Item As ToolStripItem, ByVal Key As String)
        If ShortcutKeys.ContainsKey(Key) Then
            If Item.ToolTipText.Contains("[") Then _
                Item.ToolTipText = Item.ToolTipText.Substring(0, Item.ToolTipText.IndexOf("["))
            If ShortcutKeys(Key).Key <> Keys.None Then Item.ToolTipText &= "[" & ShortcutKeys(Key).ToString & "]"
        End If
    End Sub

    Public Sub SetQueueSelector()
        QueueSelector.Items.Clear()

        For Each Item As String In QueueOrder
            QueueSelector.Items.Add(Item)
        Next Item

        QueueSelector.Items.Add("Add...")
        If CurrentQueue IsNot Nothing Then QueueSelector.SelectedItem = CurrentQueue.Name
    End Sub

    Private Sub KeyDelayTimer_Tick() Handles KeyDelayTimer.Tick
        KeyDelayTimer.Stop()
    End Sub

End Class