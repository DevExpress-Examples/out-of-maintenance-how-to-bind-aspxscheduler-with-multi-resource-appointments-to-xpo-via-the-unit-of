Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Web
Imports DevExpress.XtraScheduler
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpress.Web.ASPxEditors
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxHiddenField

Public Class UserAppointmentSaveCallbackCommand
	Inherits AppointmentFormSaveCallbackCommand
	Public Sub New(ByVal control As ASPxScheduler)
		MyBase.New(control)
	End Sub

	Protected Overrides Sub AssignControllerValues()
		Dim tbSubject As ASPxTextBox = CType(FindControlByID("tbSubject"), ASPxTextBox)
		Dim tbLocation As ASPxTextBox = CType(FindControlByID("tbLocation"), ASPxTextBox)
		Dim tbDescription As ASPxMemo = CType(FindControlByID("tbDescription"), ASPxMemo)
		Dim edtStartDate As ASPxDateEdit = CType(FindControlByID("edtStartDate"), ASPxDateEdit)
		Dim edtEndDate As ASPxDateEdit = CType(FindControlByID("edtEndDate"), ASPxDateEdit)
		Dim chkAllDay As ASPxCheckBox = CType(FindControlByID("chkAllDay"), ASPxCheckBox)
		Dim edtShowTimeAs As ASPxComboBox = CType(FindControlByID("edtStatus"), ASPxComboBox)
		Dim edtLabel As ASPxComboBox = CType(FindControlByID("edtLabel"), ASPxComboBox)
		Dim chkReminder As ASPxCheckBox = CType(FindControlByID("chkReminder"), ASPxCheckBox)
		Dim cbReminder As ASPxComboBox = CType(FindControlByID("cbReminder"), ASPxComboBox)
		Dim rpResources As Repeater = CType(FindControlByID("rpResources"), Repeater)
		Dim clientStart As DateTime = DateTime.MinValue
		Dim clientEnd As DateTime = DateTime.MinValue
		If edtStartDate IsNot Nothing Then
			clientStart = FromClientTime(edtStartDate.Date)
			Controller.EditedAppointmentCopy.Start = clientStart
		End If
		If edtEndDate IsNot Nothing Then
			clientEnd = FromClientTime(edtEndDate.Date)
			Controller.EditedAppointmentCopy.End = clientEnd
		End If
		If tbSubject IsNot Nothing Then
			Controller.Subject = tbSubject.Text
		End If
		If tbLocation IsNot Nothing Then
			Controller.Location = tbLocation.Text
		End If
		If tbDescription IsNot Nothing Then
			Controller.Description = tbDescription.Text
		End If
		If chkAllDay IsNot Nothing Then
			Controller.AllDay = chkAllDay.Checked
		End If
		If edtShowTimeAs IsNot Nothing Then
			Controller.StatusId = Convert.ToInt32(edtShowTimeAs.Value)
		End If
		If edtLabel IsNot Nothing Then
			Controller.LabelId = Convert.ToInt32(edtLabel.Value)
		End If
		If chkReminder.Checked Then
			Dim reminderTime As TimeSpan = TimeSpan.Parse(CStr(cbReminder.Value))
			Controller.HasReminder = True
			Controller.ReminderTimeBeforeStart = reminderTime
		End If
		Controller.ResourceIds.Clear()
		For Each ri As RepeaterItem In rpResources.Items
			Dim chkResource As ASPxCheckBox = TryCast(ri.FindControl("chkResource"), ASPxCheckBox)
			If chkResource.Checked = True Then
				Dim field As ASPxHiddenField = TryCast(ri.FindControl("hfResource"), ASPxHiddenField)
				Dim resourceIdObject As Object = Nothing
				Dim success As Boolean = field.TryGet("resourceId", resourceIdObject)
				If (Not success) Then
					Continue For
				End If
				Dim resourceIdString As String = resourceIdObject.ToString()
				Dim resourceId As Integer = Integer.Parse(resourceIdString)
				Controller.ResourceIds.Add(resourceId)
			End If
		Next ri
		AssignControllerRecurrenceValues(clientStart)
	End Sub
End Class
