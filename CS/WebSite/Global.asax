<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) {
        // Code that runs on application startup
        string conn = DevExpress.Xpo.DB.AccessConnectionProviderMultiUserThreadSafe.GetConnectionString(Server.MapPath("~\\App_Data\\XPOBoundMode.mdb"), String.Empty, String.Empty);
        if (conn != null) {
            // Code that runs on application startup
            DevExpress.Xpo.Metadata.XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            dict.GetDataStoreSchema(typeof(XPAppointment).Assembly);
            DevExpress.Xpo.DB.IDataStore store = DevExpress.Xpo.XpoDefault.GetConnectionProvider(conn, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            store = new DevExpress.Xpo.DB.DataCacheNode(new DevExpress.Xpo.DB.DataCacheRoot(store));
            object layer = new DevExpress.Xpo.ThreadSafeDataLayer(dict, store);
            Application.Add("XpoLayer", layer);
        }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
