using System;
using System.Collections.Generic;
using System.Web;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxHiddenField;

public class UserAppointmentSaveCallbackCommand : AppointmentFormSaveCallbackCommand {
    public UserAppointmentSaveCallbackCommand(ASPxScheduler control)
        : base(control) {
    }

    protected override void AssignControllerValues() {
        ASPxTextBox tbSubject = (ASPxTextBox)FindControlByID("tbSubject");
        ASPxTextBox tbLocation = (ASPxTextBox)FindControlByID("tbLocation");
        ASPxMemo tbDescription = (ASPxMemo)FindControlByID("tbDescription");
        ASPxDateEdit edtStartDate = (ASPxDateEdit)FindControlByID("edtStartDate");
        ASPxDateEdit edtEndDate = (ASPxDateEdit)FindControlByID("edtEndDate");
        ASPxCheckBox chkAllDay = (ASPxCheckBox)FindControlByID("chkAllDay");
        ASPxComboBox edtShowTimeAs = (ASPxComboBox)FindControlByID("edtStatus");
        ASPxComboBox edtLabel = (ASPxComboBox)FindControlByID("edtLabel");
        ASPxCheckBox chkReminder = (ASPxCheckBox)FindControlByID("chkReminder");
        ASPxComboBox cbReminder = (ASPxComboBox)FindControlByID("cbReminder");
        Repeater rpResources = (Repeater)FindControlByID("rpResources");
        DateTime clientStart = DateTime.MinValue;
        DateTime clientEnd = DateTime.MinValue;
        if (edtStartDate != null) {
            clientStart = FromClientTime(edtStartDate.Date);
            Controller.EditedAppointmentCopy.Start = clientStart;
        }
        if (edtEndDate != null) {
            clientEnd = FromClientTime(edtEndDate.Date);
            Controller.EditedAppointmentCopy.End = clientEnd;
        }
        if (tbSubject != null)
            Controller.Subject = tbSubject.Text;
        if (tbLocation != null)
            Controller.Location = tbLocation.Text;
        if (tbDescription != null)
            Controller.Description = tbDescription.Text;
        if (chkAllDay != null)
            Controller.AllDay = chkAllDay.Checked;
        if (edtShowTimeAs != null)
            Controller.StatusId = Convert.ToInt32(edtShowTimeAs.Value);
        if (edtLabel != null)
            Controller.LabelId = Convert.ToInt32(edtLabel.Value);
        if (chkReminder.Checked) {
            TimeSpan reminderTime = TimeSpan.Parse((string)cbReminder.Value);
            Controller.HasReminder = true;
            Controller.ReminderTimeBeforeStart = reminderTime;
        }
        Controller.ResourceIds.Clear();
        foreach (RepeaterItem ri in rpResources.Items) {
            ASPxCheckBox chkResource = ri.FindControl("chkResource") as ASPxCheckBox;
            if (chkResource.Checked == true) {
                ASPxHiddenField field = ri.FindControl("hfResource") as ASPxHiddenField;
                object resourceIdObject = null;
                bool success = field.TryGet("resourceId", out resourceIdObject);
                if (!success)
                    continue;
                string resourceIdString = resourceIdObject.ToString();
                int resourceId = int.Parse(resourceIdString);
                Controller.ResourceIds.Add(resourceId);
            }
        }
        AssignControllerRecurrenceValues(clientStart);
    }
}
