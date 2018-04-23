Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Xpo
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Xml

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private unitOfWork As UnitOfWork
	Public Shared RandomInstance As New Random()
	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		PrepareDataSources()
		SetupMappings()
		ProvideRowInsertion()
		AttachDataSources()
		PopulateDataSources()
	End Sub
	Private Sub ProvideRowInsertion()
		Dim aptProvider As New XPORowInsertionProvider()
		aptProvider.ProvideRowInsertion(ASPxScheduler1, Me.appointmentDataSource, Me.unitOfWork)
	End Sub
	Private Sub PrepareDataSources()
		Me.unitOfWork = New UnitOfWork(TryCast(Application("XpoLayer"), IDataLayer))
		Me.appointmentDataSource.Session = unitOfWork
		Me.resourceDataSource.Session = unitOfWork
	End Sub
	Private Sub AttachDataSources()
		ASPxScheduler1.AppointmentDataSource = Me.appointmentDataSource
		ASPxScheduler1.ResourceDataSource = Me.resourceDataSource
		If (Not IsPostBack) Then
			ASPxScheduler1.DataBind()
		End If
	End Sub
	Private Sub SetupMappings()
		ASPxScheduler1.Storage.BeginUpdate()
		Try
			Dim aptMappings As ASPxAppointmentMappingInfo = ASPxScheduler1.Storage.Appointments.Mappings
			aptMappings.AllDay = "AllDay"
			aptMappings.Description = "Description"
			aptMappings.End = "Finish"
			aptMappings.Label = "Label"
			aptMappings.Location = "Location"
			aptMappings.RecurrenceInfo = "Recurrence"
			aptMappings.ReminderInfo = "Reminder"
			aptMappings.ResourceId = "ResourceIds"
			aptMappings.Start = "Created"
			aptMappings.Status = "Status"
			aptMappings.Subject = "Subject"
			aptMappings.Type = "AppointmentType"
			aptMappings.AppointmentId = "Oid"
			ASPxScheduler1.Storage.Appointments.ResourceSharing = True

			Dim resourceMappings As ASPxResourceMappingInfo = ASPxScheduler1.Storage.Resources.Mappings
			resourceMappings.Caption = "Name"
			resourceMappings.ResourceId = "Oid"
		Finally
			ASPxScheduler1.Storage.EndUpdate()
		End Try
	End Sub

	Private Sub CommitChanges()
		unitOfWork.CommitChanges()
	End Sub
	Protected Sub ASPxScheduler1_OnAppointmentsInserted(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
		CommitChanges()
	End Sub
	Protected Sub ASPxScheduler1_AppointmentsDeleted(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
		CommitChanges()
	End Sub
	Protected Sub ASPxScheduler1_AppointmentsChanged(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
		CommitChanges()
	End Sub

	Public Class XPORowInsertionProvider
		Private insertedAppointmentsId As New List(Of Integer)()
		Private control As ASPxScheduler
		Private unitOfWork As UnitOfWork
		Public Sub ProvideRowInsertion(ByVal control As ASPxScheduler, ByVal dataSource As XpoDataSource, ByVal unitOfWork As UnitOfWork)
			Me.control = control
			Me.unitOfWork = unitOfWork
			AddHandler control.AppointmentRowInserted, AddressOf ASPxScheduler1_AppointmentRowInserted
			AddHandler dataSource.Inserted, AddressOf dataSource_OnInserted
			AddHandler control.AppointmentsInserted, AddressOf ControlOnAppointmentsInserted
		End Sub
		Private Sub dataSource_OnInserted(ByVal sender As Object, ByVal e As XpoDataSourceInsertedEventArgs)
			CType(e.Value, XPObject).Save()
			Me.unitOfWork.CommitChanges()
			Me.insertedAppointmentsId.Add((CType(e.Value, XPObject)).Oid)
		End Sub
		Protected Sub ASPxScheduler1_AppointmentRowInserted(ByVal sender As Object, ByVal e As ASPxSchedulerDataInsertedEventArgs)
			e.KeyFieldValue = Me.insertedAppointmentsId(Me.insertedAppointmentsId.Count - 1)
		End Sub
		Private Sub ControlOnAppointmentsInserted(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
			Dim count As Integer = e.Objects.Count
			System.Diagnostics.Debug.Assert(count = insertedAppointmentsId.Count)
			SetAppointmentsId(sender, e)
		End Sub
		Private Sub SetAppointmentsId(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
			Dim appointments As AppointmentBaseCollection = CType(e.Objects, AppointmentBaseCollection)
			Dim storage As ASPxSchedulerStorage = CType(sender, ASPxSchedulerStorage)
			Dim count As Integer = appointments.Count
			System.Diagnostics.Debug.Assert(count = insertedAppointmentsId.Count)
			For i As Integer = 0 To count - 1
				Dim apt As Appointment = appointments(i)
				storage.SetAppointmentId(apt, insertedAppointmentsId(i))
			Next i
			insertedAppointmentsId.Clear()
		End Sub
	End Class
	#Region "Populate database with initial data"
	Private Sub PopulateDataSources()
		Dim res As New XPCollection(Of XPResource)(unitOfWork)
		res.Load()
		ASPxScheduler1.Storage.Resources.Clear()
		If res.Count = 0 Then
			PopulateDataSourcesCore()
		End If
		CommitChanges()
		ASPxScheduler1.DataBind()
	End Sub
	Private Sub PopulateDataSourcesCore()
		Dim res As XPResource
		For i As Integer = 0 To 4
			res = CreateEmployee("Resource" & i.ToString())
		Next i
	End Sub
	Private Function CreateEmployee(ByVal name As String) As XPResource
		Dim res As New XPResource(unitOfWork)
		res.Name = name
		res.Save()
		Return res
	End Function
	Private Sub CreateTasks(ByVal res As XPResource)
		CreateTask(res, "meeting", RandomInstance.Next(0, 4), RandomInstance.Next(0, 5))
		CreateTask(res, "meeting", RandomInstance.Next(0, 4), RandomInstance.Next(0, 5))
	End Sub

	Private Sub CreateTask(ByVal res As XPResource, ByVal taskName As String, ByVal status As Integer, ByVal label As Integer)
		Dim apt As New XPAppointment(unitOfWork)
		apt.ResourceIds = GenerateResourcesCollectionForAppointments()
		apt.Subject = res.Name & "'s " & taskName

		Dim rangeInHours As Integer = 72
		apt.Created = DateTime.Today + TimeSpan.FromHours(RandomInstance.Next(0, rangeInHours))
		apt.Finish = apt.Created + TimeSpan.FromHours(RandomInstance.Next(0, rangeInHours \ 12))

		apt.Status = status
		apt.Label = label

		apt.Save()
	End Sub

	Private Function GenerateResourcesCollectionForAppointments() As String
		Dim resourceIds As New ResourceIdCollection()
		Dim count As Integer = ASPxScheduler1.Storage.Resources.Count
		For i As Integer = 0 To count - 1
			resourceIds.Add(ASPxScheduler1.Storage.Resources(i).Id)
		Next i
		Dim helper As New AppointmentResourceIdCollectionXmlPersistenceHelper(resourceIds)
		Return helper.ToXml()
	End Function
	#End Region

End Class
