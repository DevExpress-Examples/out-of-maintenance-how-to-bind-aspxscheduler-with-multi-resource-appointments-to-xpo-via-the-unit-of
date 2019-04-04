<!-- default file list -->
*Files to look at*:

* [UserAppointmentFormClass.cs](./CS/WebSite/App_Code/UserAppointmentFormClass.cs) (VB: [UserAppointmentFormClass.vb](./VB/WebSite/App_Code/UserAppointmentFormClass.vb))
* [XPObjects.cs](./CS/WebSite/App_Code/XPObjects.cs) (VB: [XPObjects.vb](./VB/WebSite/App_Code/XPObjects.vb))
* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
* [UserAppointmentForm.ascx](./CS/WebSite/MyForms/UserAppointmentForm.ascx) (VB: [UserAppointmentForm.ascx](./VB/WebSite/MyForms/UserAppointmentForm.ascx))
* [UserAppointmentForm.ascx.cs](./CS/WebSite/MyForms/UserAppointmentForm.ascx.cs) (VB: [UserAppointmentForm.ascx.vb](./VB/WebSite/MyForms/UserAppointmentForm.ascx.vb))
<!-- default file list end -->
# How to bind ASPxScheduler with multi-resource appointments to XPO via the Unit of Work


<p>This example illustrates how the appointment editing form with multiple resources selection can be implemented in a project with ASPxScheduler control bound to XPO via the <a href="http://documentation.devexpress.com/#XPO/CustomDocument2138"><u>Unit of Work</u></a>.</p><p>To switch the scheduler to multi-resource mode, set the <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerAppointmentStorageBase_ResourceSharingtopic"><u>ResourceSharing</u></a> property to true. Then implement a custom appointment editing form and adjust the appointment field mapping for the ResourceIds property.</p><p>Note also how the ResourceIds property of the XPAppointment XPO object is implemented and the role of XPORowInsertionProvider class.</p>


<h3>Description</h3>

<p>Note: starting from the v2009 vol.2 release the standard appointment form contains an ASPxListBox control which provides the required functionality for assigning multiple resources. </p><p>For ASPxScheduler releases prior 9.2, the following workaround is used.<br />
A custom appointment editing form contains a data-bound control with multiple selection capability. The control is based on a combination of ASPxCheckBox and ASPxHiddenField controls, enclosed into a Repeater control template.</p><br />


<br/>


