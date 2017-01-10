var debug = document.getElementById("Debug");
if(!debug)
    debug = document.createElement("span");
document.body.appendChild(debug);
debug.id = "Debug";
debug.setAttribute("contentEditable", true);
debug.style["position"] = "absolute";
debug.style["left"] = "0px";
debug.style["top"] = "310px";
debug.style["width"] = "800px";
debug.style["height"] = "800px";
debug.style["overflow"] = "auto"


var Container = Sys.UI.TBase.Create("panel");
Container.SetProps  ({
                        Parent: document.body , 
                        Left : 2,
                        Top : 2,
                        Width : 400,
                        Height : 300,
                        Color : Sys.TColor.Random(),
                        Name : "Container"
                    });

var Panel1 = Sys.UI.TBase.Create("panel");
Panel1.SetProps  ({
                        Parent: Container , 
                        Left : 10,
                        Top : 10,
                        Width : 100,
                        Height : 100,
                        Color : Sys.TColor.Random(),
                        Name : "Panel1"
                    });
                    
var Panel2 = Sys.UI.TBase.Create("panel");
Panel2.SetProps  ({
                        Parent: Container , 
                        Left : 200,
                        Top : 10,
                        Width : 100,
                        Height : 100,
                        Color : Sys.TColor.Random(),
                        Name : "Panel2",
                        AllowDrop : true
                    });

var Panel3 = Sys.UI.TBase.Create("panel");
Panel3.SetProps  ({
                        Parent: Container , 
                        Left : 110,
                        Top : 120,
                        Width : 100,
                        Height : 100,
                        Color : Sys.TColor.Random(),
                        Name : "Panel3"
                    });
Panel1.OnMouseDown.AttachEvent(Panel1_mousedown);
Panel1.setAllowDrop(true);
Panel3.setAllowDrop(true);
function Panel1_mousedown(obj)
{
    Panel1.BeginDrag();
    Sys.Debug.Clear();
    Sys.Debug.Writeln("Name = "+ this.getName());
    Sys.Debug.ShowPropertiesWithValue(obj.Event);
}
Panel2.OnDragDrop.AttachEvent(OnDragDrop);
Panel2.OnDragEnter.AttachEvent(OnDragEnter);
Panel2.OnDragExit.AttachEvent(OnDragExit);
Panel2.OnDragOver.AttachEvent(OnDragOver);
function OnDragDrop(obj)
{
    Sys.Debug.Clear();
    Sys.Debug.Writeln("OnDragDrop Name = "+ this.getName());
    Sys.Debug.ShowPropertiesWithValue(obj.Event.DragSource.getName());
}
function OnDragEnter(obj)
{
    Sys.Debug.Clear();
    Sys.Debug.Writeln("OnDragEnter Name = "+ this.getName());
    Sys.Debug.ShowPropertiesWithValue(obj.Event.DragSource.getName());
}
function OnDragExit(obj)
{
    Sys.Debug.Clear();
    Sys.Debug.Writeln("OnDragExit Name = "+ this.getName());
    Sys.Debug.ShowPropertiesWithValue(obj.Event.DragSource.getName());
}
function OnDragOver(obj)
{
    Sys.Debug.Clear();
    Sys.Debug.Writeln("OnDragOver Name = "+ this.getName());
    Sys.Debug.ShowPropertiesWithValue(obj.Event.DragSource.getName());
}