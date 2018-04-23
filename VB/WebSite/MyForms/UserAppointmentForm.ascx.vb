Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.Web.ASPxHiddenField
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.XtraScheduler

Partial Public Class AppointmentForm
	Inherits SchedulerFormControl
	Public ReadOnly Property CanShowReminders() As Boolean
		Get
			Return (CType(Parent, AppointmentFormTemplateContainer)).Control.Storage.EnableReminders
		End Get
	End Property

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		PrepareChildControls()
		tbSubject.Focus()
	End Sub

	Public Overrides Sub DataBind()
		MyBase.DataBind()
		Dim container As AppointmentFormTemplateContainer = CType(Parent, AppointmentFormTemplateContainer)

		Dim apt As Appointment = container.Appointment
		edtLabel.SelectedIndex = apt.LabelId
		edtStatus.SelectedIndex = apt.StatusId

		AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence

		If container.Appointment.HasReminder Then
			cbReminder.Value = container.Appointment.Reminder.TimeBeforeStart.ToString()
			chkReminder.Checked = True
		Else
			cbReminder.ClientEnabled = False
		End If

		Dim resources As IList = TryCast(container.ResourceDataSource, IList)
		Dim count As Integer = Me.rpResources.Items.Count
		For i As Integer = 0 To count - 1
			Dim resourceIdString As String = (CType(resources(i), ListEditItem)).Value.ToString()
			Dim caption As String = (CType(resources(i), ListEditItem)).Text
			If resourceIdString = "null" Then
				Continue For
			End If
			Dim resourceId As Integer = Integer.Parse(resourceIdString)

			Dim repeaterItem As RepeaterItem = Me.rpResources.Items(i)
			Dim chkBox As ASPxCheckBox = TryCast(repeaterItem.FindControl("chkResource"), ASPxCheckBox)
			Dim hiddenField As ASPxHiddenField = TryCast(repeaterItem.FindControl("hfResource"), ASPxHiddenField)

			chkBox.Text = caption
			chkBox.Checked = apt.ResourceIds.Contains(resourceId)
			hiddenField.Set("resourceId", resourceId)
		Next i

		btnOk.ClientSideEvents.Click = container.SaveHandler
		btnCancel.ClientSideEvents.Click = container.CancelHandler
		btnDelete.ClientSideEvents.Click = container.DeleteHandler
	End Sub

	Protected Overrides Sub PrepareChildControls()
		Dim container As AppointmentFormTemplateContainer = CType(Parent, AppointmentFormTemplateContainer)
		Dim control As ASPxScheduler = container.Control

		AppointmentRecurrenceForm1.EditorsInfo = New EditorsInfo(control, control.Styles.FormEditors, control.Images.FormEditors, control.Styles.Buttons)
		MyBase.PrepareChildControls()
	End Sub
	Protected Overrides Function GetChildEditors() As ASPxEditBase()
		Dim edits() As ASPxEditBase = { lblSubject, tbSubject, lblLocation, tbLocation, lblLabel, edtLabel, lblStartDate, edtStartDate, lblEndDate, edtEndDate, lblStatus, edtStatus, lblAllDay, chkAllDay, lblResource, tbDescription, cbReminder }
		Return edits
	End Function
	Protected Overrides Function GetChildButtons() As ASPxButton()
		Dim buttons() As ASPxButton = { btnOk, btnCancel, btnDelete }
		Return buttons
	End Function
End Class
