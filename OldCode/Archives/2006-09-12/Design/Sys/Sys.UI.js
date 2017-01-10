/*
    Author:             John Huang
    Start Date:         2006-04-30 
    Last Modification:  2006-05-06
    Description:        Base components for all UIComponents
*/
/*T__Rects*/
Sys.UI.T__Rects = Sys.TObject.SubClass();
Sys.UI.T__Rects.CreateClass = function()
{
	/*T__Rects
    This object is only being used in UI.TBase
    it use to save localtion information.
	*/
    var obj = Sys.UI.T__Rects.prototype
    obj.Left = 0; obj.Top =0; obj.Width = 0; obj.Height = 0; obj.Right = 0; obj.Bottom = 0;
    obj.sLeft = 0; obj.sTop =0; obj.sWidth = 0; obj.sHeight = 0; obj.sRight = 0; obj.sBottom = 0;
    obj.OriginalWidth = 0; obj.OriginalHeight =0; //parent width and parent height
    obj.ALeft = true; obj.ATop = true; obj.ARight = false; obj.ABottom = false;
    obj.DockLeft = null; obj.DockRight = null; obj.DockTop = null; obj.DockBottom = null; obj.DockClient = null; obj.Others = null;
    obj.MaxHeight = 0; obj.MaxWidth = 0; obj.MinHeight = 0; obj.MinWidth = 0;
    obj.CalScrollWidth = 0; obj.CalScrollHeight = 0;
    obj.Dock = "none"; //can be left, top, right, bottom, none and client;
    obj.OriginalOrder = 0;
    obj.IsCalculating = false;
    obj.CheckWidthRange = function(w)
    {
        if(this.MinWidth > 0 && w < this.MinWidth) return this.MinWidth;
        if(this.MaxWidth > 0 && w > this.MaxWidth) return this.MaxWidth;
        return w;
    };
    obj.CheckHeightRange = function(h)
    {
        if(this.MinHeight > 0 && h < this.MinHeight) return this.MinHeight;
        if(this.MaxHeight > 0 && h > this.MaxHeight) return this.MaxHeight;
        return h;
    };
    obj.setAnchors = function(L,T,R,B)
    {
        /*if(!(L||T||R||B))
            return false;*/
          this.ALeft = L;
          this.ATop = T;
          this.ARight = R;
          this.ABottom = B;
          return true;
    };
    obj.setDock = function(Value)
    {
        Value = Value.toString().toLowerCase();
        if(this.Dock == Value) return false;
        switch(Value)
        {
            case "none":
                this.ALeft = true; this.ATop = true; this.ARight = false; this.ABottom = false;
                break;
            case "left":
                this.ALeft = true; this.ATop = true; this.ARight = false; this.ABottom = true;
                break;
            case "top":
                this.ALeft = true; this.ATop = true; this.ARight = true; this.ABottom = false;
                break;
            case "right":
                this.ALeft = false; this.ATop = true; this.ARight = true; this.ABottom = true;
                break;
            case "bottom":
                this.ALeft = true; this.ATop = false; this.ARight = true; this.ABottom = true;
                break;
            case "client":
                this.ALeft = true; this.ATop = true; this.ARight = true; this.ABottom = true;
                break;
            default:
                throw Error.Create(10010, v);
        }
        this.Dock = Value;
        return true;
    };
    obj.DockSortLeft = function(a,b)
    {
        if(a.__Rects.sLeft > b.__Rects.sLeft) return 1;
        if(a.__Rects.sLeft < b.__Rects.sLeft) return -1;
        if(a.__Rects.sLeft = b.__Rects.sLeft) return a.__Rects.OriginalOrder <b.__Rects.OriginalOrder ? -1 : 1;
    };
    obj.DockSortRight = function(a,b)
    {
        if(a.__Rects.sRight > b.__Rects.sRight) return -1;
        if(a.__Rects.sRight < b.__Rects.sRight) return 1;
        if(a.__Rects.sRight = b.__Rects.sRight) return a.__Rects.OriginalOrder <b.__Rects.OriginalOrder ? 1 : -1;
    };
    obj.DockSortTop = function(a,b)
    {
        if(a.__Rects.sTop > b.__Rects.sTop) return 1;
        if(a.__Rects.sTop < b.__Rects.sTop) return -1;
        if(a.__Rects.sTop = b.__Rects.sTop) return a.__Rects.OriginalOrder <b.__Rects.OriginalOrder ? -1 : 1;
    };
    obj.DockSortBottom = function(a,b)
    {
        if(a.__Rects.sBottom > b.__Rects.sBottom) return -1;
        if(a.__Rects.sBottom < b.__Rects.sBottom) return 1;
        if(a.__Rects.sBottom = b.__Rects.sBottom) return a.__Rects.OriginalOrder <b.__Rects.OriginalOrder ? 1 : -1;
    };
    obj.ClearList = function()
    {
        this.DockLeft.Clear();
        this.DockTop.Clear();
        this.DockRight.Clear();
        this.DockBottom.Clear();
        this.DockClient.Clear();
        this.Others.Clear();
    };
    obj.Sort = function()
    {
        this.DockLeft.Sort();
        this.DockTop.Sort();
        this.DockRight.Sort();
        this.DockBottom.Sort();
    };
    obj.Create = function()
    {
        this.DockLeft = Sys.TList.Create(); this.DockLeft.setUseCompareFunction(true); this.DockLeft.setCompareFunction(this.DockSortLeft);
        this.DockRight = Sys.TList.Create();this.DockRight.setUseCompareFunction(true); this.DockRight.setCompareFunction(this.DockSortRight);
        this.DockTop = Sys.TList.Create();this.DockTop.setUseCompareFunction(true); this.DockTop.setCompareFunction(this.DockSortTop);
        this.DockBottom = Sys.TList.Create();this.DockBottom.setUseCompareFunction(true); this.DockBottom.setCompareFunction(this.DockSortBottom);
        this.DockClient = Sys.TList.Create();
        this.Others = Sys.TList.Create();
    };
};
Sys.UI.T__Rects.CreateClass();
/*TUIBaseEvent*/
Sys.UI.TUIBaseEvent = Sys.TEvent.SubClass();
Sys.UI.TUIBaseEvent.CreateClass = function()
{
	var obj = Sys.UI.TUIBaseEvent.prototype;
	obj.__Elements = null;
	obj.Event = null;
	obj.CanBubble = false;
	obj.CanPropagate = true;
    obj.__BeforeCall = function()
    {
        this.Super("__BeforeCall");
        this.Event = Sys.Browser.ConvertEventObject(this.__Arguments[0]);
        return true;
    };
    obj.__AfterCall = function()
    {
        this.Super("__AfterCall");
        if(!this.CanBubble)
        {
            if(this.Event)
                if(this.Event.StopBubble)
                    this.Event.StopBubble();
        }
        if(!this.CanPropagate)
        {
            if(this.Event)
                if(this.Event.StopPropagation)
                    this.Event.StopPropagation();
        }
        this.Event = null;
    };
    obj.__AfterAttachEvent = function()
    {
        if(!this.__Elements) return;
        if(this.IsHandleListEmpty()) return;
        for(var i in this.__Elements)
        {
            if(!this.__Elements[i][2])
            {
                if(Sys.Browser.AttachEvent(this.__Elements[i][0], this.__Elements[i][1], this.Call))
                    this.__Elements[i][2] = true;
            }
        }
    };
    obj.__AfterDetachEvent = function()
    {
        if(!this.__Elements) return;
        if(!this.IsHandleListEmpty()) return;
        for(var i in this.__Elements)
        {
            if(this.__Elements[i][2])
            {
                if(Sys.Browser.DetachEvent(this.__Elements[i][0], this.__Elements[i][1], this.Call))
                    this.__Elements[i][2] = false;
            }
        }
    };
    obj.AddElement =function(Element, EventType)
    {
        if(!this.__Elements)
            this.__Elements = {};
        this.__Elements[Sys.GenerateAnonymous()] = new Array(Element, EventType, false);
        this.AttachEvent(Sys.UI.AllEvents.Call, Sys.UI.AllEvents);
    };
    obj.RemoveElement = function(Element, EventType)
    {
        if(!this.__Elements) return;
        for(var i in this.__Elements)
        {
            if(this.__Elements[i][0] == Element && this.__Elements[i][1] == EventType)
            {
                if(this.__Elements[i][2])
                    Sys.Browser.DetachEvent(this.__Elements[i][0], this.__Elements[i][1], this.Call);
                delete this.__Elements[i];
                if(Sys.IsEmptyObject(this.__Elements)) this.__Elements = null;
                return;
            }
        }
    };
    obj.RemoveAllElements = function()
    {
        if(!this.__Elements) return;
        for(var i in this.__Elements)
        {
            if(this.__Elements[i][0] == Element && this.__Elements[i][1] == EventType)
            {
                if(this.__Elements[i][2])
                    Sys.Browser.DetachEvent(this.__Elements[i][0], this.__Elements[i][1], this.Call);
                this.__Elements[i] = null;
            }
        }
        this.__Elements = null;
    };
    obj.Free = function()
    {
        this.RemoveAllElements();
        this.Super("Free");
    };
};
Sys.UI.TUIBaseEvent.CreateClass();
/*Sys.UI.AllEvents*/
(function()
{
    var obj = Sys.UI.AllEvents;
    obj.Dragging = false;
    obj.DragStarted = false;
    obj.DragThreshold = 3;
    obj.DragSource = null;
    obj.DragStartX = -1;
    obj.DragStartY = -1;
    obj.OriginalEventObject = null;
    obj.OnEvent = Sys.TEvent.Create(obj, obj);
    obj.Resizing = false;
    obj.ResizeThreshold = 5;
    obj.ResizeSource = null;
    obj.ResizeMovingCursor = "";
    obj.ResizeStartX = -1;
    obj.ResizeStartY = -1;
    obj.ResizeOriginalRect = {Left:0,Top:0,Width:0,Height:0};
    obj.__ResizeCheckRect = function(obj, X, Y)
    {
        //var rt = Sys.UI.AllEvents.ResizeThreshold;
        var rt = this.ResizeThreshold;
        if(X >= 0 && X <= rt && Y >= 0 && Y <= rt)
            return "nw-resize";
        if(X > rt && X < obj.getWidth()- rt && Y >= 0 && Y <= rt)
            return "n-resize";
        if(X > obj.getWidth() - rt && X <= obj.getWidth() && Y >= 0 && Y <= rt)
            return "ne-resize";
        if(X > obj.getWidth() - rt && X <= obj.getWidth() && Y > rt && Y < obj.getHeight() - rt)
            return "e-resize";
        if(X > obj.getWidth() - rt && X <= obj.getWidth() && Y >= obj.getHeight() - rt && Y <= obj.getHeight())
            return "se-resize";
        if(X > rt && X < obj.getWidth() - rt && Y >= obj.getHeight() - rt && Y <= obj.getHeight())
            return "s-resize";
        if(X >=0 && X <= rt && Y >= obj.getHeight() - rt && Y <= obj.getHeight())
            return "sw-resize";
        if(X >=0 && X <= rt && Y > rt && Y < obj.getHeight() - rt)
            return "w-resize";
        if(obj.__Movable())
        {
            var rr = obj.__GetMovingRect();
            if(X>rr.Left && X<rr.Left+rr.Width && Y>rr.Top && Y< rr.Top+rr.Top+rr.Height)
                return "cmove";
        }
        return "";
    };
    obj.__StartResizing = function(obj)
    {
        this.Resizing = true;
        this.ResizeSource = obj;
        this.ResizeStartX = Sys.Screen.LastMouseScreenPoint.X;
        this.ResizeStartY = Sys.Screen.LastMouseScreenPoint.Y;
        this.ResizeOriginalRect = obj.getRect();
        obj.__NeedResetCursor = true;
    };
    obj.__Resize = function(evn)
    {
        if(evn.Event.type == "blur") return;
        if(evn.Event.type == "mouseout") return;
        if(evn.Event.X < 0) return;
        if(evn.Event.Y < 0) return;
        var AllEvn = this;//Sys.UI.AllEvents;
        var obj = Sys.UI.AllEvents.ResizeSource;
        var o = evn.getSender();
        o.__SetDragCursor(AllEvn.ResizeMovingCursor);
        var r = Sys.UI.AllEvents.ResizeOriginalRect;
        var dx = evn.Event.ScreenX - AllEvn.ResizeStartX;
        var dy = evn.Event.ScreenY - AllEvn.ResizeStartY;
        var r1 = obj.getRect();
        switch(AllEvn.ResizeMovingCursor)
        {
            case "nw-resize":
                obj.setRect(obj.getLeft()+dx, obj.getTop() + dy, obj.getWidth() - dx, obj.getHeight() - dy);
                break;
            case "n-resize":
                obj.setRect(obj.getLeft(), obj.getTop() + dy, obj.getWidth(), obj.getHeight() - dy);
                break;
            case "ne-resize":
                obj.setRect(obj.getLeft(), obj.getTop() + dy, obj.getWidth() + dx, obj.getHeight() - dy);
                break;
            case "e-resize":
                obj.setRect(obj.getLeft(), obj.getTop() , obj.getWidth() + dx, obj.getHeight() );
                break;
            case "se-resize":
                obj.setRect(obj.getLeft(), obj.getTop() , obj.getWidth() + dx, obj.getHeight() + dy);
                break;
            case "s-resize":
                obj.setRect(obj.getLeft(), obj.getTop() , obj.getWidth() , obj.getHeight() + dy);
                break;
            case "sw-resize":
                obj.setRect(obj.getLeft()+dx, obj.getTop() , obj.getWidth() - dx, obj.getHeight() + dy);
                break;
            case "w-resize":
                obj.setRect(obj.getLeft()+dx, obj.getTop() , obj.getWidth() - dx, obj.getHeight());
                break;
            case "cmove":
                obj.setRect(obj.getLeft()+dx, obj.getTop()+dy , obj.getWidth(), obj.getHeight());
                break;
        }
        r2 = obj.getRect();
        if(r1.Left!=r2.Left || r1.Top!=r2.Top || r1.Width != r2.Width || r1.Height != r2.Height)
        {
            Sys.UI.AllEvents.ResizeStartX = Sys.Screen.LastMouseScreenPoint.X;
            Sys.UI.AllEvents.ResizeStartY = Sys.Screen.LastMouseScreenPoint.Y;
        }
        switch(evn.Event.Type)
        {
            case "mousemove":
                break;
            case "mouseup":   
                this.__EndResizing(o);
                break;
            default:
                if(evn.Event.Key == 27)
                {
                    o.setRect(AllEvn.ResizeOriginalRect);
                    this.__EndResizing(o);
                }
        }
    };
    obj.__EndResizing = function(obj)
    {
        this.Resizing = false;
        this.ResizeSource = null;
        this.ResizeMovingCursor = "";
        obj.__ResetCursor();
    };
    obj.Call = function(EventObj)
    {
        this.OriginalEventObject = EventObj;
        var t = EventObj.Event.Type;
        if  (
                    t == "click" || t == "dblclick" ||
                    t == "mousedown" || t == "mousemove" || 
                    t == "mouseout" || t == "mouseover" || 
                    t == "mouseup" 
            )
        {
            Sys.Screen.LastMouseScreenPoint.X = EventObj.Event.ScreenX;
            Sys.Screen.LastMouseScreenPoint.Y = EventObj.Event.ScreenY;
            Sys.Screen.LastMouseClientPoint.X = EventObj.Event.ClientX;
            Sys.Screen.LastMouseClientPoint.Y = EventObj.Event.ClientY;
            var o = EventObj.getSender();
            if(!this.Dragging)
            {
                if((!this.Resizing)&&(t == "mousedown"||t == "mousemove") && o.__Resizable())
                {
                    var mc = this.__ResizeCheckRect(o, EventObj.Event.X, EventObj.Event.Y);
                    if(mc)
                    {
                        o.__SetDragCursor(mc);
                        if(EventObj.Event.BtnLeft&& t == "mousedown")
                        {
                            this.ResizeMovingCursor = mc;
                            this.__StartResizing(o);
                        }
                    }
                    else o.__ResetCursor();
                }
                else o.__ResetCursor();
                
            }
        }
        this.OnEvent.Call(this.OriginalEventObject);
        if(this.Dragging || this.Resizing) 
        {            
            if  (
                    t == "click" || t == "dblclick" ||
                    t == "keydown" || t == "keypress" ||  
                    t == "keyup" || t == "mousedown" || 
                    t == "mousemove" || t == "mouseout" || 
                    t == "mouseover" || t == "mouseup" 
                )
            {
                if(this.Dragging)
                {
                    this.OriginalEventObject.Event.DragSource = this.DragSource;
                    if(!this.DragStarted)
                    {
                        if(t == "mousemove"||t == "mouseup")
                          this.OriginalEventObject.getSender().__DragEvent.call(this.OriginalEventObject.getSender(), this.OriginalEventObject);
                    }
                    else
                        this.OriginalEventObject.getSender().__DragEvent.call(this.OriginalEventObject.getSender(), this.OriginalEventObject);
                }
                if(this.Resizing && t!= "mouseout" && t != "mouseover" )
                {
                    this.__Resize(this.OriginalEventObject);
                }
            }
        }
    };
})();
/*TBase*/
Sys.UI.TBase = Sys.TObject.SubClass();
Sys.UI.TBase.CreateClass = function()
{
    var obj = Sys.UI.TBase.prototype;
    obj._EL = null;
    obj._InternalName = "";
    obj.getElement = function(){return this._EL;};
    obj.ChildControls = null;
    obj._Parent = null;
    obj.getParent = function(){return this._Parent;};
    obj.setParent = function(NewParent)
    {
        if(NewParent == this) return;
        if(NewParent&&NewParent == this._Parent) return;
        /*remove current node from current parent*/
         if(Sys.InstanceOf(this._Parent, Sys.UI.TBase))
            this._Parent.RemoveChild(this);
        else
            if(this._EL)
                if(this._EL.parentElement)
                    this._EL.parentElement.removeChild(this._EL);
        this._Parent = null;
        //Add to new parent
        if(!NewParent)  return;
        if(Sys.InstanceOf(NewParent, Sys.UI.TBase))
            NewParent.AddChild(this);
        else
            NewParent.appendChild(this._EL);
    };
    obj.AddChild = function(NewChild, Before)
    {
        var idx = -1;
        /*validation*/
        if(!Sys.InstanceOf(NewChild, Sys.UI.TBase)) throw Error.Create(10006);
        if(NewChild._Parent) Error.Create(1007);
        if(Before)
        {
            if(!Sys.InstanceOf(Before)) throw Error.Create(1006);
            idx = this.ChildControls.IndexOf(Before);
            if(idx == -1) throw Error.Create(1008);
        }
        if(idx == -1)
        {//Append
            this._EL.appendChild(NewChild._EL);
            this.ChildControls.Add(NewChild);
        }
        else
        {//insert
            this._EL.insertBefore(NewChild._EL, Before._EL);
            this.ChildControls.Add(idx, NewChild);
        }
        NewChild._Parent = this;
        return NewChild;
    };
    obj.RemoveChild = function(Child)
    {
        var idx = this.ChildControls.IndexOf(Child);
        if(idx == -1) throw Error.Create(1009);
        this._EL.removeChild(Child._EL);
        this.ChildControls.DeleteAt(idx);
        Child._Parent = null;
        return Child;
    };
    obj.ReadCSS = function(Name){return this._EL.style[Name];};
    obj.WriteCSS = function()
    {
        if(arguments.length==1)
        {
            if(typeof(arguments[0])=="object")
            {
                for(var i in arguments[0])
                {
                    this._EL.style[i] = arguments[0][i];
                }
                return;
            }
        }
        for(var i = 0; i < arguments.length; i = i + 2)
            this._EL.style[arguments[i]] = arguments[i+1];
    };
    obj.setVisible = function(Value){this.WriteCSS("display", Value ? "inline" : "none");};
    obj.getVisible = function(){return this.ReadCSS("display") == "none" ? false : true;};
    obj.__SetMisc = function()
    {
        if(!Sys.InstanceOf(this._Parent, Sys.UI.TBase)) return;
        this.__Rects.sLeft = this.__Rects.Left;
        this.__Rects.sTop = this.__Rects.Top;
        this.__Rects.Right = this._Parent.getClientWidth() - this.__Rects.Left - this.__Rects.Width;
        this.__Rects.sRight = this.__Rects.Right;
        this.__Rects.Bottom = this._Parent.getClientHeight() - this.__Rects.Top - this.__Rects.Height;
        this.__Rects.sBottom = this.__Rects.Bottom;
        this.__Rects.OriginalWidth = this._Parent.getClientWidth();
        this.__Rects.OriginalHeight = this._Parent.getClientHeight();
    };
    obj.getLeft = function(){return this._EL.offsetLeft;};
    obj.setLeft = function(Value)
    {
        this.__Rects.Left = Value;
        this.__SetMisc();
        this.SetBounds();
    };
    obj.getTop = function(){return this._EL.offsetTop;};
    obj.setTop = function(Value)
    {
        this.__Rects.Top = Value;
        this.__SetMisc();
        this.SetBounds();
    };
    obj.getWidth = function(){return this._EL.offsetWidth;};
    obj.setWidth = function(Value)
    {
        this.__Rects.Width = this.__Rects.CheckWidthRange(Value);
        this.__SetMisc();
        this.SetBounds();
    };
    obj.getHeight = function(){return this._EL.offsetHeight;}
    obj.setHeight = function(Value)
    {
        this.__Rects.Height = this.__Rects.CheckHeightRange(Value);
        this.__SetMisc();
        this.SetBounds();
    };
    obj.getLocation = function(){return {X: this.getLeft(), Y: this.getTop()};};
    obj.setLocation = function()
    {
        if(arguments.length==1)
        {
            this.__Rects.Left = arguments[0].X;
            this.__Rects.Top = arguments[0].Y;
            this.__SetMisc();
            this.SetBounds();
            return
        }
        if(arguments.length > 1)
        {
            this.__Rects.Left = arguments[0];
            this.__Rects.Top = arguments[1];
            this.__SetMisc();
            this.SetBounds();
            return
        }
        Error.Create(10011, "Location");
    };
    obj.getSize = function(){return {X: this.getWidth(), Y: this.getHeight()};};
    obj.setSize = function()
    {
        if(arguments.length==1)
        {
            this.__Rects.Width = arguments[0].X;
            this.__Rects.Height = arguments[0].Y;
            this.__SetMisc();
            this.SetBounds();
            return
        }
        if(arguments.length > 1)
        {
            this.__Rects.Width = arguments[0];
            this.__Rects.Height = arguments[1];
            this.__SetMisc();
            this.SetBounds();
            return
        }
        Error.Create(10011, "Size");
    };
    obj.getRect = function(){return {Left: this.getLeft(), Top: this.getTop(), Width: this.getWidth(), Height: this.getHeight()};};
    obj.setRect = function()
    {
        if(arguments.length==1)
        {
            this.__Rects.Left = arguments[0].Left;
            this.__Rects.Top = arguments[0].Top;
            this.__Rects.Width = arguments[0].Width;
            this.__Rects.Height = arguments[0].Height;
            this.__SetMisc();
            this.SetBounds();
            return
        }
        if(arguments.length == 4)
        {
            this.__Rects.Left = arguments[0];
            this.__Rects.Top = arguments[1];
            this.__Rects.Width = arguments[2];
            this.__Rects.Height = arguments[3];
            this.__SetMisc();
            this.SetBounds();
            return
        }
        Error.Create(10011, "Rect");
    };
    obj.getClientWidth = function(){return this._EL.clientWidth;};
    obj.getClientHeight = function(){return this._EL.clientHeight;};
    obj.getScrollLeft = function(){return this._EL.scrollLeft;};
    obj.setScrollLeft = function(Value){this._EL.scrollLeft = Value;};
    obj.getScrollTop = function(){return this._EL.scrollTop;};
    obj.setScrollTop = function(Value){this._EL.scrollTop = Value;};
    obj.getScrollWidth = function(){return this._EL.scrollWidth;};
    obj.getScrollHeight = function(){return this._EL.scrollHeight;};
    obj.getConstraints = function(){return {MaxHeight:this.__Rects.MaxHeight, MaxWidth: this.__Rects.MaxWidth, MinHeight: this.__Rects.MinHeight, MinWidth : this.__Rects.MinWidth};};
    obj.setConstraints = function()
    {
        if(arguments.length == 4)
        {
            this.__Rects.MinWidth = arguments[0]; 
            this.__Rects.MinHeight = arguments[1];
            this.__Rects.MaxWidth = arguments[2]; 
            this.__Rects.MaxHeight = arguments[3]; 
        }
        else
        {
            this.__Rects.MaxHeight = arguments[0].MaxHeight; 
            this.__Rects.MaxWidth = arguments[0].MaxWidth; 
            this.__Rects.MinHeight = arguments[0].MinHeight; 
            this.__Rects.MinWidth = arguments[0].MinWidth; 
        }
        this.SetBounds();
    };
    obj.getAnchors = function(){return {Left: this.__Rects.ALeft, Top: this.__Rects.ATop, Right: this.__Rects.ARight, Bottom: this.__Rects.ABottom};};
    obj.setAnchors = function()
    {
        var r = false;
        if(arguments.length == 1)
            r = this.__Rects.setAnchors(arguments[0].Left, arguments[0].Top, arguments[0].Right, arguments[0].Bottom);
        else if(arguments.length == 4)
            r = this.__Rects.setAnchors(arguments[0], arguments[1], arguments[2], arguments[3]);
        else Error.Create(10011, "Anchors");
        if(r)
            this.SetBounds();
    };
    obj.getDock = function(){return this.__Rects.Dock};
    obj.setDock = function(Value)
    {
        if(this.__Rects.setDock(Value))
            this.SetBounds();
    };
    obj.__Rects = null;
    obj.__ReCalculateSize = function(pStopLookParent)
    {
        var R = this.__Rects, c, cR;
        var R1 = {Left:0,Top:0,Width:0,Bottom:0}, t;
        if(R.IsCalculating) return;
        try
        {
            if(!pStopLookParent)
            {
                var vParent = this.getParent();
                if(Sys.InstanceOf(vParent, Sys.UI.TBase))
                    vParent.SetBounds(true);
            }        
            R.ClearList();    
            this.ChildControls.First();
            while(!this.ChildControls.getEof())
            {
                c = this.ChildControls.Value();
                if(!c.getVisible())
                {
                    this.ChildControls.Next();
                    continue;
                }
                cR = c.__Rects;
                cR.OriginalOrder = this.ChildControls.GetBookmark();
                if(c.getDock()=="left"){R.DockLeft.Append(c);}
                else if(c.getDock()=="top"){R.DockTop.Append(c);}
                else if(c.getDock()=="right"){R.DockRight.Append(c);}
                else if(c.getDock()=="bottom"){R.DockBottom.Append(c);}
                else if(c.getDock()=="client"){R.DockClient.Append(c);}
                else
                {
                    if(cR.Height>0 && cR.Width>0 )
                    {
                        t = cR.Left + cR.Width;
                        R1.Width = R1.Width > t ? R1.Width : t;
                        t = cR.Top + cR.Height;
                        R1.Height = R1.Height > t ? R1.Height : t;
                    }
                    if(!((cR.ALeft)&&(cR.ATop)&&(!cR.ARight)&&(!cR.ABottom)))
                        R.Others.Append(c);
                }
                this.ChildControls.Next();
            }
            t = this.getClientWidth();
            R1.Width = R1.Width > t ? R1.Width : t;
            t = this.getClientHeight();
            R1.Height = R1.Height > t ? R1.Height : t;
            R.Sort();
Recalc :
            while(true)
            {

                t = Sys.AssignValues(R1);
                t.h1 = 0; t.w1 = 0;
                R.DockTop.First();
                while(!R.DockTop.getEof())
                {
                    c = R.DockTop.Value();
                    cR = c.__Rects;
                    //if(cR.Height>0 && cR.Width>0 )
                    //{
                        cR.Left = t.Left;
                        cR.Top = t.Top;
                        cR.Width = cR.CheckWidthRange(t.Width);
                        t.Top = t.Top + cR.Height;
                        t.Height = t.Height - cR.Height;
                        if(cR.Width > R1.Width)
                        {
                            R1.Width = cR.Width;
                            continue Recalc;
                        }
                    //}
                    c.SetBounds(true);
                    R.DockTop.Next();
                }
                R.DockBottom.First();
                while(!R.DockBottom.getEof())
                {
                    c = R.DockBottom.Value();
                    cR = c.__Rects;
                    //if(cR.Height>0 && cR.Width>0 )
                    //{
                        cR.Left = t.Left;
                        cR.Top = t.Top + t.Height - cR.Height;
                        cR.Width = cR.CheckWidthRange(t.Width);
                        t.Height = t.Height - cR.Height;
                        t.h1 = t.h1 + cR.Height;
                        if(cR.Width > R1.Width)
                        {
                            R1.Width = cR.Width;
                            continue Recalc;
                        }
                        c.SetBounds(true);
                    //}
                    R.DockBottom.Next();
                }
                R.DockLeft.First();
                while(!R.DockLeft.getEof())
                {
                    c = R.DockLeft.Value();
                    cR = c.__Rects;
                    //if(cR.Height>0 && cR.Width>0 )
                    //{
                        cR.Left = t.Left;
                        cR.Top = t.Top;
                        cR.Height = cR.CheckHeightRange(t.Height);
                        t.Left = t.Left + cR.Width;
                        t.Width = t.Width - cR.Width;
                        if(cR.Height + t.Top + t.h1 > R1.Height)
                        {
                            R1.Height = cR.Height + t.Top + t.h1;
                            continue Recalc;
                        }
                    //}
                    c.SetBounds(true);
                    R.DockLeft.Next();
                }
                R.DockRight.First();
                while(!R.DockRight.getEof())
                {
                    c = R.DockRight.Value();
                    cR = c.__Rects;
                    //if(cR.Height>0 && cR.Width>0 )
                    //{
                        cR.Left = t.Left + t.Width - cR.Width;
                        cR.Top = t.Top;
                        cR.Height = cR.CheckHeightRange(t.Height);
                        t.Width = t.Width - cR.Width;
                        t.w1 = t.w1 + cR.Width;
                        if(cR.Height + t.Top + t.h1 > R1.Height)
                        {
                            R1.Height = cR.Height + t.Top + t.h1;
                            continue Recalc;
                        }
                    //}
                    c.SetBounds(true);
                    R.DockRight.Next();
                }
                R.DockClient.First();
                while(!R.DockClient.getEof())
                {
                    c = R.DockClient.Value();
                    cR = c.__Rects;
                    cR.Left = t.Left, cR.Top = t.Top;
                    cR.Height = cR.CheckHeightRange(t.Height);
                    cR.Width = cR.CheckWidthRange(t.Width);
                    if(cR.Height > t.Height || cR.Width > t.Width)
                    {
                        R1.Width = cR.Width > t.Width ? cR.Width + t.Left + t.w1 : R1.Width ;
                        R1.Height = cR.Height > t.Height ? cR.Height + t.Top + t.w1 : R1.Height ;
                        continue Recalc;
                    }
                    c.SetBounds(true);
                    R.DockClient.Next();
                }
                R.Others.First();
                while(!R.Others.getEof())
                {
                    c = R.Others.Value();
                    cR = c.__Rects;
                    if((cR.ALeft)&&(cR.ATop)&&(cR.ARight)&&(cR.ABottom))
                    {
                        cR.Width = cR.CheckWidthRange(this.getClientWidth() - cR.Left - cR.Right);
                        cR.Height = cR.CheckHeightRange(this.getClientHeight() - cR.Top - cR.Bottom);
                    }
                    else if((cR.ALeft)&&(cR.ATop)&&(cR.ARight)&&(!cR.ABottom))
                    {
                        cR.Width = cR.CheckWidthRange(this.getClientWidth() - cR.Left - cR.Right);
                    }
                    else if((cR.ALeft)&&(cR.ATop)&&(!cR.ARight)&&(cR.ABottom))
                    {
                        cR.Height = cR.CheckHeightRange(this.getClientHeight() - cR.Top - cR.Bottom);
                    }
                    else if((cR.ALeft)&&(!cR.ATop)&&(cR.ARight)&&(cR.ABottom))
                    {
                        cR.Top = this.getClientHeight() - cR.Height - cR.Bottom;
                        cR.Width = cR.CheckWidthRange(this.getClientWidth() - cR.Left - cR.Right);
                    }
                    else if((cR.ALeft)&&(!cR.ATop)&&(cR.ARight)&&(!cR.ABottom))
                    {
                        cR.Width = this.getClientWidth() - cR.Left - cR.Right;
                        cR.Top = cR.Top + (this.getClientHeight() - cR.OriginalHeight)/2;
                        cR.OriginalHeight = this.getClientHeight();
                    }
                    else if((cR.ALeft)&&(!cR.ATop)&&(!cR.ARight)&&(cR.ABottom))
                    {
                        cR.Top = this.getClientHeight() - cR.Height - cR.Bottom;
                    }
                    else if((cR.ALeft)&&(!cR.ATop)&&(!cR.ARight)&&(!cR.ABottom))
                    {
                        cR.Top = cR.Top + (this.getClientHeight() - cR.OriginalHeight)/2;
                        cR.OriginalHeight = this.getClientHeight();
                    }
                    else if((!cR.ALeft)&&(cR.ATop)&&(cR.ARight)&&(cR.ABottom))
                    {
                        cR.Left = this.getClientWidth() - cR.Width - cR.Right;
                        cR.Height = this.getClientHeight() - cR.Top - cR.Bottom;
                    }
                    else if((!cR.ALeft)&&(cR.ATop)&&(cR.ARight)&&(!cR.ABottom))
                    {
                        cR.Left = this.getClientWidth() - cR.Width - cR.Right;
                    }
                    else if((!cR.ALeft)&&(cR.ATop)&&(!cR.ARight)&&(cR.ABottom))
                    {
                        cR.Left = cR.Left + (this.getClientWidth() - cR.OriginalWidth)/2;
                        cR.OriginalWidth = this.getClientWidth();
                        cR.Height = this.getClientHeight() - cR.Top - cR.Bottom;
                    }
                    else if((!cR.ALeft)&&(cR.ATop)&&(!cR.ARight)&&(!cR.ABottom))
                    {
                        cR.Left = cR.Left + (this.getClientWidth() - cR.OriginalWidth)/2;
                        cR.OriginalWidth = this.getClientWidth();
                    }
                    else if((!cR.ALeft)&&(!cR.ATop)&&(cR.ARight)&&(cR.ABottom))
                    {
                        cR.Left = this.getClientWidth() - cR.Width - cR.Right;
                        cR.Top = this.getClientHeight() - cR.Height - cR.Bottom;
                    }
                    else if((!cR.ALeft)&&(!cR.ATop)&&(cR.ARight)&&(!cR.ABottom))
                    {
                        cR.Left = this.getClientWidth() - cR.Width - cR.Right;
                        cR.Top = cR.Top + (this.getClientHeight() - cR.OriginalHeight)/2;
                        cR.OriginalHeight = this.getClientHeight();
                    }
                    else if((!cR.ALeft)&&(!cR.ATop)&&(!cR.ARight)&&(cR.ABottom))
                    {
                        cR.Left = cR.Left + (this.getClientWidth() - cR.OriginalWidth)/2;
                        cR.OriginalWidth = this.getClientWidth();
                        cR.Top = this.getClientHeight() - cR.Height - cR.Bottom;
                    }
                    else if((!cR.ALeft)&&(!cR.ATop)&&(!cR.ARight)&&(!cR.ABottom))
                    {
                        cR.Left = cR.Left + (this.getClientWidth() - cR.OriginalWidth)/2;
                        cR.OriginalWidth = this.getClientWidth();
                        cR.Top = cR.Top + (this.getClientHeight() - cR.OriginalHeight)/2;
                        cR.OriginalHeight = this.getClientHeight();
                    }
                    c.SetBounds(true);
                    R.Others.Next();
                }
                break;
            }
        }
        finally
        {
            R.IsCalculating = false;
        }
    };
    obj.SetBounds = function(pStopLookParent)
    {
        /*this.WriteCSS(
                        "left", this.__Rects.Left>0 ? this.__Rects.Left.toString()+"px" : "0px",
                        "top", this.__Rects.Top>0 ? this.__Rects.Top.toString()+"px" : "0px",
                        "width", this.__Rects.Width>0 ? this.__Rects.Width.toString()+"px" : "0px",
                        "height", this.__Rects.Height>0 ? this.__Rects.Height.toString()+"px" : "0px"
                     );*/
       this.WriteCSS(
                        "left", this.__Rects.Left.toString()+"px",
                        "top", this.__Rects.Top.toString()+"px",
                        "width", this.__Rects.Width>0 ? this.__Rects.Width.toString()+"px" : "0px",
                        "height", this.__Rects.Height>0 ? this.__Rects.Height.toString()+"px" : "0px"
                     );
        this.__ReCalculateSize(pStopLookParent);
        if  (
            (this.getLeft()!= this.__Rects.Left) ||
            (this.getTop()!= this.__Rects.Top) ||
            (this.getHeight()!= this.__Rects.Height) ||
            (this.getWidth()!= this.__Rects.Width) 
            )
        {
           /* this.WriteCSS(
                            "left", this.__Rects.Left>0 ? this.__Rects.Left.toString()+"px" : "0px",
                            "top", this.__Rects.Top>0 ? this.__Rects.Top.toString()+"px" : "0px",
                            "width", this.__Rects.Width>0 ? this.__Rects.Width.toString()+"px" : "0px",
                            "height", this.__Rects.Height>0 ? this.__Rects.Height.toString()+"px" : "0px"
                         );*/
             this.WriteCSS(
                        "left", this.__Rects.Left.toString()+"px",
                        "top", this.__Rects.Top.toString()+"px",
                        "width", this.__Rects.Width>0 ? this.__Rects.Width.toString()+"px" : "0px",
                            "height", this.__Rects.Height>0 ? this.__Rects.Height.toString()+"px" : "0px"
                     );
        }
                         
    };
    obj.getColor = function(){return Sys.TColor.Create(this.ReadCSS("backgroundColor"))};
    obj.setColor = function(Value){this.WriteCSS("backgroundColor",Sys.TColor.Create(Value).toString());};
    obj._CanFocus = false;
    obj.getCanFocus = function()
    {
        return this._CanFocus;
    };
    obj.setCanFocus = function(Value)
    {
        this._CanFocus = Value;
        this.getElement().tabIndex = this._CanFocus ? this.getTabOrder() : -1;
    };
    obj.getEnabled = function()
    {
        return !this.getElement().disabled;
    };
    obj.setEnabled = function(Value)
    {
        this.getElement().disabled = !Value;
    };
    obj._TabOrder = 1;
    obj.getTabOrder = function()
    {
        return this._TabOrder;
    };
    obj.setTabOrder = function(Value)
    {
        if(Value < 1) throw Error.Create(10012);
        this._TabOrder = Value;
        if(this.getCanFocus())
        {
            this.getElement().tabIndex = this.getTabOrder();
        }
    };
    obj.__SetZOrder = function(TopMost)
    {
        var c = this.getParent();
        if(!Sys.InstanceOf(c, Sys.UI.TBase)) return;
        c.__ResetZOrder();
        if(TopMost)
        {
            c.__ZMax++;
            this.WriteCSS("zIndex",c.__ZMax);
        }
        else
        {
            c.__ZMin--;
            this.WriteCSS("zIndex",c.__ZMin);
        }
    };
    obj.__ZMin = 0; obj.__ZMax = 0;
    obj.__ResetZOrder = function()
    {
        if(!(this.__ZMin < -32000 || this.__ZMax > 32000)) return;
        this.__ZMin = 0; this.__ZMax =0;
        var c = this.ChildControls;
        c.First();
        while(!c.getEof())
        {
            c.Value().WriteCSS("zIndex", 0);
            c.Next();
        }
    };
    obj.SendToBack = function()
    {
        this.__SetZOrder(false);
    };
    obj.BringToFront = function()
    {
        this.__SetZOrder(true);
    };
    obj._Cursor = "default";
    obj.__NeedResetCursor = true;
    obj.getCursor = function()
    {
        return this._Cursor;
    };
    obj.setCursor = function(Value)
    {
        Value = Value.toString().toLowerCase();
        if(!Sys.UI.Cursors[Value])
            Value = "default";
        this._Cursor = Value;
        this.WriteCSS("cursor", Sys.UI.Cursors[Value]);
    };
    obj.__setCursor = function(Value)
    {
        Value = Value.toString().toLowerCase();
        if(!Sys.UI.Cursors[Value])
            Value = "default";
        this.WriteCSS("cursor", Sys.UI.Cursors[Value]);
    };
    obj.__ResetCursor = function()
    {
        if(this.__NeedResetCursor)
        {
            this.setCursor(this._Cursor);
            this.__NeedResetCursor = false;
        }
    };
    obj._DragCursor = "drag";
    obj.getDragCursor = function()
    {
        return this._DragCursor;
    };
    obj.setDragCursor = function(Values)
    {
        Value = Value.toString().toLowerCase();
        if(!Sys.UI.Cursors[Value])
            Value = "default";
        this._DragCursor = Value;
    };
    /*Event*/
    obj.__CreateUIEvent = function(type)
    {
        var ret;
        ret = Sys.UI.TUIBaseEvent.Create(this, this);
        ret.AddElement(this.getElement(),type);
        return ret;
    };
    obj.OnClick = null;
    obj.OnDblClick = null;
    obj.OnEnter = null;
    obj.OnExit = null;
    obj.OnKeyDown = null;
    obj.OnKeyPress = null; 
    obj.OnKeyUp = null;
    obj.OnMouseDown = null;
    obj.OnMouseMove = null;
    obj.OnMouseOut = null;
    obj.OnMouseOver = null;
    obj.OnMouseUp = null;
    obj.OnMouseWheel = null;
    obj.OnResize = null;
    /*Drag and Drop*/
    obj._AllowDrop = false;
    obj.getAllowDrop = function(){return this._AllowDrop;};
    obj.setAllowDrop = function(Value){this._AllowDrop = Value ? true : false;};
    obj.__SetDragCursor = function(Cursor)
    {
        this.__NeedResetCursor = true;
        this.__setCursor(Cursor ? Cursor : (this.getAllowDrop()? this.getDragCursor() : "no-drop"));
    }
    obj.__DragEvent = function(evn)
    {
        var AllEvn = Sys.UI.AllEvents;
        switch(evn.Event.Type)
        {
            case "mousemove":
                if(AllEvn.DragStarted)
                {
                    if(this.getAllowDrop())
                    {
                        this.OnDragOver.Event = evn.Event;
                        this.OnDragOver.Data.Cursor = null;
                        this.OnDragOver.Call();
                        this.__SetDragCursor(this.OnDragOver.Data.Cursor);
                    }
                    this.__SetDragCursor();
                }
                else
                {
                    if( 
                            (Math.abs(Math.abs(evn.Event.ScreenX) - Math.abs(AllEvn.DragStartX)) > AllEvn.DragThreshold) ||
                            (Math.abs(Math.abs(evn.Event.ScreenY) - Math.abs(AllEvn.DragStartY)) > AllEvn.DragThreshold)
                      ) AllEvn.DragStarted = true;
                }
                break;
            case "mouseout":
                if(AllEvn.DragStarted)
                {
                    if(this.getAllowDrop())
                    {
                        this.OnDragExit.Event = evn.Event;
                        this.OnDragExit.Call();
                    }
                }
                break;
            case "mouseover":
                if(AllEvn.DragStarted)
                {
                    if(this.getAllowDrop())
                    {
                        this.OnDragEnter.Event = evn.Event;
                        this.OnDragEnter.Call();
                    }
                }
                break;
            case "mouseup":
                if(AllEvn.DragStarted && this.getAllowDrop())
                {
                    this.OnDragDrop.Event = evn.Event;
                    this.OnDragDrop.Call();
                }
                this.EndDrag();
                break;
            default:
                if(evn.Event.Key == 27)
                    this.EndDrag();
        }
    };
    obj.BeginDrag = function(Threshold) /*default = 3*/
    {
        if(!Threshold) Threshold = 3;
        Sys.UI.AllEvents.Dragging = true;
        Sys.UI.AllEvents.DragStarted = false;
        Sys.UI.AllEvents.DragThreshold = Threshold;
        Sys.UI.AllEvents.DragSource = this;
        Sys.UI.AllEvents.DragStartX = Sys.Screen.LastMouseScreenPoint.X;
        Sys.UI.AllEvents.DragStartY = Sys.Screen.LastMouseScreenPoint.Y;
        this.__NeedResetCursor = true;
    };
    obj.EndDrag = function()
    {
        var obj = Sys.UI.AllEvents;
        obj.Dragging = false;
        obj.DragThreshold = -1;
        obj.DragSource = null;
        this.__ResetCursor();
    };
    obj.OnDragDrop = null;
    obj.OnDragEnter = null;
    obj.OnDragExit = null;
    obj.OnDragOver = null;
    obj.__CreateEvents = function()
    {
        this.OnClick = this.__CreateUIEvent("click");
        this.OnDblClick = this.__CreateUIEvent("dblclick");
        this.OnEnter = this.__CreateUIEvent("focus");
        this.OnExit = this.__CreateUIEvent("blur");
        this.OnKeyDown = this.__CreateUIEvent("keydown");
        this.OnKeyPress = this.__CreateUIEvent("keypress");
        this.OnKeyUp = this.__CreateUIEvent("keyup");
        this.OnMouseDown = this.__CreateUIEvent("mousedown");
        this.OnMouseMove = this.__CreateUIEvent("mousemove");
        this.OnMouseOut = this.__CreateUIEvent("mouseout");
        this.OnMouseOver = this.__CreateUIEvent("mouseover");
        this.OnMouseUp = this.__CreateUIEvent("mouseup");
        this.OnMouseWheel = this.__CreateUIEvent("mousewheel");
        this.OnResize = this.__CreateUIEvent("resize");
        /*DragDrop*/
        this.OnDragDrop = Sys.TEvent.Create(this, this);
        this.OnDragEnter = Sys.TEvent.Create(this, this);
        this.OnDragExit = Sys.TEvent.Create(this, this);
        this.OnDragOver = Sys.TEvent.Create(this, this);
    };
    /*Resize and move*/
    obj.__Resizable = function()
    {
        return false;
    };
    obj.__Movable = function()
    {
        return false;
    }
    obj.__GetMovingRect = function() {return {Left:-1, Top:-1, Width: -1, Height: -1};};
    obj.Create = function(Type, body) //can be image, panel, edit
    {
        Type = (Type.toString()).toLowerCase();
        switch(Type)
        {
            case "doc":
                this._EL = body ? body : document.body ;                  
                break;
            case "image":
                this._EL = Sys.Doc.createElement("image");
                break;
            case "panel":
                this._EL = Sys.Doc.createElement("span");
                this._EL.setAttribute("oncontextmenu",(function(){return false;}));
                break;
            case "edit":
                //this._EL = Sys.Doc.createElement("input");
                //this._EL.type = "text";
                this._EL = Sys.Browser.CreateEdit();
                break;            
        }
        if(!this._EL) throw Error.Create(10005, Type);
        
        this._InternalName = Sys.GenerateAnonymous(); 
        this._EL.id = this._InternalName; 
        this._EL.SysRef = this;
        this.ChildControls = Sys.TList.Create();
        Sys.UI.AllControls[this._InternalName] = this;
        this.__Rects = Sys.UI.T__Rects.Create();
        this.WriteCSS("position", "absolute", "overflow", "hidden");
        this.WriteCSS("zIndex", 0);
        this.__CreateEvents();
    };
    obj.Free = function()
    {
        delete Sys.UI.AllControls[this._InternalName];
        this.setParent(null);
        this.Super("Free");
    };
};
Sys.UI.TBase.CreateClass();
/*TImage*/
