#Region "#usings_xpobject"
Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Xml
#End Region ' #usings_xpobject

#Region "#XPObject"
Public Class XPAppointment
	Inherits XPObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

		Public AllDay As Boolean ' Appointment.AllDay

		<Size(SizeAttribute.Unlimited)> _
		Public Description As String ' Appointment.Description

		Public Finish As DateTime ' Appointment.End
		Public Label As Integer ' Appointment.Label
		Public Location As String ' Appointment.Location

		<Size(SizeAttribute.Unlimited)> _
		Public Recurrence As String ' Appointment.RecurrenceInfo

		<Size(SizeAttribute.Unlimited)> _
		Public Reminder As String ' Appointment.ReminderInfo

		Public Created As DateTime ' Appointment.Start
		Public Status As Integer ' Appointment.Status
		<Size(SizeAttribute.Unlimited)> _
		Public Subject As String ' Appointment.Subject
		Public AppointmentType As Integer ' Appointment.Type

		<Association()> _
		Public ReadOnly Property Resources() As XPCollection(Of XPResource)
			Get
				Return GetCollection(Of XPResource)("Resources")
			End Get
		End Property

		<NonPersistent()> _
		Public Property ResourceIds() As String
			Get
				Return GenerateResourceIdsString()
			End Get
			Set(ByVal value As String)

				Dim resourceIds_Renamed As ResourceIdCollection = GenerateResourceIdsString(value)
				Resources.SuspendChangedEvents()
				Try
					ClearResources()
					Dim count As Integer = resourceIds_Renamed.Count
					For i As Integer = 0 To count - 1
						Dim resource As XPResource = Me.Session.GetObjectByKey(Of XPResource)(resourceIds_Renamed(i))
						If resource IsNot Nothing Then
							Resources.Add(resource)
						End If
					Next i
				Finally
					Resources.ResumeChangedEvents()
				End Try
			End Set
		End Property

		Private Sub ClearResources()
			Dim count As Integer = Resources.Count
			Do While count > 0
				Resources.Remove(Resources(0))
				count -= 1
			Loop
		End Sub
		Private Function GenerateResourceIdsString(ByVal xml As String) As ResourceIdCollection
			Dim result As New ResourceIdCollection()
			If String.IsNullOrEmpty(xml) Then
				Return result
			End If

			Return AppointmentResourceIdCollectionXmlPersistenceHelper.ObjectFromXml(result, xml)
		End Function

		Private Function GenerateResourceIdsString() As String
			Dim resourceIds As New ResourceIdCollection()
			Dim count As Integer = Resources.Count
			For i As Integer = 0 To count - 1
				resourceIds.Add(Resources(i).Oid)
			Next i

			Dim helper As New AppointmentResourceIdCollectionXmlPersistenceHelper(resourceIds)
			Return helper.ToXml()
		End Function
End Class

	' XP object
	Public Class XPResource
		Inherits XPObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

		<Size(SizeAttribute.Unlimited)> _
		Public Name As String ' Resource.Caption

		<Association()> _
		Public ReadOnly Property Appointments() As XPCollection(Of XPAppointment)
			Get
				Return GetCollection(Of XPAppointment)("Appointments")
			End Get
		End Property
	End Class
#End Region ' #XPObject
