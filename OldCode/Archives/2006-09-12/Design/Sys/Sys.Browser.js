/*
    Author:         John Huang
    Start Date:     2006-04-29 
    Description:    Detect browser, encapsulate native event object
                    and the code that browser related.
                    System only support IE and Gecko.
*/
(function()
{
	Sys.Browser = {};
    var obj = Sys.Browser;
    obj.browserType = "unknown";
    if (window.external) {obj.browserType = "ie"}
    else if (window.__defineGetter__) {obj.browserType = "gecko"}
    else if ((/opera/gi).test(navigator.userAgent)){obj.browserType = "opera"}
    else if ((/Safari|Konqueror|KHTML/gi).test(navigator.userAgent)){obj.browserType = "safari"}
    obj.ie = false;
    obj.gecko = false;
    obj.opera = false;
    obj.safari = false;
    if(obj.browserType!="unknown")
        obj[obj.browserType] = true;
    obj.os = "";
	if (!navigator.userAgent.match("Windows")){obj.os = "windows"}
	if (navigator.userAgent.match("Mac OS")){obj.os = "mac"}
	if (navigator.userAgent.match("Linux")){obj.os = "linux"}
	
    obj.cookieEnabled = navigator.cookieEnabled;
    
    
    obj.Browser = "unknown";
    if (window.external) {obj.Browser = "ie"}
    else if (window.__defineGetter__) {obj.Browser = "gecko"}
    else if (window.opera){obj.Browser = "opera"}
    else if (navigator.userAgent.match("Safari")){obj.Browser = "safari"}
    obj.ie = false;
    obj.gecko = false;
    obj.opera = false;
    obj.safari = false;
    if(obj.Browser!="unknown")
        obj[obj.Browser] = true;
    obj.CookieEnabled = navigator.cookieEnabled;
    obj.AttachEvent = function(Element, Type, Handle)
    {
        if(Element.attachEvent)
            return Element.attachEvent("on"+Type, Handle);
        if(Element.addEventListener)
            return Element.addEventListener(Type, Handle, false);
        Element["on"+Type] = Handle;
        return true;
    };
    obj.DetachEvent = function(Element, Type, Handle)
    {
        if(Element.detachEvent)
            return Element.detachEvent("on"+Type, Handle);
        if(Element.removeEventListener)
            return Element.removeEventListener(Type, Handle, false);
        Element["on"+Type] = null;
        return true;
    };
    obj.ConvertEventObject = function(evn)
    {
        var ret = new Object();
        ret._EventObject = evn;
        ret.DateTime = new Date();
        ret.AltKey = evn.altKey ? true : false;
        ret.AltKeyLeft = evn.altLeft ? true : false;
        ret.AltKeyRight = ret.AltKey;
        ret.CtrlKey = evn.ctrlKey ? true: false;
        ret.CtrlKeyLeft = (evn.ctrlLeft ? true : false);
        ret.CtrlKeyRight = ret.CtrlKey;
        ret.ShiftKey = evn.shiftKet ? true : false;
        ret.ShiftKeyLeft = evn.shiftLeft ? true : false;
        ret.ShiftKeyRight = ret.ShiftKey;
        ret.MetaKey = evn.metaKey ? true : false;
        ret.Button = evn.button ? evn.button : 0;
        
        if(Sys.Browser.gecko||Sys.Browser.opera)
        {
            ret.BtnLeft = evn.button == 0;
            ret.BtnRight = evn.button == 2;
            ret.BtnMiddle = evn.button == 1;
        }
        else
        {
            ret.BtnLeft = (evn.button & 1) ? true : false;
            ret.BtnRight = (evn.button & 2) ? true : false;
            ret.BtnMiddle = (evn.button & 4) ? true : false;
        }
        ret.Key = evn.keyCode ? evn.keyCode : (evn.which ? evn.which : (evn.charCode ? evn.charCode : 0)) ;
        ret.Type = evn.type ? evn.type : "";
        ret.ClientX = evn.clientX ? evn.clientX : 0;
        ret.ClientY = evn.clientY ? evn.clientY : 0;
        ret.ScreenX = evn.screenX ? evn.screenX : 0;
        ret.ScreenY = evn.screenY ? evn.screenY : 0;
        if(evn.offsetX != undefined)
        {
            ret.X = evn.offsetX ? evn.offsetX : 0;
            ret.Y = evn.offsetY ? evn.offsetY : 0;
        }
        else
        {
            ret.X = evn.layerX ? evn.layerX : 0;
            ret.Y = evn.layerY ? evn.layerY : 0
        }
        ret.StopBubble = function()
        {
            if(!this._EventObject) return;
            if(this._EventObject.cancelBubble!=undefined) this._EventObject.cancelBubble = true;
            if(typeof(this.preventDefault)=="function") this.preventDefault();
            return;
        };
        ret.StopPropagation = function()
        {
            if(!this._EventObject) return;
            try{this._EventObject.returnValue = false}catch(e){};
            if(typeof(this._EventObject.stopPropagation) == "function") this._EventObject.stopPropagation();
        };
        if(Sys.Browser.ie)
        {
            ret.SourceElement = evn.srcElement;
            ret.ToElement = evn.toElement;
        }
        else
        {
            try 
            {
                ret.SourceElement = (evn.target && evn.target.nodeType==3) ? evn.target.parentNode : evn.target;
			}
			catch(e)
			{
				ret.SourceElement = evn.target;
			}
			try 
			{
				ret.ToElement = (evn.relatedTarget && evn.relatedTarget.nodeType==3) ? evn.relatedTarget.parentNode : evn.relatedTarget;
			}
			catch(e)
			{
				ret.ToElement = evn.relatedTarget;
			}
        }
        ret.WheelDelta = evn.wheelDelta ? evn.wheelDelta : 0;
        //ret.SourceElement_id = ret.SourceElement.id;
        //ret.ToElement_id = ret.ToElement.id;
        return ret;
    };
    obj.CreateEdit = function()
    {
        var ret;
        if(Sys.Browser.ie)
        {
            ret = Sys.Doc.createElement("span");
            ret.setAttribute("contentEditable", true);
            return ret;
        }
        if(Sys.Browser.gecko)
        {
            ret = Sys.Doc.createElement("iframe");
            ret.contentDocument.designMode="on";
            return ret;
        }
        ret = Sys.Doc.createElement("input");
        ret.type = "text";
        return ret;
    };
})();
