using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using System.Collections;
using System.Collections.Generic;
using DevExpress.XtraScheduler.Xml;

public partial class _Default : System.Web.UI.Page {
    UnitOfWork unitOfWork;
    public static Random RandomInstance = new Random();
    protected void Page_Init(object sender, EventArgs e) {
        PrepareDataSources();
        SetupMappings();
        ProvideRowInsertion();
        AttachDataSources();
        PopulateDataSources();
    }
    void ProvideRowInsertion() {
        XPORowInsertionProvider aptProvider = new XPORowInsertionProvider();
        aptProvider.ProvideRowInsertion(ASPxScheduler1, this.appointmentDataSource, this.unitOfWork);
    }
    void PrepareDataSources() {
        this.unitOfWork = new UnitOfWork(Application["XpoLayer"] as IDataLayer);
        this.appointmentDataSource.Session = unitOfWork;
        this.resourceDataSource.Session = unitOfWork;
    }
    void AttachDataSources() {
        ASPxScheduler1.AppointmentDataSource = this.appointmentDataSource;
        ASPxScheduler1.ResourceDataSource = this.resourceDataSource;
        if (!IsPostBack) {
            ASPxScheduler1.DataBind();
        }
    }
    void SetupMappings() {
        ASPxScheduler1.Storage.BeginUpdate();
        try {
            ASPxAppointmentMappingInfo aptMappings = ASPxScheduler1.Storage.Appointments.Mappings;
            aptMappings.AllDay = "AllDay";
            aptMappings.Description = "Description";
            aptMappings.End = "Finish";
            aptMappings.Label = "Label";
            aptMappings.Location = "Location";
            aptMappings.RecurrenceInfo = "Recurrence";
            aptMappings.ReminderInfo = "Reminder";
            aptMappings.ResourceId = "ResourceIds";
            aptMappings.Start = "Created";
            aptMappings.Status = "Status";
            aptMappings.Subject = "Subject";
            aptMappings.Type = "AppointmentType";
            aptMappings.AppointmentId = "Oid";
            ASPxScheduler1.Storage.Appointments.ResourceSharing = true;

            ASPxResourceMappingInfo resourceMappings = ASPxScheduler1.Storage.Resources.Mappings;
            resourceMappings.Caption = "Name";
            resourceMappings.ResourceId = "Oid";
        }
        finally {
            ASPxScheduler1.Storage.EndUpdate();
        }
    }

    private void CommitChanges() {
        unitOfWork.CommitChanges();
    }
    protected void ASPxScheduler1_OnAppointmentsInserted(object sender, PersistentObjectsEventArgs e) {
        CommitChanges();
    }
    protected void ASPxScheduler1_AppointmentsDeleted(object sender, PersistentObjectsEventArgs e) {
        CommitChanges();
    }
    protected void ASPxScheduler1_AppointmentsChanged(object sender, PersistentObjectsEventArgs e) {
        CommitChanges();
    }

    public class XPORowInsertionProvider {
        List<int> insertedAppointmentsId = new List<int>();
        ASPxScheduler control;
        UnitOfWork unitOfWork;
        public void ProvideRowInsertion(ASPxScheduler control, XpoDataSource dataSource, UnitOfWork unitOfWork) {
            this.control = control;
            this.unitOfWork = unitOfWork;
            control.AppointmentRowInserted += new ASPxSchedulerDataInsertedEventHandler(ASPxScheduler1_AppointmentRowInserted);
            dataSource.Inserted += new XpoDataSourceInsertedEventHandler(dataSource_OnInserted);
            control.AppointmentsInserted += new PersistentObjectsEventHandler(ControlOnAppointmentsInserted);
        }
        void dataSource_OnInserted(object sender, XpoDataSourceInsertedEventArgs e) {
            ((XPObject)e.Value).Save();
            this.unitOfWork.CommitChanges();
            this.insertedAppointmentsId.Add(((XPObject)e.Value).Oid);
        }
        protected void ASPxScheduler1_AppointmentRowInserted(object sender, ASPxSchedulerDataInsertedEventArgs e) {
            e.KeyFieldValue = this.insertedAppointmentsId[this.insertedAppointmentsId.Count - 1];
        }
        void ControlOnAppointmentsInserted(object sender, PersistentObjectsEventArgs e) {
            int count = e.Objects.Count;
            System.Diagnostics.Debug.Assert(count == insertedAppointmentsId.Count);
            SetAppointmentsId(sender, e);
        }
        void SetAppointmentsId(object sender, PersistentObjectsEventArgs e) {
            AppointmentBaseCollection appointments = (AppointmentBaseCollection)e.Objects;
            ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
            int count = appointments.Count;
            System.Diagnostics.Debug.Assert(count == insertedAppointmentsId.Count);
            for (int i = 0; i < count; i++) {
                Appointment apt = appointments[i];
                storage.SetAppointmentId(apt, insertedAppointmentsId[i]);
            }
            insertedAppointmentsId.Clear();
        }
    }
    #region Populate database with initial data
    void PopulateDataSources() {
        XPCollection<XPResource> res = new XPCollection<XPResource>(unitOfWork);
        res.Load();
        ASPxScheduler1.Storage.Resources.Clear();
        if (res.Count == 0)
            PopulateDataSourcesCore();
        CommitChanges();
        ASPxScheduler1.DataBind();
    }
    void PopulateDataSourcesCore() {
        XPResource res;
        for (int i = 0; i < 5; i++)
            res = CreateEmployee("Resource" + i.ToString());
    }
    XPResource CreateEmployee(string name) {
        XPResource res = new XPResource(unitOfWork);
        res.Name = name;
        res.Save();
        return res;
    }
    void CreateTasks(XPResource res) {
        CreateTask(res, "meeting", RandomInstance.Next(0, 4), RandomInstance.Next(0, 5));
        CreateTask(res, "meeting", RandomInstance.Next(0, 4), RandomInstance.Next(0, 5));
    }

    void CreateTask(XPResource res, string taskName, int status, int label) {
        XPAppointment apt = new XPAppointment(unitOfWork);
        apt.ResourceIds = GenerateResourcesCollectionForAppointments();
        apt.Subject = res.Name + "'s " + taskName;

        int rangeInHours = 72;
        apt.Created = DateTime.Today + TimeSpan.FromHours(RandomInstance.Next(0, rangeInHours));
        apt.Finish = apt.Created + TimeSpan.FromHours(RandomInstance.Next(0, rangeInHours / 12));

        apt.Status = status;
        apt.Label = label;

        apt.Save();
    }

    string GenerateResourcesCollectionForAppointments() {
        ResourceIdCollection resourceIds = new ResourceIdCollection();
        int count = ASPxScheduler1.Storage.Resources.Count;
        for (int i = 0; i < count; i++)
            resourceIds.Add(ASPxScheduler1.Storage.Resources[i].Id);
        AppointmentResourceIdCollectionXmlPersistenceHelper helper = new AppointmentResourceIdCollectionXmlPersistenceHelper(resourceIds);
        return helper.ToXml();
    }
    #endregion
    protected void ASPxScheduler1_AppointmentFormShowing(object sender, AppointmentFormEventArgs e) {
        e.Container = new AppointmentFormTemplateContainer((ASPxScheduler)sender);
    }
    protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, SchedulerCallbackCommandEventArgs e) {
        if (e.CommandId == SchedulerCallbackCommandId.AppointmentSave) {
            e.Command = new UserAppointmentSaveCallbackCommand((ASPxScheduler)sender);
        }
    }
}
