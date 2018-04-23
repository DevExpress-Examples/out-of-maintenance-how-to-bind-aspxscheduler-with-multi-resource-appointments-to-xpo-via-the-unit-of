<%@ Application Language="vb" %>

<script runat="server">

	Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
		' Code that runs on application startup
		Dim conn As String = DevExpress.Xpo.DB.AccessConnectionProviderMultiUserThreadSafe.GetConnectionString(Server.MapPath("~\App_Data\XPOBoundMode.mdb"), String.Empty, String.Empty)
		If conn IsNot Nothing Then
			' Code that runs on application startup
			Dim dict As DevExpress.Xpo.Metadata.XPDictionary = New DevExpress.Xpo.Metadata.ReflectionDictionary()
			dict.GetDataStoreSchema(GetType(XPAppointment).Assembly)
			Dim store As DevExpress.Xpo.DB.IDataStore = DevExpress.Xpo.XpoDefault.GetConnectionProvider(conn, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema)
			store = New DevExpress.Xpo.DB.DataCacheNode(New DevExpress.Xpo.DB.DataCacheRoot(store))
			Dim layer As Object = New DevExpress.Xpo.ThreadSafeDataLayer(dict, store)
			Application.Add("XpoLayer", layer)
		End If
	End Sub

	Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
		'  Code that runs on application shutdown

	End Sub

	Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
		' Code that runs when an unhandled error occurs

	End Sub

	Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
		' Code that runs when a new session is started

	End Sub

	Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
		' Code that runs when a session ends. 
		' Note: The Session_End event is raised only when the sessionstate mode
		' is set to InProc in the Web.config file. If session mode is set to StateServer 
		' or SQLServer, the event is not raised.

	End Sub

</script>