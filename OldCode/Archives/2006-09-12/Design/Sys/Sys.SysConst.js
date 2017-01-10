/*
    Author:         John Huang
    Start Date:     2006-04-25 
    Description:    Define system namespace and all constants
*/
Sys = {};
Sys.Resources = {};
/*Error Messages*/
(function()
{
	var obj = Sys.Resources;
	obj.sysMessages = {};
	obj = obj.sysMessages;
	obj[1000] = ["Could not call an abstract method directly, it must be implemented first", ""];
	obj[1001] = ["Could not convert Month", ""];
	obj[1002] = ["Could not convert Week Day", ""];
	obj[1003] = ["Could not convert AM/PM", ""];
	obj[1004] = ["Invalid interval value.", ""];
	obj[1005] = ["Invalid Enumeration Value", ""];
	obj[1006] = ["Invalid Flag Value", ""];
	obj[1007] = ["Could not inherit from a sealed class.", ""];
	obj[1008] = ["Could not inherit from an interface.", ""];
})();

/*Date format*/
(function()
{
    var obj = Sys.Resources;
	obj.datetimeFormat = {};
	obj = obj.datetimeFormat;
    obj.current = "en-US";
    obj["en-US"] = 
    {
        longMonths : ["January","February","March","April","May","June","July","August","September","October","November","December"],
        shortMonths : ["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"],
        longWeekdays : ["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],
        shortWeekdays : ["Sun","Mon","Tue","Wed","Thu","Fri","Sat"],
        AM_PM : ["AM", "PM"],
        A_P : ["A", "P"]
    };
    obj["en-US"]["default"] = "mm/dd/yyyy h:n am/pm";
    obj["formats"] = {
                        year: ["yyyy", "yy"],
                        month: ["mmmm", "mmm", "mm", "m"],
                        weekday: ["wkl", "wk"],
                        day: ["dd", "d"],
                        hour: ["hh", "h"],
                        minute: ["nn", "n"],
                        second: ["ss", "s"],
                        millisecond: ["ms1", "ms"],
                        A: ["am/pm", "a/p"]
                     };
    obj["regularExpression"] = /yyyy|yy|mmmm|mmm|mm|dd|hh|nn|ss|ms1|ms|am\/pm|wk1|wk|a\/p|m|d|h|n|s|./gi;
    obj["regularExpressionIs12"] = /am\/pm|a\/p/gi;
})();
/*


window.Sys ={};
Sys.DesignMode = true;
Sys.Browser = {}
Sys.Debug = {};
Sys.Resources = {};
Sys.Resources.DateTimeFormat = {};
Sys.Resources.SysBuf = {};
Sys.Resources.SysMessages = {};

Sys.UI = {};
Sys.UI.AllControls = {};
Sys.UI.AllEvents = {};
Sys.UI.Cursors = {};
(function()
{
    var obj = Sys.UI.Cursors;
    obj.AddCursor = function(CursorName, Cursor)
    {
        CursorName = CursorName.toString().toLowerCase();
        if(arguments.length == 1)
        {
            this[CursorName] = CursorName;
            return;
        }
        this[CursorName] = Cursor;
    };
    obj.AddCustomCursor = function(CursorName, CursorFileName)
    {
        this.AddCursor(CursorName, "url('"+CursorFileName+"'), default");
    };
    obj.AddCursor("crosshair");
    obj.AddCursor("default");
    obj.AddCursor("pointer");
    obj.AddCursor("move"); 
    obj.AddCursor("cmove","pointer"); //move Component
    obj.AddCursor("e-resize");
    obj.AddCursor("ne-resize");
    obj.AddCursor("nw-resize");
    obj.AddCursor("n-resize");
    obj.AddCursor("se-resize");
    obj.AddCursor("sw-resize");
    obj.AddCursor("s-resize");
    obj.AddCursor("w-resize");
    obj.AddCursor("text");
    obj.AddCursor("wait");
    obj.AddCursor("help");
    obj.AddCursor("not-allowed");
    obj.AddCursor("col-resize");
    obj.AddCursor("row-resize");
    obj.AddCustomCursor("sql","Sys/Resources/Cursors/sql.cur");
    obj.AddCustomCursor("drag","Sys/Resources/Cursors/dd01-drag.cur");
    obj.AddCustomCursor("no-drop","Sys/Resources/Cursors/dd01-nodrop.cur");
    obj.AddCustomCursor("multi-drag","Sys/Resources/Cursors/dd01-multidrag.cur");
})();

Sys.Version = 0.00;
Sys.Doc = document;
Sys.Win = window;
Sys.Screen = {};
*/
/***Sys.Resources.DateTimeFormat***/
/*(function()
{
    var obj = Sys.Resources.DateTimeFormat;
    obj.Current = "en-US";
    obj["en-US"] = 
    {
        LongMonths : ["January","February","March","April","May","June","July","August","September","October","November","December"],
        ShortMonths : ["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"],
        LongWeekdays : ["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],
        ShortWeekdays : ["Sun","Mon","Tue","Wed","Thu","Fri","Sat"],
        AM_PM : ["AM", "PM"],
        A_P : ["A", "P"],
        Default: "mm/dd/yyyy h:n am/pm"
    };
    obj["Formats"] = {
                        Year: ["yyyy", "yy"],
                        Month: ["mmmm", "mmm", "mm", "m"],
                        WeekDay: ["wkl", "wk"],
                        Day: ["dd", "d"],
                        Hour: ["hh", "h"],
                        Minute: ["nn", "n"],
                        Second: ["ss", "s"],
                        Millisecond: ["ms1", "ms"],
                        A: ["am/pm", "a/p"]
                     };
    obj["RegularExpression"] = /yyyy|yy|mmmm|mmm|mm|dd|hh|nn|ss|ms1|ms|am\/pm|wk1|wk|a\/p|m|d|h|n|s|./gi;
    obj["RegularExpressionIs12"] = /am\/pm|a\/p/gi;
})();
*/
/***Sys.Resources.SysMessages***/
/*(function()
{
    var obj = Sys.Resources.SysMessages;
    obj[10000] = ["List out of bound",""];
    obj[10001] = ["Could not attach a non-function handle to an event",""];
    obj[10002] = ["Handle of a thread must be a function.",""];
    obj[10003] = ["Could not create object {0}.",""];
    obj[10004] = ["{0} must be > 0.",""];
    obj[10005] = ["Unknown type {0}.",""];
    obj[10006] = ["Child object must be a UI object.",""];
    obj[10007] = ["Child object aleady owned by a object.",""];
    obj[10008] = ["Could not add child before this object.",""];
    obj[10009] = ["Could not find child object.",""];
    obj[10010] = ["{0} is a invalid Dock value.","Valid value should be \"none\", \"left\", \"top\", \"right\", \"bottom\" and \"fill\""];
    obj[10011] = ["Could not set {0} for the control.",""];
    obj[10012] = ["TabOrder must be between 1 and 32767.",""];
})();

*/