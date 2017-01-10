/*Test Sys.UI*/
/*Create debug container*/
var debug = document.getElementById("Debug");
if(!debug)
    debug = document.createElement("span");
document.body.appendChild(debug);
debug.id = "Debug";
debug.setAttribute("contentEditable", true);
debug.style["position"] = "absolute";
debug.style["left"] = "0px";
debug.style["top"] = "500px";
debug.style["width"] = "800px";
debug.style["height"] = "800px";
debug.style["overflow"] = "auto"
/*Test removeChild*/
var o1 = Sys.UI.TBase.Create("panel");
//o1.getElement().innerHTML = "adfadfa";rt
o1.setParent(document.body);
o1.setLeft(18);
o1.setTop(28);
o1.setWidth(400);
o1.setHeight(400);
o1.setColor(Sys.TColor.Random());
o1.setCanFocus(true);
o1.setVisible(true);
//o1.WriteCSS("cursor", "url('Sys/Resources/Cursors/stopwtch.ani'), default" );
//o1.WriteCSS("cursor" );
//o1.setCursor("not-allowed");
//o1.setCursor("no-drop");

//o1.OnMouseUp.AttachEvent(o1_event);
//o1.OnMouseOut.AttachEvent(o1_event);
Sys.UI.AllEvents.OnEvent.AttachEvent(o1_event);
function o1_event(evn)
{
if(this.OriginalEventObject.__Sender._InternalName !='sYs__88') return;
    if(this.OriginalEventObject.Event.Type == "mousemove") return;
    //if(this.OriginalEventObject.Event.Type == "mouseover") return;
    //if(this.OriginalEventObject.Event.Type == "mouseout") return;
    Sys.Debug.Clear();
    Sys.Debug.Writeln("name = "+this.OriginalEventObject.__Sender._InternalName);
    Sys.Debug.ShowPropertiesWithValue(this.OriginalEventObject.Event);
    //Sys.Debug.ShowPropertiesWithValue(Sys.Screen);
}

var edit1 = document.createElement("input");
edit1.type = "text"
edit1.style["position"] = "absolute";
edit1.style["left"] = "0px";
edit1.style["top"] = "450px";
document.body.appendChild(edit1);
var a = Sys.Browser.AttachEvent(edit1, "keydown", LeftPressed)
function LeftPressed ()
{
    Sys.Debug.Clear();
    Sys.Debug.Writeln("arguments.length = "+arguments.length );
    Sys.Debug.Writeln("TabIndex = "+o1.getElement().tabIndex );
    //Sys.Debug.ShowPropertiesWithValue(o1.getElement());
    Sys.Debug.ShowPropertiesWithValue(arguments[0]);
    Sys.Debug.Writeln("length = "+arguments[0].length );
}
/*function LeftPressed (event)
{
    var evn = Sys.Browser.ConvertEventObject(event);
    switch(evn.Key)
    {
        case 37: 
            o1.setWidth(o1.getWidth() - 1);
            break;
        case 39: 
            o1.setWidth(o1.getWidth() + 1);
            break;
        case 38: 
            o1.setHeight(o1.getHeight() - 1);
            break;
        case 40: 
            o1.setHeight(o1.getHeight() + 1);
            break;
    }
   // WriteInfo();
}*/

var o2 = Sys.UI.TBase.Create("panel");
o2.setParent(o1);
o2.setRect(10, 11, 20, 20);
//o2.setConstraints(50,50,100,100);
o2.setColor(Sys.TColor.Random());
o2.setDock("top");
o2.setEnabled(true);

var o3 = Sys.UI.TBase.Create("panel");
o3.setParent(o1);
o3.setRect(9, 21, 20, 20);
o3.setColor(Sys.TColor.Random());
o3.setDock("bottom");

/*
var o4 = Sys.UI.TBase.Create("panel");
o4.setParent(o1);
o4.setRect(9, 21, 20, 20);
o4.setColor(Sys.TColor.Random());
o4.setDock("top");


var o5 = Sys.UI.TBase.Create("panel");
o5.setParent(o1);
o5.setRect(9, 21, 20, 20);
o5.setColor(Sys.TColor.Random());
o5.setDock("right");


var o6 = Sys.UI.TBase.Create("panel");
o6.setParent(o1);
o6.setRect(9, 21, 20, 20);
o6.setColor(Sys.TColor.Random());
o6.setDock("bottom");
*/
//o2.setAnchors(true,true,false,true);
//WriteInfo();
//o1.getElement().scrollLeft = 100;
/*function WriteInfo()
{
    Sys.Debug.Clear();
    Sys.Debug.Writeln("o1.Width = "+o1.getWidth()+"; o1.Height = "+o1.getHeight());
    Sys.Debug.Writeln("o1.ClientWidth = "+o1.getClientWidth()+"; o1.ClientHeight = "+o1.getClientHeight());
    Sys.Debug.Writeln("--------------------");
    Sys.Debug.Writeln("o2.Left = "+o2.getLeft()+"; o2.Top = "+o2.getTop()+"; o2.Width = "+o2.getWidth()+"; o2.Height = "+o2.getHeight());
    Sys.Debug.Writeln("o2.__Rects.OriginalWidth = "+o2.__Rects.OriginalWidth+"; o2.__Rects.OriginalHeight = "+o2.__Rects.OriginalHeight+"; o2.__Rects.Left = "+o2.__Rects.Left+"; o2.__Rects.Top = "+o2.__Rects.Top+"; o2.__Rects.Right = "+o2.__Rects.Right+" o2.__Rects.Bottom = "+o2.__Rects.Bottom);
    Sys.Debug.Writeln("--------------------");
    Sys.Debug.Writeln("o3.Left = "+o3.getLeft()+"; o3.Top = "+o3.getTop()+"; o3.Width = "+o3.getWidth()+"; o3.Height = "+o3.getHeight());
    Sys.Debug.Writeln("o3.__Rects.OriginalWidth = "+o3.__Rects.OriginalWidth+"; o3.__Rects.OriginalHeight = "+o3.__Rects.OriginalHeight+"; o3.__Rects.Left = "+o3.__Rects.Left+"; o3.__Rects.Top = "+o3.__Rects.Top+"; o3.__Rects.Right = "+o3.__Rects.Right+" o3.__Rects.Bottom = "+o3.__Rects.Bottom);
    Sys.Debug.Writeln("--------------------");
    Sys.Debug.Writeln("o4.Left = "+o4.getLeft()+"; o4.Top = "+o4.getTop()+"; o4.Width = "+o4.getWidth()+"; o4.Height = "+o4.getHeight());
    Sys.Debug.Writeln("o4.__Rects.OriginalWidth = "+o4.__Rects.OriginalWidth+"; o4.__Rects.OriginalHeight = "+o4.__Rects.OriginalHeight+"; o4.__Rects.Left = "+o4.__Rects.Left+"; o4.__Rects.Top = "+o4.__Rects.Top+"; o4.__Rects.Right = "+o4.__Rects.Right+" o4.__Rects.Bottom = "+o4.__Rects.Bottom);
    Sys.Debug.Writeln("--------------------");
    Sys.Debug.Writeln("o5.Left = "+o5.getLeft()+"; o5.Top = "+o5.getTop()+"; o5.Width = "+o5.getWidth()+"; o5.Height = "+o5.getHeight());
    Sys.Debug.Writeln("o5.__Rects.OriginalWidth = "+o5.__Rects.OriginalWidth+"; o5.__Rects.OriginalHeight = "+o5.__Rects.OriginalHeight+"; o5.__Rects.Left = "+o5.__Rects.Left+"; o5.__Rects.Top = "+o5.__Rects.Top+"; o5.__Rects.Right = "+o5.__Rects.Right+" o5.__Rects.Bottom = "+o5.__Rects.Bottom);
    Sys.Debug.Writeln("--------------------");
    Sys.Debug.Writeln("o6.Left = "+o6.getLeft()+"; o6.Top = "+o6.getTop()+"; o6.Width = "+o6.getWidth()+"; o6.Height = "+o6.getHeight());
    Sys.Debug.Writeln("o6.__Rects.OriginalWidth = "+o6.__Rects.OriginalWidth+"; o6.__Rects.OriginalHeight = "+o6.__Rects.OriginalHeight+"; o6.__Rects.Left = "+o6.__Rects.Left+"; o6.__Rects.Top = "+o6.__Rects.Top+"; o6.__Rects.Right = "+o6.__Rects.Right+" o6.__Rects.Bottom = "+o6.__Rects.Bottom);
}*/
Sys.Debug.Clear();
//Sys.Debug.ShowPropertiesWithValue(o1._EL.attributes);
/*Sys.Debug.Writeln("o1.clientLeft = "+o1.getElement().clientLeft);//.toString());
Sys.Debug.Writeln("o1.clientTop = "+o1.getElement().clientTop);//.toString());
Sys.Debug.Writeln("o1.clientWidth = "+o1.getElement().clientWidth);//.toString());
Sys.Debug.Writeln("o1.clientHeight = "+o1.getElement().clientHeight);//.toString());

Sys.Debug.Writeln("o1.offsetLeft = "+o1.getElement().offsetLeft);//.toString());
Sys.Debug.Writeln("o1.offsetTop = "+o1.getElement().offsetTop);//.toString());
Sys.Debug.Writeln("o1.offsetWidth = "+o1.getElement().offsetWidth);//.toString());
Sys.Debug.Writeln("o1.offsetHeight = "+o1.getElement().offsetHeight);//.toString());

Sys.Debug.Writeln("o1.scrollLeft = "+o1.getElement().scrollLeft);//.toString());
Sys.Debug.Writeln("o1.scrollTop = "+o1.getElement().scrollTop);//.toString());
Sys.Debug.Writeln("o1.scrollWidth = "+o1.getElement().scrollWidth);//.toString());
Sys.Debug.Writeln("o1.scrollHeight = "+o1.getElement().scrollHeight);//.toString());


Sys.Debug.Writeln("o2.clientLeft = "+o2.getElement().clientLeft);//.toString());
Sys.Debug.Writeln("o2.clientTop = "+o2.getElement().clientTop);//.toString());
Sys.Debug.Writeln("o2.clientWidth = "+o2.getElement().clientWidth);//.toString());
Sys.Debug.Writeln("o2.clientHeight = "+o2.getElement().clientHeight);//.toString());

Sys.Debug.Writeln("o2.offsetLeft = "+o2.getElement().offsetLeft);//.toString());
Sys.Debug.Writeln("o2.offsetTop = "+o2.getElement().offsetTop);//.toString());
Sys.Debug.Writeln("o2.offsetWidth = "+o2.getElement().offsetWidth);//.toString());
Sys.Debug.Writeln("o2.offsetHeight = "+o2.getElement().offsetHeight);//.toString());

Sys.Debug.Writeln("o2.scrollLeft = "+o2.getElement().scrollLeft);//.toString());
Sys.Debug.Writeln("o2.scrollTop = "+o2.getElement().scrollTop);//.toString());
Sys.Debug.Writeln("o2.scrollWidth = "+o2.getElement().scrollWidth);//.toString());
Sys.Debug.Writeln("o2.scrollHeight = "+o2.getElement().scrollHeight);//.toString());
o1.getElement().style.borderRightWidth = "30px";
*/
//o2.Free();
DoNothing();