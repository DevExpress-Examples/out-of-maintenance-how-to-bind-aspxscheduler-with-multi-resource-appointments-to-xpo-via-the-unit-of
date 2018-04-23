<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v9.1, Version=9.1.0.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4"
    Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>

<%@ Register Assembly="DevExpress.Xpo.v9.1, Version=9.1.0.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4"
    Namespace="DevExpress.Xpo" TagPrefix="dxxpo" %>

<%@ Register Assembly="DevExpress.XtraScheduler.v9.1.Core, Version=9.1.0.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4"
    Namespace="DevExpress.XtraScheduler" TagPrefix="cc2" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v9.1" Namespace="DevExpress.Web.ASPxScheduler"
    TagPrefix="dxwschs" %>

<%@ Register assembly="DevExpress.Xpo.v9.1" Namespace="DevExpress.Xpo" TagPrefix="dxxpo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server"
            OnAppointmentsInserted="ASPxScheduler1_OnAppointmentsInserted" 
            OnAppointmentsChanged="ASPxScheduler1_AppointmentsChanged" 
            OnAppointmentsDeleted="ASPxScheduler1_AppointmentsDeleted" GroupType="Resource" 
            OnBeforeExecuteCallbackCommand="ASPxScheduler1_BeforeExecuteCallbackCommand" Start="2009-05-04">
            <OptionsForms AppointmentFormTemplateUrl="~/MyForms/UserAppointmentForm.ascx" />
        <Views>
<DayView ShowWorkTimeOnly="True"><TimeRulers>
<cc2:TimeRuler></cc2:TimeRuler>
</TimeRulers>

<AppointmentDisplayOptions SnapToCellsMode="Never"></AppointmentDisplayOptions>
</DayView>

<WorkWeekView><TimeRulers>
<cc2:TimeRuler></cc2:TimeRuler>
</TimeRulers>
</WorkWeekView>

<TimelineView IntervalCount="5"><Scales>
<cc2:TimeScaleYear Enabled="False"></cc2:TimeScaleYear>
<cc2:TimeScaleQuarter Enabled="False"></cc2:TimeScaleQuarter>
<cc2:TimeScaleMonth Enabled="False"></cc2:TimeScaleMonth>
<cc2:TimeScaleWeek></cc2:TimeScaleWeek>
<cc2:TimeScaleDay Width="150"></cc2:TimeScaleDay>
<cc2:TimeScaleHour Enabled="False"></cc2:TimeScaleHour>
<cc2:TimeScaleFixedInterval Enabled="False"></cc2:TimeScaleFixedInterval>
</Scales>

<AppointmentDisplayOptions TimeDisplayType="Text" AppointmentAutoHeight="True" SnapToCellsMode="Auto"></AppointmentDisplayOptions>
</TimelineView>
            <MonthView ResourcesPerPage="1">
                <AppointmentDisplayOptions AppointmentAutoHeight="True" ShowRecurrence="True" ShowReminder="True" />
            </MonthView>
</Views>
        </dxwschs:ASPxScheduler>
        
        <dxxpo:XpoDataSource ID="appointmentDataSource" runat="server" TypeName="XPAppointment" />
        <dxxpo:XpoDataSource ID="resourceDataSource" runat="server" TypeName="XPResource" />&nbsp;
    
    </div>
    </form>
</body>
</html>
