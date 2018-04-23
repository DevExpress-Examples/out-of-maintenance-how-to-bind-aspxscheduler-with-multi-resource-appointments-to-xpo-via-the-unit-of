using System;
using System.Collections;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxHiddenField;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;

public partial class AppointmentForm : SchedulerFormControl {
    public bool CanShowReminders {
        get {
            return ((AppointmentFormTemplateContainer)Parent).Control.Storage.EnableReminders;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        PrepareChildControls();
        tbSubject.Focus();
    }

    public override void DataBind() {
        base.DataBind();
        AppointmentFormTemplateContainer container = (AppointmentFormTemplateContainer)Parent;

        Appointment apt = container.Appointment;
        edtLabel.SelectedIndex = apt.LabelId;
        edtStatus.SelectedIndex = apt.StatusId;

        AppointmentRecurrenceForm1.Visible = container.ShouldShowRecurrence;

        if (container.Appointment.HasReminder) {
            cbReminder.Value = container.Appointment.Reminder.TimeBeforeStart.ToString();
            chkReminder.Checked = true;
        } else {
            cbReminder.ClientEnabled = false;
        }

        IList resources = container.ResourceDataSource as IList;
        int count = this.rpResources.Items.Count;
        for (int i = 0; i < count; i++) {
            string resourceIdString = ((ListEditItem)resources[i]).Value.ToString();
            string caption = ((ListEditItem)resources[i]).Text;
            if (resourceIdString == "null")
                continue;
            int resourceId = int.Parse(resourceIdString);

            RepeaterItem repeaterItem = this.rpResources.Items[i];
            ASPxCheckBox chkBox = repeaterItem.FindControl("chkResource") as ASPxCheckBox;
            ASPxHiddenField hiddenField = repeaterItem.FindControl("hfResource") as ASPxHiddenField;

            chkBox.Text = caption;
            chkBox.Checked = apt.ResourceIds.Contains(resourceId);
            hiddenField.Set("resourceId", resourceId);
        }

        btnOk.ClientSideEvents.Click = container.SaveHandler;
        btnCancel.ClientSideEvents.Click = container.CancelHandler;
        btnDelete.ClientSideEvents.Click = container.DeleteHandler;
    }

    protected override void PrepareChildControls() {
        AppointmentFormTemplateContainer container = (AppointmentFormTemplateContainer)Parent;
        ASPxScheduler control = container.Control;

        AppointmentRecurrenceForm1.EditorsInfo = new EditorsInfo(control, control.Styles.FormEditors, control.Images.FormEditors, control.Styles.Buttons);
        base.PrepareChildControls();
    }
    protected override ASPxEditBase[] GetChildEditors() {
        ASPxEditBase[] edits = new ASPxEditBase[] {
			lblSubject, tbSubject,
			lblLocation, tbLocation,
			lblLabel, edtLabel,
			lblStartDate, edtStartDate,
			lblEndDate, edtEndDate,
			lblStatus, edtStatus,
			lblAllDay, chkAllDay,
			lblResource, 
			tbDescription, cbReminder
		};
        return edits;
    }
    protected override ASPxButton[] GetChildButtons() {
        ASPxButton[] buttons = new ASPxButton[] {
			btnOk, btnCancel, btnDelete
		};
        return buttons;
    }
}
